using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(false)]
    public class ItemStyleMapper
    {
        private ElementStyle m_ElementStyle=null;

        private ColorExMapper m_BackColor1=null;
        private ColorExMapper m_BackColor2=null;
        private ColorExMapper m_ForeColor=null;
        private ColorExMapper m_BorderColor=null;

        public ItemStyleMapper(ElementStyle style)
        {
            m_ElementStyle=style;
            m_BackColor1=new ColorExMapper("BackColor",style);
            m_BackColor2=new ColorExMapper("BackColor2",style);
            m_ForeColor=new ColorExMapper("TextColor",style);
            m_BorderColor=new ColorExMapper("BorderColor",style);
        }

        /// <summary>
        /// Gets or sets a background color or starting color for gradient background.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorExMapper BackColor1
        {
            get
            {
                return m_BackColor1;
            }
        }
        /// <summary>
        /// Gets or sets a background color or ending color for gradient background.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorExMapper BackColor2
        {
            get
            {
                return m_BackColor2;
            }
        }

        /// <summary>
        /// Gets or sets a text color.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorExMapper ForeColor
        {
            get
            {
                return m_ForeColor;
            }
        }

        /// <summary>
        /// Gets or sets the gradient angle.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int GradientAngle
        {
            get
            {
                return m_ElementStyle.BackColorGradientAngle;
            }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["BackColorGradientAngle"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the style Font
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Font Font
        {
            get
            {
                return m_ElementStyle.Font;
            }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["Font"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool WordWrap
        {
            get { return m_ElementStyle.WordWrap; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["WordWrap"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Specifies alignment of the text.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StringAlignment Alignment
        {
            get { return (StringAlignment)m_ElementStyle.TextAlignment; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["TextAlignment"].SetValue(m_ElementStyle,(eStyleTextAlignment)value);
            }
        }

        /// <summary>
        /// Specifies alignment of the text.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StringAlignment LineAlignment
        {
            get { return (StringAlignment)m_ElementStyle.TextLineAlignment; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["TextLineAlignment"].SetValue(m_ElementStyle,(eStyleTextAlignment)value);
            }
        }

        /// <summary>
        /// Specifies how to trim characters when text does not fit.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StringTrimming TextTrimming
        {
            get { return (StringTrimming)m_ElementStyle.TextTrimming; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["TextTrimming"].SetValue(m_ElementStyle,(eStyleTextTrimming)value);
            }
        }

        /// <summary>
        /// Specifies background image.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackgroundImage
        {
            get { return m_ElementStyle.BackgroundImage; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImage"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Specifies background image position when container is larger than image.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eBackgroundImagePosition BackgroundImagePosition
        {
            get { return (eBackgroundImagePosition)m_ElementStyle.BackgroundImagePosition; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImagePosition"].SetValue(m_ElementStyle,(eStyleBackgroundImage)value);
            }
        }

        /// <summary>
        /// Specifies the transparency of background image.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public byte BackgroundImageAlpha
        {
            get { return m_ElementStyle.BackgroundImageAlpha; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImageAlpha"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the corner type.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eCornerType CornerType
        {
            get { return m_ElementStyle.CornerType; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["CornerType"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the diameter in pixels of the corner type rounded or diagonal.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerDiameter
        {
            get { return m_ElementStyle.CornerDiameter; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["CornerDiameter"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the border type.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eBorderType Border
        {
            get { return eBorderType.None; }
            set
            {
                eStyleBorderType b=eStyleBorderType.None;
                if(value!=eBorderType.None)
                    b=eStyleBorderType.Solid;
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle,b);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle,b);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle,b);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle,b);
            }
        }

        /// <summary>
        /// Gets or sets dash style for single line border type.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashStyle BorderDashStyle
        {
            get { return DashStyle.Solid ; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the border sides that are displayed.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eBorderSide BorderSide
        {
            get { return eBorderSide.None; }
            set
            {
                if((value & eBorderSide.Left)==eBorderSide.Left)
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle,eStyleBorderType.Solid);
                else
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle,eStyleBorderType.None);

                if((value & eBorderSide.Right)==eBorderSide.Right)
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle,eStyleBorderType.Solid);
                else
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle,eStyleBorderType.None);

                if((value & eBorderSide.Top)==eBorderSide.Top)
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle,eStyleBorderType.Solid);
                else
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle,eStyleBorderType.None);

                if((value & eBorderSide.Bottom)==eBorderSide.Bottom)
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle,eStyleBorderType.Solid);
                else
                    TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle,eStyleBorderType.None);

            }
        }

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorExMapper BorderColor
        {
            get { return m_BorderColor; }
        }

        

        /// <summary>
        /// Gets or sets the line tickness of single line border.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderWidth
        {
            get { return m_ElementStyle.BorderLeftWidth; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeftWidth"].SetValue(m_ElementStyle,value);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderTopWidth"].SetValue(m_ElementStyle,value);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderRightWidth"].SetValue(m_ElementStyle,value);
                TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottomWidth"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the left text margin.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MarginLeft
        {
            get { return m_ElementStyle.MarginLeft; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["MarginLeft"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the right text margin.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MarginRight
        {
            get { return m_ElementStyle.MarginRight; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["MarginRight"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the top text margin.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MarginTop
        {
            get { return m_ElementStyle.MarginTop; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["MarginTop"].SetValue(m_ElementStyle,value);
            }
        }

        /// <summary>
        /// Gets or sets the bottom text margin.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MarginBottom
        {
            get { return m_ElementStyle.MarginBottom; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)["MarginBottom"].SetValue(m_ElementStyle,value);
            }
        }

    }

    /// <summary>
    /// ColorEx object that provides the transparency setting ability.
    /// </summary>
    [ToolboxItem(false)]
    public class ColorExMapper
    {
        private ElementStyle m_ElementStyle=null;
        private string m_ColorProperty="";
        public ColorExMapper(string colorProperty, ElementStyle style)
        {
            m_ColorProperty=colorProperty;
            m_ElementStyle=style;
        }

        /// <summary>
        /// Gets or sets the Color object which does not include transparency.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get { return (Color)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].GetValue(m_ElementStyle); }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].SetValue(m_ElementStyle, value);
            }
        }
        
        /// <summary>
        /// Indicates the transparency for the color.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public byte Alpha
        {
            get { return ((Color)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].GetValue(m_ElementStyle)).A; }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].SetValue(m_ElementStyle, Color.FromArgb(value,this.Color));
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
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eColorSchemePart ColorSchemePart
        {
            get { return (eColorSchemePart)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty+"SchemePart"].GetValue(m_ElementStyle); }
            set
            {
                TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty + "SchemePart"].SetValue(m_ElementStyle, value);
            }
        }
    }
}
