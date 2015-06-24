using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for BubbleBarGroup.
	/// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), Designer("DevComponents.DotNetBar.Design.BubbleBarTabDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class BubbleBarTab:Component,ISimpleTab,IBlock
	{
		#region Private Properties
		private string m_Text="";
		private BubbleBar m_Parent=null;
		private bool m_Visible=true;
		private Rectangle m_DisplayRectangle=Rectangle.Empty;
		private Color m_BackColor=Color.Empty;
		private Color m_BackColor2=Color.Empty;
		private int m_BackColorGradientAngle=90;
		private Color m_LightBorderColor=Color.Empty;
		private Color m_DarkBorderColor=Color.Empty;
		private Color m_BorderColor=Color.Empty;
		private Color m_TextColor=Color.Empty;
		private string m_Name="";
		private eTabItemColor m_PredefinedColor=eTabItemColor.Default;
		private object m_Tag=null;
		private BubbleButtonCollection m_Buttons=null;
		private bool m_Focus=false;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public BubbleBarTab():this(null){}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="container">Container object.</param>
		public BubbleBarTab(IContainer container)
		{
			if(container!=null)
				container.Add(this);
			m_Buttons=new BubbleButtonCollection(this);
		}


		/// <summary>
		/// Gets the collection of the buttons associated with the tab.
		/// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Buttons"), Description("Returns collection of buttons on the tab."), Editor("DevComponents.DotNetBar.Design.BubbleButtonCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
		public BubbleButtonCollection Buttons
		{
			get {return m_Buttons;}
		}

		/// <summary>
		/// Gets or sets the text displayed on the tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates the text displayed on the tab."),Localizable(true)]
		public string Text
		{
			get {return m_Text;}
			set
			{
				if(m_Text==value)
					return;
				m_Text=value;
				if(m_Parent!=null)
					m_Parent.OnTabTextChanged(this);
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
					m_Parent.OnTabVisibleChanged(this);
				Refresh();
			}
		}

		/// <summary>
		/// Gets the display bounds of the tab.
		/// </summary>
		[Browsable(false)]
		public Rectangle DisplayRectangle
		{
			get {return m_DisplayRectangle;}
		}

		/// <summary>
		/// Gets or sets the object that contains data about the tab. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
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
		/// Gets or sets the object that contains data about the tab. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Data"),Description("Indicates text that contains data about the cell.")]
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
		/// Gets or sets the background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab background color."),Category("Style")]
		public Color BackColor
		{
			get {return m_BackColor;}
			set
			{
				m_BackColor=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor()
		{
			return !m_BackColor.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor()
		{
			BackColor=Color.Empty;
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
		/// Gets or sets the border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab border color."),Category("Style")]
		public Color BorderColor
		{
			get {return m_BorderColor;}
			set
			{
				m_BorderColor=value;
				this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderColor()
		{
			return !m_BorderColor.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderColor()
		{
			BorderColor=Color.Empty;
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

		/// <summary>
		/// Gets or sets name of the tab item that can be used to identify item from the code.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Design"),Description("Indicates the name used to identify item.")]
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
		/// Gets or sets the predefined tab color.
		/// </summary>
		[Browsable(true), DefaultValue(eTabItemColor.Default),Category("Style"),Description("Applies predefined color to tab.")]
		public eTabItemColor PredefinedColor
		{
			get {return m_PredefinedColor;}
			set
			{
				m_PredefinedColor=value;
				TabColorScheme.ApplyPredefinedColor(this,m_PredefinedColor);
				this.Refresh();
			}
		}

		/// <summary>
		/// Returns the font for the tab text.
		/// </summary>
		/// <returns>Reference to the font object.</returns>
		public Font GetTabFont()
		{
			if(m_Parent!=null)
				return m_Parent.Font;
			else
				return SystemInformation.MenuFont;
		}

		/// <summary>
		/// Returns true if tab is selected tab.
		/// </summary>
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				if(m_Parent!=null)
				{
					return (m_Parent.SelectedTab==this);
				}
					return false;
			}
		}

		/// <summary>
		/// Returns true if mouse is over the tab.
		/// </summary>
		[Browsable(false)]
		public bool IsMouseOver
		{
			get
			{
				if(m_Parent!=null)
					return (m_Parent.GetMouseOverTab()==this);
				return false;
			}
		}

		/// <summary>
		/// Gets the tab alignment.
		/// </summary>
		eTabStripAlignment ISimpleTab.TabAlignment
		{
			get
			{
				if(m_Parent!=null)
				{
					if(m_Parent.Alignment==eBubbleButtonAlignment.Top)
						return eTabStripAlignment.Bottom;
				}
				return eTabStripAlignment.Top;
			}
		}

		/// <summary>
		/// Returns reference to parent object or null if tab does not have parent.
		/// </summary>
		[Browsable(false)]
		public BubbleBar Parent
		{
			get {return m_Parent;}
		}

		/// <summary>
		/// Sets the parent of the tab.
		/// </summary>
		/// <param name="parent">Reference to the tab parent object or null.</param>
		internal void SetParent(BubbleBar parent)
		{
			m_Parent=parent;
		}

		private void Refresh()
		{
			if(m_Parent!=null)
			{
				Rectangle r=this.DisplayRectangle;
				r.Width+=16;
				r.X-=8;
				m_Parent.Invalidate(r);
				m_Parent.Update();
			}
		}

		/// <summary>
		/// Called after new button is added to the Buttons collection.
		/// </summary>
		/// <param name="button">Reference to the added button.</param>
		internal void OnButtonInserted(BubbleButton button)
		{
			if(m_Parent!=null)
				m_Parent.OnButtonInserted(this, button);
		}

		/// <summary>
		/// Called after specified button has been removed.
		/// </summary>
		/// <param name="button">Button that was removed.</param>
		internal void OnButtonRemoved(BubbleButton button)
		{
			if(m_Parent!=null)
				m_Parent.OnButtonRemoved(this,button);
		}

		/// <summary>
		/// Called after all buttons have been removed.
		/// </summary>
		internal void OnButtonsCollectionClear()
		{
			if(m_Parent!=null)
				m_Parent.OnButtonsCollectionClear(this);	
		}

		/// <summary>
		/// Gets or sets whether tab has design-time focus.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Focus
		{
			get {return m_Focus;}
			set {m_Focus=value;}
		}
		#endregion

		#region IBlock
		/// <summary>
		/// Gets or sets the bounds of the content block.
		/// </summary>
		Rectangle IBlock.Bounds
		{
			get
			{
				return m_DisplayRectangle;
			}
			set
			{
				m_DisplayRectangle=value;
			}
		}
		#endregion
	}

	
}
