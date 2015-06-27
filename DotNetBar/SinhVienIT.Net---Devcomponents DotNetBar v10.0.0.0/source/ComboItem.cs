using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
	/// <summary>
	/// Summary description for ComboItem.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class ComboItem:Component
	{
		private string m_Text="";
		private int m_ImageIndex=-1;
		private Image m_Image;
		private StringFormat m_TextFormat;
		private HorizontalAlignment m_ImagePosition=HorizontalAlignment.Left;
		
		private string m_FontName="";
		private FontStyle m_FontStyle=FontStyle.Regular;
		private float m_FontSize=8;

		private Color m_ForeColor=Color.Empty;
		private Color m_BackColor=Color.Empty;

        private object m_Tag;

		internal ComboBoxEx m_ComboBox=null;
		internal bool IsFontItem=false;

		/// <summary>
		/// Creates new instance of ComboItem.
		/// </summary>
		public ComboItem()
		{
			m_TextFormat=DevComponents.DotNetBar.BarFunctions.CreateStringFormat(); //new StringFormat(StringFormat.GenericDefault);
			m_TextFormat.Alignment=StringAlignment.Near;
        }

        /// <summary>
        /// Initializes a new instance of the ComboItem class.
        /// </summary>
        /// <param name="text"></param>
        public ComboItem(string text) : this()
        {
            this.m_Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the ComboItem class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="foreColor"></param>
        public ComboItem(string text, Color foreColor) : this()
        {
            this.m_Text = text;
            this.m_ForeColor = foreColor;
        }

        /// <summary>
        /// Initializes a new instance of the ComboItem class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        public ComboItem(string text, Color foreColor, Color backColor)
            : this()
        {
            this.m_Text = text;
            this.m_ForeColor = foreColor;
            this.m_BackColor = backColor;
        }

        /// <summary>
        /// Initializes a new instance of the ComboItem class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public ComboItem(string text, Image image)
            : this()
        {
            this.m_Text = text;
            this.m_Image = image;
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
            }
            base.Dispose(disposing);
        }

        /// <summary>
		/// Gets or sets the text associated with this item.
		/// </summary>
		[DefaultValue(""),Browsable(true), Localizable(true)]
		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text=value;
			}
		}

		/// <summary>
		/// Gets or sets the index value of the image assigned to the item.
		/// </summary>
        [DefaultValue(-1), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)), Localizable(true)]
		public int ImageIndex
		{
			get
			{
				return m_ImageIndex;
			}
			set
			{
				m_ImageIndex=value;
			}
		}

		/// <summary>
		/// Gets or sets the text alignment..
		/// </summary>
		[DefaultValue(StringAlignment.Near)]
		public StringAlignment TextAlignment
		{
			get
			{
				return m_TextFormat.Alignment;
			}
			set
			{
				m_TextFormat.Alignment=value;
			}
		}

		/// <summary>
		/// Gets or sets the line alignment for the item.
		/// </summary>
		[DefaultValue(StringAlignment.Near)]
		public StringAlignment TextLineAlignment
		{
			get
			{
				return m_TextFormat.LineAlignment;
			}
			set
			{
				m_TextFormat.LineAlignment=value;
			}
		}

		/// <summary>
		/// Gets or sets the value that encapsulates text layout information (such as alignment, orientation, tab stops, and clipping) and display manipulations.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StringFormat TextFormat
		{
			get
			{
				return m_TextFormat;
			}
			set
			{
				m_TextFormat=value;
			}
		}

		/// <summary>
		/// Gets or sets the image horizontal image position.
		/// </summary>
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment ImagePosition
		{
			get
			{
				return m_ImagePosition;
			}
			set
			{
				m_ImagePosition=value;
			}
		}

		/// <summary>
		/// Gets or sets the font name used to draw the item text.
		/// </summary>
		[DefaultValue("")]
		public String FontName
		{
			get
			{
				return m_FontName;
			}
			set
			{
				m_FontName=value;
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return m_ForeColor;
			}
			set
			{
				m_ForeColor=value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeForeColor()
		{
			return !m_ForeColor.IsEmpty;
		}

		/// <summary>
		/// Gets or sets the background color of the item.
		/// </summary>
		public Color BackColor
		{
			get
			{
				return m_BackColor;
			}
			set
			{
				m_BackColor=value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor()
		{
			return !m_BackColor.IsEmpty;
		}

		/// <summary>
		/// Specifies style information applied to text.
		/// </summary>
		[DefaultValue(FontStyle.Regular)]
		public System.Drawing.FontStyle FontStyle
		{
			get
			{
				return m_FontStyle;
			}
			set
			{
				m_FontStyle=value;
			}
		}

		/// <summary>
		/// Gets the em-size of this Font object in design units.
		/// </summary>
		[DefaultValue(8f)]
		public float FontSize
		{
			get
			{
				return m_FontSize;
			}
			set
			{
				m_FontSize=value;
			}
		}

		/// <summary>
		/// Gets or sets the image assigned to this item.
        /// </summary>
        [DefaultValue(null), Localizable(true)]
		public System.Drawing.Image Image
		{
			get
			{
				return m_Image;
			}
			set
			{
				m_Image=value;
			}
		}

		/// <summary>
		/// Overridden. Returns a human-readable string representation of this object.
		/// </summary>
		/// <returns>A string that represents this object.</returns>
		public override string ToString()
		{
			return m_Text;
		}

		/// <summary>
		/// Gets or sets an object that contains data to associate with the item.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				if(m_ComboBox!=null)
				{
					return m_ComboBox.Images;
				}
				return null;
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public ComboBoxEx Parent
		{
			get
			{
				return m_ComboBox;
			}
		}
	}
}
