using System;
using System.Drawing;
using System.ComponentModel;

#if AdvTree
using DevComponents.Tree.Design;
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Represents visual style of an User Interface Element.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false),TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class ElementStyle:System.ComponentModel.IComponent
	{
		#region Events
		/// <summary>
		/// Occurs when appearance property of the style has changed.
		/// </summary>
		public event EventHandler StyleChanged;
		/// <summary>
		/// Occurs when component is Disposed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when component is Disposed.")]
		public event EventHandler Disposed;
		#endregion

		#region Private Variables
		const int DEFAULT_CORNER_DIAMETER=8;

		private Color m_BackColor=Color.Empty;
		private eColorSchemePart m_BackColorSchemePart=eColorSchemePart.None;
		private Color m_BackColor2=Color.Empty;
		private eColorSchemePart m_BackColor2SchemePart=eColorSchemePart.None;
		private int m_BackColorGradientAngle=0;
		private Image m_BackgroundImage=null;
		private eStyleBackgroundImage m_BackgroundImagePosition=eStyleBackgroundImage.Stretch;
		private byte m_BackgroundImageAlpha=255;
        private eGradientType m_BackColorGradientType = eGradientType.Linear;

		// Font
		private Font m_Font=null;

		// Text Formating
		private bool m_WordWrap=false;
		private eStyleTextAlignment m_TextAlignment=eStyleTextAlignment.Near;
		private eStyleTextAlignment m_TextLineAlignment=eStyleTextAlignment.Center;
		private eStyleTextTrimming m_TextTrimming=eStyleTextTrimming.EllipsisCharacter;
		//private string m_TextFormat="";

		private Color m_TextColor=Color.Empty;
		private eColorSchemePart m_TextColorSchemePart=eColorSchemePart.None;
		private Color m_TextShadowColor=Color.Empty;
		private eColorSchemePart m_TextShadowColorSchemePart=eColorSchemePart.None;
		private Point m_TextShadowOffset=Point.Empty;

		// Style margins
		private int m_MarginLeft=0;
		private int m_MarginRight=0;
		private int m_MarginTop=0;
		private int m_MarginBottom=0;

		// Style inside padding
		private int m_PaddingLeft=0;
		private int m_PaddingRight=0;
		private int m_PaddingTop=0;
		private int m_PaddingBottom=0;

		// Border
		private eStyleBorderType m_BorderLeft=eStyleBorderType.None;
		private eStyleBorderType m_BorderRight=eStyleBorderType.None;
		private eStyleBorderType m_BorderTop=eStyleBorderType.None;
		private eStyleBorderType m_BorderBottom=eStyleBorderType.None;

		private Color m_BorderColor=Color.Empty;
		private eColorSchemePart m_BorderColorSchemePart=eColorSchemePart.None;
        private Color m_BorderColor2 = Color.Empty;
        private eColorSchemePart m_BorderColor2SchemePart = eColorSchemePart.None;
        private int m_BorderGradientAngle = 90;

        private Color m_BorderColorLight = Color.Empty;
        private eColorSchemePart m_BorderColorLightSchemePart = eColorSchemePart.None;
        private Color m_BorderColorLight2 = Color.Empty;
        private eColorSchemePart m_BorderColorLight2SchemePart = eColorSchemePart.None;
        private int m_BorderLightGradientAngle = 90;

		private Color m_BorderLeftColor=Color.Empty;
		private eColorSchemePart m_BorderLeftColorSchemePart=eColorSchemePart.None;
		private Color m_BorderRightColor=Color.Empty;
		private eColorSchemePart m_BorderRightColorSchemePart=eColorSchemePart.None;
		private Color m_BorderTopColor=Color.Empty;
		private eColorSchemePart m_BorderTopColorSchemePart=eColorSchemePart.None;
		private Color m_BorderBottomColor=Color.Empty;
		private eColorSchemePart m_BorderBottomColorSchemePart=eColorSchemePart.None;

		private int m_BorderLeftWidth=0;
		private int m_BorderRightWidth=0;
		private int m_BorderTopWidth=0;
		private int m_BorderBottomWidth=0;

		private eCornerType m_CornerType=eCornerType.Square;
		private eCornerType m_CornerTypeTopLeft=eCornerType.Inherit;
		private eCornerType m_CornerTypeTopRight=eCornerType.Inherit;
		private eCornerType m_CornerTypeBottomLeft=eCornerType.Inherit;
		private eCornerType m_CornerTypeBottomRight=eCornerType.Inherit;
		private int m_CornerDiameter=DEFAULT_CORNER_DIAMETER;

		private Size m_Size=Size.Empty;
		private bool m_SizeChanged=true;

		private string m_Name="";
		private string m_Description="";

		private int m_MaximumHeight=0;
		private System.ComponentModel.ISite m_ComponentSite=null;
		private bool m_DesignMode=false;
		#if AdvTree
		private ElementStyleCollection m_Parent=null;
        private int m_MaximumWidth=0;
        #endif
        private ColorScheme m_ColorScheme=null;
        private BackgroundColorBlendCollection m_BackColorBlend = new BackgroundColorBlendCollection();
        private string m_Class = "";
        private bool m_FreezeEvents = false;
		#endregion

		/// <summary>Creates new instance of the class.</summary>
		public ElementStyle()
		{
            m_BackColorBlend.Parent = this;
		}

		#region Properties
        /// <summary>
        /// Gets the collection that defines the multicolor gradient background.
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection that defines the multicolor gradient background.")]
        public BackgroundColorBlendCollection BackColorBlend
        {
            get { return m_BackColorBlend; }
        }

		/// <summary>
		/// Gets or sets the background color for UI element. If used in combination with
		/// BackgroundColor2 is specifies starting gradient color.
		/// </summary>
		[DevCoSerialize(),Browsable(true),Category("Background"),Description("Gets or sets the background color for UI element."),Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)),TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BackColor
		{
			get
			{
				return GetColor(m_BackColor,m_BackColorSchemePart);
			}
			set
			{
				if(value!=this.BackColor && (m_BackColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BackColorSchemePart=eColorSchemePart.None;
				m_BackColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether BackgroundColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor()
		{return (!m_BackColor.IsEmpty && m_BackColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets BackgroundColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor()
		{
			m_BackColor=Color.Empty;
            this.OnStyleChanged();
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as background color. Setting
		/// this property overrides the setting of the corresponding BackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BackColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
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
        [DevCoSerialize(), Browsable(true), Category("Background"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Gets or sets the target gradient background color for UI element."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BackColor2
		{
			get {return GetColor(m_BackColor2,m_BackColor2SchemePart);}
			set
			{
				if(value!=this.BackColor2 && (m_BackColor2SchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BackColor2SchemePart=eColorSchemePart.None;
				m_BackColor2=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether BackgroundColor2 should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{return (!m_BackColor2.IsEmpty && m_BackColor2SchemePart==eColorSchemePart.None);}
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
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BackColor2 property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
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
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(0), Description("Gets or sets the background gradient angle.")]
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
        /// Gets or sets the background gradient fill type. Default value is Linear.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(eGradientType.Linear), Description("Indicates background gradient fill type.")]
        public eGradientType BackColorGradientType
        {
            get { return m_BackColorGradientType; }
            set
            {
                if (m_BackColorGradientType != value)
                {
                    m_BackColorGradientType = value;
                    this.OnStyleChanged();
                }
            }
        }

		/// <summary>
		/// Specifies background image.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(null), Description("Specifies background image."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Image BackgroundImage
		{
			get {return m_BackgroundImage;}
			set
			{
				m_BackgroundImage=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundImage to it's default value null (VB Nothing). Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackgroundImage()
		{
			m_BackgroundImage=null;
		}

		/// <summary>
		/// Specifies background image position when container is larger than image.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(eStyleBackgroundImage.Stretch), Description("Specifies background image position when container is larger than image.")]
		public eStyleBackgroundImage BackgroundImagePosition
		{
			get {return m_BackgroundImagePosition;}
			set
			{
				if(m_BackgroundImagePosition!=value)
				{
					m_BackgroundImagePosition=value;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Specifies the transparency of background image.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue((byte)255), Description("Specifies the transparency of background image.")]
		public byte BackgroundImageAlpha
		{
			get {return m_BackgroundImageAlpha;}
			set
			{
				if(m_BackgroundImageAlpha!=value)
				{
					m_BackgroundImageAlpha=value;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text color displayed in this UI element.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Properties"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Gets or sets the text color displayed in this UI element."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color TextColor
		{
			get {return GetColor(m_TextColor,m_TextColorSchemePart);}
			set
			{
				if(value!=this.TextColor && (m_TextColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.TextColorSchemePart=eColorSchemePart.None;
				m_TextColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether TextColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTextColor()
		{return (!m_TextColor.IsEmpty && m_TextColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets TextColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTextColor()
		{
			m_TextColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as text color. Setting
		/// this property overrides the setting of the corresponding TextColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through TextColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart TextColorSchemePart
		{
			get {return m_TextColorSchemePart;}
			set
			{
				m_TextColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the text shadow color.
		/// </summary>
		[DevCoSerialize(), Browsable(true),Category("Text Properties"),Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates text shadow color."),TypeConverter(typeof(ColorConverter))]
		public Color TextShadowColor
		{
			get {return GetColor(m_TextShadowColor,m_TextShadowColorSchemePart);}
			set
			{
				if(m_TextShadowColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty)
					this.TextShadowColorSchemePart=eColorSchemePart.None;
				m_TextShadowColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether TextShadowColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTextShadowColor()
		{return (!m_TextShadowColor.IsEmpty && m_TextShadowColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets TextColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTextShadowColor()
		{
			m_TextShadowColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as text shadow color. Setting
		/// this property overrides the setting of the corresponding TextShadowColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through TextColor property.
		/// </summary>
		[DevCoSerialize(), Browsable(false),DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart TextShadowColorSchemePart
		{
			get {return m_TextShadowColorSchemePart;}
			set
			{
				m_TextShadowColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Indicates text shadow offset in pixels
		/// </summary>
		[DevCoSerialize(), Browsable(true),Category("Text Properties"),Description("Indicates text shadow offset in pixels.")]
		public Point TextShadowOffset
		{
			get { return m_TextShadowOffset; }
			set
			{
				m_TextShadowOffset=value;
				OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether TextShadowOffset should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTextShadowOffset()
		{return (!m_TextShadowOffset.IsEmpty);}
		/// <summary>
		/// Resets TextShadowOffset to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTextShadowOffset()
		{
			m_TextShadowOffset=Point.Empty;
		}

		/// <summary>
		/// Gets or sets the Font used to draw this the text.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Properties"), DefaultValue(null), Description("Gets or sets the Font used to draw this the text.")]
		public System.Drawing.Font Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				m_Font=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(false), Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
		public bool WordWrap
		{
			get {return m_WordWrap;}
			set
			{
				m_WordWrap=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Specifies alignment of the text.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(eStyleTextAlignment.Near), Description("Specifies alignment of the text.")]
		public eStyleTextAlignment TextAlignment
		{
			get {return m_TextAlignment;}
			set
			{
				m_TextAlignment=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Specifies alignment of the text.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(eStyleTextAlignment.Center), Description("Specifies alignment of the text.")]
		public eStyleTextAlignment TextLineAlignment
		{
			get {return m_TextLineAlignment;}
			set
			{
				m_TextLineAlignment=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Specifies how to trim characters when text does not fit.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(eStyleTextTrimming.EllipsisCharacter), Description("Specifies how to trim characters when text does not fit.")]
		public eStyleTextTrimming TextTrimming
		{
			get {return m_TextTrimming;}
			set
			{
				if(m_TextTrimming!=value)
				{
					m_TextTrimming=value;
					this.OnStyleChanged();
				}
			}
		}

//		/// <summary>
//		/// Gets or sets the text formatting options.
//		/// </summary>
//		[Browsable(true),Category("Text Formatting"),DefaultValue(""),Description("Gets or sets the text formatting options.")]
//		public string TextFormat
//		{
//			get {return m_TextFormat;}
//			set
//			{
//				m_TextFormat=value;
//				this.OnStyleChanged();
//			}
//		}

        /// <summary>
        /// Gets the total horizontal margin (Left + Right)
        /// </summary>
        [Browsable(false)]
        public int MarginHorizontal
        {
            get { return m_MarginLeft + m_MarginRight; }
        }

        /// <summary>
        /// Gets the total vertical margin (Top + Bottom)
        /// </summary>
        [Browsable(false)]
        public int MarginVertical
        {
            get { return m_MarginTop + m_MarginBottom; }
        }


		/// <summary>
		/// Gets or sets the left margin.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Margins"), DefaultValue(0), Description("Specifies left margin.")]
		public int MarginLeft
		{
			get {return m_MarginLeft;}
			set 
			{
				m_MarginLeft=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the right margin.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Margins"), DefaultValue(0), Description("Specifies right margin.")]
		public int MarginRight
		{
			get {return m_MarginRight;}
			set 
			{
				m_MarginRight=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the top margin.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Margins"), DefaultValue(0), Description("Specifies top margin.")]
		public int MarginTop
		{
			get {return m_MarginTop;}
			set 
			{
				m_MarginTop=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the bottom margin.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Margins"), DefaultValue(0), Description("Specifies bottom margin.")]
		public int MarginBottom
		{
			get {return m_MarginBottom;}
			set 
			{
				m_MarginBottom=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether any style property has changed which could influence the size of the style.
		/// </summary>
		internal bool SizeChanged
		{
			get {return m_SizeChanged;}
			set {m_SizeChanged=value;}
		}

		/// <summary>
		/// Gets the calcuated size of the element style.
		/// </summary>
		[Browsable(false)]
		public System.Drawing.Size Size
		{
			get {return m_Size;}
		}
		/// <summary>
		/// Sets size of the element style.
		/// </summary>
		/// <param name="size">Indicates new size.</param>
		internal void SetSize(Size size)
		{
			m_Size=size;
		}

		/// <summary>
		/// Gets or sets the border type for all sides of the element.
		/// </summary>
		[DefaultValue(eStyleBorderType.None),Browsable(true),Category("Border"),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Description("Indicates the border type for all sides of the element.")]
		public eStyleBorderType Border
		{
			get {return m_BorderTop;}
			set
			{
                if (this.DesignMode)
                {
                    TypeDescriptor.GetProperties(this)["BorderLeft"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderRight"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderTop"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderBottom"].SetValue(this, value);
                }
                else
                {
                    this.BorderLeft = value;
                    this.BorderRight = value;
                    this.BorderTop = value;
                    this.BorderBottom = value;
                }
			}
		}

		/// <summary>
		/// Gets or sets border width in pixels.
		/// </summary>
		[Browsable(true),DefaultValue(0),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Category("Border"),Description("Indicates border width in pixels.")]
		public int BorderWidth
		{
			get {return m_BorderTopWidth;}
			set
			{
                if (this.DesignMode)
                {
                    TypeDescriptor.GetProperties(this)["BorderLeftWidth"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderRightWidth"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderTopWidth"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["BorderBottomWidth"].SetValue(this, value);
                }
                else
                {
                    this.BorderLeftWidth = value;
                    this.BorderRightWidth = value;
                    this.BorderTopWidth = value;
                    this.BorderBottomWidth = value;
                }
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the border type for top side of the element.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eStyleBorderType.None), Browsable(true), Category("Border"), Description("Indicates the border type for top side of the element.")]
		public eStyleBorderType BorderTop
		{
			get {return m_BorderTop;}
			set
			{
				if(m_BorderTop!=value)
				{
					m_BorderTop=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border type for bottom side of the element.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eStyleBorderType.None), Browsable(true), Category("Border"), Description("Indicates the border type for bottom side of the element.")]
		public eStyleBorderType BorderBottom
		{
			get {return m_BorderBottom;}
			set
			{
				if(m_BorderBottom!=value)
				{
					m_BorderBottom=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border type for left side of the element.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eStyleBorderType.None), Browsable(true), Category("Border"), Description("Indicates the border type for left side of the element.")]
		public eStyleBorderType BorderLeft
		{
			get {return m_BorderLeft;}
			set
			{
				if(m_BorderLeft!=value)
				{
					m_BorderLeft=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border type for right side of the element.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eStyleBorderType.None), Browsable(true), Category("Border"), Description("Indicates the border type for right side of the element.")]
		public eStyleBorderType BorderRight
		{
			get {return m_BorderRight;}
			set
			{
				if(m_BorderRight!=value)
				{
					m_BorderRight=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets border width in pixels.
		/// </summary>
        [DevCoSerialize(), Browsable(true), DefaultValue(0), Category("Border"), Description("Indicates border width in pixels.")]
		public int BorderTopWidth
		{
			get {return m_BorderTopWidth;}
			set
			{
				if(m_BorderTopWidth!=value)
				{
					m_BorderTopWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets border width in pixels.
		/// </summary>
        [DevCoSerialize(), Browsable(true), DefaultValue(0), Category("Border"), Description("Indicates border width in pixels.")]
		public int BorderBottomWidth
		{
			get {return m_BorderBottomWidth;}
			set
			{
				if(m_BorderBottomWidth!=value)
				{
					m_BorderBottomWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets border width in pixels.
		/// </summary>
        [DevCoSerialize(), Browsable(true), DefaultValue(0), Category("Border"), Description("Indicates border width in pixels.")]
		public int BorderLeftWidth
		{
			get {return m_BorderLeftWidth;}
			set
			{
				if(m_BorderLeftWidth!=value)
				{
					m_BorderLeftWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets border width in pixels.
		/// </summary>
        [DevCoSerialize(), Browsable(true), DefaultValue(0), Category("Border"), Description("Indicates border width in pixels.")]
		public int BorderRightWidth
		{
			get {return m_BorderRightWidth;}
			set
			{
				if(m_BorderRightWidth!=value)
				{
					m_BorderRightWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border color for all sides. Specifing the color for the side will override this value.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates border color for all sides. Specifing the color for the side will override this value."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BorderColor
		{
			get {return GetColor(m_BorderColor,m_BorderColorSchemePart);}
			set
			{
				if(value!=this.BorderColor && (m_BorderColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BorderColorSchemePart=eColorSchemePart.None;
				m_BorderColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether BorderColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderColor()
		{return (!m_BorderColor.IsEmpty && m_BorderColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets BorderColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderColor()
		{
			m_BorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as border color. Setting
		/// this property overrides the setting of the corresponding BorderColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BorderColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart BorderColorSchemePart
		{
			get {return m_BorderColorSchemePart;}
			set
			{
				m_BorderColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

        /// <summary>
        /// Gets or sets the target background gradient color for border on all sides. Specifing the color for the side will override this value. Gradient border colors
        /// be employed only when per side border color is not specified.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates target background gradient color for border on all sides. "), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
        public Color BorderColor2
        {
            get { return GetColor(m_BorderColor2, m_BorderColor2SchemePart); }
            set
            {
                if (value != this.BorderColor2 && (m_BorderColor2SchemePart != eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
                    this.BorderColor2SchemePart = eColorSchemePart.None;
                m_BorderColor2 = value;
                this.OnStyleChanged();
            }
        }
        /// <summary>
        /// Indicates whether BorderColor3 should be serialized. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColor2()
        { return (!m_BorderColor2.IsEmpty && m_BorderColor2SchemePart == eColorSchemePart.None); }
        /// <summary>
        /// Resets BorderColor to it's default value. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void ResetBorderColor2()
        {
            m_BorderColor2 = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color scheme color that is used as taget gradient border color. Setting
        /// this property overrides the setting of the corresponding BorderColor property.
        /// Color scheme colors are automatically managed and are based on current system colors.
        /// That means if colors on the system change the color scheme will ensure that it's colors
        /// are changed as well to fit in the color scheme of target system. Set this property to
        /// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
        /// specify explicit color to use through BorderColor property.
        /// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
        public eColorSchemePart BorderColor2SchemePart
        {
            get { return m_BorderColor2SchemePart; }
            set
            {
                m_BorderColor2SchemePart = value;
                this.OnStyleChanged();
            }
        }

        /// <summary>
        /// Gets or sets the border gradient angle. Default value is 90.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(90), Description("Gets or sets the border gradient angle.")]
        public int BorderGradientAngle
        {
            get
            {
                return m_BorderGradientAngle;
            }
            set
            {
                if (m_BorderGradientAngle != value)
                {
                    m_BorderGradientAngle = value;
                    this.OnStyleChanged();

                }
            }
        }

        /// <summary>
        /// Gets or sets the color for light border part when etched border is used.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates color for light border part when etched border is used."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
        public Color BorderColorLight
        {
            get { return GetColor(m_BorderColorLight, m_BorderColorLightSchemePart); }
            set
            {
                if (value != this.BorderColorLight && (m_BorderColorLightSchemePart != eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
                    this.BorderColorLightSchemePart = eColorSchemePart.None;
                m_BorderColorLight = value;
                this.OnStyleChanged();
            }
        }
        /// <summary>
        /// Indicates whether BorderColorLight should be serialized. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColorLight()
        { return (!m_BorderColorLight.IsEmpty && m_BorderColorLightSchemePart == eColorSchemePart.None); }
        /// <summary>
        /// Resets BorderColor to it's default value. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void ResetBorderColorLight()
        {
            m_BorderColorLight = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color scheme color that is used as border light color for etched border style. Setting
        /// this property overrides the setting of the corresponding BorderColor property.
        /// Color scheme colors are automatically managed and are based on current system colors.
        /// That means if colors on the system change the color scheme will ensure that it's colors
        /// are changed as well to fit in the color scheme of target system. Set this property to
        /// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
        /// specify explicit color to use through BorderColor property.
        /// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
        public eColorSchemePart BorderColorLightSchemePart
        {
            get { return m_BorderColorLightSchemePart; }
            set
            {
                m_BorderColorLightSchemePart = value;
                this.OnStyleChanged();
            }
        }

        /// <summary>
        /// Gets or sets the target background gradient color for border on all sides. Specifing the color for the side will override this value. Gradient border colors
        /// be employed only when per side border color is not specified.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates target background gradient color for border on all sides. "), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
        public Color BorderColorLight2
        {
            get { return GetColor(m_BorderColorLight2, m_BorderColorLight2SchemePart); }
            set
            {
                if (value != this.BorderColorLight2 && (m_BorderColorLight2SchemePart != eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
                    this.BorderColorLight2SchemePart = eColorSchemePart.None;
                m_BorderColorLight2 = value;
                this.OnStyleChanged();
            }
        }
        /// <summary>
        /// Indicates whether BorderColor2 should be serialized. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColorLight2()
        { return (!m_BorderColorLight2.IsEmpty && m_BorderColorLight2SchemePart == eColorSchemePart.None); }
        /// <summary>
        /// Resets BorderColorLight2 to it's default value. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void ResetBorderColorLight2()
        {
            m_BorderColorLight2 = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color scheme color that is used as taget gradient border light color for etched border style. Setting
        /// this property overrides the setting of the corresponding BorderColor property.
        /// Color scheme colors are automatically managed and are based on current system colors.
        /// That means if colors on the system change the color scheme will ensure that it's colors
        /// are changed as well to fit in the color scheme of target system. Set this property to
        /// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
        /// specify explicit color to use through BorderColor property.
        /// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
        public eColorSchemePart BorderColorLight2SchemePart
        {
            get { return m_BorderColorLight2SchemePart; }
            set
            {
                m_BorderColorLight2SchemePart = value;
                this.OnStyleChanged();
            }
        }

        /// <summary>
        /// Gets or sets the light border gradient angle. Default value is 90.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Background"), DefaultValue(90), Description("Gets or sets the border gradient angle.")]
        public int BorderLightGradientAngle
        {
            get
            {
                return m_BorderLightGradientAngle;
            }
            set
            {
                if (m_BorderLightGradientAngle != value)
                {
                    m_BorderLightGradientAngle = value;
                    this.OnStyleChanged();

                }
            }
        }

		/// <summary>
		/// Gets or sets the background color for the left side border.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates background color for the left side border."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BorderLeftColor
		{
			get {return GetColor(m_BorderLeftColor,m_BorderLeftColorSchemePart);}
			set
			{
				if(value!=this.BorderLeftColor && (m_BorderLeftColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BorderLeftColorSchemePart=eColorSchemePart.None;
				m_BorderLeftColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether property should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderLeftColor()
		{return (!m_BorderLeftColor.IsEmpty && m_BorderLeftColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets property to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderLeftColor()
		{
			m_BorderLeftColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as left border color. Setting
		/// this property overrides the setting of the corresponding BorderLeftColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BorderLeftColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart BorderLeftColorSchemePart
		{
			get {return m_BorderLeftColorSchemePart;}
			set
			{
				m_BorderLeftColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background color for the right side border.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates background color for the right side border."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BorderRightColor
		{
			get {return GetColor(m_BorderRightColor,m_BorderRightColorSchemePart);}
			set
			{
				if(value!=this.BorderRightColor && (m_BorderRightColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BorderRightColorSchemePart=eColorSchemePart.None;
				m_BorderRightColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether property should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderRightColor()
		{return (!m_BorderRightColor.IsEmpty && m_BorderRightColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets property to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderRightColor()
		{
			m_BorderRightColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as right border color. Setting
		/// this property overrides the setting of the corresponding BorderRightColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BorderRightColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart BorderRightColorSchemePart
		{
			get {return m_BorderRightColorSchemePart;}
			set
			{
				m_BorderRightColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background color for the top side border.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates background color for the top side border."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BorderTopColor
		{
			get {return GetColor(m_BorderTopColor,m_BorderTopColorSchemePart);}
			set
			{
				if(value!=this.BorderTopColor && (m_BorderTopColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BorderTopColorSchemePart=eColorSchemePart.None;
				m_BorderTopColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether property should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderTopColor()
		{return (!m_BorderTopColor.IsEmpty && m_BorderTopColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets property to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderTopColor()
		{
			m_BorderTopColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as top border color. Setting
		/// this property overrides the setting of the corresponding BorderTopColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BorderTopColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart BorderTopColorSchemePart
		{
			get {return m_BorderTopColorSchemePart;}
			set
			{
				m_BorderTopColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background color for the bottom side border.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates background color for the bottom side border."), TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
		public Color BorderBottomColor
		{
			get {return GetColor(m_BorderBottomColor,m_BorderBottomColorSchemePart);}
			set
			{
				if(value!=this.BorderBottomColor && (m_BorderBottomColorSchemePart!=eColorSchemePart.None && !value.IsEmpty || value.IsEmpty))
					this.BorderBottomColorSchemePart=eColorSchemePart.None;
				m_BorderBottomColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Indicates whether property should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderBottomColor()
		{return (!m_BorderBottomColor.IsEmpty && m_BorderBottomColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets property to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderBottomColor()
		{
			m_BorderBottomColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as bottom border color. Setting
		/// this property overrides the setting of the corresponding BorderBottomColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through BorderBottomColor property.
		/// </summary>
        [DevCoSerialize(), Browsable(false), DefaultValue(eColorSchemePart.None)]
		public eColorSchemePart BorderBottomColorSchemePart
		{
			get {return m_BorderBottomColorSchemePart;}
			set
			{
				m_BorderBottomColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

        /// <summary>
        /// Gets the total horizontal padding (Left + Right)
        /// </summary>
        [Browsable(false)]
        public int PaddingHorizontal
        {
            get { return m_PaddingLeft + m_PaddingRight; }
        }

        /// <summary>
        /// Gets the total vertical padding (Top + Bottom)
        /// </summary>
        [Browsable(false)]
        public int PaddingVertical
        {
            get { return m_PaddingTop + m_PaddingBottom; }
        }

        /// <summary>
        /// Gets or sets the padding space in pixels for all 4 sides of the box.
        /// </summary>
        [Browsable(true), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("PaddingTop"), Description("Indicates padding space in pixels for all 4 sides of the box.")]
        public int Padding
        {
            get { return m_PaddingTop; }
            set
            {
                if (this.DesignMode)
                {
                    TypeDescriptor.GetProperties(this)["PaddingLeft"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["PaddingRight"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["PaddingTop"].SetValue(this, value);
                    TypeDescriptor.GetProperties(this)["PaddingBottom"].SetValue(this, value);
                }
                else
                {
                    this.PaddingLeft = value;
                    this.PaddingRight = value;
                    this.PaddingTop = value;
                    this.PaddingBottom = value;
                }
                this.OnStyleChanged();
                this.OnSizePropertyChanged();
            }
        }

		/// <summary>
		/// Gets or sets the amount of space to insert between the top border of the element and the content. 
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Padding"), DefaultValue(0), Description("Indicates the amount of space to insert between the top border of the element and the content.")]
		public int PaddingTop
		{
			get {return m_PaddingTop;}
			set 
			{
				m_PaddingTop=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the amount of space to insert between the bottom border of the element and the content. 
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Padding"), DefaultValue(0), Description("Indicates the amount of space to insert between the bottom border of the element and the content.")]
		public int PaddingBottom
		{
			get {return m_PaddingBottom;}
			set 
			{
				m_PaddingBottom=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the amount of space to insert between the left border of the element and the content. 
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Padding"), DefaultValue(0), Description("Indicates the amount of space to insert between the left border of the element and the content.")]
		public int PaddingLeft
		{
			get {return m_PaddingLeft;}
			set 
			{
				m_PaddingLeft=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the amount of space to insert between the right border of the element and the content. 
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Padding"), DefaultValue(0), Description("Indicates the amount of space to insert between the right border of the element and the content.")]
		public int PaddingRight
		{
			get {return m_PaddingRight;}
			set 
			{
				m_PaddingRight=value;
				this.OnStyleChanged();
				this.OnSizePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the name of the style.
		/// </summary>
        [DevCoSerialize(), Browsable(true), DefaultValue(""), Category("Design"), Description("Indicates name of the style.")]
		public string Name
		{
			get
			{
				if(m_ComponentSite!=null)
					m_Name=m_ComponentSite.Name;
				return m_Name;
			}
			set
			{
				if(m_ComponentSite!=null)
					m_ComponentSite.Name=value;
				if(value==null)
					m_Name="";
				else
					m_Name=value;
			}
		}

        /// <summary>
        /// Gets or sets the class style belongs to. The Class styles are used to apply predefined values to the styles that belong to the same class.
        /// This feature is used to manage color schemes/tables per class style.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Design"), Description("Indicates the class style belongs to.")]
#if DOTNETBAR
        [Editor("DevComponents.DotNetBar.Design.ElementStyleClassTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
#endif
        public string Class
        {
            get { return m_Class; }
            set
            {
                if (value == null)
                    throw new NullReferenceException("null is not valid value for this property");

                if (m_Class != value)
                {
                    m_Class = value;
                    OnStyleChanged();
                }
            }
        }
		
		/// <summary>
		/// Gets or sets the description of the style.
		/// </summary>
		[DevCoSerialize(), Browsable(true), DefaultValue(""), Category("Design"), Description("Indicates description of the style.")]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description=value;
			}
		}

		/// <summary>
		/// Gets or sets the Site associated with this component. Used by Windows forms designer.
		/// </summary>
		[System.ComponentModel.Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual System.ComponentModel.ISite Site
		{
			get
			{
				return m_ComponentSite;
			}
			set
			{
				m_ComponentSite = value;
			}
		}


		/// <summary>
		/// Gets or sets the maximum height of the element. This property should be used in
		/// conjunction with the <see cref="WordWrap">WordWrap</see> property to limit the size of
		/// text bounding box.
		/// </summary>
		/// <remarks>Default value is 0 which indicates that height of the style is unlimited.</remarks>
        [DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(0), Description("Indicates the maximum height of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
		public int MaximumHeight
		{
			get {return m_MaximumHeight;}
			set
			{
				if(m_MaximumHeight!=value)
				{
					m_MaximumHeight=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
        }

#if AdvTree
		/// <summary>
		/// Gets or sets the maximum width of the element. This property should be used in
		/// conjunction with the <see cref="WordWrap">WordWrap</see> property to limit the size of
		/// text bounding box.
		/// </summary>
		/// <remarks>Default value is 0 which indicates that width of the style is not limited.</remarks>
		[DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(0), Description("Indicates the maximum width of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
		public int MaximumWidth
		{
			get {return m_MaximumWidth;}
			set
			{
				if(m_MaximumWidth!=value)
				{
					m_MaximumWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}
#endif

        /// <summary>
		/// Returns System.Drawing.StringFormat constructed from current style settings.
		/// </summary>
		internal System.Drawing.StringFormat StringFormat
		{
			get
			{
				System.Drawing.StringFormat format=new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericDefault);
				format.Alignment=(StringAlignment)m_TextAlignment;
				format.LineAlignment=(StringAlignment)m_TextLineAlignment;
				format.Trimming=(StringTrimming)m_TextTrimming;
				if(m_WordWrap)
					format.FormatFlags=format.FormatFlags & ~(format.FormatFlags & StringFormatFlags.NoWrap);
				else
					format.FormatFlags=format.FormatFlags | StringFormatFlags.NoWrap;
				return format;
			}
		}

        private eOrientation m_TextOrientation = eOrientation.Horizontal;
        [DefaultValue(eOrientation.Horizontal)]
        internal eOrientation TextOrientation
        {
            get { return m_TextOrientation; }
            set
            {
                if (m_TextOrientation != value)
                {
                    m_TextOrientation = value;
                    this.OnStyleChanged();
                }
            }
        }

        private bool _UseMnemonic = false;
        /// <summary>
        /// Gets or sets a value indicating whether the control interprets an ampersand character (&amp;) in the control's Text property to be an access key prefix character. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether the control interprets an ampersand character (&amp;) in the control's Text property to be an access key prefix character.")]
        public bool UseMnemonic
        {
            get { return _UseMnemonic; }
            set
            {
                _UseMnemonic = value;
                this.OnStyleChanged();
            }
        }

        private bool _HideMnemonic = false;
        /// <summary>
        /// Indicates whether control hides the underlines of the letter prefixed by ampersand character when UseMnemonic=true
        /// </summary>
        [DefaultValue(false),Category("Appearance"), Description("Indicates whether control hides the underline of the letter prefixed by ampersand character when UseMnemonic=true")]
        public bool HideMnemonic
        {
            get { return _HideMnemonic; }
            set
            {
                _HideMnemonic = value;
            }
        }
        
        

        /// <summary>
        /// Returns eTextFormat constructed from current style settings.
        /// </summary>
        internal eTextFormat TextFormat
        {
            get
            {
                eTextFormat format = eTextFormat.Default;
                if (m_TextAlignment == eStyleTextAlignment.Center)
                    format |= eTextFormat.HorizontalCenter;
                else if (m_TextAlignment == eStyleTextAlignment.Far)
                    format |= eTextFormat.Right;

                if (m_TextLineAlignment == eStyleTextAlignment.Center)
                    format |= eTextFormat.VerticalCenter;
                else if (m_TextLineAlignment == eStyleTextAlignment.Far)
                    format |= eTextFormat.Bottom;

                if (m_WordWrap)
                    format |= eTextFormat.WordBreak;
                else
                    format |= eTextFormat.SingleLine;

                if (m_TextTrimming != eStyleTextTrimming.None)
                    format |= eTextFormat.EndEllipsis;
                if (_HideMnemonic)
                    format |= eTextFormat.HidePrefix;
                if(!_UseMnemonic && !_HideMnemonic)
                    format |= eTextFormat.NoPrefix;

                if (m_TextOrientation == eOrientation.Vertical)
                    format |= eTextFormat.Vertical;

                return format;
            }
        }

		/// <summary>
		/// Gets or sets the border corner type for all 4 sides. Default corner type is Square.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), Description("Indicates border corner type.")]
		public eCornerType CornerType
		{
			get {return m_CornerType;}
			set
			{
				if(m_CornerType!=value)
				{
					m_CornerType=value;
					if(m_CornerType==eCornerType.Inherit)
						m_CornerType=eCornerType.Square;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border corner type for top left corner. Default value is Inherit which means that setting from CornerType property is used.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eCornerType.Inherit), Browsable(true), Category("Border"), Description("Indicates top left border corner type.")]
		public eCornerType CornerTypeTopLeft
		{
			get {return m_CornerTypeTopLeft;}
			set
			{
				if(m_CornerTypeTopLeft!=value)
				{
					m_CornerTypeTopLeft=value;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border corner type for top right corner. Default value is Inherit which means that setting from CornerType property is used.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eCornerType.Inherit), Browsable(true), Category("Border"), Description("Indicates top right border corner type.")]
		public eCornerType CornerTypeTopRight
		{
			get {return m_CornerTypeTopRight;}
			set
			{
				if(m_CornerTypeTopRight!=value)
				{
					m_CornerTypeTopRight=value;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border corner type for bottom left corner. Default value is Inherit which means that setting from CornerType property is used.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eCornerType.Inherit), Browsable(true), Category("Border"), Description("Indicates bottom left border corner type.")]
		public eCornerType CornerTypeBottomLeft
		{
			get {return m_CornerTypeBottomLeft;}
			set
			{
				if(m_CornerTypeBottomLeft!=value)
				{
					m_CornerTypeBottomLeft=value;
					this.OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border corner type for bottom right corner. Default value is Inherit which means that setting from CornerType property is used.
		/// </summary>
        [DevCoSerialize(), DefaultValue(eCornerType.Inherit), Browsable(true), Category("Border"), Description("Indicates bottom right border corner type.")]
		public eCornerType CornerTypeBottomRight
		{
			get {return m_CornerTypeBottomRight;}
			set
			{
				if(m_CornerTypeBottomRight!=value)
				{
					m_CornerTypeBottomRight=value;
					this.OnStyleChanged();
				}
			}
		}


		/// <summary>
		/// Gets or sets the diameter in pixels of the corner type rounded or diagonal.
		/// </summary>
        [DevCoSerialize(), Browsable(true), Category("Border"), DefaultValue(DEFAULT_CORNER_DIAMETER), Description("Indicates diameter in pixels of the corner type rounded or diagonal.")]
		public int CornerDiameter
		{
			get {return m_CornerDiameter;}
			set
			{
				if(m_CornerDiameter!=value)
				{
					m_CornerDiameter=value;
					OnStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets whether to paint left border for the style.
		/// </summary>
		internal bool PaintLeftBorder
		{
			get
			{
				if(this.BorderLeft!=eStyleBorderType.None && this.BorderLeftWidth>0 &&
                    (!this.BorderColor.IsEmpty || !this.BorderLeftColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderLeftColorSchemePart != eColorSchemePart.None))
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets whether to paint right border for the style.
		/// </summary>
		internal bool PaintRightBorder
		{
			get
			{
                if (this.BorderRight != eStyleBorderType.None && this.BorderRightWidth > 0 &&
                    (!this.BorderColor.IsEmpty || !this.BorderRightColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderRightColorSchemePart != eColorSchemePart.None))
                    return true;
				return false;
			}
		}

		/// <summary>
		/// Gets whether to paint top border for the style.
		/// </summary>
		internal bool PaintTopBorder
		{
			get
			{
				if(this.BorderTop!=eStyleBorderType.None && this.BorderTopWidth>0 &&
                    (!this.BorderColor.IsEmpty || !this.BorderTopColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderTopColorSchemePart != eColorSchemePart.None))
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets whether to paint bottom border for the style.
		/// </summary>
		internal bool PaintBottomBorder
		{
			get
			{
				if(this.BorderBottom!=eStyleBorderType.None && this.BorderBottomWidth>0 &&
                    (!this.BorderColor.IsEmpty || !this.BorderBottomColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderBottomColorSchemePart != eColorSchemePart.None))
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets whether to paint any border for the style.
		/// </summary>
		internal bool PaintBorder
		{
			get
			{
				return PaintTopBorder | PaintBottomBorder | PaintLeftBorder | PaintRightBorder;
			}
		}

        /// <summary>
        /// Gets whether to paint any border for the style.
        /// </summary>
        internal bool PaintAllBorders
        {
            get
            {
                return PaintTopBorder && PaintBottomBorder && PaintLeftBorder && PaintRightBorder;
            }
        }
		#endregion

		#region Methods

		/// <summary>
		/// Releases all resources used in this control. After calling Dispose()
		/// object is not in valid state and cannot be recovered to the valid state.
		/// Recreation of the object is required.
		/// </summary>
		public virtual void Dispose()
		{
			if(Disposed != null)
				Disposed(this,EventArgs.Empty);
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_BackgroundImage);
            }
		}

        /// <summary>
        /// Applies a "child/inherited" style text and box properties but not any background or border properties to this style. 
        /// Use this method to create style hierarchies.
        /// </summary>
        /// <param name="style">Style to apply to this style</param>
        /// <seealso cref="ApplyStyle"/>
        public void ApplyFontStyle(ElementStyle style)
        {
            // TODO: Verify that ALL properties are applied!
            if (style.Font != null)
                this.Font = style.Font;
            if (style.MarginBottom != 0)
                this.MarginBottom = style.MarginBottom;
            if (style.MarginLeft != 0)
                this.MarginLeft = style.MarginLeft;
            if (style.MarginRight != 0)
                this.MarginRight = style.MarginRight;
            if (style.MarginTop != 0)
                this.MarginTop = style.MarginTop;
            if (style.MaximumHeight != 0)
                this.MaximumHeight = style.MaximumHeight;
            if (style.MaximumWidth != 0)
                this.MaximumWidth = style.MaximumWidth;
            if (style.PaddingBottom != 0)
                this.PaddingBottom = style.PaddingBottom;
            if (style.PaddingLeft != 0)
                this.PaddingLeft = style.PaddingLeft;
            if (style.PaddingRight != 0)
                this.PaddingRight = style.PaddingRight;
            if (style.PaddingTop != 0)
                this.PaddingTop = style.PaddingTop;
            if (style.TextAlignment != this.TextAlignment)
                this.TextAlignment = style.TextAlignment;
            if (!style.TextColor.IsEmpty)
                this.TextColor = style.TextColor;
            else if (style.TextColorSchemePart != eColorSchemePart.None)
                this.TextColorSchemePart = style.TextColorSchemePart;
            if (style.TextLineAlignment != this.TextLineAlignment)
                this.TextLineAlignment = style.TextLineAlignment;
            if (style.TextTrimming == eStyleTextTrimming.EllipsisCharacter)
                this.TextTrimming = style.TextTrimming;
            if (style.WordWrap != false)
                this.WordWrap = style.WordWrap;
            if (!style.TextShadowColor.IsEmpty)
                this.TextShadowColor = style.TextShadowColor;
            if (!style.TextShadowOffset.IsEmpty)
                this.TextShadowOffset = style.TextShadowOffset;
        }

		/// <summary>
		/// Applies a "child/inherited" style to this style. Use this method to create style
		/// hierarchies.
		/// </summary>
		/// <remarks>
		/// This method is used to support style hierarchies where a base style is defined
		/// and inherited/child styles are derived and based on it. By using this method on the
		/// base style you can apply only style changes defined by the child style. For example if
		/// you defined a base style for normal user interface element then in most cases you do
		/// not want to redefine the styling for the case when same user interface element is
		/// selected. You will just defined the behavior of the selected state and then apply it to
		/// the base normal style using ApplyStyle method.
		/// </remarks>
		/// <param name="style">Style to apply to current style.</param>
		public virtual void ApplyStyle(ElementStyle style)
		{
			// TODO: Verify that ALL properties are applied!
            if (!style.BackColor.IsEmpty)
                this.BackColor = style.BackColor;
            else if (style.BackColorSchemePart != eColorSchemePart.None)
                this.BackColorSchemePart = style.BackColorSchemePart;
			if(!style.BackColor2.IsEmpty)
				this.BackColor2=style.BackColor2;
            else if (style.BackColor2SchemePart != eColorSchemePart.None)
                this.BackColor2SchemePart = style.BackColor2SchemePart;
			if(style.BackColorGradientAngle!=0)
				this.BackColorGradientAngle=style.BackColorGradientAngle;
            this.BackColorGradientType=style.BackColorGradientType;
			if(style.BackgroundImage!=null)
				this.BackgroundImage=style.BackgroundImage;
			if(style.BackgroundImageAlpha!=255)
				this.BackgroundImageAlpha=style.BackgroundImageAlpha;
			if(style.BackgroundImagePosition!=eStyleBackgroundImage.Stretch)
				this.BackgroundImagePosition=style.BackgroundImagePosition;
			if(style.BorderBottom!=eStyleBorderType.None)
				this.BorderBottom=style.BorderBottom;
			if(!style.BorderBottomColor.IsEmpty)
				this.BorderBottomColor=style.BorderBottomColor;
            else if (style.BorderBottomColorSchemePart != eColorSchemePart.None)
                this.BorderBottomColorSchemePart = style.BorderBottomColorSchemePart;
			if(style.BorderBottomWidth!=0)
				this.BorderBottomWidth=style.BorderBottomWidth;
			
            if(!style.BorderColor.IsEmpty)
				this.BorderColor=style.BorderColor;
            else if (style.BorderColorSchemePart != eColorSchemePart.None)
                this.BorderColorSchemePart = style.BorderColorSchemePart;
            if (!style.BorderColor2.IsEmpty || !style.BorderColor.IsEmpty)
                this.BorderColor2 = style.BorderColor2;
            else if (style.BorderColor2SchemePart != eColorSchemePart.None)
                this.BorderColor2SchemePart = style.BorderColor2SchemePart;
            if (style.BorderGradientAngle != 90)
                this.BorderGradientAngle = style.BorderGradientAngle;

            if (!style.BorderColorLight.IsEmpty)
                this.BorderColorLight = style.BorderColorLight;
            else if (style.BorderColorLightSchemePart != eColorSchemePart.None)
                this.BorderColorLightSchemePart = style.BorderColorLightSchemePart;
            if (!style.BorderColorLight2.IsEmpty || !style.BorderColorLight.IsEmpty)
                this.BorderColorLight2 = style.BorderColorLight2;
            else if (style.BorderColorLight2SchemePart != eColorSchemePart.None)
                this.BorderColorLight2SchemePart = style.BorderColorLight2SchemePart;
            if (style.BorderLightGradientAngle != 90)
                this.BorderLightGradientAngle = style.BorderLightGradientAngle;

			if(style.BorderLeft!=eStyleBorderType.None)
				this.BorderLeft=style.BorderLeft;
			if(!style.BorderLeftColor.IsEmpty)
				this.BorderLeftColor=style.BorderLeftColor;
            else if (style.BorderLeftColorSchemePart != eColorSchemePart.None)
                this.BorderLeftColorSchemePart = style.BorderLeftColorSchemePart;
			if(style.BorderLeftWidth!=0)
				this.BorderLeftWidth=style.BorderLeftWidth;
			if(style.BorderTop!=eStyleBorderType.None)
				this.BorderTop=style.BorderTop;
            else if (style.BorderTopColorSchemePart != eColorSchemePart.None)
                this.BorderTopColorSchemePart = style.BorderTopColorSchemePart;
			if(!style.BorderTopColor.IsEmpty)
				this.BorderTopColor=style.BorderTopColor;
			if(style.BorderTopWidth!=0)
				this.BorderTopWidth=style.BorderTopWidth;
			if(style.BorderRight!=eStyleBorderType.None)
				this.BorderRight=style.BorderRight;
            else if (style.BorderRightColorSchemePart != eColorSchemePart.None)
                this.BorderRightColorSchemePart = style.BorderRightColorSchemePart;
			if(!style.BorderRightColor.IsEmpty)
				this.BorderRightColor=style.BorderRightColor;
			if(style.BorderRightWidth!=0)
				this.BorderRightWidth=style.BorderRightWidth;
			if(style.CornerDiameter!=DEFAULT_CORNER_DIAMETER)
				this.CornerDiameter=style.CornerDiameter;
			if(style.CornerType!=eCornerType.Square)
				this.CornerType=style.CornerType;
			if(style.CornerTypeTopLeft!=eCornerType.Inherit)
				this.CornerTypeTopLeft=style.CornerTypeTopLeft;
			if(style.CornerTypeTopRight!=eCornerType.Inherit)
				this.CornerTypeTopRight=style.CornerTypeTopRight;
			if(style.CornerTypeBottomLeft!=eCornerType.Inherit)
				this.CornerTypeBottomLeft=style.CornerTypeBottomLeft;
			if(style.CornerTypeBottomRight!=eCornerType.Inherit)
				this.CornerTypeBottomRight=style.CornerTypeBottomRight;
			if(style.Font!=null)
				this.Font=style.Font;
			if(style.MarginBottom!=0)
				this.MarginBottom=style.MarginBottom;
			if(style.MarginLeft!=0)
				this.MarginLeft=style.MarginLeft;
			if(style.MarginRight!=0)
				this.MarginRight=style.MarginRight;
			if(style.MarginTop!=0)
				this.MarginTop=style.MarginTop;
			if(style.MaximumHeight!=0)
				this.MaximumHeight=style.MaximumHeight;
            if(style.PaddingBottom!=0)
				this.PaddingBottom=style.PaddingBottom;
			if(style.PaddingLeft!=0)
				this.PaddingLeft=style.PaddingLeft;
			if(style.PaddingRight!=0)
				this.PaddingRight=style.PaddingRight;
			if(style.PaddingTop!=0)
				this.PaddingTop=style.PaddingTop;
			if(style.TextAlignment!=this.TextAlignment)
				this.TextAlignment=style.TextAlignment;
			if(!style.TextColor.IsEmpty)
				this.TextColor=style.TextColor;
            else if (style.TextColorSchemePart != eColorSchemePart.None)
                this.TextColorSchemePart = style.TextColorSchemePart;
//			if(style.TextFormat!="")
//				this.TextFormat=style.TextFormat;
            if (style.TextLineAlignment != this.TextLineAlignment)
                this.TextLineAlignment=style.TextLineAlignment;
			if(style.TextTrimming!=eStyleTextTrimming.EllipsisCharacter)
				this.TextTrimming=style.TextTrimming;
			if(style.WordWrap!=false)
				this.WordWrap=style.WordWrap;
            if (!style.TextShadowColor.IsEmpty)
                this.TextShadowColor = style.TextShadowColor;
            if (!style.TextShadowOffset.IsEmpty)
                this.TextShadowOffset = style.TextShadowOffset;
            if (style.UseMnemonic)
                this.UseMnemonic = style.UseMnemonic;
            if (style.HideMnemonic)
                this.HideMnemonic = style.HideMnemonic;

            if (style.BackColorBlend.Count > 0)
            {
                this.BackColorBlend.Clear();
                foreach (BackgroundColorBlend b in style.BackColorBlend)
                    this.BackColorBlend.Add(new BackgroundColorBlend(b.Color, b.Position));
            }
            else if (!style.BackColor.IsEmpty || !style.BackColor2.IsEmpty)
                this.BackColorBlend.Clear();
		}

        /// <summary>
        /// Gets whether custom has any of properties changed.
        /// </summary>
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Custom
        {
            get
            {
                // TODO: Verify that ALL properties are applied!
                if (!this.BackColor.IsEmpty)
                    return true;
                if (!this.BackColor2.IsEmpty)
                    return true;
                if (this.BackColorBlend.Count > 0)
                    return true;
                if (this.BackColorGradientAngle != 0)
                    return true;
                if (this.BackColorGradientType != eGradientType.Linear)
                    return true;
                if (this.BackgroundImage != null)
                    return true;
                if (this.BackgroundImageAlpha != 255)
                    return true;
                if (this.BackgroundImagePosition != eStyleBackgroundImage.Stretch)
                    return true;
                if (this.BorderBottom != eStyleBorderType.None)
                    return true;
                if (!this.BorderBottomColor.IsEmpty)
                    return true;
                if (this.BorderBottomWidth != 0)
                    return true;
                if (!this.BorderColor.IsEmpty)
                    return true;
                if (this.BorderLeft != eStyleBorderType.None)
                    return true;
                if (!this.BorderLeftColor.IsEmpty)
                    return true;
                if (this.BorderLeftWidth != 0)
                    return true;
                if (this.BorderTop != eStyleBorderType.None)
                    return true;
                if (!this.BorderTopColor.IsEmpty)
                    return true;
                if (this.BorderTopWidth != 0)
                    return true;
                if (this.BorderRight != eStyleBorderType.None)
                    return true;
                if (!this.BorderRightColor.IsEmpty)
                    return true;
                if (this.BorderRightWidth != 0)
                    return true;
                if (this.CornerDiameter != DEFAULT_CORNER_DIAMETER)
                    return true;
                if (this.CornerType != eCornerType.Square)
                    return true;
                if (this.CornerTypeTopLeft != eCornerType.Inherit)
                    return true;
                if (this.CornerTypeTopRight != eCornerType.Inherit)
                    return true;
                if (this.CornerTypeBottomLeft != eCornerType.Inherit)
                    return true;
                if (this.CornerTypeBottomRight != eCornerType.Inherit)
                    return true;
                if (this.Font != null)
                    return true;
                if (this.MarginBottom != 0)
                    return true;
                if (this.MarginLeft != 0)
                    return true;
                if (this.MarginRight != 0)
                    return true;
                if (this.MarginTop != 0)
                    return true;
                if (this.MaximumHeight != 0)
                    return true;
                if (this.PaddingBottom != 0)
                    return true;
                if (this.PaddingLeft != 0)
                    return true;
                if (this.PaddingRight != 0)
                    return true;
                if (this.PaddingTop != 0)
                    return true;
                if (this.TextAlignment != eStyleTextAlignment.Near)
                    return true;
                if (!this.TextColor.IsEmpty)
                    return true;
                if (this.TextLineAlignment != eStyleTextAlignment.Center)
                    return true;
                if (this.TextTrimming != eStyleTextTrimming.EllipsisCharacter)
                    return true;
                if (this.WordWrap != false)
                    return true;
                if (!this.TextShadowColor.IsEmpty)
                    return true;
                if (!this.TextShadowOffset.IsEmpty)
                    return true;
//#if AdvTree
				if (this.MaximumWidth!=0)
					return true;
//#endif
                return false;
            }
        }

		/// <summary>
		/// Makes an exact copy of the style.
		/// </summary>
		/// <returns>New copy of ElementStyle object.</returns>
		public ElementStyle Copy()
		{
			ElementStyle style=new ElementStyle();
			style.BackColor=this.BackColor;
			style.BackColorSchemePart=this.BackColorSchemePart;
			style.BackColor2=this.BackColor2;
			style.BackColor2SchemePart=this.BackColor2SchemePart;
			style.BackColorGradientAngle=this.BackColorGradientAngle;
            style.BackColorGradientType = this.BackColorGradientType;
            if (this.BackgroundImage != null)
                style.BackgroundImage = (Image) this.BackgroundImage.Clone();
			style.BackgroundImageAlpha=this.BackgroundImageAlpha;
			style.BackgroundImagePosition=this.BackgroundImagePosition;
			style.BorderColor=this.BorderColor;
			style.BorderColorSchemePart=this.BorderColorSchemePart;
            style.BorderColor2 = this.BorderColor2;
            style.BorderColor2SchemePart = this.BorderColor2SchemePart;
            style.BorderGradientAngle = this.BorderGradientAngle;

            style.BorderColorLight = this.BorderColorLight;
            style.BorderColorLightSchemePart = this.BorderColorLightSchemePart;
            style.BorderColorLight2 = this.BorderColorLight2;
            style.BorderColorLight2SchemePart = this.BorderColorLight2SchemePart;
            style.BorderLightGradientAngle = this.BorderLightGradientAngle;

			style.BorderBottom=this.BorderBottom;
			style.BorderBottomColor=this.BorderBottomColor;
			style.BorderBottomColorSchemePart=this.BorderBottomColorSchemePart;
			style.BorderBottomWidth=this.BorderBottomWidth;
			style.BorderLeft=this.BorderLeft;
			style.BorderLeftWidth=this.BorderLeftWidth;
			style.BorderLeftColor=this.BorderLeftColor;
			style.BorderLeftColorSchemePart=this.BorderLeftColorSchemePart;
			style.BorderRight=this.BorderRight;
			style.BorderRightWidth=this.BorderRightWidth;
			style.BorderRightColor=this.BorderRightColor;
			style.BorderRightColorSchemePart=this.BorderRightColorSchemePart;
			style.BorderTop=this.BorderTop;
			style.BorderTopWidth=this.BorderTopWidth;
			style.BorderBottomColor=this.BorderBottomColor;
			style.BorderBottomColorSchemePart=this.BorderBottomColorSchemePart;
			style.CornerDiameter=this.CornerDiameter;
			style.CornerType=this.CornerType;
			style.CornerTypeTopLeft=this.CornerTypeTopLeft;
			style.CornerTypeTopRight=this.CornerTypeTopRight;
			style.CornerTypeBottomLeft=this.CornerTypeBottomLeft;
			style.CornerTypeBottomRight=this.CornerTypeBottomRight;
			style.Font=this.Font;
			style.MarginBottom=this.MarginBottom;
			style.MarginLeft=this.MarginLeft;
			style.MarginRight=this.MarginRight;
			style.MarginTop=this.MarginTop;
			style.PaddingBottom=this.PaddingBottom;
			style.PaddingLeft=this.PaddingLeft;
			style.PaddingRight=this.PaddingRight;
			style.PaddingTop=this.PaddingTop;
			style.TextAlignment=this.TextAlignment;
			style.TextColor=this.TextColor;
			style.TextColorSchemePart=this.TextColorSchemePart;
			style.TextLineAlignment=this.TextLineAlignment;
			style.TextTrimming=this.TextTrimming;
			style.WordWrap=this.WordWrap;
			style.Description=this.Description;
            style.TextShadowColor = this.TextShadowColor;
            style.TextShadowColorSchemePart = this.TextShadowColorSchemePart;
            style.TextShadowOffset = this.TextShadowOffset;
            style.Class = m_Class;
            style.UseMnemonic = _UseMnemonic;
            style.HideMnemonic = _HideMnemonic;
//#if AdvTree
			style.MaximumWidth = this.MaximumWidth;
//#endif
            foreach (BackgroundColorBlend b in m_BackColorBlend)
                style.BackColorBlend.Add(new BackgroundColorBlend(b.Color, b.Position));

			return style;
		}

        /// <summary>
        /// Reset all style properties to default values.
        /// </summary>
        public void Reset()
        {
            m_BackColor=Color.Empty;
		    m_BackColorSchemePart=eColorSchemePart.None;
		    m_BackColor2=Color.Empty;
		    m_BackColor2SchemePart=eColorSchemePart.None;
		    m_BackColorGradientAngle=0;
            m_BackColorGradientType = eGradientType.Linear;
		    m_BackgroundImage=null;
		    m_BackgroundImagePosition=eStyleBackgroundImage.Stretch;
		    m_BackgroundImageAlpha=255;
            m_BackColorBlend.Clear();

		    // Font
		    m_Font=null;

		    // Text Formating
		    m_WordWrap=false;
		    m_TextAlignment=eStyleTextAlignment.Near;
		    m_TextLineAlignment=eStyleTextAlignment.Center;
		    m_TextTrimming=eStyleTextTrimming.EllipsisCharacter;
		
            m_TextColor=Color.Empty;
		    m_TextColorSchemePart=eColorSchemePart.None;
            m_TextShadowColor = Color.Empty;
            m_TextShadowColorSchemePart = eColorSchemePart.None;
            m_TextShadowOffset = Point.Empty;

		    // Style margins
		    m_MarginLeft=0;
		    m_MarginRight=0;
		    m_MarginTop=0;
		    m_MarginBottom=0;

		    // Style inside padding
		    m_PaddingLeft=0;
		    m_PaddingRight=0;
		    m_PaddingTop=0;
		    m_PaddingBottom=0;

		    // Border
		    m_BorderLeft=eStyleBorderType.None;
		    m_BorderRight=eStyleBorderType.None;
		    m_BorderTop=eStyleBorderType.None;
		    m_BorderBottom=eStyleBorderType.None;

		    m_BorderColor=Color.Empty;
		    m_BorderColorSchemePart=eColorSchemePart.None;
            m_BorderColor2 = Color.Empty;
            m_BorderColor2SchemePart = eColorSchemePart.None;
            m_BorderGradientAngle = 90;
            m_BorderColorLight = Color.Empty;
            m_BorderColorLightSchemePart = eColorSchemePart.None;
            m_BorderColorLight2 = Color.Empty;
            m_BorderColorLight2SchemePart = eColorSchemePart.None;
            m_BorderLightGradientAngle = 90;

		    m_BorderLeftColor=Color.Empty;
		    m_BorderLeftColorSchemePart=eColorSchemePart.None;
		    m_BorderRightColor=Color.Empty;
		    m_BorderRightColorSchemePart=eColorSchemePart.None;
		    m_BorderTopColor=Color.Empty;
		    m_BorderTopColorSchemePart=eColorSchemePart.None;
		    m_BorderBottomColor=Color.Empty;
		    m_BorderBottomColorSchemePart=eColorSchemePart.None;

		    m_BorderLeftWidth=0;
		    m_BorderRightWidth=0;
		    m_BorderTopWidth=0;
		    m_BorderBottomWidth=0;

		    m_CornerType=eCornerType.Square;
		    m_CornerTypeTopLeft=eCornerType.Inherit;
		    m_CornerTypeTopRight=eCornerType.Inherit;
		    m_CornerTypeBottomLeft=eCornerType.Inherit;
		    m_CornerTypeBottomRight=eCornerType.Inherit;
		    m_CornerDiameter=DEFAULT_CORNER_DIAMETER;
            m_Class = "";
		    m_MaximumHeight=0;
        }

		/// <summary>Returns default style for the Cell object.</summary>
		/// <param name="defaultNodeStyle">
		/// Reference to the default style for the Node. Cell style is based on the given
		/// node style.
		/// </param>
		public static ElementStyle GetDefaultCellStyle(ElementStyle defaultNodeStyle)
		{
			ElementStyle es=new ElementStyle();
			if(defaultNodeStyle!=null)
			{
				es.TextColor=defaultNodeStyle.TextColor;
				es.Font=defaultNodeStyle.Font;
				es.TextAlignment=defaultNodeStyle.TextAlignment;
//				es.TextFormat=defaultNodeStyle.TextFormat;
				es.TextLineAlignment=defaultNodeStyle.TextLineAlignment;
				es.TextTrimming=defaultNodeStyle.TextTrimming;
				es.WordWrap=defaultNodeStyle.WordWrap;
			}
			if(es.TextColor.IsEmpty)
				es.TextColor=SystemColors.ControlText;
			return es;
		}

		/// <summary>Returns default style for disabled cells.</summary>
		/// <returns>Returns new instance of ElementStyle object.</returns>
		public static ElementStyle GetDefaultDisabledCellStyle()
		{
			ElementStyle es=new ElementStyle();
			es.TextColor=SystemColors.GrayText;
			return es;
		}

		/// <summary>Returns default style for the selected cell object.</summary>
		/// <returns>New instance of the ElementStyle object.</returns>
		public static ElementStyle GetDefaultSelectedCellStyle()
		{
			ElementStyle es=new ElementStyle();
//			es.TextColor=SystemColors.HighlightText;
//			es.BackgroundColor=SystemColors.Highlight;
			return es;
		}

		/// <summary>
		/// Sets Alpha value for all colors defined by style to specified value.
		/// </summary>
		/// <param name="style">Style to change.</param>		
		/// <param name="alpha">Alpha value for the colors.</param>
		public static void SetColorsAlpha(ElementStyle style, int alpha)
		{
			if(!style.TextColor.IsEmpty)
				style.TextColor=Color.FromArgb(alpha,style.TextColor);
			if(!style.BackColor.IsEmpty)
				style.BackColor=Color.FromArgb(alpha,style.BackColor);
			if(!style.BackColor2.IsEmpty)
				style.BackColor2=Color.FromArgb(alpha,style.BackColor2);
			if(!style.BorderColor.IsEmpty)
				style.BorderColor=Color.FromArgb(alpha,style.BorderColor);
			if(!style.BorderBottomColor.IsEmpty)
				style.BorderBottomColor=Color.FromArgb(alpha,style.BorderBottomColor);
			if(!style.BorderLeftColor.IsEmpty)
				style.BorderLeftColor=Color.FromArgb(alpha,style.BorderLeftColor);
			if(!style.BorderRightColor.IsEmpty)
				style.BorderRightColor=Color.FromArgb(alpha,style.BorderRightColor);
			if(!style.BorderRightColor.IsEmpty)
				style.BorderRightColor=Color.FromArgb(alpha,style.BorderRightColor);
			if(!style.BorderTopColor.IsEmpty)
				style.BorderTopColor=Color.FromArgb(alpha,style.BorderTopColor);
		}
		

		#endregion

		#region Private Implementation
        internal bool FreezeEvents
        {
            get { return m_FreezeEvents; }
            set { m_FreezeEvents = value; }
        }

		private void OnStyleChanged()
		{
            if (m_FreezeEvents) return;
			if(StyleChanged!=null)
				StyleChanged(this,new EventArgs());
		}
		private void OnSizePropertyChanged()
		{
			m_SizeChanged=true;
		}

		/// <summary>
		/// Gets or sets whether ElementStyle is in design mode.
		/// </summary>
		internal bool DesignMode
		{
			get {return m_DesignMode;}
			set {m_DesignMode=value;}
		}

		private Color GetColor(Color color, eColorSchemePart p)
		{
			if(p==eColorSchemePart.None)
				return color;
			ColorScheme cs=this.GetColorScheme();
			if(cs==null)
				return color;
            switch (p)
            {
                case eColorSchemePart.PanelBackground:
                    return cs.PanelBackground;
                case eColorSchemePart.PanelBackground2:
                    return cs.PanelBackground2;
                case eColorSchemePart.PanelBorder:
                    return cs.PanelBorder;
                case eColorSchemePart.PanelText:
                    return cs.PanelText;
                case eColorSchemePart.BarBackground:
                    return cs.BarBackground;
                case eColorSchemePart.BarBackground2:
                    return cs.BarBackground2;
                case eColorSchemePart.BarCaptionBackground:
                    return cs.BarCaptionBackground;
                    case eColorSchemePart.BarCaptionBackground2:
                    return cs.BarCaptionBackground2;
                case eColorSchemePart.BarCaptionInactiveBackground:
                    return cs.BarCaptionInactiveBackground;
                case eColorSchemePart.BarCaptionInactiveBackground2:
                    return cs.BarCaptionInactiveBackground2;
                case eColorSchemePart.BarCaptionInactiveText:
                    return cs.BarCaptionInactiveText;
                case eColorSchemePart.BarCaptionText:
                    return cs.BarCaptionText;
                case eColorSchemePart.BarDockedBorder:
                    return cs.BarDockedBorder;
                case eColorSchemePart.BarFloatingBorder:
                    return cs.BarFloatingBorder;
                case eColorSchemePart.BarPopupBackground:
                    return cs.BarPopupBackground;
                case eColorSchemePart.BarPopupBorder:
                    return cs.BarPopupBorder;
                case eColorSchemePart.BarStripeColor:
                    return cs.BarStripeColor;
                case eColorSchemePart.CustomizeBackground:
                    return cs.CustomizeBackground;
                case eColorSchemePart.CustomizeBackground2:
                    return cs.CustomizeBackground2;
                case eColorSchemePart.CustomizeText:
                    return cs.CustomizeText;
                case eColorSchemePart.DockSiteBackColor:
                    return cs.DockSiteBackColor;
                case eColorSchemePart.DockSiteBackColor2:
                    return cs.DockSiteBackColor2;
                case eColorSchemePart.ExplorerBarBackground:
                    return cs.ExplorerBarBackground;
                case eColorSchemePart.ExplorerBarBackground2:
                    return cs.ExplorerBarBackground2;
                case eColorSchemePart.ItemBackground:
                    return cs.ItemBackground;
                case eColorSchemePart.ItemBackground2:
                    return cs.ItemBackground2;
                case eColorSchemePart.ItemCheckedBackground:
                    return cs.ItemCheckedBackground;
                case eColorSchemePart.ItemCheckedBackground2:
                    return cs.ItemCheckedBackground2;
                case eColorSchemePart.ItemCheckedBorder:
                    return cs.ItemCheckedBorder;
                case eColorSchemePart.ItemCheckedText:
                    return cs.ItemCheckedText;
                case eColorSchemePart.ItemDesignTimeBorder:
                    return cs.ItemDesignTimeBorder;
                case eColorSchemePart.ItemDisabledBackground:
                    return cs.ItemDisabledBackground;
                case eColorSchemePart.ItemDisabledText:
                    return cs.ItemDisabledText;
                case eColorSchemePart.ItemExpandedBackground:
                    return cs.ItemExpandedBackground;
                case eColorSchemePart.ItemExpandedBackground2:
                    return cs.ItemExpandedBackground2;
                case eColorSchemePart.ItemExpandedBorder:
                    return cs.ItemExpandedBorder;
                case eColorSchemePart.ItemExpandedShadow:
                    return cs.ItemExpandedShadow;
                case eColorSchemePart.ItemExpandedText:
                    return cs.ItemExpandedText;
                case eColorSchemePart.ItemHotBackground:
                    return cs.ItemHotBackground;
                case eColorSchemePart.ItemHotBackground2:
                    return cs.ItemHotBackground2;
                case eColorSchemePart.ItemHotBorder:
                    return cs.ItemHotBorder;
                case eColorSchemePart.ItemHotText:
                    return cs.ItemHotText;
                case eColorSchemePart.ItemPressedBackground:
                    return cs.ItemPressedBackground;
                case eColorSchemePart.ItemPressedBackground2:
                    return cs.ItemPressedBackground2;
                case eColorSchemePart.ItemPressedBorder:
                    return cs.ItemPressedBorder;
                case eColorSchemePart.ItemPressedText:
                    return cs.ItemPressedText;
                case eColorSchemePart.ItemSeparator:
                    return cs.ItemSeparator;
                case eColorSchemePart.ItemSeparatorShade:
                    return cs.ItemSeparatorShade;
                case eColorSchemePart.ItemText:
                    return cs.ItemText;
                case eColorSchemePart.MenuBackground:
                    return cs.MenuBackground;
                case eColorSchemePart.MenuBackground2:
                    return cs.MenuBackground2;
                case eColorSchemePart.MenuBarBackground:
                    return cs.MenuBarBackground;
                case eColorSchemePart.MenuBarBackground2:
                    return cs.MenuBarBackground2;
                case eColorSchemePart.MenuBorder:
                    return cs.MenuBorder;
                case eColorSchemePart.MenuSide:
                    return cs.MenuSide;
                case eColorSchemePart.MenuSide2:
                    return cs.MenuSide2;
                case eColorSchemePart.MenuUnusedBackground:
                    return cs.MenuUnusedBackground;
                case eColorSchemePart.MenuUnusedSide:
                    return cs.MenuUnusedSide;
                case eColorSchemePart.MenuUnusedSide2:
                    return cs.MenuUnusedSide2;
            }
			return (Color)cs.GetType().GetProperty(p.ToString()).GetValue(cs,null);
		}
		
		public override string ToString()
		{
			string s=this.Name;
			if(this.Description!="")
				s+=" "+this.Description;
			return s;
		}


		#if !NOTREE
        private int m_MaximumWidth=0;
		/// <summary>
		/// Gets or sets the maximum width of the element. This property should be used in
		/// conjunction with the <see cref="WordWrap">WordWrap</see> property to limit the size of
		/// text bounding box.
		/// </summary>
		/// <remarks>Default value is 0 which indicates that width of the style is not limited.</remarks>
		[DevCoSerialize(), Browsable(true), Category("Text Formatting"), DefaultValue(0), Description("Indicates the maximum width of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
		public int MaximumWidth
		{
			get {return m_MaximumWidth;}
			set
			{
				if(m_MaximumWidth!=value)
				{
					m_MaximumWidth=value;
					this.OnStyleChanged();
					this.OnSizePropertyChanged();
				}
			}
		}
        /// <summary>
		/// Returns reference to ColorScheme object used by this style.
		/// </summary>
		/// <returns>Instance of ColorScheme object or null if object could not be obtained.</returns>
		internal ColorScheme GetColorScheme()
		{
			if(m_Parent!=null && m_Parent.TreeControl!=null)
				return m_Parent.TreeControl.ColorScheme;
			else if(m_TreeControl!=null)
				return m_TreeControl.ColorScheme;
			return m_ColorScheme;
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetColorScheme(ColorScheme cs)
        {
            m_ColorScheme = cs;
        }

        private DevComponents.AdvTree.ElementStyleCollection m_Parent = null;
		/// <summary>
		/// Gets or sets the reference to the parent collection.
		/// </summary>
        internal DevComponents.AdvTree.ElementStyleCollection Parent
		{
			get {return m_Parent;}
			set {m_Parent=value;}
		}

		private DevComponents.AdvTree.AdvTree m_TreeControl;
		/// <summary>
		/// Gets or sets the tree control style is assigned to.
		/// </summary>
        internal DevComponents.AdvTree.AdvTree TreeControl
		{
			get {return m_TreeControl;}
			set {m_TreeControl=value;}
		}

		#else
		/// <summary>
		/// Returns reference to ColorScheme object used by this style.
		/// </summary>
		/// <returns>Instance of ColorScheme object or null if object could not be obtained.</returns>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public ColorScheme GetColorScheme()
		{
			return m_ColorScheme;
		}
		internal void SetColorScheme(ColorScheme cs)
		{
			m_ColorScheme=cs;
		}
		#endif

		#endregion
    }

    #region eGradientType
    /// <summary>
    /// Specifies the type of the gradient fill.
    /// </summary>
    public enum eGradientType
    {
        /// <summary>
        /// Represents linear gradient fill.
        /// </summary>
        Linear,
        /// <summary>
        /// Represents radial gradient fill.
        /// </summary>
        Radial
    }
    #endregion
}
