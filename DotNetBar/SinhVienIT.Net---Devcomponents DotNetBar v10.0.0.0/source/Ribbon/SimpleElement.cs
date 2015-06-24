using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SimpleElement.
	/// </summary>
	internal class SimpleElement:ISimpleElement
	{
		#region Private Variables
		private Rectangle m_Bounds=Rectangle.Empty;
		private Rectangle m_ImageBounds=Rectangle.Empty;
		private Rectangle m_TextBounds=Rectangle.Empty;
		private Image m_Image=null;
		private eSimplePartAlignment m_ImageAlignment=eSimplePartAlignment.NearCenter;
		private string m_Text="";
		private int m_FixedWidth=0;
		#endregion

		#region ISimpleElement Members

		public int FixedWidth
		{
			get
			{
				return m_FixedWidth;
			}
			set
			{
				m_FixedWidth=value;
			}
		}

		public bool ImageVisible
		{
			get
			{
				return m_Image!=null;
			}
		}

		public System.Drawing.Size ImageLayoutSize
		{
			get
			{
				if(m_Image==null)
					return Size.Empty;
				return m_Image.Size;
			}
		}

		public DevComponents.DotNetBar.eSimplePartAlignment ImageAlignment
		{
			get
			{
				return m_ImageAlignment;
			}
			set
			{
				m_ImageAlignment=value;
			}
		}

		public int ImageTextSpacing
		{
			get
			{
				return 1;
			}
		}

		public bool TextVisible
		{
			get
			{
				return true;
			}
		}

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

		public Rectangle Bounds
		{
			get { return m_Bounds; }
			set { m_Bounds=value; }
		}

		public Rectangle TextBounds
		{
			get { return m_TextBounds; }
			set { m_TextBounds=value; }
		}

		public Rectangle ImageBounds
		{
			get { return m_ImageBounds; }
			set { m_ImageBounds=value; }
		}

		public System.Drawing.Image Image
		{
			get { return m_Image; }
			set { m_Image=value; }
		}
		#endregion
	}
}
