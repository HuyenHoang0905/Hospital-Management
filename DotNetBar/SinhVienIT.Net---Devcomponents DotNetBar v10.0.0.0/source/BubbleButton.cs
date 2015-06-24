using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents a button used on BubbleBar control.
	/// </summary>
	[ToolboxItem(false),DesignTimeVisible(false),DefaultEvent("Click")]
	public class BubbleButton:Component,DevComponents.UI.ContentManager.IBlock
	{
		#region Private Variables
		private string m_TooltipText="";
//		private System.Drawing.Icon m_Icon=null;
		private int m_ImageIndex=-1;
		private int m_ImageIndexLarge=-1;
		private bool m_Enabled=true;
		private Image m_ImageCached=null;
		private Image m_ImageLargeCached=null;
		private Image m_Image=null;
		private Image m_ImageLarge=null;
		private Rectangle m_DisplayRectangle=Rectangle.Empty;
		private Rectangle m_MagnifiedDisplayRectangle=Rectangle.Empty;
		private bool m_MouseOver=false;
		private bool m_MouseDown=false;
		//private SubItemsCollection m_MenuItems;
		private BubbleButtonCollection m_ParentCollection=null;
		private string m_Name="";
		private bool m_Visible=true;
		private object m_Tag=null;
		private bool m_Focus=false;
		private eShortcut m_Shortcut=eShortcut.None;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when user clicks the button.
		/// </summary>
		public event ClickEventHandler Click;
        /// <summary>
        /// Occurs when mouse enters the button.
        /// </summary>
        public event EventHandler MouseEnter;
        /// <summary>
        /// Occurs when mouse leaves the button.
        /// </summary>
        public event EventHandler MouseLeave;
		#endregion

		/// <summary>
		/// Creates new instance of button object.
		/// </summary>
		public BubbleButton()
		{
		}

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_ImageLarge);
                BarUtilities.DisposeImage(ref m_ImageCached);
                BarUtilities.DisposeImage(ref m_ImageLargeCached);
            }
            base.Dispose(disposing);
        }
		#region Public Properties
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
                if (m_Shortcut != eShortcut.None && this.GetBubbleBar()!=null)
                {
                    this.GetBubbleBar().RefreshHasShortcut();
                }
            }
		}

		/// <summary>
		/// Gets or sets the tooltip for the button.
		/// </summary>
		[Category("Appearance"),DefaultValue(""),Description("Indicates tooltip for the button."), Localizable(true)]
		public string TooltipText
		{
			get {return m_TooltipText;}
			set {m_TooltipText=value;}
		}

