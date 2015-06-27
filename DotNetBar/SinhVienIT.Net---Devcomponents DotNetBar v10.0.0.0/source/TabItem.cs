using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Editors;
using DevComponents.UI.ContentManager;
using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the Tab on the Tab-Strip control.
	/// </summary>
    [Designer("DevComponents.DotNetBar.Design.TabItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxItem(false), ComVisible(false), DesignTimeVisible(false)]
	public class TabItem : Component, IBlock, ISimpleTab
	{
		#region Private Properties
		private int m_ImageIndex=-1;
		private Image m_Image;
		private Icon m_Icon;
		private string m_Text="";
		private TabStrip m_Parent=null;
		private bool m_Visible=true;
		private Rectangle m_DisplayRectangle=Rectangle.Empty;
		private BaseItem m_DotNetBarItem=null;
		private Control m_BoundControl=null;
		private object m_ItemData=null;
		
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
		private string m_Tooltip="";
        private Rectangle m_CloseButtonBounds = Rectangle.Empty;
        private bool m_CloseButtonMouseOver = false;
        private bool m_CloseButtonVisible = true;
		#endregion

        #region Events
        /// <summary>
        /// Occurs when mouse is pressed over the tab item.
        /// </summary>
        public event MouseEventHandler MouseDown;
        /// <summary>
        /// Occurs when mouse button is released over the tab item.
        /// </summary>
        public event MouseEventHandler MouseUp;
        /// <summary>
        /// Occurs when mouse hovers over the tab item.
        /// </summary>
        public event EventHandler MouseHover;
        /// <summary>
        /// Occurs when mouse enters the tab item.
        /// </summary>
        public event EventHandler MouseEnter;
        /// <summary>
        /// Occurs when mouse leaves the tab item.
        /// </summary>
        public event EventHandler MouseLeave;
        /// <summary>
        /// Occurs when mouse moves over the tab item.
        /// </summary>
        public event MouseEventHandler MouseMove;
        /// <summary>
        /// Occurs when mouse click is performed on the tab item.
        /// </summary>
        public event EventHandler Click;
        #endregion

        #region Properties, Constructor
        /// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="container">Container object.</param>
		public TabItem(IContainer container)
		{
			container.Add(this);
		}
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TabItem()
		{
		}
		/// <summary>
		/// Gets or sets the tab Image index.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(-1),Category("Appearance"),Description("Indicates the tab image index"),Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor)),TypeConverter(typeof(ImageIndexConverter))]
		public int ImageIndex
		{
			get {return m_ImageIndex;}
			set
			{
				if(m_ImageIndex==value)
					return;
				m_ImageIndex=value;
				if(m_Parent!=null)
				{
					m_Parent.NeedRecalcSize=true;
					m_Parent.ResetTabHeight();
				}
                Refresh();
				OnImageChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImageIndex()
		{
			this.ImageIndex=-1;
		}
		// Property Editor support for ImageIndex selection
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public ImageList ImageList
		{
			get
			{
				if(m_Parent!=null)
					return m_Parent.ImageList;
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the tab image.
        /// </summary>
        [Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Appearance"),Description("Indicates the tab image.")]
		public Image Image
		{
			get {return m_Image;}
			set
			{
				if(m_Image==value)
					return;
				m_Image=value;
				if(m_Parent!=null)
				{
					m_Parent.NeedRecalcSize=true;
					m_Parent.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImage()
		{
			this.Image=null;
		}

		/// <summary>
		/// Gets or sets the tab icon. Icon has same functionality as Image except that it support Alpha blending.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Appearance"),Description("Indicates the tab icon. Icon has same functionality as Image except that it support Alpha blending.")]
		public Icon Icon
		{
			get {return m_Icon;}
			set
			{
				if(m_Icon==value)
					return;
				m_Icon=value;
				if(m_Parent!=null)
				{
					m_Parent.NeedRecalcSize=true;
					m_Parent.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetIcon()
		{
			this.Icon=null;
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
					m_Parent.NeedRecalcSize=true;
				Refresh();
				OnTextChanged();
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

		private void Refresh()
		{
			if(m_Parent!=null)
				m_Parent.Refresh();
		}

		/// <summary>
		/// Gets the display bounds of the tab.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public Rectangle DisplayRectangle
		{
			get {return m_DisplayRectangle;}
		}

        /// <summary>
        /// Gets or sets the bounds of the close button rectangle if displayed on the tab. You should not set value of this property.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle CloseButtonBounds
        {
            get { return m_CloseButtonBounds; }
            set { m_CloseButtonBounds = value; }
        }

		internal Rectangle _DisplayRectangle
		{
			get {return m_DisplayRectangle;}
			set {m_DisplayRectangle=value;}
		}
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BaseItem AttachedItem
		{
			get {return m_DotNetBarItem;}
			set {m_DotNetBarItem=value;}
		}

        [Browsable(false)]
        internal bool CloseButtonMouseOver
        {
            get { return m_CloseButtonMouseOver; }
            set { m_CloseButtonMouseOver = value; }
        }

		/// <summary>
		/// Gets or sets the control that is attached to this tab. When tab is selected the control Visible property is set to true and when tab is unselected the control Visible property is set to false.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Behavior"),Description("Indicates the control that is attached to this tab.")]
		public Control AttachedControl
		{
			get {return m_BoundControl;}
			set
			{
				m_BoundControl=value;
				if(m_BoundControl!=null)
				{
                    if (m_Parent != null && !m_Parent.MdiTabbedDocuments /*&& (BarFunctions.IsHandleValid(m_Parent) || !(m_BoundControl is TabControlPanel))*/)
					{
						if(m_Parent.SelectedTab==this)
							m_BoundControl.Visible=true;
						else
							m_BoundControl.Visible=false;
					}
				}
			}
		}

		public TabStrip Parent
		{
			get
			{
				return m_Parent;
			}
		}

		[Browsable(false),DevCoBrowsable(true),DefaultValue(null)]
		public object ItemData
		{
			get {return m_ItemData;}
			set {m_ItemData=value;}
		}

		internal void SetParent(TabStrip tabstrip)
		{
			m_Parent=tabstrip;
		}

		internal Image GetImage()
		{
			if(m_Image!=null)
				return m_Image;
			if(m_ImageIndex>=0 && m_Parent!=null && m_Parent.ImageList!=null && m_ImageIndex<m_Parent.ImageList.Images.Count)
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
				if(this.DesignMode)
				{
					if(m_Parent!=null && m_Parent.Parent is TabControl)
					{
						m_Parent.Parent.Invalidate(true);
						((TabControl)m_Parent.Parent).ApplyDefaultPanelStyle(this.AttachedControl as TabControlPanel);
						m_Parent.Parent.Update();
					}
					else
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets an object that contains data to associate with the item.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
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
		/// Gets/Sets informational text (tooltip) for the tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(""),Category("Appearance"),Description("Indicates the text that is displayed when mouse hovers over the tab."),Localizable(true)]
		public string Tooltip
		{
			get
			{
			
				return m_Tooltip;
			}
			set
			{
				m_Tooltip=value;
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
		/// Gets the tab alignment.
		/// </summary>
		[Browsable(false)]
		public eTabStripAlignment TabAlignment
		{
			get
			{
				return m_Parent.TabAlignment;
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
					return (m_Parent.MouseOverTab==this);
				return false;
			}
		}

		private void OnImageChanged()
		{
			if(this.DesignMode && m_Parent!=null)
			{
				if(m_Parent.Parent is TabControl)
				{
					((TabControl)m_Parent.Parent).SyncTabStripSize();
					((TabControl)m_Parent.Parent).RecalcLayout();
				}
				else
				{
					m_Parent.RecalcSize();
					m_Parent.Refresh();
				}
			}
		}

		private void OnTextChanged()
		{
			this.OnImageChanged();
		}

        internal void InvokeMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        internal void InvokeMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }

        /// <summary>
        /// Raises the Click event.
        /// </summary>
        public void PerformClick()
        {
            InvokeClick(new EventArgs());
        }

        internal void InvokeClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        internal void InvokeMouseEnter(EventArgs e)
        {
            if (MouseEnter != null)
                MouseEnter(this, e);
        }

        internal void InvokeMouseLeave(EventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(this, e);
        }

        internal void InvokeMouseHover(EventArgs e)
        {
            if (MouseHover != null)
                MouseHover(this, e);
        }

        internal void InvokeMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
        }

        /// <summary>
        /// Gets or sets whether Close button on the tab is visible when TabStrip.CloseButtonOnTabsVisible property is set to true. Default value is true. You can use this property
        /// to selectively hide Close button on tabs.
        /// </summary>
        [Browsable(true), Description("Indicates whether Close button on the tab is visible when TabStrip.CloseButtonOnTabsVisible property is set to true."), DefaultValue(true), Category("Appearance")]
        public bool CloseButtonVisible
        {
            get { return m_CloseButtonVisible; }
            set
            {
                if (m_CloseButtonVisible != value)
                {
                    m_CloseButtonVisible = value;
                    TabStrip ts = m_Parent;
                    if (ts != null && ts.CloseButtonOnTabsVisible)
                    {
                        ts.RecalcSize();
                        ts.Invalidate();
                    }
                }
            }
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
				return this._DisplayRectangle;
			}
			set
			{
				this._DisplayRectangle=value;
			}
		}
		#endregion
	}
}
