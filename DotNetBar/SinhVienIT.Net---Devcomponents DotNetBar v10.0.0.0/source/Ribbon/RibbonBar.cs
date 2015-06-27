using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents a single ribbon container control.
	/// </summary>
    [ToolboxBitmap(typeof(RibbonBar), "Ribbon.RibbonBar.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.RibbonBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
	public class RibbonBar:ItemControl
	{
		#region Events
		/// <summary>
		/// Occurs when dialog launcher button in title bar is clicked. Use DialogLauncherVisible property to show the button in title bar.
		/// </summary>
		[Description("Occurs when dialog launcher button in title bar is clicked. Use DialogLauncherVisible property to show the button in title bar.")]
		public event EventHandler LaunchDialog;

        /// <summary>
        /// Occurs when overflow button for control is created because control size is below the minimum size required to display complete content of the control.
        /// This event allows you to get access to the internal overflow button that is created and change it's properties if required.
        /// </summary>
        [Description("Occurs when overflow button for control is created because control size is below the minimum size required to display complete content of the control.")]
        public event OverflowButtonEventHandler OverflowButtonSetup;

        /// <summary>
        /// Occurs after overflow button setup is complete and all items contained by this control are moved to it.
        /// </summary>
        [Description("Occurs after overflow button setup is complete and all items contained by this control are moved to it.")]
        public event OverflowButtonEventHandler OverflowButtonSetupComplete;

        /// <summary>
        /// Occurs before overflow button is destroyed.
        /// </summary>
        [Description("Occurs before overflow button is destroyed.")]
        public event OverflowButtonEventHandler OverflowButtonDestroy;

        /// <summary>
        /// Occurs when mouse enters dialog launcher button.
        /// </summary>
        [Description("")]
        public event EventHandler DialogLauncherMouseEnter;

        /// <summary>
        /// Occurs when mouse leaves dialog launcher button.
        /// </summary>
        [Description("Occurs when mouse leaves dialog launcher button.")]
        public event EventHandler DialogLauncherMouseLeave;

        /// <summary>
        /// Occurs when mouse hovers over the dialog launcher button.
        /// </summary>
        [Description("Occurs when mouse hovers over the dialog launcher button.")]
        public event EventHandler DialogLauncherMouseHover;

        /// <summary>
        /// Occurs when mouse is pressed over the dialog launcher button.
        /// </summary>
        [Description("Occurs when mouse is pressed over the dialog launcher button.")]
        public event MouseEventHandler DialogLauncherMouseDown;
		#endregion

		#region Private Variables & Constructor
		private ItemContainer m_ItemContainer=null;
		private Rectangle m_TitleRectangle=Rectangle.Empty;
		private ElementStyle m_TitleStyle=null;
        private ElementStyle m_TitleStyleMouseOver = null;
		private SimpleElement m_TitleElement=new SimpleElement();
		private bool m_DialogLauncherVisible=false;
		private Rectangle m_DialogLauncherRect=Rectangle.Empty;
		private Image m_DialogLauncherButton=null;
		private Image m_DialogLauncherMouseOverButton=null;
		private bool m_MouseOverDialogLauncher=false;

        private bool m_AutoOverflow = true;
        private RibbonOverflowButtonItem m_OverflowButton = null;
        private RibbonBar m_OverflowRibbonBar = null;
        private string m_OverflowButtonText = "";
        private Image m_OverflowButtonImage = null;
        private int m_ResizeOrderIndex = 0;
        private bool m_SuspendRecalcSize = false;
        private int m_MaximumOverflowTextLength = 25;
		private Size m_BeforeOverflowSize=Size.Empty;
        private bool m_MouseOver = false;
        private ElementStyle m_BackgroundMouseOverStyle = new ElementStyle();
        private eRibbonTitlePosition m_TitlePosition = eRibbonTitlePosition.Bottom;
        private bool m_TitleVisible = true;
        private bool m_EnableDragDrop = false;
        private ElementStyle m_DefaultBackgroundStyle = new ElementStyle();
        private ElementStyle m_DefaultBackgroundMouseOverStyle = new ElementStyle();
        private ElementStyle m_DefaultTitleStyle = new ElementStyle();
        private ElementStyle m_DefaultTitleStyleMouseOver = new ElementStyle();
        private bool m_CanCustomize = true;
        private ArrayList m_AutoSizeBagList = new ArrayList();
        private bool m_AutoSizeItems = true;
        private Size m_FullItemsSize = Size.Empty;
        private Size m_LastReducedSize = Size.Empty;
        private string m_DialogLauncherAccessibleName = "";
        private bool m_IsOverflowRibbon = false;
        private GalleryContainer m_GalleryStretch = null;
        private Timer m_MouseOverTimer = null;
        private bool m_BackgroundHoverEnabled = true;
        private bool m_UseSpecKeyTipsPositioning = false;

		public RibbonBar()
		{
            //this.BackColor = Color.Transparent;
			m_ItemContainer=new ItemContainer();
			m_ItemContainer.GlobalItem=false;
			m_ItemContainer.ContainerControl=this;
			m_ItemContainer.Stretch=false;
			m_ItemContainer.Displayed=true;
			m_ItemContainer.Style=eDotNetBarStyle.StyleManagerControlled;
			this.ColorScheme.Style=eDotNetBarStyle.StyleManagerControlled;
			m_ItemContainer.SetOwner(this);
			m_ItemContainer.SetSystemContainer(true);
			this.SetBaseItemContainer(m_ItemContainer);
            this.DragDropSupport = true;
			m_TitleStyle=new ElementStyle();
			m_TitleStyle.SetColorScheme(this.GetColorScheme());
			m_TitleStyle.StyleChanged+=new EventHandler(ElementStyleChanged);

            m_TitleStyleMouseOver = new ElementStyle();
            m_TitleStyleMouseOver.SetColorScheme(this.GetColorScheme());
            m_TitleStyleMouseOver.StyleChanged += new EventHandler(ElementStyleChanged);

            m_BackgroundMouseOverStyle.SetColorScheme(this.GetColorScheme());
            m_BackgroundMouseOverStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);

            this.ItemAdded += new EventHandler(RibbonBar_ItemAdded);
            this.ItemRemoved += new ItemRemovedEventHandler(RibbonBar_ItemRemoved);
		}

        protected override void Dispose(bool disposing)
        {
            DestroyMouseOverTimer();
            if (m_OverflowRibbonBar != null)
            {
                m_OverflowRibbonBar.Dispose();
                m_OverflowRibbonBar = null;
            }

            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_DialogLauncherButton);
                BarUtilities.DisposeImage(ref m_OverflowButtonImage);
                BarUtilities.DisposeImage(ref m_DialogLauncherMouseOverButton);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates new accessibility instance.
        /// </summary>
        /// <returns>Reference to AccessibleObject.</returns>
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new RibbonBarAccessibleObject(this);
        }
		#endregion

        #region Internal Implementation
        private ButtonItem _QatButtonParent;
        /// <summary>
        /// Gets or sets parent button when on QAT.
        /// </summary>
        [Browsable(false)]
        internal DevComponents.DotNetBar.ButtonItem QatButtonParent
        {
            get
            {
                return _QatButtonParent;
            }
            set
            {
                _QatButtonParent = value;
            }
        }
        private bool _IsOnQat;
        /// <summary>
        /// Gets or sets whether this RibbonBar is on QAT.
        /// </summary>
        [Browsable(false)]
        internal bool IsOnQat
        {
            get
            {
                return _IsOnQat;
            }
            set
            {
                _IsOnQat = value;
            }
        }

        /// <summary>
        /// Gets or sets whether Office 2007 Design Guidelines specification for positioning KeyTips is used.
        /// </summary>
        [DefaultValue(false), Category("KeyTips"), Description("Indicates whether Office 2007 Design Guidelines specification for positioning KeyTips is used.")]
        public bool UseSpecKeyTipsPositioning
        {
            get { return m_UseSpecKeyTipsPositioning; }
            set
            {
                m_UseSpecKeyTipsPositioning = value;
            }
        }

        protected override void OnItemClick(BaseItem item)
        {
            base.OnItemClick(item);

            if (this.IsOverflowRibbon && this.Parent is MenuPanel && item!=null && item.AutoCollapseOnClick && item is ButtonItem && !item.Name.StartsWith("sysgallery"))
            {
                ButtonItem b = item as ButtonItem;
                if(!b.AutoExpandOnClick)
                    CloseOverflowPopup();
            }
        }

        /// <summary>
        /// Closes the RibbonBar overflow popup if control is in overflow mode and displays the overflow popup that shows complete content of the control.
        /// </summary>
        public void CloseOverflowPopup()
        {
            if (this.IsOverflowRibbon)
            {
                if (this.Parent is MenuPanel)
                {
                    RibbonOverflowButtonItem rb = ((MenuPanel)this.Parent).ParentItem as RibbonOverflowButtonItem;
                    if (rb != null && rb.Expanded)
                        rb.Expanded = false;
                }
            }
            else if (this.OverflowState && m_OverflowButton != null && m_OverflowButton.Expanded)
            {
                m_OverflowButton.Expanded = false;
            }
        }

        /// <summary>
        /// Gets or sets the Accessible name for the Dialog Launcher button
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Accessibility"), Description("Indicates Accessible name for the Dialog Launcher button.")]
        public string DialogLauncherAccessibleName
        {
            get { return m_DialogLauncherAccessibleName; }
            set { m_DialogLauncherAccessibleName = value; }
        }

        /// <summary>
        /// Specifies the background style of the control when mouse is over the control. Style specified here will be applied to the
        /// BackgroundStyle.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Indicates mouse over background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundMouseOverStyle
        {
            get { return m_BackgroundMouseOverStyle; }
            //#if FRAMEWORK20
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_BackgroundMouseOverStyle = value;
            //}
            //#endif
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackgroundMouseOverStyle()
		{
			m_BackgroundMouseOverStyle.StyleChanged-=new EventHandler(this.VisualPropertyChanged);
			m_BackgroundMouseOverStyle=new ElementStyle();
			m_BackgroundMouseOverStyle.StyleChanged+=new EventHandler(this.VisualPropertyChanged);
            this.Invalidate();
		}

        protected virtual ElementStyle GetBackgroundMouseOverStyle()
        {
            if (!m_BackgroundHoverEnabled) return null;

            if (this.OverflowState && m_OverflowButton.Expanded)
                return this.GetBackgroundStyle();

            if (m_BackgroundMouseOverStyle.Custom)
            {
                ElementStyle copy = m_DefaultBackgroundMouseOverStyle.Copy();
                copy.ApplyStyle(m_BackgroundMouseOverStyle);
                return copy;
            }
            return m_DefaultBackgroundMouseOverStyle;
        }

        protected override ElementStyle GetBackgroundStyle()
        {
            if (this.OverflowState && m_OverflowButton.Expanded)
            {
                Rendering.Office2007Renderer r = this.GetRenderer() as Rendering.Office2007Renderer;
                if (r != null)
                {
                    return RibbonPredefinedColorSchemes.StyleFromRibbonBarStateColorTable(r.ColorTable.RibbonBar.Expanded);
                }
            }
            if (this.BackgroundStyle.Custom)
            {
                ElementStyle copy = m_DefaultBackgroundStyle.Copy();
                copy.ApplyStyle(BackgroundStyle);
                return copy;
            }
            return m_DefaultBackgroundStyle;
        }

        private ElementStyle GetTitleStyle()
        {
            if (m_TitleStyle.Custom)
            {
                ElementStyle copy = m_DefaultTitleStyle.Copy();
                copy.ApplyStyle(m_TitleStyle);
                return copy;
            }
            return m_DefaultTitleStyle;
        }

        private ElementStyle GetTitleMouseOverStyle()
        {
            if (!m_BackgroundHoverEnabled)
                return null;

            if (m_TitleStyleMouseOver.Custom)
            {
                ElementStyle copy = m_DefaultTitleStyleMouseOver.Copy();
                copy.ApplyStyle(m_TitleStyleMouseOver);
                return copy;
            }
            return m_DefaultTitleStyleMouseOver;
        }

        private void VisualPropertyChanged(object sender, EventArgs e)
        {
            if (this.GetDesignMode())
            {
                this.RecalcLayout();
            }
            else if (this.Parent != null && this.Parent.Site != null && this.Parent.Site.DesignMode)
            {
                this.RecalcLayout();
            }
        }

		/// <summary>
		/// Gets or sets image that is used as dialog luncher button in ribbon title bar. Default value is null which indicates that
		/// default appearance for the button is used.
		/// </summary>
		[Browsable(true),DefaultValue(null), Category("Appearance"),Description("Indicates image that is used as dialog luncher button in ribbon title bar.")]
		public Image DialogLauncherButton
		{
			get {return m_DialogLauncherButton;}
			set
			{
				m_DialogLauncherButton=value;
				OnDialogLuncherButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets image that is used as dialog luncher button when mouse is over the button.
		/// Note that if this property is set you also must set the DialogLauncherButton property. Images set to both
		/// properties must have same size. Default value is null which indicates that
		/// default appearance for the button is used.
		/// </summary>
		[Browsable(true),DefaultValue(null), Category("Appearance"),Description("Indicates image that is used as dialog launcher button when mouse is over the button.")]
		public Image DialogLauncherMouseOverButton
		{
			get {return m_DialogLauncherMouseOverButton;}
			set
			{
				m_DialogLauncherMouseOverButton=value;
			}
		}

	    /// <summary>
	    /// Gets or sets maximum text length for automatic overflow button text. When overflow button is created due to the
        /// reduced size of the control text for the button can be specified using OverflowButtonText property. If
        /// text is not specified RibbonBar.Text property is used as overflow button text. In that case
        /// this property specifies maximum length of the text to display on the button. Default value is 25. You can set
        /// this property to 0 to use complete text regardless of length.
	    /// </summary>
        [DefaultValue(25), Category("Overflow"), Browsable(true), Description("Indicates maximum text length for automatic overflow button text.")]
	    public int MaximumOverflowTextLength
	    {
            get
            {
                return m_MaximumOverflowTextLength;
            }
	        set
	        {
                m_MaximumOverflowTextLength = value;
	        }
	    }
        /// <summary>
        /// Gets or sets resize order index of the control. When control is parented to RibbonPanel control (which is the case when control is
        /// used as part of RibbonControl) index specified here indicates the order in which controls that are part of the same panel
        /// are resized. Lower index value indicates that control should be resized later when size needs to be reduced or earlier when size needs
        /// to be increased. Default value is 0.
        /// </summary>
        [Browsable(true), Category("Overflow"), DefaultValue(0), Description("Indicates resize order index of the control.")]
        public int ResizeOrderIndex
        {
            get { return m_ResizeOrderIndex; }
            set { m_ResizeOrderIndex = value; }
        }

        /// <summary>
        /// Gets or sets whether automatic overflow handling is enabled. When overflow is enabled if control is resized below the
        /// size that is needed to display its complete content overflow button is created and all content is moved to the overflow button
        /// popup. Control will only display overflow button when in this state and user can click overflow button to display the actual
        /// content of the control.
        /// Default value is true.
        /// </summary>
        public bool AutoOverflowEnabled
        {
            get { return m_AutoOverflow; }
            set
            {
                if (m_AutoOverflow != value)
                {
                    m_AutoOverflow = value;
                    this.OnAutoOverflowChanged();
                }
            }
        }

        private void OnAutoOverflowChanged()
        {
            if (m_OverflowButton != null)
                DisposeOverflowButton();
            this.RecalcLayout();
        }

        /// <summary>
        /// Gets or sets the text for overflow button that is created when ribbon bar size is reduced so it cannot display all its content.
        /// When control is resized so it cannot display its content overflow button is created which is displayed on face of the control.
        /// Complete content of the control is then displayed on popup toolbar when user clicks overflow button.
        /// </summary>
        [Browsable(true), DefaultValue(""), Description("Indicates text for overflow button that is created when ribbon bar size is reduced so it cannot display all its content"), Category("Overflow")]
        public string OverflowButtonText
        {
            get { return m_OverflowButtonText; }
            set
            {
                m_OverflowButtonText = value;
                OnOverflowButtonChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Image for overflow button that is created when ribbon bar size is reduced so it cannot display all its content.
        /// When control is resized so it cannot display its content overflow button is created which is displayed on face of the control.
        /// Complete content of the control is then displayed on popup toolbar when user clicks overflow button. This Image is also used when
        /// RibbonBar is added to the Quick Access Toolbar to identify the RibbonBar button.
        /// </summary>
        [Browsable(true), DefaultValue(null), Description("Indicates image for overflow button that is created when ribbon bar size is reduced so it cannot display all its content"), Category("Overflow")]
        public Image OverflowButtonImage
        {
            get { return m_OverflowButtonImage; }
            set
            {
                m_OverflowButtonImage = value;
                OnOverflowButtonChanged();
            }
        }

        private void OnOverflowButtonChanged()
        {
            // Updates overflow button properties if button exists...
            if (m_OverflowButton != null)
            {
                m_OverflowButton.Text = m_OverflowButtonText;
                m_OverflowButton.Image = m_OverflowButtonImage;
            }
        }

        /// <summary>
        /// Invokes the LaunchDialog event to execute default launch dialog action.
        /// </summary>
        public void DoLaunchDialog()
        {
            InvokeLaunchDialog(new EventArgs());
        }

		protected virtual void InvokeLaunchDialog(EventArgs e)
		{
			if(LaunchDialog!=null)
				LaunchDialog(this,e);
		}

		/// <summary>
		/// Gets or sets whether dialog launcher button is visible in title of the ribbon. Default value is false.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Behavior"),Description("Indicates whether dialog launcher button is visible in title of the ribbon.")]
		public bool DialogLauncherVisible
		{
			get {return m_DialogLauncherVisible;}
			set
			{
				m_DialogLauncherVisible=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}
		/// <summary>
		/// Gets or sets default layout orientation inside the control. You can have multiple layouts inside of the control by adding
		/// one or more instances of the ItemContainer object and changing it's LayoutOrientation property.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Layout"), DefaultValue(eOrientation.Horizontal), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual eOrientation LayoutOrientation
		{
			get {return m_ItemContainer.LayoutOrientation;}
			set
			{
				m_ItemContainer.LayoutOrientation=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

        /// <summary>
        /// Gets or sets spacing in pixels between items. Default value is 1.
        /// </summary>
        [Browsable(true), DefaultValue(1), Category("Layout"), Description("Indicates spacing in pixels between items.")]
        public int ItemSpacing
        {
            get { return m_ItemContainer.ItemSpacing; }
            set
            {
                m_ItemContainer.ItemSpacing = value;
                if (this.DesignMode) RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether items contained by container are resized to fit the container bounds. When container is in horizontal
        /// layout mode then all items will have the same height. When container is in vertical layout mode then all items
        /// will have the same width. Default value is true.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(true), Category("Layout")]
        public bool ResizeItemsToFit
        {
            get { return m_ItemContainer.ResizeItemsToFit; }
            set
            {
                m_ItemContainer.ResizeItemsToFit = value;
                if (this.DesignMode) RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the item alignment when container is in horizontal layout. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalItemsAlignment.Left), Category("Layout"), Description("Indicates item alignment when container is in horizontal layout."), DevCoBrowsable(true)]
        public eHorizontalItemsAlignment HorizontalItemAlignment
        {
            get { return m_ItemContainer.HorizontalItemAlignment; }
            set
            {
                m_ItemContainer.HorizontalItemAlignment = value;
                if (this.DesignMode) RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the item vertical alignment. Default value is Top.
        /// </summary>
        [Browsable(true), DefaultValue(eVerticalItemsAlignment.Top), Category("Layout"), Description("Indicates item item vertical alignment."), DevCoBrowsable(true)]
        public eVerticalItemsAlignment VerticalItemAlignment
        {
            get { return m_ItemContainer.VerticalItemAlignment; }
            set
            {
                m_ItemContainer.VerticalItemAlignment = value;
                if (this.DesignMode) RecalcLayout();
            }
        }

		/// <summary>
		/// Returns collection of items on a bar.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
		public SubItemsCollection Items
		{
			get
			{
                if (OverflowState && !this.DesignMode)
                    return m_OverflowRibbonBar.Items; // m_OverflowButton.SubItems;
				return m_ItemContainer.SubItems;
			}
		}

        /// <summary>
        /// Called when item on popup container is right-clicked.
        /// </summary>
        /// <param name="item">Instance of the item that is right-clicked.</param>
        protected override void OnPopupItemRightClick(BaseItem item)
        {
            if (item != null && !item.CanCustomize) return;

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null)
            {
                rc.ShowCustomizeContextMenu(item, false);
            }
        }

        internal Rectangle DialogLauncherRect
        {
            get { return m_DialogLauncherRect; }
        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
            if (m_DialogLauncherRect.Contains(e.X, e.Y) && !m_DialogLauncherRect.IsEmpty && m_TitleVisible && !this.OverflowState)
            {
                this.Invalidate(m_DialogLauncherRect);
                OnDialogLauncherMouseDown(e);
            }

            if (e.Button == MouseButtons.Right)
            {
                RibbonControl rc = GetRibbonControl();
                if(rc!=null)
                    rc.OnRibbonBarRightClick(this, e.X, e.Y);
            }
		}

        /// <summary>
        /// Gets or sets whether ribbon bar can be customized by end user i.e. added to Quick Access Toolbar.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether ribbon bar can be customized by end user i.e. added to Quick Access Toolbar.")]
        public virtual bool CanCustomize
        {
            get
            {
                return m_CanCustomize;
            }
            set
            {
                m_CanCustomize = value;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (m_DialogLauncherRect.Contains(e.X, e.Y) && e.Button == MouseButtons.Left)
            {
                if(!m_DialogLauncherRect.IsEmpty)
                    this.Invalidate(m_DialogLauncherRect);
                if (this.Parent is MenuPanel)
                {
                    PopupItem item = ((MenuPanel)this.Parent).ParentItem as PopupItem;
                    if (item != null)
                        item.ClosePopup();
                }
                else if (this.Parent is Bar && ((Bar)this.Parent).BarState == eBarState.Popup)
                {
                    PopupItem item = ((Bar)this.Parent).ParentItem as PopupItem;
                    if (item != null)
                        item.ClosePopup();
                }
                InvokeLaunchDialog(new EventArgs());
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            SetMouseOver(true); // m_MouseOver = true;
            if ((m_TitleStyleMouseOver != null && m_TitleStyleMouseOver.Custom || m_DefaultTitleStyleMouseOver.Custom) && m_TitleVisible && !this.OverflowState)
            {
                this.Invalidate(GetTitleRectangle());
            }
            ElementStyle mouseOverStyle = GetBackgroundMouseOverStyle();
            if (mouseOverStyle != null && mouseOverStyle.Custom)
                this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            Point p = this.PointToClient(Control.MousePosition);
            if (this.ClientRectangle.Contains(p))
            {
                Form form = this.FindForm();
                if (form != null && Form.ActiveForm == form)
                {
                    // Mouse is over the control inside of the RibbonBar, event should be ignored...
                    CreateMouseOverTimer();
                    return;
                }
            }
            
            SetMouseOver(false); // m_MouseOver = false;
			
			SetMouseOverDialogLauncher(false);

            if ((m_TitleStyleMouseOver != null && m_TitleStyleMouseOver.Custom || m_DefaultTitleStyleMouseOver.Custom) && m_TitleVisible && !this.OverflowState)
            {
                this.Invalidate(GetTitleRectangle());
            }
            ElementStyle mouseOverStyle = GetBackgroundMouseOverStyle();
            if (mouseOverStyle != null && mouseOverStyle.Custom)
                this.Invalidate();
        }

        private void CreateMouseOverTimer()
        {
            if (m_MouseOverTimer != null) return;

            m_MouseOverTimer = new Timer();
            m_MouseOverTimer.Interval = 500;
            m_MouseOverTimer.Tick += new EventHandler(MouseOverTimer_Tick);
            m_MouseOverTimer.Enabled = true;
            m_MouseOverTimer.Start();
        }

        private void MouseOverTimer_Tick(object sender, EventArgs e)
        {
            Point p = this.PointToClient(Control.MousePosition);
            if (!this.ClientRectangle.Contains(p) || !this.Visible)
            {
                m_MouseOverTimer.Stop();
                m_MouseOverTimer.Enabled = false;
                SetMouseOver(false);
                DestroyMouseOverTimer();
            }
        }

        private void DestroyMouseOverTimer()
        {
            if (m_MouseOverTimer != null)
            {
                m_MouseOverTimer.Stop();
                m_MouseOverTimer.Dispose();
                m_MouseOverTimer = null;
            }
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(m_DialogLauncherVisible)
			{
				if(m_DialogLauncherRect.Contains(e.X,e.Y))
					SetMouseOverDialogLauncher(true);
				else
					SetMouseOverDialogLauncher(false);
			}
		}

        protected override void InternalMouseMove(MouseEventArgs e)
        {
            if (m_TitleVisible && !m_TitleRectangle.IsEmpty && m_TitleRectangle.Contains(e.X, e.Y))
            {
                if (m_ItemContainer.HotSubItem != null)
                    m_ItemContainer.InternalMouseLeave();
                return;
            }
            base.InternalMouseMove(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (m_MouseOverDialogLauncher)
                OnDialogLauncherMouseHover(new EventArgs());
        }

		private void SetMouseOverDialogLauncher(bool value)
		{
			if(m_MouseOverDialogLauncher!=value)
			{
                m_MouseOverDialogLauncher=value;
                
                this.Invalidate(m_DialogLauncherRect);

                if (value)
                    OnDialogLauncherMouseEnter(new EventArgs());
                else
                    OnDialogLauncherMouseLeave(new EventArgs());
			}
		}

        /// <summary>
        /// Invokes DialogLauncherMouseEnter event.
        /// </summary>
        protected virtual void OnDialogLauncherMouseEnter(EventArgs e)
        {
            if (DialogLauncherMouseEnter != null)
                DialogLauncherMouseEnter(this, e);
        }

        /// <summary>
        /// Invokes DialogLauncherMouseLeave event.
        /// </summary>
        protected virtual void OnDialogLauncherMouseLeave(EventArgs e)
        {
            if (DialogLauncherMouseLeave != null)
                DialogLauncherMouseLeave(this, e);
        }

        /// <summary>
        /// Invokes DialogLauncherMouseHover event.
        /// </summary>
        protected virtual void OnDialogLauncherMouseHover(EventArgs e)
        {
            if (DialogLauncherMouseHover != null)
                DialogLauncherMouseHover(this, e);
        }

        protected virtual void OnDialogLauncherMouseDown(MouseEventArgs e)
        {
            if (DialogLauncherMouseDown != null)
                DialogLauncherMouseDown(this, e);
        }

        /// <summary>
        /// Returns the collection of items with the specified name.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName)
        {
            RibbonControl rc = GetRibbonControl();
            if (rc == null)
                return base.GetItems(ItemName);
            return rc.RibbonStrip.GetItems(ItemName);
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName, Type itemType)
        {
            RibbonControl rc = GetRibbonControl();
            if (rc == null)
                return base.GetItems(ItemName, itemType);
            return rc.RibbonStrip.GetItems(ItemName, itemType);
        }

        /// <summary>
        /// Returns the first item that matches specified name.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        public override BaseItem GetItem(string ItemName)
        {
            RibbonControl rc = GetRibbonControl();
            if (rc == null)
            {
                BaseItem item = base.GetItem(ItemName);
                return item;
            }
            return rc.RibbonStrip.GetItem(ItemName);
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="useGlobalName">Indicates whether GlobalName property is used for searching.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            RibbonControl rc = GetRibbonControl();
            if (rc == null)
                return base.GetItems(ItemName, itemType, useGlobalName);

            return rc.RibbonStrip.GetItems(ItemName, itemType, useGlobalName);
        }

        protected override void OnShowKeyTipsChanged()
        {
            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && this.ShowKeyTips)
            {
                rc.RibbonStrip.OnRibbonBarShowKeyTips(this);
            }
            base.OnShowKeyTipsChanged();
        }

        protected override BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
        {
            BaseItem item = base.GetItemForMnemonic(container, charCode, deepScan, stackKeys);
            
            if (item ==null && this.OverflowState)
            {
                item = m_OverflowRibbonBar.GetItemForMnemonic(m_OverflowRibbonBar.m_ItemContainer, charCode, deepScan, stackKeys);
            }
            return item;
        }
		#endregion

		#region Layout Support
		protected override void OnTextChanged(EventArgs e)
		{
			m_TitleElement.Text=this.Text;
            if (m_OverflowButton != null) m_OverflowButton.Text = GetOverflowButtonText();
			this.RecalcLayout();
			base.OnTextChanged (e);
		}

		private void OnDialogLuncherButtonChanged()
		{
			this.RecalcLayout();
		}

        private void InitDefaultStyles()
        {
            ColorScheme cs = this.GetColorScheme();
            m_DefaultBackgroundStyle.SetColorScheme(cs);
            m_DefaultBackgroundMouseOverStyle.SetColorScheme(cs);
            m_DefaultTitleStyle.SetColorScheme(cs);
            m_DefaultTitleStyle.HideMnemonic = true;
            m_DefaultTitleStyleMouseOver.SetColorScheme(cs);
            m_DefaultTitleStyleMouseOver.HideMnemonic = true;

            // Initialize Default Styles
            RibbonPredefinedColorSchemes.ApplyRibbonBarElementStyle(m_DefaultBackgroundStyle, m_DefaultBackgroundMouseOverStyle,
                m_DefaultTitleStyle, m_DefaultTitleStyleMouseOver, this);
        }

        private void RibbonBar_ItemRemoved(object sender, ItemRemovedEventArgs e)
        {
            if (sender == m_GalleryStretch)
                m_GalleryStretch = null;
        }

        private void RibbonBar_ItemAdded(object sender, EventArgs e)
        {
            if (sender is GalleryContainer)
            {
                GalleryContainer gc = sender as GalleryContainer;
                if (gc.StretchGallery && m_GalleryStretch == null)
                    m_GalleryStretch = gc;
            }
        }

        internal GalleryContainer GalleryStretch
        {
            get { return m_GalleryStretch; }
            set { m_GalleryStretch = value; }
        }

        private Size GetItemContainerMinimumSize()
        {
			Rectangle r=base.GetItemContainerRectangle();
            if (m_TitleVisible)
            {
                Rectangle rTitle = GetTitleRectangle();
                if (m_TitlePosition == eRibbonTitlePosition.Top)
                {
                    rTitle.Height++;
                    r.Y += rTitle.Height;
                    r.Height -= rTitle.Height;
                }
                else
                {
                    rTitle.Height++;
                    r.Height -= rTitle.Height;
                }
            }

			return r.Size;
        }

		protected override void RecalcSize()
		{
            if (!BarFunctions.IsHandleValid(this) || m_SuspendRecalcSize)
				return;
            
            InitDefaultStyles();
            ElementStyle backStyle = GetBackgroundStyle();
            if (m_TitleVisible)
            {
                SimpleElementLayoutInfo li = new SimpleElementLayoutInfo();
                m_TitleElement.Image = m_DialogLauncherButton;
                if (m_DialogLauncherButton != null && m_DialogLauncherVisible)
                {
                    m_TitleElement.ImageAlignment = eSimplePartAlignment.FarCenter;
                }

                li.Element = m_TitleElement;
                li.LayoutStyle = GetTitleStyle();
                li.Font = this.Font;
                li.LeftToRight = (this.RightToLeft != RightToLeft.Yes);
                li.VerticalPartAlignment = false;
                li.Left = this.ClientRectangle.X;
                li.Top = this.ClientRectangle.Y;
                m_TitleElement.FixedWidth = this.ClientRectangle.Width;

                if (backStyle != null)
                {
                    //if (backStyle.PaintLeftBorder)
                    //{
                    //    li.Left += backStyle.BorderLeftWidth;
                    //    m_TitleElement.FixedWidth -= backStyle.BorderLeftWidth;
                    //}

                    if (backStyle.PaintRightBorder)
                        m_TitleElement.FixedWidth -= backStyle.BorderRightWidth;

                    if (backStyle.PaintTopBorder)
                        li.Top += backStyle.BorderTopWidth;
                }

                using (Graphics g = this.CreateGraphics())
                {
                    li.Graphics = g;
                    SimpleElementLayout.LayoutSimpleElement(li);
                    if (m_TitlePosition == eRibbonTitlePosition.Bottom)
                    {
                        li.Top = this.ClientRectangle.Bottom - m_TitleElement.Bounds.Height;
                        if (backStyle != null && backStyle.PaintBottomBorder)
                            li.Top -= backStyle.BorderBottomWidth;
                        SimpleElementLayout.LayoutSimpleElement(li);
                    }
                }
                m_TitleRectangle = m_TitleElement.Bounds;
            }
            else
                m_TitleRectangle = Rectangle.Empty;

            RestoreAutoSizedItems();
            m_FullItemsSize = Size.Empty;

            if (this.Parent is RibbonPanel)
            {
                RibbonControl rc = this.GetRibbonControl();
                if ((rc != null) && !rc.AutoSize)
                {
                    // The height of the RibbonBar is controlled by the height of the panel so whatever height is here we apply to ItemContainer
                    m_ItemContainer.MinimumSize = new Size(0, GetItemContainerMinimumSize().Height);
                }
            }
            else if(this.IsOverflowRibbon && !this.AutoSize)
                m_ItemContainer.MinimumSize = new Size(0, GetItemContainerMinimumSize().Height);
            else
                m_ItemContainer.MinimumSize = Size.Empty;

			base.RecalcSize();

            Size contentSize = this.GetContentBasedSize();
            if (ResizeGallery(contentSize.Width))
            {
                m_LastReducedSize = Size.Empty;
                return;
            }

            if (this.AutoOverflowEnabled && !this.DesignMode)
            {    
                if (contentSize.Width > this.Width)
                {
                    if (m_AutoSizeBagList.Count == 0 && m_AutoSizeItems && this.Width > 1 && !this.OverflowState)
                    {
                        // Try to reduce the size of the Ribbon by manipulating size of the items
                        AutoReduceItemSize(contentSize.Width);
                        base.RecalcSize();
                        Size sa = this.GetContentBasedSize();
                        if (sa.Width > this.Width)
                        {
                            RestoreAutoSizedItems();
                        }
                        else
                        {
                            m_LastReducedSize = sa;
                            m_FullItemsSize = contentSize;
                            contentSize = sa;
                        }
                    }

                    if (!this.OverflowState && contentSize.Width > this.Width)
                    {
                        m_BeforeOverflowSize = contentSize;
                        this.SetupOverflowButton();
                        base.RecalcSize();
                    }
                }
                else if (this.OverflowState)
                {
                    if (m_BeforeOverflowSize.IsEmpty)
                    {
                        this.DisposeOverflowButton();
                        base.RecalcSize();
                        Size newSize = this.GetContentBasedSize();
                        if (newSize.Width > this.Width)
                        {
                            this.SetupOverflowButton();
                            base.RecalcSize();
                        }
                    }
                    else if (m_BeforeOverflowSize.Width <= this.Width)
                    {
                        this.DisposeOverflowButton();
                        base.RecalcSize();
                        contentSize = this.GetContentBasedSize();
                        if (ResizeGallery(contentSize.Width))
                            m_LastReducedSize = Size.Empty;
                    }
                    else if (!m_LastReducedSize.IsEmpty && m_LastReducedSize.Width <= this.Width)
                    {
                        this.DisposeOverflowButton();
                        AutoReduceItemSize(m_LastReducedSize.Width);
                        base.RecalcSize();
                        contentSize = this.GetContentBasedSize();
                        if (ResizeGallery(contentSize.Width))
                            m_LastReducedSize = Size.Empty;
                    }
                }
                else
                    m_LastReducedSize = Size.Empty;
            }
		}

        internal Size BeforeOverflowSize
        {
            get { return m_BeforeOverflowSize; }
        }

        internal Size LastReducedSize
        {
            get { return m_LastReducedSize; }
        }

        private bool ResizeGallery(int contentBasedWidth)
        {
            if (m_GalleryStretch == null || !m_GalleryStretch.Visible)
                return false;
            // Check whether gallery can be reduced/increased in size
            int diff = this.Width - contentBasedWidth;

            if (m_GalleryStretch.DisplayRectangle.Width + diff >= m_GalleryStretch.MinimumSize.Width)
            {
                int previousWidth = m_GalleryStretch.DisplayRectangle.Width;
                m_GalleryStretch.RecommendedSize = new Size(m_GalleryStretch.DisplayRectangle.Width + diff, m_GalleryStretch.DefaultSize.Height);
                m_GalleryStretch.RecalcSize();
                int offset = m_GalleryStretch.DisplayRectangle.Width - previousWidth;
                OffsetItems(m_GalleryStretch.Parent, m_GalleryStretch.Parent.SubItems.IndexOf(m_GalleryStretch) + 1, offset);
                return Math.Abs(offset) >= Math.Abs(diff);
            }

            return false;
        }

        private void OffsetItems(BaseItem parent, int startOffsetIndex, int offset)
        {
            int c = parent.SubItems.Count;
            for (int i = startOffsetIndex; i < c; i++)
            {
                BaseItem item = parent.SubItems[i];
                if (item.Visible)
                {
                    Rectangle r = item.Bounds;
                    r.Offset(offset, 0);
                    item.Bounds = r;
                }
            }
            parent.SetDisplayRectangle(new Rectangle(parent.DisplayRectangle.X, parent.DisplayRectangle.Y, parent.DisplayRectangle.Width + offset, parent.DisplayRectangle.Height));
            //parent.WidthInternal = parent.WidthInternal + offset;
        }

        private void AutoReduceItemSize(int contentBasedWidth)
        {
            if (m_AutoSizeBagList.Count > 0) return;

            // Reduce the size of gallery if any
            if (m_GalleryStretch != null) m_GalleryStretch.RecommendedSize = new Size(m_GalleryStretch.MinimumSize.Width, m_GalleryStretch.HeightInternal);

            // Go through all items and change properties to try to reduce the ribbon size
            AutoReduceItemContainerSize(m_ItemContainer);
        }

        private void AutoReduceItemContainerSize(ItemContainer cont)
        {
            if (cont.LayoutOrientation == eOrientation.Vertical)
            {
                foreach (BaseItem item in cont.SubItems)
                {
                    ButtonItem b = item as ButtonItem;
                    if (b != null && b.ButtonStyle == eButtonStyle.ImageAndText && (b.ImagePosition == eImagePosition.Left || b.ImagePosition == eImagePosition.Right))
                    {
                        BaseItemAutoSizeBag bag = AutoSizeBagFactory.CreateAutoSizeBag(b);
                        bag.RecordSetting(b);
                        bool gi = b.GlobalItem;
                        b.GlobalItem = false;
                        b.ButtonStyle = eButtonStyle.Default;
                        b.ImageFixedSize = new Size(16, 16);
                        b.UseSmallImage = true;
                        b.GlobalItem = gi;
                        m_AutoSizeBagList.Add(bag);
                    }
                    else if (item is ItemContainer && !(item is GalleryContainer))
                        AutoReduceItemContainerSize(item as ItemContainer);
                }
            }
            else
            {
                ArrayList buttons = new ArrayList();
                foreach (BaseItem item in cont.SubItems)
                {
                    if (item is ItemContainer && !(item is GalleryContainer))
                        AutoReduceItemContainerSize(item as ItemContainer);
                    else if (item is ButtonItem)
                    {
                        ButtonItem b = item as ButtonItem;
                        if (b.ImagePosition == eImagePosition.Top || b.ImagePosition == eImagePosition.Bottom)
                        {
                            buttons.Add(b);
                        }
                    }
                }
                if (buttons.Count > 1)
                {
                    int c = buttons.Count / 3;
                    int lowerBound = buttons.Count - c * 3;
                    for (int i = buttons.Count - 1; i >=lowerBound; i--)
                    {
                        ButtonItem b=buttons[i] as ButtonItem;
                        BaseItemAutoSizeBag bag = AutoSizeBagFactory.CreateAutoSizeBag(b);
                        bag.RecordSetting(b);
                        bool gi = b.GlobalItem;
                        b.GlobalItem = false;
                        b.ImagePosition = eImagePosition.Left;
                        b.ButtonStyle = eButtonStyle.ImageAndText;
                        b.ImageFixedSize = new Size(16, 16);
                        if (b.TextMarkupBody != null && b.TextMarkupBody.HasExpandElement) b.Text = TextMarkup.MarkupParser.RemoveExpand(b.Text);
                        b.GlobalItem = gi;
                        m_AutoSizeBagList.Add(bag);
                    }
                    BaseItemAutoSizeBag bagCont = AutoSizeBagFactory.CreateAutoSizeBag(cont);
                    m_AutoSizeBagList.Add(bagCont);
                    bagCont.RecordSetting(cont);
                    cont.LayoutOrientation = eOrientation.Vertical;
                    cont.MultiLine = true;
                }
            }
        }

        internal void RestoreAutoSizedItems()
        {
            if (m_GalleryStretch != null) m_GalleryStretch.RecommendedSize = Size.Empty;
            if (m_AutoSizeBagList.Count == 0) return;

            BaseItemAutoSizeBag[] bagItems = new BaseItemAutoSizeBag[m_AutoSizeBagList.Count];
            m_AutoSizeBagList.CopyTo(bagItems);
            foreach (BaseItemAutoSizeBag ab in bagItems)
                ab.RestoreSettings();

            m_AutoSizeBagList.Clear();
        }

        internal bool IsItemsAutoSizeActive
        {
            get
            {
                return (m_AutoSizeBagList.Count>0);
            }
        }


        internal bool IsOverflowRibbon
        {
            get { return m_IsOverflowRibbon; }
            set { m_IsOverflowRibbon = value; }
        }

        private RibbonBar CreateOverflowRibbon()
        {
            return CreateOverflowRibbon(false);
        }

        internal RibbonBar CreateOverflowRibbon(bool createHandle)
        {
            RibbonBar bar = new RibbonBar();
            bar.ShortcutsEnabled = false;
            bar.AutoOverflowEnabled = false;
            bar.Style = this.Style;
            bar.BackgroundStyle.ApplyStyle(this.BackgroundStyle);
            bar.BackgroundMouseOverStyle.ApplyStyle(this.BackgroundMouseOverStyle);
            bar.Text = this.Text;
            bar.TitleStyle.ApplyStyle(this.TitleStyle);
            bar.TitleStyleMouseOver.ApplyStyle(this.TitleStyleMouseOver);
            bar.LayoutOrientation = this.LayoutOrientation;
            CopyIOwnerEvents(bar);
            bar.DialogLauncherVisible = this.DialogLauncherVisible;
            bar.DialogLauncherMouseDown = this.DialogLauncherMouseDown;
            bar.DialogLauncherMouseEnter = this.DialogLauncherMouseEnter;
            bar.DialogLauncherMouseHover = this.DialogLauncherMouseHover;
            bar.DialogLauncherMouseLeave = this.DialogLauncherMouseLeave;
            bar.DialogLauncherButton = this.DialogLauncherButton;
            bar.DialogLauncherMouseOverButton = this.DialogLauncherMouseOverButton;
            bar.Images = this.Images;
            bar.ImagesLarge = this.ImagesLarge;
            bar.ImagesMedium = this.ImagesMedium;
            bar.FadeEffect = this.FadeEffect;
            bar.LaunchDialog = this.LaunchDialog;
            bar.ItemSpacing= this.ItemSpacing;
            bar.HorizontalItemAlignment = this.HorizontalItemAlignment;
            bar.IsOverflowRibbon = true;
            bar.AutoSizeItems = this.AutoSizeItems;
#if!TRIAL
            bar.LicenseKey = this.LicenseKey;
#endif
            bar.MaximumOverflowTextLength = this.MaximumOverflowTextLength;
            bar.VerticalItemAlignment = this.VerticalItemAlignment;
            //bar.AutoSize = true;
            if (createHandle && !BarFunctions.IsHandleValid(this))
            {
                this.CreateHandle();
                this.RecalcLayout();
            }
            if (this.OverflowState && !m_BeforeOverflowSize.IsEmpty)
            {
                Size barSize = m_BeforeOverflowSize;
                bar.Size = barSize;
            }
            else
                bar.Size = GetFullContentBasedSize();
            bar.TitleVisible = this.TitleVisible;
            bar.Visible = true;
            return bar;
        }
        private eVerticalItemsAlignment m_OriginalVerticalItemAlign = eVerticalItemsAlignment.Top;
        private eOrientation m_OriginalLayoutOrientation = eOrientation.Horizontal;
        private eHorizontalItemsAlignment m_OriginalHorizontalItemAlign = eHorizontalItemsAlignment.Left;
        private void SetupOverflowButton()
        {
            if (m_OverflowButton != null || m_ItemContainer.SubItems.Count==0)
                return;
            m_SuspendRecalcSize = true;

            
            RibbonOverflowButtonItem overflowButton = new RibbonOverflowButtonItem();
            overflowButton.Name = "sysOverflowButton";
            overflowButton.Text = GetOverflowButtonText();
            overflowButton.RibbonBar = this;
            overflowButton.Image = GetOverflowButtonImage();
            overflowButton.ButtonStyle = eButtonStyle.ImageAndText;
            overflowButton.ImagePosition = eImagePosition.Top;
            overflowButton.AutoExpandOnClick = true;
            overflowButton.PopupType = ePopupType.Menu;
            overflowButton.ImagePaddingHorizontal = 16;
            overflowButton.VerticalPadding = 12;
            overflowButton.SubItemsExpandWidth = 14;
            overflowButton.HotTrackingStyle = eHotTrackingStyle.None;
            overflowButton.PopupOpen += new DotNetBarManager.PopupOpenEventHandler(OverflowButton_PopupOpen);
            if (OverflowButtonSetup != null)
                OverflowButtonSetup(this, new OverflowButtonEventArgs(overflowButton));

            ArrayList items = new ArrayList();
            m_ItemContainer.SubItems.CopyTo(items);
            m_ItemContainer.SubItems.Clear();

            //ItemContainer cont = m_ItemContainer.Copy() as ItemContainer;
            //m_OverflowButton.SubItems.Add(cont);
            m_ItemContainer.SubItems.Add(overflowButton);
            m_OverflowRibbonBar = CreateOverflowRibbon();
            m_OverflowRibbonBar.OverflowParent = this;

            m_OriginalVerticalItemAlign = m_ItemContainer.VerticalItemAlignment;
            m_OriginalLayoutOrientation = m_ItemContainer.LayoutOrientation;
            m_OriginalHorizontalItemAlign = m_ItemContainer.HorizontalItemAlignment;
            m_ItemContainer.VerticalItemAlignment = eVerticalItemsAlignment.Top;
            m_ItemContainer.LayoutOrientation = eOrientation.Horizontal;
            m_ItemContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;

            IOwner owner = (IOwner)this;
            foreach (BaseItem item in items)
            {
                m_OverflowRibbonBar.Items.Add(item);
                owner.AddShortcutsFromItem(item);
            }
            
            ItemContainer ic = new ItemContainer();
            ic.Stretch = true;
            ControlContainerItem cont = new ControlContainerItem();
            cont.AllowItemResize = false;
            ic.SubItems.Add(cont);
            cont.Control = m_OverflowRibbonBar;
            overflowButton.SubItems.Add(ic);
			m_ItemContainer.RefreshImageSize();

            m_OverflowButton = overflowButton;

            if (OverflowButtonSetupComplete != null)
                OverflowButtonSetupComplete(this, new OverflowButtonEventArgs(m_OverflowButton));
            m_SuspendRecalcSize = false;
        }

        protected override void OnClick(EventArgs e)
        {
            if (m_OverflowButton != null)
            {
                Point p = this.PointToClient(Control.MousePosition);
                if(!m_OverflowButton.DisplayRectangle.Contains(p))
                    m_OverflowButton.Expanded = !m_OverflowButton.Expanded;
            }
            base.OnClick(e);
        }

        private RibbonBar _OverflowParent;
        internal RibbonBar OverflowParent
        {
            get { return _OverflowParent; }
            set { _OverflowParent = value; }
        }

        /// <summary>
        /// Returns reference to the overflow button that is used by control.
        /// </summary>
        internal RibbonBar OverflowRibbonBar
        {
            get { return m_OverflowRibbonBar; }
        }

        private void OverflowButton_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            if (m_OverflowRibbonBar != null)
            {
                m_OverflowRibbonBar.BackgroundStyle.ApplyStyle(this.BackgroundStyle);
                m_OverflowRibbonBar.BackgroundMouseOverStyle.ApplyStyle(this.BackgroundMouseOverStyle);
                m_OverflowRibbonBar.TitleStyle.ApplyStyle(this.TitleStyle);
                m_OverflowRibbonBar.TitleStyleMouseOver.ApplyStyle(this.TitleStyleMouseOver);
                if (this.ShowKeyTips)
                    m_OverflowRibbonBar.ShowKeyTips = true;
            }
        }

        private void DisposeOverflowButton()
        {
            if (m_OverflowButton == null)
                return;

            m_SuspendRecalcSize = true;
            if (OverflowButtonDestroy != null)
                OverflowButtonDestroy(this, new OverflowButtonEventArgs(m_OverflowButton));
            m_OverflowButton.PopupOpen -= new DotNetBarManager.PopupOpenEventHandler(OverflowButton_PopupOpen);
            m_ItemContainer.SubItems.Remove(m_OverflowButton);
            m_ItemContainer.VerticalItemAlignment = m_OriginalVerticalItemAlign;
            m_ItemContainer.LayoutOrientation = m_OriginalLayoutOrientation;
            m_ItemContainer.HorizontalItemAlignment = m_OriginalHorizontalItemAlign;
            ArrayList items=new ArrayList();
            m_OverflowRibbonBar.Items.CopyTo(items);
            m_OverflowRibbonBar.Items.Clear();
            m_OverflowButton.SubItems.Clear();

            IOwner owner = (IOwner)this;
            foreach (BaseItem item in items)
            {
                owner.RemoveShortcutsFromItem(item);
                m_ItemContainer.SubItems.Add(item);
            }
			if(m_OverflowButtonImage==null)
			{
				m_OverflowButton.Image.Dispose();
				m_OverflowButton.Image=null;
			}
            m_OverflowButton.Dispose();
            m_OverflowRibbonBar.Dispose();
            m_OverflowButton = null;
            m_OverflowRibbonBar = null;
            m_SuspendRecalcSize = false;
            m_BeforeOverflowSize = Size.Empty;
        }

		internal Image GetOverflowButtonImage()
		{
			if(m_OverflowButtonImage==null)
				return BarFunctions.LoadBitmap("SystemImages.RibbonOverflow.png");
			return m_OverflowButtonImage;
		}

		private string GetOverflowButtonText()
		{
			string text = m_OverflowButtonText;
			if (text == "")
			{
				text = this.Text;
                if (m_MaximumOverflowTextLength > 0 && text.Length >= m_MaximumOverflowTextLength)
                    text = text.Substring(0, m_MaximumOverflowTextLength) + "...";
                else if (text.IndexOf(' ') > 0)
                    text += " <expand/>";
			}
			return text;
		}

        protected override void OnResize(EventArgs e)
        {
            if (this.OverflowState)
                m_OverflowButton.Bounds = this.GetItemContainerRectangle();
            base.OnResize(e);
        }

        /// <summary>
        /// Gets whether control is in overflow state or not.
        /// </summary>
        [Browsable(false)]
        public bool OverflowState
        {
            get
            {
                if (m_OverflowButton != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets whether ButtonItem objects hosted on control are resized to reduce the space consumed by ribbon bar when required.
        /// Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether ButtonItem objects hosted on control are resized to reduce the space consumed by ribbon bar when required.")]
        public bool AutoSizeItems
        {
            get
            {
                return m_AutoSizeItems;
            }
            set
            {
                m_AutoSizeItems = value;
            }
        }

		/// <summary>
		/// Resets cached content size. Content size is cached to improve performance of the control when control is
		/// switched into overflow mode displaying a single button. If you change content of Rendering, hide/show items or
		/// add new items you will need to call this method to erase cached value and allow the full Rendering size to be calculated.
		/// </summary>
		public void ResetCachedContentSize()
		{
			m_BeforeOverflowSize=Size.Empty;
		}

        /// <summary>
        /// Returns size of the control based on current content size.
        /// </summary>
        /// <returns>Size based on content.</returns>
        internal Size GetContentBasedSize()
        {
            return GetControlSize(new Size(m_ItemContainer.WidthInternal, m_ItemContainer.HeightInternal));
        }

        private Size GetControlSize(Size innerContentSize)
        {
            ElementStyle style = GetBackgroundStyle();
            Size size = new Size((innerContentSize.Width > 0 ? innerContentSize.Width : 32) + ElementStyleLayout.HorizontalStyleWhiteSpace(style),
                innerContentSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(style));
            if (m_TitleVisible && !this.OverflowState)
            {
                size.Height += m_TitleRectangle.Height;
            }

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                size.Width += 1;
                size.Height += 4;
            }

            return size;
        }

        internal Size GetFullContentBasedSize()
        {
            if (!m_FullItemsSize.IsEmpty)
                return GetControlSize(m_FullItemsSize);
            else
                return GetContentBasedSize();
        }

		/// <summary>
		/// Sets the height of the control to the automatically calculated height based on content.
		/// </summary>
		public override void AutoSyncSize()
		{
			if(!this.AutoSize || this.OverflowState)
				return;
			if(m_ItemContainer.NeedRecalcSize)
				return;

            this.Height = GetContentBasedSize().Height;
		}

        /// <summary>
        /// Returns automatically calculated height of the control given current content.
        /// </summary>
        /// <returns>Height in pixels.</returns>
        public override int GetAutoSizeHeight()
        {
            return GetContentBasedSize().Height;
        }

        /// <summary>
        /// Returns automatically calculated width of the control given current content.
        /// </summary>
        /// <returns>Width in pixels.</returns>
        public int GetAutoSizeWidth()
        {
            return GetContentBasedSize().Width;
        }

		protected override Rectangle GetItemContainerRectangle()
		{
			Rectangle r=base.GetItemContainerRectangle();
            //r.Inflate(-2, 0);
            if (m_TitleVisible &&!this.OverflowState)
            {
                Rectangle rTitle = GetTitleRectangle();
                if (m_TitlePosition == eRibbonTitlePosition.Top)
                {
                    rTitle.Height++;
                    r.Y += rTitle.Height;
                    r.Height -= rTitle.Height;
                }
                else
                {
                    //rTitle.Height++;
                    r.Height -= rTitle.Height;
                }
            }

			return r;
		}

		private Rectangle GetBackgroundRectangle()
		{
			Rectangle r=this.ClientRectangle;
			
            //Rectangle rTitle=GetTitleRectangle();
            //if (m_TitlePosition == eRibbonTitlePosition.Top)
            //{
            //    rTitle.Height++;
            //    r.Y += rTitle.Height;
            //    r.Height -= rTitle.Height;
            //}
            //else
            //{
            //    rTitle.Height++;
            //    r.Height -= rTitle.Height;
            //}
			
			return r;
		}

        /// <summary>
        /// Gets the bounds of the title. 
        /// </summary>
		public Rectangle GetTitleRectangle()
		{
			return m_TitleRectangle;
		}

#if FRAMEWORK20
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle r = this.ClientRectangle;
                if (!m_TitleRectangle.IsEmpty && m_TitleVisible)
                {
                    if (m_TitlePosition == eRibbonTitlePosition.Top)
                    {
                        r.Y += m_TitleRectangle.Height;
                        r.Height -= m_TitleRectangle.Height;
                    }
                    else
                        r.Height -= m_TitleRectangle.Height;
                }

                return r;
            }
        }
#endif
		#endregion

		#region Display Support
		/// <summary>
		/// Gets/Sets the visual style for items and color scheme.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the visual style of the control.")]
		public eDotNetBarStyle Style
		{
			get
			{
				return m_ItemContainer.Style;
			}
			set
			{
                m_ItemContainer.Style = value;
                this.ColorScheme.Style = m_ItemContainer.EffectiveStyle;
				this.Invalidate();
				this.RecalcLayout();
			}
		}

        internal eDotNetBarStyle EffectiveStyle
        {
            get
            {
                return m_ItemContainer.EffectiveStyle;
            }
        }

        protected override Font GetKeyTipFont()
        {
            RibbonControl rc = this.GetRibbonControl();
            if (rc!=null)
            {
                RibbonStrip rs = rc.RibbonStrip;
                if (rs.KeyTipsFont != null)
                    return rs.KeyTipsFont;
            }

            return base.GetKeyTipFont();
        }

        protected override bool NeedsTopLevelKeyTipCanvasParent
        {
            get
            {
                if (this.DialogLauncherVisible && this.DialogLauncherKeyTip != "")
                    return true;
                return false;
            }
        }

        internal override void PaintContainerKeyTips(BaseItem container, Rendering.BaseRenderer renderer, KeyTipsRendererEventArgs e)
        {
            base.PaintContainerKeyTips(container, renderer, e);

            if (this.DialogLauncherVisible && this.DialogLauncherKeyTip != "")
            {
                string stack = ((IKeyTipsControl)this).KeyTipsKeysStack;
                string keyTip = this.DialogLauncherKeyTip;
                if (stack != "" && !keyTip.StartsWith(stack))
                    return;
                e.ReferenceObject = this;
                e.KeyTip = keyTip;

                Size padding = KeyTipsPainter.KeyTipsPadding;
                Size size = TextDrawing.MeasureString(e.Graphics, keyTip, e.Font);
                size.Width += padding.Width;
                size.Height += padding.Height;
                Rectangle r = new Rectangle(this.Width - size.Width, this.Height + 4, size.Width, size.Height);
                e.Bounds = r;
                renderer.DrawKeyTips(e);
            }
        }

        private string _DialogLauncherKeyTip = "";
        /// <summary>
        /// Gets or sets the KeyTip for the dialog launcher button.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("Indicates KeyTip for the dialog launcher button.")]
        public string DialogLauncherKeyTip
        {
            get { return _DialogLauncherKeyTip; }
            set
            {
                if (value == null) value = "";
                _DialogLauncherKeyTip = value;
            }
        }
        

        //protected override void PaintKeyTips(Graphics g)
        //{
        //    base.PaintKeyTips(g);
        //    if (this.ShowKeyTips && this.DialogLauncherKeyTip != "" && this.DialogLauncherKeyTip!=null)
        //    {
        //        char c = NativeFunctions.GetAccessKey(this.Text);
        //        if (c == char.MinValue)
        //            return;

        //        string keyTip = c.ToString().ToUpper();
        //        if (keyTip != "")
        //        {
        //            KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, keyTip, GetKeyTipFont(), this);

        //            Size padding = KeyTipsPainter.KeyTipsPadding;
        //            Size size = TextDrawing.MeasureString(g, keyTip, e.Font);
        //            size.Width += padding.Width;
        //            size.Height += padding.Height;
        //            Rectangle r = new Rectangle((this.Width - size.Width),
        //                this.Height-2 /*m_TitleRectangle.Height / 2*/,
        //                size.Width, size.Height);

        //            e.Bounds = r;

        //            this.GetRenderer().DrawKeyTips(e);
        //        }
        //    }
        //}

        protected override Rectangle GetKeyTipCanvasBounds()
        {
            RibbonControl rc = this.GetRibbonControl();
            if (rc != null)
            {
                Point p = rc.PointToClient(this.PointToScreen(Point.Empty));
                return new Rectangle(p.X, p.Y - 8, this.Bounds.Width+12, this.Bounds.Height + 20);
            }
            else
                return new Rectangle(0, 0, this.Width, this.Height);
        }

        protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
        {
            Size padding = KeyTipsPainter.KeyTipsPadding;
            Size size = TextDrawing.MeasureString(g, keyTip, font);
            size.Width += padding.Width;
            size.Height += padding.Height;

            Rectangle ib = GetItemClientRectangle(item);
            Rectangle r = new Rectangle(ib.Right - size.Width - 1, ib.Bottom - size.Height+(int)(size.Height/3) + 8, size.Width, size.Height);

            if (item == m_OverflowButton)
            {
                r.Y = this.Height - 2;
                r.X = ib.X + (ib.Width - size.Width - 2) / 2;
            }
            else if (m_UseSpecKeyTipsPositioning)
            {
                if (item.HeightInternal * 1.2 >= m_ItemContainer.HeightInternal || item is GalleryContainer)
                {
                    r.Y = this.Height - size.Height - 5;
                    if(item is GalleryContainer)
                        r.X = ib.Right - (size.Width - 2) / 2;
                    else
                        r.X = ib.X + (ib.Width - size.Width - 2) / 2;
                }
                else if (ib.Bottom * 1.2 >= m_ItemContainer.HeightInternal)
                    r.Y = this.Height - size.Height - 5;
                else if (item.Parent is ItemContainer && ib.Y >= 0 && ib.Y <= 12)
                    r.Y = 0;
                else if (r.Bottom > this.Height)
                    r.Y = this.Height - size.Height;
                else if (ib.Y >= this.Height * .25)
                    r.Y = (int)(this.Height * .4);
            }
            else if(item is GalleryContainer)
                r.X = ib.Right - (size.Width - 2) / 2;

            return r;
        }

        private Rectangle GetItemClientRectangle(BaseItem item)
        {
            Rectangle r = item.DisplayRectangle;
            return r;
        }

        protected override bool ProcessMnemonic(BaseItem container, char charCode)
        {
            // Process mnemonic only if in KeyTips mode
            if (this.Parent is IKeyTipsControl && !((IKeyTipsControl)this.Parent).ShowKeyTips)
            {
                return false;
            }

            bool ret = base.ProcessMnemonic(container, charCode);
            if (!ret && this.DialogLauncherVisible && this.DialogLauncherKeyTip != "")
            {
                string stack = ((IKeyTipsControl)this).KeyTipsKeysStack;
                string keyTipsString = stack + charCode.ToString();
                keyTipsString = keyTipsString.ToUpper();
                if (this.DialogLauncherKeyTip.ToUpper() == keyTipsString)
                {
                    this.DoLaunchDialog();
                    ret = true;
                }
                else if (this.DialogLauncherKeyTip.ToUpper().StartsWith(keyTipsString))
                {
                    ((IKeyTipsControl)this).KeyTipsKeysStack += charCode.ToString().ToUpper();
                }
            }
            return ret;
        }

		protected override void PaintControlBackground(ItemPaintArgs pa)
		{
            bool mouseOver = m_MouseOver;
            bool fade = m_FadeImageState!=null;

            if (fade)
                mouseOver = false;
            GraphicsPath insideClip = null;
            ElementStyle backStyle = GetBackgroundStyle();
            bool disposeBackStyle = fade;
            try
            {
                if (backStyle != null)
                {
                    Rectangle r = GetBackgroundRectangle();
                    pa.Graphics.SetClip(r, CombineMode.Replace);
                    ElementStyle mouseOverStyle = GetBackgroundMouseOverStyle();
                    if (mouseOver && backStyle != null && mouseOverStyle != null && mouseOverStyle.Custom)
                    {
                        backStyle = backStyle.Copy();
                        disposeBackStyle = true;
                        backStyle.ApplyStyle(mouseOverStyle);
                    }

                    ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(backStyle, pa.Graphics, r, EffectiveStyle == eDotNetBarStyle.Office2007);
                    ElementStyleDisplay.Paint(displayInfo);
                    pa.Graphics.ResetClip();
                    displayInfo.Bounds = GetBackgroundRectangle();
                    // Adjust so the title shows over the inside light border line
                    displayInfo.Bounds.X--;
                    displayInfo.Bounds.Width++;
                    insideClip = ElementStyleDisplay.GetInsideClip(displayInfo);
                    displayInfo.Bounds.X++;
                    displayInfo.Bounds.Width--;
                }

                if (insideClip != null)
                    pa.Graphics.SetClip(insideClip, CombineMode.Replace);

                m_DialogLauncherRect = Rectangle.Empty;

                if (m_TitleVisible && !this.OverflowState)
                {
                    ElementStyle style = GetTitleStyle();
                    ElementStyle styleMouseOver = GetTitleMouseOverStyle();
                    if (mouseOver && style != null && styleMouseOver != null && styleMouseOver.Custom)
                    {
                        style = style.Copy();
                        style.ApplyStyle(styleMouseOver);
                    }

                    if (style != null)
                    {
                        SimpleNodeDisplayInfo info = new SimpleNodeDisplayInfo(style, pa.Graphics, m_TitleElement, this.Font, (this.RightToLeft == RightToLeft.Yes));
                        if (m_DialogLauncherVisible)
                        {
                            if (m_DialogLauncherButton == null)
                            {
                                Rectangle textRect = m_TitleElement.TextBounds;
                                textRect.Width -= m_TitleRectangle.Height;
                                if (this.RightToLeft == RightToLeft.Yes)
                                    textRect.X += m_TitleRectangle.Height;
                                info.TextBounds = textRect;
                            }
                            else
                            {
                                if (m_MouseOverDialogLauncher && m_DialogLauncherMouseOverButton != null)
                                    m_TitleElement.Image = m_DialogLauncherMouseOverButton;
                                else
                                    m_TitleElement.Image = m_DialogLauncherButton;
                            }
                        }

                        SimpleNodeDisplay.Paint(info);

                        if (m_DialogLauncherVisible && m_TitleElement.Image == null)
                            PaintDialogLauncher(pa);
                        else
                            m_DialogLauncherRect = m_TitleElement.ImageBounds;
                    }
                }

                pa.Graphics.ResetClip();

                m_FadeImageLock.AcquireReaderLock(-1);
                try
                {
                    if (m_FadeImageState != null)
                    {
                        Graphics g = pa.Graphics;
                        Rectangle r = new Rectangle(0, 0, this.Width, this.Height);

                        System.Drawing.Imaging.ColorMatrix matrix1 = new System.Drawing.Imaging.ColorMatrix();
                        matrix1[3, 3] = (float)((float)m_FadeAlpha / 255);
                        using (System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes())
                        {
                            imageAtt.SetColorMatrix(matrix1);

                            g.DrawImage(m_FadeImageState, r, 0, 0, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);
                        }
                        return;
                    }
                }
                finally
                {
                    m_FadeImageLock.ReleaseReaderLock();
                }
            }
            finally
            {
                if (insideClip != null) insideClip.Dispose();
                if (disposeBackStyle && backStyle != null) backStyle.Dispose();
            }
		}

		private void PaintDialogLauncher(ItemPaintArgs pa)
		{
			Rectangle r=new Rectangle(m_TitleRectangle.Right-(m_TitleRectangle.Height-5)-3,m_TitleRectangle.Y+2,
                m_TitleRectangle.Height-5,m_TitleRectangle.Height-5);
            if (this.RightToLeft == RightToLeft.Yes)
                r.X = m_TitleRectangle.X;
			Graphics g=pa.Graphics;

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                int ds = (int)Math.Min(15, m_TitleRectangle.Height - 1);
                r = new Rectangle(m_TitleRectangle.Right - ds - 2, m_TitleRectangle.Y + (m_TitleRectangle.Height-ds)/2, ds, ds);
                if (this.RightToLeft == RightToLeft.Yes)
                    r.X = m_TitleRectangle.X+2;

                RibbonBarRendererEventArgs e = new RibbonBarRendererEventArgs(g, r, this);
                if (m_MouseOverDialogLauncher)
                {
                    if (Control.MouseButtons == MouseButtons.Left)
                        e.Pressed = true;
                    else
                        e.MouseOver = true;
                }
                this.GetRenderer().DrawRibbonDialogLauncher(e);
                m_DialogLauncherRect = r;
                return;
            }

			if(this.AntiAlias)
				g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.Default; // AntiAlias;

			using(LinearGradientBrush brush=DisplayHelp.CreateLinearGradientBrush(r,ControlPaint.LightLight(pa.Colors.BarBackground),pa.Colors.BarBackground,90))
			{
				g.FillRectangle(brush,r);
			}

			Rectangle rDot=new Rectangle(r.X+(int)Math.Ceiling((float)(r.Width-8)/2),r.Bottom-4,2,2);
			g.FillRectangle(SystemBrushes.ControlText,rDot);
			rDot.Offset(rDot.Width+1,0);
			g.FillRectangle(SystemBrushes.ControlText,rDot);
			rDot.Offset(rDot.Width+1,0);
			g.FillRectangle(SystemBrushes.ControlText,rDot);

			if(this.AntiAlias)
				g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			m_DialogLauncherRect=r;
		}

        /// <summary>
        /// Gets or sets whether ribbon bar title is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether ribbon bar title is visible")]
        public bool TitleVisible
        {
            get { return m_TitleVisible; }
            set
            {
                m_TitleVisible = value;
                this.RecalcLayout();
            }
        }

		/// <summary>
		/// Specifies the style of the title of the control.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies the style of the title of the control"),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyle TitleStyle
		{
			get {return m_TitleStyle;}
            //#if FRAMEWORK20
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_TitleStyle = value;
            //}
            //#endif
		}

		/// <summary>
		/// Resets TitleStyle property to its default value. Used by Windows Forms designer for design-time support.
		/// </summary>
		public void ResetTitleStyle()
		{
			m_TitleStyle.StyleChanged-=new EventHandler(this.ElementStyleChanged);
			m_TitleStyle=new ElementStyle();
			m_TitleStyle.StyleChanged+=new EventHandler(this.ElementStyleChanged);
            this.Invalidate();
		}

        /// <summary>
        /// Specifies the style of the title of the control when mouse is over the control.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Specifies the style of the title of the control when mouse is over the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle TitleStyleMouseOver
        {
            get { return m_TitleStyleMouseOver; }
            //#if FRAMEWORK20
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_TitleStyleMouseOver = value;
            //}
            //#endif
        }

        /// <summary>
        /// Resets TitleStyle property to its default value. Used by Windows Forms designer for design-time support.
        /// </summary>
        public void ResetTitleStyleMouseOver()
        {
            m_TitleStyleMouseOver.StyleChanged -= new EventHandler(this.ElementStyleChanged);
            m_TitleStyleMouseOver = new ElementStyle();
            m_TitleStyleMouseOver.StyleChanged += new EventHandler(this.ElementStyleChanged);
            this.Invalidate();
        }

        /// <summary>
        /// Gets the title bounds.
        /// </summary>
        internal Rectangle TitleRectangle
        {
            get { return m_TitleElement.Bounds; }
        }

		private void ElementStyleChanged(object sender, EventArgs e)
		{
			if(this.DesignMode)
			{
				this.RecalcLayout();
			}
		}
		#endregion

        #region Fade Background Effect Support
        private Bitmap m_FadeImageState = null;
        private int m_FadeDirection = 1;
        private int m_FadeAlpha = 0;
        private bool m_FadeAnimation = false;
		private System.Threading.ReaderWriterLock m_FadeImageLock=new System.Threading.ReaderWriterLock();
		private void SetMouseOver(bool value)
		{
			if (value != m_MouseOver)
			{
				bool fadeEnabled = IsFadeEnabled && this.Enabled && this.Visible && this.Width>0 && this.Height>0;
				if (fadeEnabled)
				{
					bool createImage=false;
                    bool erroredOut = false;
                    try
                    {
                        m_FadeImageLock.AcquireReaderLock(-1);
                        try
                        {
                            createImage = (m_FadeImageState == null);
                        }
                        finally
                        {
                            m_FadeImageLock.ReleaseReaderLock();
                        }

                        if (createImage)
                        {
                            m_MouseOver = true;

                            bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
                            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
                            if (readerLockHeld)
                            {
                                cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
                            }
                            else
                            {
                                m_FadeImageLock.AcquireWriterLock(-1);
                            }

                            try
                            {
                                m_FadeImageState = GetCurrentStateImage();
                            }
                            finally
                            {
                                if (readerLockHeld)
                                {
                                    m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
                                }
                                else
                                {
                                    m_FadeImageLock.ReleaseWriterLock();
                                }
                            }
                        }
                    }
                    catch (ApplicationException)
                    {
                        erroredOut = true;
                    }
                    
					m_FadeDirection = 1;
                    
					m_MouseOver = value;

                    if (!erroredOut)
                    {
                        if (!m_MouseOver)
                            m_FadeDirection = -1;
                        else
                            m_FadeAlpha = 10;

                        FadeAnimator.Fade(this, new EventHandler(this.OnFadeChanged));
                        m_FadeAnimation = true;
                    }
				}
				else
					m_MouseOver = value;
			}
		}

		private void StopFade()
		{
			if (!m_FadeAnimation)
				return;

			m_FadeAnimation = false;
			FadeAnimator.StopFade(this, new EventHandler(OnFadeChanged));
            
			bool disposeImage=false;
			m_FadeImageLock.AcquireReaderLock(-1);
			try
			{
				disposeImage=(m_FadeImageState != null);
			}
			finally
			{
				m_FadeImageLock.ReleaseReaderLock();
			}
			if (disposeImage)
				DisposeFadeImage();

			if (m_FadeAlpha > 230)
				m_FadeAlpha = 255;
			else if (m_FadeAlpha < 0)
				m_FadeAlpha = 0;
		}

		private void DisposeFadeImage()
		{
			bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
			System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
			
			if (readerLockHeld)
			{
				cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
			}
			else
			{
				m_FadeImageLock.AcquireWriterLock(-1);
			}

			try
			{
				m_FadeImageState.Dispose();
				m_FadeImageState=null;
			}
			finally
			{
				if (readerLockHeld)
				{
					m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
				}
				else
				{
					m_FadeImageLock.ReleaseWriterLock();
				}
			}
		}


        private void OnFadeChanged(object sender, EventArgs e)
        {
            m_FadeAlpha += (m_FadeDirection * 65);

            if (m_FadeDirection < 0 && m_FadeAlpha <= 0 || m_FadeDirection > 0 && m_FadeAlpha >= 255)
            {
                StopFade();
            }

            this.Invalidate();
        }

        private Bitmap GetCurrentStateImage()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            bitmap.MakeTransparent();
            Graphics g = Graphics.FromImage(bitmap);

            try
            {
                bool antiAlias = this.AntiAlias;
                ColorScheme cs = this.GetColorScheme();

                if (antiAlias)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                }

                ItemPaintArgs pa = GetItemPaintArgs(g);
                this.PaintControlBackground(pa);
            }
            finally
            {
                g.Dispose();
            }
            return bitmap;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopFade();
            base.OnHandleDestroyed(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!this.Visible)
            {
                StopFade();
                if (this.IsOverflowRibbon)
                    ((IOwner)this).OnApplicationDeactivate();
            }
            base.OnVisibleChanged(e);
        }
        #endregion

        #region Licensing

        protected override void OnPaint(PaintEventArgs e)
        {
            ColorScheme cs = this.GetColorScheme();
			m_TitleStyle.SetColorScheme(cs);
            m_TitleStyleMouseOver.SetColorScheme(cs);
            m_BackgroundMouseOverStyle.SetColorScheme(cs);

            InitDefaultStyles();
            //if (this.Parent.Controls.IndexOf(this) == 1)
            //{
            //    e.Graphics.ResetClip();
            //    e.Graphics.FillRectangle(Brushes.Red, this.ClientRectangle);
            //    return;
            //}
            base.OnPaint(e);

            #if !TRIAL
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(180, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
            #else
            if (NativeFunctions.ColorExpAlt() || !NativeFunctions.CheckedThrough)
		    {
			    e.Graphics.Clear(SystemColors.Control);
                return;
            }
            #endif
        }

#if !TRIAL
        private string m_LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return m_LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                m_LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion

        #region Misc Properties
        /// <summary>
        /// Gets or sets whether control changes its background when mouse is over the control.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether control changes its background when mouse is over the control.")]
        public bool BackgroundHoverEnabled
        {
            get { return m_BackgroundHoverEnabled; }
            set { m_BackgroundHoverEnabled = value; }
        }

        /// <summary>
        /// Gets or sets whether external ButtonItem object is accepted in drag and drop operation. UseNativeDragDrop must be set to true in order for this property to be effective.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
        public override bool AllowExternalDrop
        {
            get { return base.AllowExternalDrop; }
            set { base.AllowExternalDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether native .NET Drag and Drop is used by control to perform drag and drop operations. AllowDrop must be set to true to allow drop of the items on control.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
        public override bool UseNativeDragDrop
        {
            get { return base.UseNativeDragDrop; }
            set { base.UseNativeDragDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether automatic drag & drop support is enabled. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Specifies whether automatic drag & drop support is enabled.")]
        public virtual bool EnableDragDrop
        {
            get { return m_EnableDragDrop; }
            set { m_EnableDragDrop = value; }
        }

        ///// <summary>
        ///// Gets or sets the Bar back color.
        ///// </summary>
        //[Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies background color of the Bar."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public override Color BackColor
        //{
        //    get
        //    {
        //        return base.BackColor;
        //    }
        //    set
        //    {
        //        base.BackColor = value;
        //    }
        //}
        //[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //public override void ResetBackColor()
        //{
        //    base.BackColor = Color.Transparent;
        //}
        //public bool ShouldSerializeBackColor()
        //{
        //    return base.BackColor != Color.Transparent;
        //}

        #endregion
    }

    #region OverflowButton Event Code
    /// <summary>
    /// Defines event handler for overflow button events.
    /// </summary>
    /// <param name="sender">Control that is hosting the overflow button.</param>
    /// <param name="e">Event arguments</param>
    public delegate void OverflowButtonEventHandler(object sender, OverflowButtonEventArgs e);

    /// <summary>
    /// Represents event arguments for overflow button events.
    /// </summary>
    public class OverflowButtonEventArgs : EventArgs
    {
        private readonly ButtonItem m_OverflowButton;

        /// <summary>
        /// Creates new instance of the class and initializes it.
        /// </summary>
        /// <param name="ob">Reference to overflow button.</param>
        public OverflowButtonEventArgs(ButtonItem ob)
        {
            m_OverflowButton = ob;
        }

        /// <summary>
        /// Returns reference to the overflow button that is used by control.
        /// </summary>
        public ButtonItem OverflowButton
        {
            get { return m_OverflowButton; }
        }
    }
    #endregion

    #region RibbonBarAccessibleObject
    /// <summary>
    /// Represents class for RibbonBar Accessibility support.
    /// </summary>
    public class RibbonBarAccessibleObject : ItemControlAccessibleObject
    {
        /// <summary>
        /// Creates new instance of the object and initializes it with owner control.
        /// </summary>
        /// <param name="owner">Reference to owner control.</param>
        public RibbonBarAccessibleObject(ItemControl owner)
            : base(owner) { }

        /// <summary>
        /// Returns number of child objects.
        /// </summary>
        /// <returns>Total number of child objects.</returns>
        public override int GetChildCount()
        {
            int count = base.GetChildCount();
            if (this.Owner is RibbonBar && ((RibbonBar)this.Owner).DialogLauncherVisible)
                count++;
            return count;
        }

        /// <summary>
        /// Returns reference to child object given the index.
        /// </summary>
        /// <param name="iIndex">0 based index of child object.</param>
        /// <returns>Reference to child object.</returns>
        public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
        {
            RibbonBar bar = this.Owner as RibbonBar;
            if (bar != null && bar.DialogLauncherVisible && iIndex == GetChildCount() - 1)
            {
                return new DialogLauncherAccessibleObject(bar);
            }

            return base.GetChild(iIndex);
        }
    }
    #endregion

    #region DialogLauncherAccessibleObject
    public class DialogLauncherAccessibleObject : System.Windows.Forms.AccessibleObject
    {
        private RibbonBar m_Owner = null;
        private bool m_Hot = false;

        public DialogLauncherAccessibleObject(RibbonBar owner)
        {
            m_Owner = owner;
            m_Owner.DialogLauncherMouseEnter += new EventHandler(OnMouseEnter);
            m_Owner.DialogLauncherMouseLeave += new EventHandler(OnMouseLeave);
        }

        private void OnMouseEnter(object sender, System.EventArgs e)
        {
            m_Hot = true;
        }

        private void OnMouseLeave(object sender, System.EventArgs e)
        {
            m_Hot = false;
        }

        public override string Name
        {
            get
            {
                if (m_Owner == null)
                    return "";

                if (m_Owner.DialogLauncherAccessibleName != "")
                    return m_Owner.DialogLauncherAccessibleName;

                if (m_Owner.Text != null)
                    return m_Owner.Text.Replace("&", "");

                return "";
            }
            set
            {
                m_Owner.DialogLauncherAccessibleName = value;
            }
        }

        public override string Description
        {
            get
            {
                return m_Owner.Text + " Dialog Launcher";
            }
        }

        public override System.Windows.Forms.AccessibleRole Role
        {
            get
            {
                if (m_Owner == null || !m_Owner.IsAccessible)
                    return System.Windows.Forms.AccessibleRole.None;

                return System.Windows.Forms.AccessibleRole.PushButton;
            }
        }

        public override System.Windows.Forms.AccessibleStates State
        {
            get
            {
                if (m_Owner == null)
                    return System.Windows.Forms.AccessibleStates.Unavailable;

                System.Windows.Forms.AccessibleStates state = 0;

                if (!m_Owner.IsAccessible)
                    return System.Windows.Forms.AccessibleStates.Unavailable;

                if (!m_Owner.Visible)
                    state |= System.Windows.Forms.AccessibleStates.Invisible;
                else if (!m_Owner.Enabled)
                {
                    return System.Windows.Forms.AccessibleStates.Unavailable;
                }
                else
                {
					#if FRAMEWORK20
                    state |= AccessibleStates.HasPopup;
					#endif
                    if (m_Hot)
                        state |= (System.Windows.Forms.AccessibleStates.Focused | System.Windows.Forms.AccessibleStates.HotTracked);
                    else
                        state |= System.Windows.Forms.AccessibleStates.Focusable;
                }
                return state;
            }
        }

        public override System.Windows.Forms.AccessibleObject Parent
        {
            get { if (m_Owner == null) return null; return m_Owner.AccessibilityObject; }
        }

        public override System.Drawing.Rectangle Bounds
        {
            get
            {
                if (m_Owner == null)
                    return System.Drawing.Rectangle.Empty;

                return m_Owner.RectangleToScreen(m_Owner.DialogLauncherRect);
            }
        }

        public override int GetChildCount()
        {
            return 0;
        }

        public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
        {
            return null;
        }

        public override string DefaultAction
        {
            get
            {
                return "Press";
            }
        }

        public override void DoDefaultAction()
        {
            if (m_Owner == null)
                return;

            m_Owner.DoLaunchDialog(); 
            base.DoDefaultAction();
        }

        public override string KeyboardShortcut
        {
            get
            {
                return "";
            }
        }

        public override System.Windows.Forms.AccessibleObject GetSelected()
        {
            if (m_Owner == null)
                return base.GetSelected();

            if (m_Hot)
                return this;
            
            return base.GetSelected();
        }

        public override System.Windows.Forms.AccessibleObject HitTest(int x, int y)
        {
            if (m_Owner == null)
                return base.HitTest(x, y);

            Point screen = new Point(x, y);
            Point p = m_Owner.PointToClient(screen);
            if (m_Owner.DialogLauncherRect.Contains(p))
                return this;

            return base.HitTest(x, y);
        }

        public override System.Windows.Forms.AccessibleObject Navigate(System.Windows.Forms.AccessibleNavigation navdir)
        {
            return base.Navigate(navdir);
        }
    }
    #endregion
}