//		/// <summary>
//		/// Gets or sets the icon used on the button. The icon should at least have two sizes. Normal and enlarged size.
//		/// Proper size will be determined based on the settings for image size on BubbleBar object.
//		/// </summary>
//		[Browsable(true),Category("Images"),Description("Indicates icons used on the button."),DefaultValue(null)]
//		public Icon Icon
//		{
//			get {return m_Icon;}
//			set
//			{
//				m_Icon=value;
//				OnAppearanceChanged();
//			}
//		}

		/// <summary>
		/// Gets or sets the default image used on the button. Note that for improved appearance of the buttons when enlarged
		/// you should set the ImageIndexLarge to the large version of the image specified here. The image size should be the same size
		/// that is specified by the image size properties on BubbleBar object.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates default image used on the button."),Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(ImageIndexConverter)),DefaultValue(-1)]
		public int ImageIndex
		{
			get {return m_ImageIndex;}
			set
			{
				m_ImageIndex=value;
				m_ImageCached=null;
				OnImageIndexChanged();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets reference to the internal cached image loaded from the ImageIndex.
		/// </summary>
		internal Image ImageCached
		{
			get {return m_ImageCached;}
		}

		/// <summary>
		/// Gets reference to the internal cached image loaded from the ImageIndex.
		/// </summary>
		internal Image ImageLargeCached
		{
			get {return m_ImageLargeCached;}
		}

		/// <summary>
		/// Specifies the Button image.
        /// </summary>
        [Browsable(true),Category("Images"),Description("The image that will be displayed on the face of the button."),DefaultValue(null)]
		public System.Drawing.Image Image
		{
			get {return m_Image;}
			set
			{
				m_Image=value;
				this.OnImageChanged();
			}
		}

		/// <summary>
		/// Specifies enlarged the Button image.
        /// </summary>
        [Browsable(true),Category("Images"),Description("Indicates enlarged image that will be displayed on the face of the button."),DefaultValue(null)]
		public System.Drawing.Image ImageLarge
		{
			get {return m_ImageLarge;}
			set
			{
				m_ImageLarge=value;
				this.OnImageChanged();
			}
		}

		/// <summary>
		/// Gets or sets the image index of the enlarged image for the button. Note that if large image is not specified the
		/// default image will be enlarged which might not result in perfect image appearance.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates image index of the enlarged image for the button."),Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),TypeConverter(typeof(ImageIndexConverter)),DefaultValue(-1)]
		public int ImageIndexLarge
		{
			get {return m_ImageIndexLarge;}
			set
			{
				m_ImageIndexLarge=value;
				m_ImageLargeCached=null;
				OnImageIndexChanged();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Property for Property Editor support for ImageIndex selection.
		/// </summary>
		[Browsable(false),EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				BubbleBar bar=this.GetBubbleBar();
				if(bar!=null)
					return bar.Images;
				return null;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether button is enabled.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether button is enabled.")]
		public bool Enabled
		{
			get {return m_Enabled;}
			set
			{
				if(m_Enabled!=value)
				{
					m_Enabled=value;
					this.OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Gets the display rectangle occupied by the button.
		/// </summary>
		[Browsable(false)]
		public Rectangle DisplayRectangle
		{
			get {return m_DisplayRectangle;}
		}

		/// <summary>
		/// Sets the display rectangle of the button.
		/// </summary>
		/// <param name="r">New display rectangle of the button.</param>
		internal void SetDisplayRectangle(Rectangle r)
		{
			m_DisplayRectangle=r;
		}

		/// <summary>
		/// Gets or sets the magnified display rectangle of the button.
		/// </summary>
		[Browsable(false)]
		public Rectangle MagnifiedDisplayRectangle
		{
			get {return m_MagnifiedDisplayRectangle;}
		}

		/// <summary>
		/// Sets the magnified display rectangle of the button.
		/// </summary>
		/// <param name="r">New magnified display rectangle.</param>
		internal void SetMagnifiedDisplayRectangle(Rectangle r)
		{
			m_MagnifiedDisplayRectangle=r;
		}

		/// <summary>
		/// Gets whether mouse is over the button.
		/// </summary>
		[Browsable(false)]
		public bool MouseOver
		{
			get {return m_MouseOver;}
		}

		/// <summary>
		/// Sets whether mouse is over the button.
		/// </summary>
		/// <param name="value">True if mouse is over the button otherwise false.</param>
		internal void SetMouseOver(bool value)
		{
            bool changed=m_MouseOver!=value;
			m_MouseOver=value;
            UpdateCursor();
            if (changed)
            {
                if (value)
                    OnMouseEnter();
                else
                    OnMouseLeave();
            }
		}

        /// <summary>
        /// Raises the MouseEnter event.
        /// </summary>
        protected virtual void OnMouseEnter()
        {
            EventHandler eh = MouseEnter;
            if (eh != null)
                eh(this, new EventArgs());
        }
        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        protected virtual void OnMouseLeave()
        {
            EventHandler eh = MouseLeave;
            if (eh != null)
                eh(this, new EventArgs());
        }

        private void UpdateCursor()
        {
            BubbleBar parent = GetBubbleBar();
            if (parent == null || _Cursor == null) return;

            if (m_MouseOver)
            {
                if (_OldCursor == null)
                {
                    _OldCursor = Cursor.Current;
                }
                parent.Cursor = _Cursor;
            }
            else
            {
                if (_OldCursor != null)
                {
                    parent.Cursor = _OldCursor;
                    _OldCursor = null;
                }
            }
        }

		/// <summary>
		/// Gets whether left mouse button is pressed on the button.
		/// </summary>
		[Browsable(false)]
		public bool MouseDown
		{
			get {return m_MouseDown;}
		}

		/// <summary>
		/// Sets whether left mouse button is pressed over this button.
		/// </summary>
		/// <param name="value">True if left mouse button is pressed otherwise false.</param>
		internal void SetMouseDown(bool value)
		{
			m_MouseDown=value;
		}

		/// <summary>
		/// Returns name of the button that can be used to identify it from the code.
		/// </summary>
		[Browsable(false),Category("Design"),Description("Indicates the name used to identify button.")]
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
		/// Gets or sets a value indicating whether button is visible.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Determines whether the button is visible or hidden.")]
		public virtual bool Visible
		{
			get
			{
				return m_Visible;
			}
			set
			{
				if(m_Visible==value)
					return;

				m_Visible=value;

				OnVisibleChanged();
			}
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
		/// Returns the reference to parent tab.
		/// </summary>
		[Browsable(false)]
		public BubbleBarTab Parent
		{
			get
			{
				if(m_ParentCollection!=null)
					return m_ParentCollection.Parent;
				return null;
			}
		}
		#endregion

		#region Public Methods
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Gets or sets whether button has design-time focus.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Focus
		{
			get {return m_Focus;}
			set {m_Focus=value;}
		}

		/// <summary>
		/// Sets the parent collection button belongs to.
		/// </summary>
		/// <param name="value">Parent collection of the item.</param>
		internal void SetParentCollection(BubbleButtonCollection value)
		{
			m_ParentCollection=value;
		}

		private void OnAppearanceChanged()
		{
			BubbleBar bar=this.GetBubbleBar();
			if(bar==null)
				return;
			bar.Refresh();
		}

		private void OnImageIndexChanged()
		{
			BubbleBar bar=this.GetBubbleBar();
			if(bar==null)
				return;
			if(m_ImageIndex>=0 && m_ImageCached==null)
			{
				if(bar.Images!=null && m_ImageIndex<bar.Images.Images.Count)
					m_ImageCached=bar.Images.Images[m_ImageIndex];
			}

			if(m_ImageIndexLarge>=0 && m_ImageLargeCached==null)
			{
				if(bar.ImagesLarge!=null && m_ImageIndexLarge<bar.ImagesLarge.Images.Count)
					m_ImageLargeCached=bar.ImagesLarge.Images[m_ImageIndex];
			}
		}

		private void OnImageChanged()
		{
			BubbleBar bar=this.GetBubbleBar();
			if(bar!=null && this.Site!=null && this.Site.DesignMode)
				bar.Refresh();
		}

		/// <summary>
		/// Returns the reference to the BubbleBar that contains this button.
		/// </summary>
		/// <returns></returns>
		public BubbleBar GetBubbleBar()
		{
			if(m_ParentCollection!=null && m_ParentCollection.Parent!=null && m_ParentCollection.Parent.Parent!=null)
				return m_ParentCollection.Parent.Parent;
			return null;
		}

		private void OnVisibleChanged()
		{
			BubbleBar bar=this.GetBubbleBar();
			if(bar!=null)
				bar.OnButtonVisibleChanged(this);
		}

		/// <summary>
		/// Invokes button's Click event.
		/// </summary>
		/// <param name="source">Indicates source of the event.</param>
		public void InvokeClick(eEventSource source, MouseButtons mouseButton)
		{
			OnClick(new ClickEventArgs(source, mouseButton));
		}

		/// <summary>
		/// Raises click event.
		/// </summary>
		/// <param name="e">Default event arguments.</param>
		protected virtual void OnClick(ClickEventArgs e)
		{
			if(Click!=null)
				Click(this,e);

			BubbleBar bar=this.GetBubbleBar();
			if(bar!=null)
				bar.InvokeButtonClick(this,e);
		}

        private Cursor _OldCursor = null;
        private Cursor _Cursor = null;
        /// <summary>
        /// Gets or sets the mouse cursor that is displayed when mouse is over the button.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indciates mouse cursor that is displayed when mouse is over the button.")]
        public Cursor Cursor
        {
            get { return _Cursor; }
            set { _Cursor = value; }
        }
		#endregion

		#region IBlock Interface
		/// <summary>
		/// Gets or sets the bounds of the content block.
		/// </summary>
		Rectangle DevComponents.UI.ContentManager.IBlock.Bounds
		{
			get {return m_DisplayRectangle;}
			set {m_DisplayRectangle=value;}
		}
		#endregion
	}

	#region Event Arguments and delegates
	public delegate void ClickEventHandler(object sender, ClickEventArgs e);

	/// <summary>
	/// Represents event arguments for Click event.
	/// </summary>
	public class ClickEventArgs:EventArgs
	{
		/// <summary>
		/// Gets the action that caused the event, event source.
		/// </summary>
		public readonly eEventSource EventSource=eEventSource.Code;
		public readonly MouseButtons Button=MouseButtons.None;

		public ClickEventArgs(eEventSource action, MouseButtons button)
		{
			this.EventSource=action;
			this.Button=button;
		}
	}
	#endregion
}
