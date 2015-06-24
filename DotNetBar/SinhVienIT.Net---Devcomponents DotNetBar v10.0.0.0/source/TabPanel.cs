using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the Tab on the Tab-Strip control.
	/// </summary>
	[ToolboxItem(false),System.Runtime.InteropServices.ComVisible(false)]
	public class TabPanel : System.Windows.Forms.Panel
	{
		private int m_ImageIndex=-1;
		private System.Drawing.Image m_Image;
		private System.Drawing.Icon m_Icon;
		private string m_Text="";
		private TabControl m_Parent=null;
		private bool m_Visible=true;
		private Rectangle m_TabDisplayRectangle=Rectangle.Empty;
		private Color m_BackColor=Color.Empty;
		private Color m_BackColor2=Color.Empty;
		private int m_BackColorGradientAngle=90;
		private Color m_LightBorderColor=Color.Empty;
		private Color m_DarkBorderColor=Color.Empty;
		private Color m_TextColor=Color.Empty;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="container">Container object.</param>
		public TabPanel(System.ComponentModel.IContainer container)
		{
			container.Add(this);
		}
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TabPanel()
		{
		}
		/// <summary>
		/// Gets or sets the tab Image index.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(-1),Category("Appearance"),Description("Indicates the tab image index")]
		public int ImageIndex
		{
			get {return m_ImageIndex;}
			set
			{
				if(m_ImageIndex==value)
					return;
				m_ImageIndex=value;
				if(m_Parent!=null)
					m_Parent.NeedRecalcSize=true;
				Refresh();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImageIndex()
		{
			m_ImageIndex=-1;
		}

		/// <summary>
		/// Gets or sets the tab image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Appearance"),Description("Indicates the tab image.")]
		public System.Drawing.Image Image
		{
			get {return m_Image;}
			set
			{
				if(m_Image==value)
					return;
				m_Image=value;
				if(m_Parent!=null)
					m_Parent.NeedRecalcSize=true;
				Refresh();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImage()
		{
			m_Image=null;
		}

		/// <summary>
		/// Gets or sets the tab icon. Icon has same functionality as Image except that it support Alpha blending.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Appearance"),Description("Indicates the tab icon. Icon has same functionality as Image except that it support Alpha blending.")]
		public System.Drawing.Icon Icon
		{
			get {return m_Icon;}
			set
			{
				if(m_Icon==value)
					return;
				m_Icon=value;
				if(m_Parent!=null)
					m_Parent.NeedRecalcSize=true;
				Refresh();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetIcon()
		{
			m_Icon=null;
		}

		/// <summary>
		/// Gets or sets the text displayed on the tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates the text displayed on the tab.")]
		public new string Text
		{
			get {return m_Text;}
			set
			{
				if(m_Text==value)
					return;
				m_Text=value;
				if(m_Parent!=null)
					m_Parent.NeedRecalcSize=true;
				Refresh();
			}
		}

		/// <summary>
		/// Gets or sets whether tab is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether the tab is visible.")]
		public bool Visible
		{
			get{return m_Visible;}
			set
			{
				if(m_Visible==value)
					return;
				m_Visible=value;
				if(m_Parent!=null)
					m_Parent.NeedRecalcSize=true;
				Refresh();
			}
		}

		private void Refresh()
		{
			if(m_Parent!=null)
				m_Parent.Refresh();
		}

		/// <summary>
		/// Gets the display bounds of the tab.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public Rectangle TabDisplayRectangle
		{
			get {return m_TabDisplayRectangle;}
		}

		internal Rectangle _TabDisplayRectangle
		{
			get {return m_TabDisplayRectangle;}
			set {m_TabDisplayRectangle=value;}
		}

		internal System.Drawing.Image GetImage()
		{
			if(m_Image!=null)
				return m_Image;
			if(m_ImageIndex>=0 && m_Parent!=null && m_Parent.ImageList!=null && m_Parent.ImageList.Images.Count<m_ImageIndex)
				return m_Parent.ImageList.Images[m_ImageIndex];
			return null;
		}

		internal Size IconSize
		{
			get
			{
				return new Size(16,16);
			}
		}

		/// <summary>
		/// Gets or sets the target gradient background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab target gradient background color."),Category("Style")]
		public Color BackColor2
		{
			get {return m_BackColor2;}
			set
			{
				m_BackColor2=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{
			return !m_BackColor2.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor2()
		{
			BackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		[Browsable(true),Description("Indicates the gradient angle."),Category("Style"),DefaultValue(90)]
		public int BackColorGradientAngle
		{
			get {return m_BackColorGradientAngle;}
			set {m_BackColorGradientAngle=value;this.Refresh();}
		}

		/// <summary>
		/// Gets or sets the light border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab light border color."),Category("Style")]
		public Color LightBorderColor
		{
			get {return m_LightBorderColor;}
			set
			{
				m_LightBorderColor=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeLightBorderColor()
		{
			return !m_LightBorderColor.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetLightBorderColor()
		{
			LightBorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the dark border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab dark border color."),Category("Style")]
		public Color DarkBorderColor
		{
			get {return m_DarkBorderColor;}
			set
			{
				m_DarkBorderColor=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeDarkBorderColor()
		{
			return !m_DarkBorderColor.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetDarkBorderColor()
		{
			DarkBorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the text color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab text color."),Category("Style")]
		public Color TextColor
		{
			get {return m_TextColor;}
			set
			{
				m_TextColor=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTextColor()
		{
			return !m_TextColor.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTextColor()
		{
			TextColor=Color.Empty;
		}

		public override string ToString()
		{
			return m_Text;
		}
	}
}
