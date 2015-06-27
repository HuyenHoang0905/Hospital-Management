using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	#region Event Delegates
	/// <summary>
	/// Delegate for OptionGroupChanging event.
	/// </summary>
	public delegate void PanelChangingEventHandler(object sender,  PanelChangingEventArgs e);

	/// <summary>
	/// Delegate for PanelPopup events.
	/// </summary>
	public delegate void PanelPopupEventHandler(object sender,  PanelPopupEventArgs e);
	#endregion

	/// <summary>
	/// Represents Outlook 2003 style navigation pane control.
	/// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.NavigationPaneDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class NavigationPane : System.Windows.Forms.UserControl
	{
		private DevComponents.DotNetBar.PanelExTitle panelTitle;
		private DevComponents.DotNetBar.NavigationBar navBar;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private int m_NavigationBarHeight=0;
        private bool m_CanCollapse = false;
        private bool m_Expanded = true;
        private bool m_ExpandedDelayed = true;
        private Image m_DefaultCollapseImage = null;
        private Image m_DefaultExpandImage = null;
        private Image m_DefaultCollapseImageMouseOver = null;
        private Image m_DefaultExpandImageMouseOver = null;

        private Image m_TitleImageCollapse = null;
        private Image m_TitleImageExpand = null;
        private Image m_TitleImageCollapseMouseOver = null;
        private Image m_TitleImageExpandMouseOver = null;
        private int m_ExpandedSize = 0;
        private int m_CollapsedSize = 0;
        private PanelEx m_PanePopupPanel = null;
        private int m_AnimationTime = 100;
        private bool m_PopupPaneVisible = false;
        private PanelEx m_PopupPanelContainer = null;
        private bool m_LayoutSuspended = false;

		#region Events
		/// <summary>
		/// Occurs when Item is clicked.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item is clicked."),Category("Navigation Pane Events")]
		public event EventHandler ItemClick;
        /// <summary>
        /// Occurs when currently selected panel is about to change.
        /// </summary>
		[System.ComponentModel.Description("Occurs when Item is clicked."),Category("Navigation Pane Events")]
		public event PanelChangingEventHandler PanelChanging;

        /// <summary>
        /// Occurs when control is looking for translated text for one of the internal text that are
        /// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
        /// your custom text to be used instead of the built-in system value.
        /// </summary>
        public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

		/// <summary>
		/// Occurs before panel is displayed on popup when control is collapsed. You can use this event to modify default
		/// size of the popup panel or cancel the popup of the panel.
		/// </summary>
		public event PanelPopupEventHandler BeforePanelPopup;

        /// <summary>
        /// Occurs after panel is displayed on popup.
        /// </summary>
        public event EventHandler AfterPanelPopup;

        /// <summary>
        /// Occurs after panel displayed on popup is closed.
        /// </summary>
        public event EventHandler PanelPopupClosed;

		/// <summary>
		/// Occurs before Expanded property is changed. You can cancel change of this property by setting Cancel=true on the event arguments.
		/// </summary>
		public event ExpandChangeEventHandler ExpandedChanging;
		/// <summary>
		/// Occurs after Expanded property has changed. You can handle ExpandedChanging event and have opportunity to cancel the change.
		/// </summary>
		public event ExpandChangeEventHandler ExpandedChanged;
        /// <summary>
        /// Occurs after Options dialog which is used to customize control's content has closed by user using OK button.
        /// </summary>
        public event EventHandler OptionsDialogClosed;
		#endregion

		public NavigationPane()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);

			navBar.ItemAdded+=new EventHandler(NavBarItemAdded);
			navBar.ItemRemoved+=new BarBaseControl.ItemRemovedEventHandler(NavBarItemRemoved);
            navBar.ApplicationMouseDown += new HandlerMessageEventHandler(ApplicationMouseDown);
            navBar.LocalizeString += new DotNetBarManager.LocalizeStringEventHandler(navBar_LocalizeString);
            navBar.OptionsDialogClosed += new EventHandler(NavBarOptionsDialogClosed);
			navBar.SplitterVisible=true;
			m_NavigationBarHeight=navBar.Height;
            panelTitle.ExpandedClick += new EventHandler(panelEx1_ExpandedClick);
            panelTitle.ExpandChangeButton.ImagePaddingHorizontal = 2;
            panelTitle.ExpandChangeButton.ImagePaddingVertical = 2;
		}

        void NavBarOptionsDialogClosed(object sender, EventArgs e)
        {
            OnOptionsDialogClosed(e);
        }

        void navBar_LocalizeString(object sender, LocalizeEventArgs e)
        {
            if (LocalizeString != null)
                LocalizeString(this, e);
        }

        protected virtual void OnOptionsDialogClosed(EventArgs e)
        {
            EventHandler handler = OptionsDialogClosed;
            if (handler != null) handler(this, e);
        }

        /// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (m_DefaultCollapseImage != null)
                    m_DefaultCollapseImage.Dispose();
                if (m_DefaultExpandImage != null)
                    m_DefaultExpandImage.Dispose();
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using(SolidBrush brush=new SolidBrush(this.NavigationBar.GetColorScheme().BarBackground))
				e.Graphics.FillRectangle(brush,this.ClientRectangle);
            //if (!m_Expanded)
            {
                using (Pen pen = new Pen(this.NavigationBar.GetColorScheme().PanelBorder, 1))
                    e.Graphics.DrawRectangle(pen, new Rectangle(0,0,this.Width-1,this.Height-1));
            }
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SetDesignMode()
		{
			navBar.SetDesignMode(this.DesignMode);
		}

        /// <summary>
        /// Returns the width of the expanded control if control is currently collapsed.
        /// </summary>
        [Browsable(false)]
        public int ExpandedSize
        {
            get { return m_ExpandedSize; }
        }

		/// <summary>
		/// Returns collection containing buttons on navigation bar.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
		public SubItemsCollection Items
		{
			get
			{
				return navBar.Items;
			}
		}

		/// <summary>
		/// Applies any layout changes and repaint the control.
		/// </summary>
		public virtual void RecalcLayout()
		{
			navBar.RecalcLayout();
		}

		private void NavBarItemAdded(object sender, EventArgs e)
		{
			ButtonItem item=sender as ButtonItem;
            if (item != null && !item.IsOnCustomizeMenu)
            {
                item.CheckedChanged += new EventHandler(ItemCheckedChanged);
                item.TextChanged += new EventHandler(ItemTextChanged);
                item.VisibleChanged += new EventHandler(ItemVisibleChanged);

                if (item.Checked)
                {
                    item.SetChecked(false);
                    item.Checked = true;
                }
            }
		}

        void ItemVisibleChanged(object sender, EventArgs e)
        {
            ButtonItem item = sender as ButtonItem;
            if (item == null || item.IsOnCustomizeMenu)
                return;
            if (item.Checked && !item.Visible)
            {
                foreach (BaseItem childItem in this.Items)
                {
                    ButtonItem button = childItem as ButtonItem;
                    if (button == null || !button.Visible)
                        continue;
                    button.Checked = true;
                    break;
                }
            }
        }

		private void NavBarItemRemoved(object sender, ItemRemovedEventArgs e)
		{
			ButtonItem item=sender as ButtonItem;
			if(item!=null)
			{
				try
				{
					item.CheckedChanged-=new EventHandler(ItemCheckedChanged);
                    item.TextChanged -= new EventHandler(ItemTextChanged);
                    item.VisibleChanged -= new EventHandler(ItemVisibleChanged);
				}
				catch{}
			}
		}

		private void ItemCheckedChanged(object sender, EventArgs e)
		{
			ButtonItem item=sender as ButtonItem;
			if(item==null || item.IsOnCustomizeMenu)
				return;
			foreach(Control c in this.Controls)
			{
				if(c is NavigationPanePanel)
				{
					NavigationPanePanel panel=c as NavigationPanePanel;
					if(panel.ParentItem==item)
					{
						if(item.Checked)
						{
                            if (m_Expanded)
                            {
                                if (!this.DesignMode)
                                    panel.Visible = true;
                                panel.BringToFront();
                            }
							panelTitle.Text=item.Text;
                            if (m_PanePopupPanel != null)
                                m_PanePopupPanel.Text = panelTitle.Text;
						}
						else
						{
                            if (m_Expanded)
                            {
                                if (!this.DesignMode)
                                    panel.Visible = false;
                                else
                                    panel.SendToBack();
                            }
						}
						break;
					}
				}
			}
		}

		private void ItemTextChanged(object sender, EventArgs e)
		{
			ButtonItem item=sender as ButtonItem;
			if(item==null)
				return;
            if (item.Checked)
            {
                panelTitle.Text = item.Text;
                if (m_PanePopupPanel != null)
                    m_PanePopupPanel.Text = item.Text;
            }
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);

			if(this.DesignMode)
				return;
			NavigationPanePanel panel=e.Control as NavigationPanePanel;
			if(panel==null)
				return;

			if(panel.ParentItem!=null && m_Expanded)
			{
				if(this.Items.Contains(panel.ParentItem) && panel.ParentItem is ButtonItem)
				{
					panel.Visible=((ButtonItem)panel.ParentItem).Checked;
					if(panel.Visible)
						panel.BringToFront();
				}
				else
					panel.Visible=false;
			}
			else
				panel.Visible=false;
		}

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            Application.DoEvents();
            if (m_CanCollapse)
            {
                UpdateExpandCollapseImages();
                UpdateExpandButton();
            }
        }

		/// <summary>
		/// Returns Panel associated with button on navigation bar or null if panel cannot be found.
		/// </summary>
		/// <param name="item">Button on navigation bar.</param>
		/// <returns></returns>
		public NavigationPanePanel GetPanel(ButtonItem item)
		{
			if(item==null)
				return null;
			foreach(Control c in this.Controls)
			{
				NavigationPanePanel panel=c as NavigationPanePanel;
				if(panel!=null)
				{
					if(panel.ParentItem==item)
						return panel;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets or sets the height of the navigation bar part of the navigation pane control.
		/// Navigation Bar height is automatically calculated based on the content.
		/// Setting this property suggests desired height of the navigation bar but the actual height will be
        /// calculated to ensure that complete buttons are visible so suggested and actual height might differ.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),DefaultValue(32)]
		public int NavigationBarHeight
		{
			get
			{
                if (!this.DesignMode && navBar != null && navBar.Items.Count > 0)
                    return navBar.Height;

				return m_NavigationBarHeight;
			}
			set
			{
				m_NavigationBarHeight=value;
				navBar.Height=m_NavigationBarHeight;
			}
		}

		private void OnNavBarResize(object sender, EventArgs e)
		{
			if(this.DesignMode)
				m_NavigationBarHeight=navBar.Height;
		}

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            UpdatePanePopupPanelSize();
        }

        /// <summary>
        /// Suspends normal layout logic.
        /// </summary>
        public new void SuspendLayout()
        {
            m_LayoutSuspended = true;
            base.SuspendLayout();
        }

		/// <summary>
		/// Resumes normal layout logic. Optionally forces an immediate layout of pending layout requests.
		/// </summary>
		public new void ResumeLayout(bool performLayout)
		{
            m_LayoutSuspended = false;
			navBar.Height=m_NavigationBarHeight;
			navBar.SendToBack();
			base.ResumeLayout(true);
            this.Expanded = m_ExpandedDelayed;
		}

		/// <summary>
		/// Returns reference to internal NavigationBar control.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public NavigationBar NavigationBar
		{
			get
			{
				return navBar;
			}
		}

		/// <summary>
		/// Gets or sets size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
		public Size ImageSizeSummaryLine
		{
			get {return navBar.ImageSizeSummaryLine;}
			set {navBar.ImageSizeSummaryLine=value;}
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeImageSizeSummaryLine()
		{
			return navBar.ShouldSerializeImageSizeSummaryLine();
		}

		/// <summary>
		/// Gets or sets whether Configure Buttons button is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates Configure Buttons button is visible."),DefaultValue(true)]
		public bool ConfigureItemVisible
		{
			get {return navBar.ConfigureItemVisible;}
			set {navBar.ConfigureItemVisible=value;}
		}

		/// <summary>
		/// Gets or sets whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureShowHideVisible
		{
			get {return navBar.ConfigureShowHideVisible;}
			set	{navBar.ConfigureShowHideVisible=value;}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureNavOptionsVisible
		{
			get {return navBar.ConfigureNavOptionsVisible;}
			set	{navBar.ConfigureNavOptionsVisible=value;}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureAddRemoveVisible
		{
			get {return navBar.ConfigureAddRemoveVisible;}
			set {navBar.ConfigureAddRemoveVisible=value;}
		}

		/// <summary>
		/// Returns reference to currently checked button.
		/// </summary>
		public ButtonItem CheckedButton
		{
			get {return navBar.CheckedButton;}
		}

		/// <summary>
		/// Returns reference to currently selected panel. Panels are automatically switched when buttons are checked.
		/// </summary>
		public NavigationPanePanel SelectedPanel
		{
			get {return this.GetPanel(this.CheckedButton);}
		}

		/// <summary>
		/// Increases the size of the navigation bar if possible by showing more buttons on the top.
		/// </summary>
		public void ShowMoreButtons()
		{
			navBar.ShowMoreButtons();
		}

		/// <summary>
		/// Reduces the size of the navigation bar if possible by showing fewer buttons on the top.
		/// </summary>
		public void ShowFewerButtons()
		{
			navBar.ShowFewerButtons();
		}

		/// <summary>
		/// ImageList for images used on Items.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for images used on Items.")]
		public System.Windows.Forms.ImageList Images
		{
			get
			{
				return navBar.Images;
			}
			set
			{
				navBar.Images=value;
			}
		}

		/// <summary>
		/// Returns reference to the PanelEx that is used to display title.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),Category("Appearance"),Description("PanelEx control that is used to display title.")]
		public PanelEx TitlePanel
		{
			get {return panelTitle;}
		}

		/// <summary>
		/// Gets or sets the padding in pixels at the top portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the top portion of the item."),DefaultValue(4)]
		public int ItemPaddingTop
		{
			get {return navBar.ItemPaddingTop;}
			set 
			{
				navBar.ItemPaddingTop=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets the padding in pixels for bottom portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the bottom of the item."),DefaultValue(4)]
		public int ItemPaddingBottom
		{
			get {return navBar.ItemPaddingBottom;}
			set 
			{
				navBar.ItemPaddingBottom=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar."),DefaultValue(true)]
		public bool AutoSizeButtonImage
		{
			get {return navBar.AutoSizeButtonImage;}
			set {navBar.AutoSizeButtonImage=value;}
		}

		/// <summary>
		/// Saves current visual layout of navigation bar control to XML based file.
		/// </summary>
		/// <param name="fileName">File name to save layout to.</param>
		public void SaveLayout(string fileName)
		{
			navBar.SaveLayout(fileName);
		}

		/// <summary>
		/// Saves current visual layout of navigation bar control to XmlElement.
		/// </summary>
		/// <param name="xmlParent">XmlElement object that will act as a parent for the layout definition. Exact same element should be passed into the LoadLayout method to load the layout.</param>
		public void SaveLayout(System.Xml.XmlElement xmlParent)
		{
			navBar.SaveLayout(xmlParent);
		}

		/// <summary>
		/// Gets or sets the navigation bar definition string.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string LayoutDefinition
		{
			get
			{
				return navBar.LayoutDefinition;
			}
			set
			{
				navBar.LayoutDefinition=value;
			}
		}

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return navBar.AntiAlias; }
            set
            {
                navBar.AntiAlias = value;
                if (panelTitle != null)
                    panelTitle.AntiAlias = value;
                if (m_PanePopupPanel != null)
                    m_PanePopupPanel.AntiAlias = value;
                if (m_PopupPanelContainer != null)
                    m_PopupPanelContainer.AntiAlias = value;
            }
        }

        /// <summary>
        /// Gets or sets animation time in milliseconds when control is expanded or collapsed. Default value is 100 miliseconds. You can set this to 0 (zero) to disable animation.
        /// </summary>
        [Browsable(true), DefaultValue(100), Category("Expand"), Description("Indicates animation time in milliseconds when control is expanded or collapsed.")]
        public int AnimationTime
        {
            get { return m_AnimationTime; }
            set
            {
                if (m_AnimationTime >= 0)
                    m_AnimationTime = value;
            }
        }

        /// <summary>
        /// Gets or sets whether navigation pane can be collapsed. Default value is false. When set to true
        /// expand/collapse button will be displayed in title so control can be reduced in size.
        /// </summary>
        [Browsable(true), Category("Title"), DefaultValue(false), Description("Indicates whether navigation pane can be collapsed.")]
        public bool CanCollapse
        {
            get { return m_CanCollapse; }
            set
            {
                m_CanCollapse = value;
                OnCanCollapseChanged();
                panelTitle.ExpandButtonVisible = m_CanCollapse;
            }
        }

        /// <summary>
        /// Gets or sets alignment of the expand button inside of title bar.
        /// </summary>
        [Browsable(true), DefaultValue(eTitleButtonAlignment.Right), Category("Title"), Description("Indicates the alignment of expand button inside title bar.")]
        public eTitleButtonAlignment TitleButtonAlignment
        {
            get { return panelTitle.ButtonAlignment; }
            set
            {
                panelTitle.ButtonAlignment = value;
                UpdateExpandButton();
            }
        }

        /// <summary>
        /// Gets or sets whether navigation pane is expanded. Default value is true. 
        /// When control is collapsed it is reduced in size so it consumes less space.
        /// </summary>
        [Browsable(true), Category("Title"), DefaultValue(true), Description("Indicates whether navigation pane can be collapsed.")]
        public bool Expanded
        {
            get { return m_Expanded; }
            set
            {
                if (m_LayoutSuspended)
                {
                    m_ExpandedDelayed = value;
                    return;
                }
                if (m_Expanded != value)
                {
					SetExpanded(value, eEventSource.Code);
                }
            }
        }

		private void SetExpanded(bool expanded, eEventSource source)
		{
			if(ExpandedChanging!=null)
			{
				ExpandedChangeEventArgs args=new ExpandedChangeEventArgs(source, expanded);
				ExpandedChanging(this,args);
				if(args.Cancel)
					return;
			}

			m_Expanded = expanded;
			OnExpandedChanged();

			if(ExpandedChanged!=null)
			{
				ExpandedChangeEventArgs args=new ExpandedChangeEventArgs(source, expanded);
				ExpandedChanged(this,args);
			}
		}

        private void OnExpandedChanged()
        {
            if (m_Expanded)
            {
                foreach (BaseItem item in navBar.Items)
                    item.RenderText = true;
                panelTitle.DrawText = true;
                this.PopupSelectedPaneVisible = false;
                DestroyPanePopup();
                if (this.AnimationTime == 0 || this.DesignMode)
                    TypeDescriptor.GetProperties(this)["Width"].SetValue(this, m_ExpandedSize);
                else
                {
                    Rectangle targetRect = new Rectangle(this.Location, new Size(m_ExpandedSize, this.Height));
                    if (this.Dock == DockStyle.Right)
                        targetRect.Offset(-m_ExpandedSize, 0);
                    BarFunctions.AnimateControl(this, true, m_AnimationTime, this.Bounds, targetRect);
                }
            }
            else
            {
                panelTitle.DrawText = false;
                m_ExpandedSize = this.Width;
                if (this.AnimationTime == 0 || this.DesignMode)
                    TypeDescriptor.GetProperties(this)["Width"].SetValue(this, GetCollapsedSize());
                else
                {
                    Rectangle targetRect = new Rectangle(this.Location, new Size(GetCollapsedSize(), this.Height));
                    if (this.Dock == DockStyle.Right)
                        targetRect.Offset(this.Width - targetRect.Width, 0);
                    BarFunctions.AnimateControl(this, true, m_AnimationTime, this.Bounds, targetRect);
                }
                SetupPanePopup();
                foreach (BaseItem item in navBar.Items)
                    item.RenderText = false;
                navBar.Invalidate();
            }
            this.UpdateExpandButton();
        }

        private int GetCollapsedSize()
        {
            int defaultCollapsedSize = Math.Max(panelTitle.Font.Height + 10, 34);

            if (m_CollapsedSize > 0)
                return m_CollapsedSize;

            ButtonItem item = navBar.ItemsContainer.GetFirstVisibleItem() as ButtonItem;
            if (item == null)
                return defaultCollapsedSize;

            if (item.ImageSize.IsEmpty)
                return defaultCollapsedSize;

            CompositeImage image = item.GetImage(ImageState.Default);
            if (image != null)
                return Math.Max(defaultCollapsedSize, image.RealWidth + 10);

            return defaultCollapsedSize;
        }

        void panelEx1_ExpandedClick(object sender, EventArgs e)
        {
            SetExpanded(!this.Expanded,eEventSource.Mouse);
        }

        private void ApplicationMouseDown(object sender, HandlerMessageEventArgs e)
        {
            if (this.PopupSelectedPaneVisible)
            {
                string s = NativeFunctions.GetClassName(e.hWnd);
                s = s.ToLower();
                if (s.IndexOf("combolbox") >= 0)
                    return;

                Control c = Control.FromChildHandle(e.hWnd);
                while (c != null)
                {
                    if (c == m_PopupPanelContainer || c == m_PanePopupPanel)
                        return;
                    c=c.Parent;
                }
                this.PopupSelectedPaneVisible = false;
            }
        }

        /// <summary>
        /// Gets the reference to the inner panel displaying Navigation Pane vertical text created when control is collapsed.
        /// </summary>
        [Browsable(false)]
        public PanelEx CollapsedInnerPanel
        {
            get
            {
                return m_PanePopupPanel;
            }
        }

        private void SetupPanePopup()
        {
            m_PanePopupPanel = new PanelEx();
            m_PanePopupPanel.ColorSchemeStyle = navBar.Style;
            m_PanePopupPanel.ColorScheme = navBar.GetColorScheme();
            m_PanePopupPanel.ApplyButtonStyle();
            m_PanePopupPanel.Style.VerticalText = true;
            m_PanePopupPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
            m_PanePopupPanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
            m_PanePopupPanel.Style.ResetBackColor2();
            m_PanePopupPanel.StyleMouseOver.VerticalText = true;
            m_PanePopupPanel.StyleMouseDown.VerticalText = true;
            m_PanePopupPanel.Font = panelTitle.Font;
            m_PanePopupPanel.AntiAlias = this.AntiAlias;
            UpdatePanePopupPanelSize();

            if(this.CheckedButton!=null)
                m_PanePopupPanel.Text = this.CheckedButton.Text;
            else
                m_PanePopupPanel.Text = "Navigation Pane";
            
            foreach (Control c in this.Controls)
            {
                if (c is NavigationPanePanel)
                    c.Visible = false;
            }
            this.Controls.Add(m_PanePopupPanel);
            m_PanePopupPanel.Click += new EventHandler(PanePopupPanelClick);
        }

        void PanePopupPanelClick(object sender, EventArgs e)
        {
            this.PopupSelectedPaneVisible = !this.PopupSelectedPaneVisible;
        }

        /// <summary>
        /// Popup selected pane when control is collapsed. When control is collapsed (Expanded=false) currently selected pane is not visible
        /// calling this method will popup selected pane and allow user access to it. Use PopupPaneVisible property
        /// to check whether currently selected pane is displayed as popup.
        /// </summary>
        private void PopupSelectedPane()
        {
            if (m_PopupPaneVisible)
                return;
            Control selectedNavPane = null;
            foreach (Control c in this.Controls)
            {
                if (c is NavigationPanePanel && ((NavigationPanePanel)c).ParentItem == this.CheckedButton)
                {
                    selectedNavPane = c;
                    break;
                }
            }
            if (selectedNavPane == null)
                return;

			// Animation start and end rectangles
			int h = navBar.Top - panelTitle.Bottom;
			Rectangle targetRect = new Rectangle(this.Width, panelTitle.Bottom, m_ExpandedSize, h);
			Rectangle startRect = new Rectangle(this.Width, panelTitle.Bottom, 16, h);

			if(BeforePanelPopup!=null)
			{
				PanelPopupEventArgs args=new PanelPopupEventArgs(targetRect);
				BeforePanelPopup(this, args);
                if(args.Cancel)
					return;
				targetRect=args.PopupBounds;
			}

            Control parentForm = this.FindForm();
            if (parentForm == null)
                parentForm = this.TopLevelControl;
            if (parentForm == null)
                parentForm = this.Parent;

            m_PopupPanelContainer = new PanelEx();
            m_PopupPanelContainer.Visible = false;
            m_PopupPanelContainer.ApplyLabelStyle();
            m_PopupPanelContainer.AntiAlias = this.AntiAlias;
            //m_PopupPanelContainer.Style.BorderSide = eBorderSide.All;
            //m_PopupPanelContainer.Style.Border = eBorderType.SingleLine;
            //m_PopupPanelContainer.Style.BorderWidth = 1;
            //m_PopupPanelContainer.Style.BorderColor.Color = panelTitle.Style.BorderColor.Color;
            //m_PopupPanelContainer.DockPadding.All = 1;
            parentForm.Controls.Add(m_PopupPanelContainer);
            m_PopupPanelContainer.BringToFront();
            this.Controls.Remove(selectedNavPane);
            if (selectedNavPane is NavigationPanePanel)
            {
                NavigationPanePanel panel = ((NavigationPanePanel)selectedNavPane);
                panel.DockPadding.Bottom = 1;
                if (panel.Style.Border == eBorderType.None)
                {
                    m_PopupPanelContainer.Style.BorderSide = eBorderSide.All;
                    m_PopupPanelContainer.Style.Border = eBorderType.SingleLine;
                    m_PopupPanelContainer.Style.BorderWidth = 1;
                    m_PopupPanelContainer.Style.BorderColor.Color = panelTitle.Style.BorderColor.Color;
                    m_PopupPanelContainer.DockPadding.All = 1;
                }
            }
            m_PopupPanelContainer.Controls.Add(selectedNavPane);
            selectedNavPane.Visible = true;

            if (this.Dock == DockStyle.Right)
            {
                targetRect.Offset(-(this.Width + targetRect.Width), 0);
                startRect.Offset(-(this.Width + startRect.Width), 0);
            }

            Point p = this.PointToScreen(targetRect.Location);
            targetRect.Location = parentForm.PointToClient(p);
            p = this.PointToScreen(startRect.Location);
            startRect.Location = parentForm.PointToClient(p);

            if (AnimationTime == 0)
            {
                m_PopupPanelContainer.Bounds = targetRect;
                m_PopupPanelContainer.Visible = true;
            }
            else
                BarFunctions.AnimateControl(m_PopupPanelContainer, true, AnimationTime, startRect, targetRect);

            m_PopupPanelContainer.Focus();
            m_PopupPanelContainer.Leave += new EventHandler(m_PopupPanelContainer_Leave);
            m_PopupPaneVisible = true;

            if (AfterPanelPopup != null)
                AfterPanelPopup(this, new EventArgs());
        }

        void m_PopupPanelContainer_Leave(object sender, EventArgs e)
        {
            if (PopupSelectedPaneVisible)
                PopupSelectedPaneVisible = false;
        }

		private bool m_Hiding=false;
        /// <summary>
        /// Hides popup selected pane when control is collapsed and selected pane is displayed as popup. When control is collapsed (Expanded=false)
        /// currently selected pane can be displayed as popup. Calling this method will hide the popup pane. Use PopupPaneVisible property
        /// to check whether currently selected pane is displayed as popup.
        /// </summary>
        private void HideSelectedPopupPane()
        {
            if (!m_PopupPaneVisible || m_Hiding)
                return;
            m_Hiding=true;
			try
			{
				if (AnimationTime == 0)
					m_PopupPanelContainer.Hide();
				else
				{
					Rectangle targetRect = new Rectangle(this.Width, this.Top, 16, this.Height);
					if (this.Dock == DockStyle.Right)
						targetRect.Offset(-(this.Width + targetRect.Width), 0);
					Point p = this.PointToScreen(targetRect.Location);
					targetRect.Location = m_PopupPanelContainer.Parent.PointToClient(p);
					BarFunctions.AnimateControl(m_PopupPanelContainer, false, AnimationTime, m_PopupPanelContainer.Bounds, targetRect);
				}

				Control navPanel=null;
				foreach(Control c in m_PopupPanelContainer.Controls)
				{
					if(c is NavigationPanePanel)
					{
						c.Visible=false;
						m_PopupPanelContainer.Controls.Remove(c);
						navPanel=c;
						break;
					}
				}
				if(navPanel is NavigationPanePanel)
					((NavigationPanePanel)navPanel).DockPadding.Bottom = 0;
				this.Controls.Add(navPanel);
				m_PopupPanelContainer.Leave -= new EventHandler(m_PopupPanelContainer_Leave);
				m_PopupPanelContainer.Dispose();
				m_PopupPanelContainer = null;
				m_PopupPaneVisible = false;
                OnPanelPopupClosed(EventArgs.Empty);
			}
			finally
			{
				m_Hiding=false;
			}
        }

        /// <summary>
        /// Raises the PanelPopupClosed event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnPanelPopupClosed(EventArgs e)
        {
            EventHandler handler = PanelPopupClosed;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Gets or sets whether selected pane is displayed as popup when control is collapsed (Expanded=false). Using
        /// navigation pane button that is displayed when control is collapsed user can popup or close currently selected pane without
        /// expanding the control. You can use this property to do same from code as well as check whether selected pane is displayed as
        /// popup. Note that control must be collapsed (Expanded=false) in order for this property to have any effect.
        /// </summary>
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PopupSelectedPaneVisible
        {
            get { return m_PopupPaneVisible; }
            set
            {
                if (m_PopupPaneVisible != value)
                {
                    if (value)
                        PopupSelectedPane();
                    else
                        HideSelectedPopupPane();
                }
            }
        }

        private void UpdatePanePopupPanelSize()
        {
            if (m_PanePopupPanel == null)
                return;
            int margin = 3;
            m_PanePopupPanel.Bounds=new Rectangle(new Point(this.ClientRectangle.X + margin, panelTitle.Bottom + margin),
                new Size(this.ClientRectangle.Width - margin * 2, navBar.Top - panelTitle.Bottom - margin * 2));
        }

        private void DestroyPanePopup()
        {
            if (m_PanePopupPanel != null)
            {
                this.Controls.Remove(m_PanePopupPanel);
                m_PanePopupPanel.Dispose();
                m_PanePopupPanel = null;
            }

            foreach(Control c in this.Controls)
            {
                if (c is NavigationPanePanel && ((NavigationPanePanel)c).ParentItem == this.CheckedButton)
                {
                    c.Visible = true;
                    if(this.DesignMode)
                        TypeDescriptor.GetProperties(c)["Visible"].SetValue(c, true);
                    c.BringToFront();
                    break;
                }
            }
        }

        private void OnCanCollapseChanged()
        {
            if (m_CanCollapse)
            {
                UpdateExpandCollapseImages();
            }
            UpdateExpandButton();
        }

        private Image GetCollapseImage(bool mouseOverImage)
        {
            if (mouseOverImage)
            {
                if (m_TitleImageCollapseMouseOver != null)
                    return m_TitleImageCollapseMouseOver;
                return m_DefaultCollapseImageMouseOver;
            }
            else
            {
                if (m_TitleImageCollapse != null)
                    return m_TitleImageCollapse;
                return m_DefaultCollapseImage;
            }
        }

        private Image GetExpandImage(bool mouseOverImage)
        {
            if (mouseOverImage)
            {
                if (m_TitleImageExpandMouseOver != null)
                    return m_TitleImageExpandMouseOver;
                return m_DefaultExpandImageMouseOver;
            }
            else
            {
                if (m_TitleImageExpand != null)
                    return m_TitleImageExpand;
                return m_DefaultExpandImage;
            }
        }

        internal void Office2007ColorTableChanged()
        {
            ColorSchemeStyleChanged();
        }

        private void UpdateExpandButton()
        {
            LocalizationManager lm=new LocalizationManager(navBar as IOwnerLocalize);
            if (m_Expanded)
            {
                if (panelTitle.ButtonAlignment == eTitleButtonAlignment.Right || m_TitleImageCollapse!=null)
                {
                    panelTitle.ExpandChangeButton.Image = GetCollapseImage(false);
                    panelTitle.ExpandChangeButton.HoverImage = GetCollapseImage(true);
                }
                else
                {
                    panelTitle.ExpandChangeButton.Image = GetExpandImage(false);
                    panelTitle.ExpandChangeButton.HoverImage = GetExpandImage(true);
                }
                panelTitle.ExpandChangeButton.Tooltip = lm.GetDefaultLocalizedString(LocalizationKeys.NavPaneCollapseButtonTooltip);
            }
            else
            {
                if (panelTitle.ButtonAlignment == eTitleButtonAlignment.Right || m_TitleImageExpand != null)
                {
                    panelTitle.ExpandChangeButton.Image = GetExpandImage(false);
                    panelTitle.ExpandChangeButton.HoverImage = GetExpandImage(true);
                }
                else
                {
                    panelTitle.ExpandChangeButton.Image = GetCollapseImage(false);
                    panelTitle.ExpandChangeButton.HoverImage = GetCollapseImage(true);
                }
                panelTitle.ExpandChangeButton.Tooltip = lm.GetDefaultLocalizedString(LocalizationKeys.NavPaneExpandButtonTooltip);
            }
            panelTitle.UpdateButtonPosition();
        }
	    
	    private void UpdateExpandCollapseImages()
	    {
            if (m_DefaultCollapseImage != null)
                m_DefaultCollapseImage.Dispose();
            if (m_DefaultExpandImage != null)
                m_DefaultExpandImage.Dispose();
            if (m_DefaultCollapseImageMouseOver != null)
                m_DefaultCollapseImageMouseOver.Dispose();
            if (m_DefaultExpandImageMouseOver != null)
                m_DefaultExpandImageMouseOver.Dispose();

            m_DefaultCollapseImage = UIGraphics.CreateExpandButtonImage(true, panelTitle.ColorScheme.PanelText, false);
            m_DefaultCollapseImageMouseOver = UIGraphics.CreateExpandButtonImage(true, panelTitle.ColorScheme.ItemHotText, false);
            m_DefaultExpandImage = UIGraphics.CreateExpandButtonImage(false, panelTitle.ColorScheme.PanelText, false);
            m_DefaultExpandImageMouseOver = UIGraphics.CreateExpandButtonImage(false, panelTitle.ColorScheme.ItemHotText, false);
	    }

		/// <summary>
		/// Loads navigation bar layout that was saved using SaveLayout method. Note that this method must be called after all items are created and added to the control.
		/// </summary>
		/// <param name="fileName">File to load layout from.</param>
		public void LoadLayout(string fileName)
		{
			navBar.LoadLayout(fileName);
		}

		/// <summary>
		/// Loads navigation bar layout that was saved using SaveLayout method. Note that this method must be called after all items are created and added to the control.
		/// </summary>
		/// <param name="xmlParent">Parent XML element that is used to load layout from. Note that this must be the same element that was passed into the SaveLayout method.</param>
		public void LoadLayout(System.Xml.XmlElement xmlParent)
		{
			navBar.LoadLayout(xmlParent);
		}

        /// <summary>
        /// Gets/Sets the visual style for the control. Default style is Office 2003.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the visual style of the control."), DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle Style
        {
            get
            {
                return navBar.Style;
            }
            set
            {
                navBar.Style = value;
                ColorSchemeStyleChanged();
            }
        }

        /// <summary>
        /// Updates the color scheme on child panels to reflect the style of the NavigationBar control. Calling this method is necessary only if you manually
        /// change the NavigationBar.ColorScheme property.
        /// </summary>
        public void ColorSchemeStyleChanged()
        {
            ColorScheme cs = null;
            eDotNetBarStyle style = navBar.Style;

            if (BarFunctions.IsOffice2007Style(style) || navBar.ColorScheme != null && navBar.ColorScheme.PredefinedColorScheme == ePredefinedColorScheme.SystemColors)
                cs = navBar.GetColorScheme();

            foreach (Control c in this.Controls)
            {
                if (c is PanelEx)
                {
                    PanelEx p = c as PanelEx;
                    TypeDescriptor.GetProperties(p)["ColorSchemeStyle"].SetValue(p, style);
                    if (cs != null)
                        ((PanelEx)c).ColorScheme = cs;
                }
            }

            if (BarFunctions.IsOffice2007Style(style))
                panelTitle.Style.Border = eBorderType.RaisedInner;
            else
                panelTitle.Style.Border = eBorderType.SingleLine;

            UpdateExpandCollapseImages();
            UpdateExpandButton();
        }

		#region Event Forwarding
		private void OnNavBarItemClick(object sender, EventArgs e)
		{
			if(ItemClick!=null)
				ItemClick(sender, e);
		}
		private void OnNavBarOptionGroupChanging(object sender, OptionGroupChangingEventArgs e)
		{
			NavigationPanePanel oldPanel=null, newPanel=null;
            if(e.OldChecked!=null)
				oldPanel=this.GetPanel(e.OldChecked);
			if(e.NewChecked!=null)
				newPanel=this.GetPanel(e.NewChecked);

			if(newPanel==null)
				return;

			PanelChangingEventArgs epanel=new PanelChangingEventArgs(oldPanel,newPanel);
			InvokePanelChanging(epanel);
			e.Cancel=epanel.Cancel;

		}
		protected virtual void InvokePanelChanging(PanelChangingEventArgs e)
		{
			if(PanelChanging!=null)
				PanelChanging(this,e);
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelTitle = new DevComponents.DotNetBar.PanelExTitle();
			this.navBar = new DevComponents.DotNetBar.NavigationBar();
			((ISupportInitialize)this.navBar).BeginInit();
			//this.SuspendLayout();
			// 
			// panelTitle
			// 
			this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panelTitle.Name = "panelTitle";
			this.panelTitle.Size = new System.Drawing.Size(150, 24);
			this.panelTitle.Style.Alignment = System.Drawing.StringAlignment.Near;
			this.panelTitle.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelTitle.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.panelTitle.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelTitle.Style.BorderSide = eBorderSide.Bottom;
			this.panelTitle.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.panelTitle.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelTitle.Style.GradientAngle = 90;
			this.panelTitle.Style.MarginLeft=4;
			this.panelTitle.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Near;
			this.panelTitle.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Near;
			this.panelTitle.TabIndex = 0;
            this.panelTitle.ExpandButtonVisible = false;
			this.panelTitle.Text = "Title";
			// 
			// navigationBar1
			// 
			this.navBar.BackgroundStyle.BackColor1.Color = System.Drawing.SystemColors.Control;
			this.navBar.BackgroundStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.navBar.BackgroundStyle.BorderSide = eBorderSide.Top;
			this.navBar.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			this.navBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.navBar.Location = new System.Drawing.Point(0, 128);
			this.navBar.Name = "navigationBar1";
			this.navBar.Size = new System.Drawing.Size(150, 32);
			this.navBar.TabIndex = 1;
			this.navBar.Text = "navBar";
			this.navBar.Resize+=new EventHandler(this.OnNavBarResize);
			this.navBar.ItemClick+=new EventHandler(this.OnNavBarItemClick);
			this.navBar.OptionGroupChanging+=new OptionGroupChangingEventHandler(this.OnNavBarOptionGroupChanging);
			this.navBar.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			// 
			// NavigationPane
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.navBar,
																		  this.panelTitle});
			this.Name = "NavigationPane";
			this.Size = new System.Drawing.Size(150, 192);
            this.DockPadding.All = 1;

			((ISupportInitialize)this.navBar).EndInit();
			//this.ResumeLayout(false);
		}
		#endregion
	}

	#region PanelChangingEventArgs 
	/// <summary>
	/// Represents event arguments for PanelChanging event.
	/// </summary>
	public class PanelChangingEventArgs : EventArgs
	{
		/// <summary>
		/// Set to true to cancel changing of the panel.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Panel that will be selected if operation is not cancelled.
		/// </summary>
		public readonly NavigationPanePanel NewPanel;
		/// <summary>
		/// Panel that is currently selected and which will be de-selected if operation is not cancelled.
		/// </summary>
		public readonly NavigationPanePanel OldPanel;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PanelChangingEventArgs(NavigationPanePanel oldpanel, NavigationPanePanel newpanel)
		{
			NewPanel=newpanel;
			OldPanel=oldpanel;
		}
	}
	#endregion

	#region PanelPopupEventArgs
	/// <summary>
	/// Represents event arguments for BeforePanelPopup event.
	/// </summary>
	public class PanelPopupEventArgs : EventArgs
	{
		/// <summary>
		/// Set to true to cancel popup of the panel.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Size and position of popup. You can modify this memeber to affect size and position of popup.
		/// </summary>
		public Rectangle PopupBounds=Rectangle.Empty;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PanelPopupEventArgs(Rectangle popupBounds)
		{
			this.PopupBounds=popupBounds;
		}
	}
	#endregion
}
