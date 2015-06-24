using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the toolbar control with the magnifying (bubbling) buttons.
    /// </summary>
    [ToolboxItem(true), DefaultEvent("ButtonClick"), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.BubbleBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class BubbleBar : Control, ISupportInitialize, IMessageHandlerClient
    {
        #region Private Variables
        const float _falloffFactor = .3f;

        private System.Windows.Forms.ImageList m_Images;
        private System.Windows.Forms.ImageList m_ImagesLarge = null;
        private bool m_ShowTooltips = true;
        private eOrientation m_Orientation = eOrientation.Horizontal;
        private System.Drawing.Size m_ImageSizeLarge = new System.Drawing.Size(48, 48);
        private System.Drawing.Size m_ImageSizeNormal = new System.Drawing.Size(24, 24);
        private BubbleContentManager m_ContentManager = new BubbleContentManager();
        private BubbleButtonLayoutManager m_ButtonLayoutManager = new BubbleButtonLayoutManager();
        private BubbleButton m_MouseOverButton = null;
        private BubbleButton m_MouseDownButton = null;
        private int m_AnimationTime = 100;
        private BubbleButtonDisplayInfo m_BubbleButtonDisplayInfo = new BubbleButtonDisplayInfo();
        private bool m_AntiAlias = true;
        private BubbleBarOverlay m_Overlay = null;
        private bool m_Animation = false;
        private bool m_AnimationEnabled = true;
        private bool m_IgnoreMouseMove = false;
        private bool m_EnableOverlay = true;
        private eBubbleButtonAlignment m_ButtonAlignment = eBubbleButtonAlignment.Bottom;
        private int m_ButtonMargin = 6;
        private Font m_TooltipFont = null;
        private int m_MagTooltipIncrease = 0;
        private Rectangle m_ButtonBounds = Rectangle.Empty;
        private BubbleBarTab m_SelectedTab = null;
        private BubbleBarTab m_MouseOverTab = null;
        private TabColors m_SelectedTabColors = new TabColors();
        private TabColors m_MouseOverTabColors = new TabColors();
        private BubbleBarTabCollection m_Tabs = null;
        private bool m_TabsVisible = true;
        private Rectangle m_TabsBounds = Rectangle.Empty;
        private SimpleTabLayoutManager m_TabLayoutManager = null;
        private SimpleTabDisplay m_TabDisplay = null;
        private SerialContentLayoutManager m_TabContentManager = null;
        private Point m_LastMouseOverPosition = Point.Empty;
        private BubbleButton m_IgnoreButtonMouseMove = null;
        private ElementStyle m_ButtonBackAreaStyle = new ElementStyle();
        private ElementStyle m_BackgroundStyle = new ElementStyle();
        private bool m_ButtonBackgroundStretch = true;
        private bool m_FilterInstalled = false;
        private bool m_HasShortcuts = false;
        private Color m_TooltipTextColor = Color.White;
        private Color m_TooltipOutlineColor = Color.Black;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when active tab is about to change.
        /// </summary>
        public event BubbleBarTabChangingEventHadler TabChanging;
        /// <summary>
        /// Occurs when any of the buttons is clicked. Sender object should be casted to BubbleButton to get the button that was actually clicked.
        /// </summary>
        public event ClickEventHandler ButtonClick;
        /// <summary>
        /// Occurs when mouse first enters the control and bubble effect is employed to provide feedback.
        /// </summary>
        public event EventHandler BubbleStart;
        /// <summary>
        /// Occurs when mouse leaves the control and bubble effect is ended.
        /// </summary>
        public event EventHandler BubbleEnd;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the control.
        /// </summary>
        public BubbleBar()
        {
            if (!ColorFunctions.ColorsLoaded)
            {
                NativeFunctions.RefreshSettings();
                NativeFunctions.OnDisplayChange();
                ColorFunctions.LoadColors();
            }

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ContainerControl, false);
            this.SetStyle(ControlStyles.Selectable, false);

            m_Tabs = new BubbleBarTabCollection(this);

            m_ButtonBackAreaStyle.StyleChanged += new EventHandler(ElementStyleChanged);
            m_BackgroundStyle.StyleChanged += new EventHandler(ElementStyleChanged);

            m_ContentManager.BlockSpacing = 0;
            m_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
            m_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
            m_ContentManager.ContentOrientation = eContentOrientation.Horizontal;

            m_TabLayoutManager = new SimpleTabLayoutManager();
            m_TabDisplay = new SimpleTabDisplay();
            m_TabContentManager = new SerialContentLayoutManager();

            //m_Buttons=new BubbleButtonCollection(this);
            m_BubbleButtonDisplayInfo.BubbleBar = this;
            // Make sure this is JIT-ed
            m_Overlay = new BubbleBarOverlay(this);
            m_Overlay = null;

            if (!OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
                m_EnableOverlay = false;

            ApplyButtonAlignment();

            //			m_SelectedTabColors.BorderColor=Color.Black;
            //			m_MouseOverTabColors.BorderColor=SystemColors.Highlight;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the bubble button tooltip text color. Default value is White color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates bubble button tooltip text color.")]
        public Color TooltipTextColor
        {
            get { return m_TooltipTextColor; }
            set { m_TooltipTextColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized. Used by the Windows Forms design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTooltipTextColor()
        {
            return m_TooltipTextColor != Color.White;
        }
        /// <summary>
        /// Resets the property to default value. Used by the Windows Forms design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTooltipTextColor()
        {
            TypeDescriptor.GetProperties(this)["TooltipTextColor"].SetValue(this, Color.White);
        }

        /// <summary>
        /// Gets or sets the bubble button tooltip text outline color. Default value is Black color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates bubble button tooltip text outline color.")]
        public Color TooltipOutlineColor
        {
            get { return m_TooltipOutlineColor; }
            set { m_TooltipOutlineColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized. Used by the Windows Forms design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTooltipOutlineColor()
        {
            return m_TooltipOutlineColor != Color.Black;
        }
        /// <summary>
        /// Resets the property to default value. Used by the Windows Forms design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTooltipOutlineColor()
        {
            TypeDescriptor.GetProperties(this)["TooltipOutlineColor"].SetValue(this, Color.Black);
        }

        /// <summary>
        /// Gets or sets the spacing in pixels between buttons. Default value is 0.
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates spacing in pixels between buttons."), Category("Appearance")]
        public int ButtonSpacing
        {
            get { return m_ContentManager.BlockSpacing; }
            set
            {
                m_ContentManager.BlockSpacing = value;
                this.RecalcLayout();
                this.Refresh();
            }
        }
        /// <summary>
        /// Gets or sets whether background for the buttons is stretched to consume complete width of the control. Default value is true.
        /// </summary>
        [Browsable(true), Category("Button Background"), Description("Indicates whether background for the buttons is stretched to consume complete width of the control."), DefaultValue(true)]
        public bool ButtonBackgroundStretch
        {
            get { return m_ButtonBackgroundStretch; }
            set
            {
                if (m_ButtonBackgroundStretch != value)
                {
                    m_ButtonBackgroundStretch = value;
                    if (this.DesignMode)
                        this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the style for the background of the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets the style for the background of the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return m_BackgroundStyle; }
        }

        /// <summary>
        /// Gets the style for the background of the buttons.
        /// </summary>
        [Browsable(true), Category("Button Background"), Description("Gets the style for the background of the buttons."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle ButtonBackAreaStyle
        {
            get { return m_ButtonBackAreaStyle; }
        }

        /// <summary>
        /// Gets the bounds of the buttons back area.
        /// </summary>
        [Browsable(false)]
        public Rectangle ButtonBackAreaBounds
        {
            get { return m_ButtonBounds; }
        }

        /// <summary>
        /// Gets the bounds of the tabs area.
        /// </summary>
        [Browsable(false)]
        public Rectangle TabAreaBounds
        {
            get { return m_TabsBounds; }
        }

        /// <summary>
        /// Gets or sets the duration of animation that is performed when mouse enters a button for the first time or when mouse has left the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates duration of animatio."), DefaultValue(100)]
        public int AnimationTime
        {
            get { return m_AnimationTime; }
            set { m_AnimationTime = value; }
        }

        /// <summary>
        /// Gets or sets whether bubble animation is enabled. Default value is true.
        /// </summary>
        [Browsable(false), Description("Indicates whether animation is enabled."), DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AnimationEnabled
        {
            get { return m_AnimationEnabled; }
            set
            {
                m_AnimationEnabled = value;
                if (!m_AnimationEnabled)
                    OverlayInactive();
            }
        }

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                if (m_AntiAlias != value)
                {
                    m_AntiAlias = value;
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets ImageList for images used on buttons. These images will be used as images for the buttons that are not magnified.
        /// Use ImagesLarge to specify the magnified images for the coresponding images based on the index in this list.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Button Images"), Description("ImageList for images used on buttons.")]
        public System.Windows.Forms.ImageList Images
        {
            get
            {
                return m_Images;
            }
            set
            {
                if (m_Images != null)
                    m_Images.Disposed -= new EventHandler(this.ImageListDisposed);
                m_Images = value;
                if (m_Images != null)
                    m_Images.Disposed += new EventHandler(this.ImageListDisposed);
            }
        }

        /// <summary>
        /// Gets or sets ImageList for large-sized images used on buttons when button is magnified.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Button Images"), Description("ImageList for large-sized images used on buttons when button is magnified.")]
        public System.Windows.Forms.ImageList ImagesLarge
        {
            get
            {
                return m_ImagesLarge;
            }
            set
            {
                if (m_ImagesLarge != null)
                    m_ImagesLarge.Disposed -= new EventHandler(this.ImageListDisposed);
                m_ImagesLarge = value;
                if (m_ImagesLarge != null)
                    m_ImagesLarge.Disposed += new EventHandler(this.ImageListDisposed);
            }
        }

        /// <summary>
        /// Gets or sets whether tooltips are displayed for the buttons.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true), Description("Indicates whether tooltips are displayed for the buttons.")]
        public bool ShowTooltips
        {
            get { return m_ShowTooltips; }
            set { m_ShowTooltips = value; }
        }

        /// <summary>
        /// Gets or sets size of the images when button is enlarged, default value is 48 by 48 pixels. Note that you should provide the
        /// images in the same size for the buttons through the image properties on the buttons or through ImagesLarge property.
        /// If the large images are not provided the regular button image will be automatically enlarged.
        /// </summary>
        [Browsable(true), Category("Button Images"), Description("Indicates size of the images when button is enlarged.")]
        public System.Drawing.Size ImageSizeLarge
        {
            get { return m_ImageSizeLarge; }
            set
            {
                if (m_ImageSizeLarge != value)
                {
                    m_ImageSizeLarge = value;
                    this.RecalcLayout();
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Returns whether property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>True if property is different than default value otherwise false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImageSizeLarge()
        {
            if (m_ImageSizeLarge.Width != 48 || m_ImageSizeLarge.Height != 48)
                return true;
            return false;
        }

        /// <summary>
        /// Gets or sets the normal image size for the buttons, default value is 24 by 24 pixels. This should be set to the default image size that you will use on the
        /// buttons. If the images specified for the buttons are not of the same size as the size specified here then they will
        /// be automatically resized. Normal size must always be smaller than the size specified by ImageSizeLarge property.
        /// </summary>
        [Browsable(true), Category("Button Images"), Description("Indicates size of the images when button is enlarged.")]
        public System.Drawing.Size ImageSizeNormal
        {
            get { return m_ImageSizeNormal; }
            set
            {
                if (m_ImageSizeNormal != value)
                {
                    m_ImageSizeNormal = value;
                    this.RecalcLayout();
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Returns whether property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>True if property is different than default value otherwise false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImageSizeNormal()
        {
            if (m_ImageSizeLarge.Width != 24 || m_ImageSizeLarge.Height != 24)
                return true;
            return false;
        }

        /// <summary>
        /// Gets or sets the font that is used to display tooltips.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        public Font TooltipFont
        {
            get { return m_TooltipFont; }
            set { m_TooltipFont = value; }
        }

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public BubbleBarTab SelectedTab
        {
            get { return m_SelectedTab; }
            set
            {
                if (m_SelectedTab != value)
                {
                    m_SelectedTab = value;
                    OnSelectedTabChanged();
                }
            }
        }

        /// <summary>
        /// Gets the reference to the colors that are used when tab is selected.
        /// </summary>
        [Browsable(true), Category("Tabs"), Description("Gets the reference to the colors that are used when tab is selected."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabColors SelectedTabColors
        {
            get { return m_SelectedTabColors; }
        }

        /// <summary>
        /// Gets the reference to the colors that are used when mouse is over the tab.
        /// </summary>
        [Browsable(true), Category("Tabs"), Description("Gets the reference to the colors that are used when mouse is over the tab."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabColors MouseOverTabColors
        {
            get { return m_MouseOverTabColors; }
        }

        /// <summary>
        /// Gets the collection of all tabs.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.BubbleBarTabCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), System.ComponentModel.Category("Tabs"), Description("Returns the collection of Tabs.")]
        public BubbleBarTabCollection Tabs
        {
            get { return m_Tabs; }
        }

        /// <summary>
        /// Gets or sets the button alignment.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates button alignment.")]
        public eBubbleButtonAlignment Alignment
        {
            get
            {
                return m_ButtonAlignment;
            }
            set
            {
                if (m_ButtonAlignment != value)
                {
                    m_ButtonAlignment = value;
                    ApplyButtonAlignment();
                    this.RecalcLayout();
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether tabs are visible. Default value is true.
        /// </summary>
        [Browsable(true), Category("Tabs"), Description("Indicates whether tabs are visible."), DefaultValue(true)]
        public bool TabsVisible
        {
            get { return m_TabsVisible; }
            set
            {
                if (m_TabsVisible != value)
                {
                    m_TabsVisible = value;
                    this.LayoutButtons();
                    this.RepaintAll();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Recalculates the layout of the control. This method should be called after all changes to the tabs, buttons are completed so
        /// layout of the control recalculated.
        /// </summary>
        public void RecalcLayout()
        {
            this.StopBubbleEffect();
            this.LayoutButtons();
            this.Invalidate();
            OnItemLayoutUpdated(EventArgs.Empty);
        }

        /// <summary>
        /// Occurs after internal item layout has been updated and items have valid bounds assigned.
        /// </summary>
        public event EventHandler ItemLayoutUpdated;
        /// <summary>
        /// Raises ItemLayoutUpdated event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnItemLayoutUpdated(EventArgs e)
        {
            EventHandler handler = ItemLayoutUpdated;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Returns reference to the button at specified location
        /// </summary>
        /// <param name="x">x - coordinate</param>
        /// <param name="y">y - coordinate</param>
        /// <returns>Reference to the button or null if no button could be found at given coordinates.</returns>
        public BubbleButton GetButtonAt(int x, int y)
        {
            if (m_SelectedTab == null)
                return null;
            BubbleButton ret = null;
            foreach (BubbleButton button in m_SelectedTab.Buttons)
            {
                if (!button.Visible) continue;
                if (m_MouseOverButton != null)
                {
                    Rectangle r = button.MagnifiedDisplayRectangle;
                    if (r.Contains(x, y))
                    {
                        ret = button;
                        break;
                    }
                }
                else
                {
                    Rectangle r = button.DisplayRectangle;
                    if (r.Contains(x, y))
                    {
                        ret = button;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns reference to the button at specified location
        /// </summary>
        /// <param name="p">Location coordinates</param>
        /// <returns>Reference to the button or null if no button could be found at given coordinates.</returns>
        public BubbleButton GetButtonAt(Point p)
        {
            return GetButtonAt(p.X, p.Y);
        }

        /// <summary>
        /// Returns tab at specific location.
        /// </summary>
        /// <param name="p">Coordinates to get the tab from.</param>
        /// <returns>Reference to the tab object or null if tab cannot be found at specified location</returns>
        public BubbleBarTab GetTabAt(Point p)
        {
            return GetTabAt(p.X, p.Y);
        }

        /// <summary>
        /// Returns tab at specific location.
        /// </summary>
        /// <param name="x">x - coordinate </param>
        /// <param name="y">y - coordinate</param>
        /// <returns>Reference to the tab object or null if tab cannot be found at specified location</returns>
        public BubbleBarTab GetTabAt(int x, int y)
        {
            foreach (BubbleBarTab tab in m_Tabs)
            {
                if (tab.DisplayRectangle.Contains(x, y))
                    return tab;
            }
            return null;
        }

        #endregion

        #region Internal Implementation
        protected override void OnCursorChanged(EventArgs e)
        {
            if (m_Overlay != null)
                m_Overlay.Cursor = this.Cursor;
            base.OnCursorChanged(e);
        }
        /// <summary>
        /// Invokes BubbleStart event.
        /// </summary>
        protected virtual void OnBubbleStart(EventArgs e)
        {
            if (BubbleStart != null)
                BubbleStart(this, e);
        }

        /// <summary>
        /// Invokes BubbleEnd event.
        /// </summary>
        protected virtual void OnBubbleEnd(EventArgs e)
        {
            if (BubbleEnd != null)
                BubbleEnd(this, e);
        }
        /// <summary>
        /// Invokes ButtonClick event on the control.
        /// </summary>
        /// <param name="button">Reference to the button that was clicked.</param>
        internal void InvokeButtonClick(BubbleButton button, ClickEventArgs e)
        {
            if (ButtonClick != null)
                ButtonClick(button, e);
        }
        protected override bool ProcessMnemonic(char charCode)
        {
            string s = "&" + charCode.ToString();
            s = s.ToLower();
            foreach (BubbleBarTab tab in this.Tabs)
            {
                string text = tab.Text.ToLower();
                if (text.IndexOf(s) >= 0)
                {
                    this.SelectedTab = tab;
                    return true;
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            Application.DoEvents();
            if (m_ButtonBackAreaStyle.GetColorScheme() != null)
                m_ButtonBackAreaStyle.GetColorScheme().Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width == 0 || this.Height == 0)
                return;

            this.RecalcLayout();
            this.Refresh();
        }

        private void RepaintAll()
        {
            if (m_Overlay != null)
            {
                m_Overlay.UpdateWindow();
                //				m_Overlay.Parent.Invalidate(m_Overlay.Bounds,true);
                //				m_Overlay.Parent.Update();
                //				m_Overlay.Update();
                //m_Overlay.Refresh();
                this.Refresh();
            }
            else
                this.Refresh();
        }

        private void ElementStyleChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                this.LayoutButtons();
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.BackColor.A < 255)
            {
                base.OnPaintBackground(e);
            }

            if (!this.BackColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(brush, this.DisplayRectangle);
            }

            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo();
            info.Bounds = this.DisplayRectangle;
            info.Graphics = e.Graphics;
            info.Style = m_BackgroundStyle;
            ElementStyleDisplay.Paint(info);

            if (!m_ButtonBounds.IsEmpty && this.Tabs.Count > 0)
            {
                Rectangle r = GetButtonsBackground();
                info.Bounds = r;
                info.Graphics = e.Graphics;
                info.Style = m_ButtonBackAreaStyle;
                ElementStyleDisplay.Paint(info);
            }

            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;

            if (m_AntiAlias)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            if (!m_TabsBounds.IsEmpty)
            {
                Rectangle r = GetTabBounds();
                e.Graphics.SetClip(r);
                ISimpleTab[] tabs = new ISimpleTab[m_Tabs.Count];
                m_Tabs.CopyTo(tabs);
                m_TabDisplay.Paint(e.Graphics, tabs);
                e.Graphics.ResetClip();
            }

            if (m_Overlay == null)
            {
                BubbleButtonDisplayInfo displayInfo = GetBubbleButtonDisplayInfo();
                displayInfo.Graphics = e.Graphics;

                if (m_SelectedTab != null)
                {
                    foreach (BubbleButton button in m_SelectedTab.Buttons)
                    {
                        displayInfo.Button = button;
                        BubbleButtonDisplay.Paint(displayInfo);
                    }
                }
            }

            if (this.DesignMode && this.Tabs.Count == 0)
            {
                Rectangle r = this.DisplayRectangle;
                eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter |
                    eTextFormat.EndEllipsis | eTextFormat.WordBreak;
                string INFO_TEXT = "Right-click and choose Create Tab or Button to add new items.";
                TextDrawing.DrawString(e.Graphics, INFO_TEXT, this.Font, SystemColors.ControlDarkDark, r, format);
            }

#if !TRIAL
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(128, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
#endif
            e.Graphics.TextRenderingHint = th;
            e.Graphics.SmoothingMode = sm;
        }

        internal void OnPaintOverlay(PaintEventArgs e)
        {
            BubbleButtonDisplayInfo displayInfo = GetBubbleButtonDisplayInfo();
            displayInfo.Graphics = e.Graphics;

            if (m_SelectedTab != null)
            {
                foreach (BubbleButton button in m_SelectedTab.Buttons)
                {
                    displayInfo.Button = button;
                    BubbleButtonDisplay.Paint(displayInfo);
                }
            }
        }

        private bool IsParentFormActive()
        {
            Form form = this.FindForm();
            if (form != null && form.MdiParent == null && form.Parent is Control)
            {
                form = form.Parent.FindForm();
            }

            if (form != null)
            {
                return (Form.ActiveForm == form && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild == form);
            }

            bool ret = false;
            IntPtr foregroundWindow = NativeFunctions.GetForegroundWindow();
            try
            {
                Control c = Control.FromHandle(foregroundWindow);
                if (c != null)
                    ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        private void CreateOverlay()
        {
            if (m_Overlay == null)
            {
                OnBubbleStart(new EventArgs());
            }

            if (m_Overlay != null || this.Parent == null || !m_EnableOverlay || this.DesignMode || !m_AnimationEnabled)
                return;

            Rectangle overlayBounds = this.GetOverlayBounds();
            m_Overlay = new BubbleBarOverlay(this);
            m_Overlay.Bounds = overlayBounds;
            m_Overlay.Visible = false;

            // Layout buttons for overlay
            m_ContentManager.MouseOverIndex = -1;
            this.LayoutButtons();
            if (m_MouseOverButton != null)
                m_ContentManager.MouseOverIndex = m_MouseOverButton.Parent.Buttons.IndexOf(m_MouseOverButton);

            m_Overlay.CreateControl();
            Point p = this.Parent.PointToScreen(m_Overlay.Location);
            m_Overlay.BeforeShow();
            int hwndInsertAfter = NativeFunctions.HWND_NOTOPMOST;
            if (this.FindForm() != null && this.FindForm().TopMost)
                hwndInsertAfter = NativeFunctions.HWND_TOPMOST;
            NativeFunctions.SetWindowPos(m_Overlay.Handle, hwndInsertAfter, p.X, p.Y, m_Overlay.Width, m_Overlay.Height, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
            m_Overlay.Visible = true;
            m_Overlay.UpdateWindow();
            this.Refresh();
        }

        private Rectangle GetOverlayBounds()
        {
            Rectangle r = new Rectangle(this.Location, this.Size);
            if (m_ButtonAlignment == eBubbleButtonAlignment.Top || m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
            {
                int increase = (m_ImageSizeLarge.Height - m_ImageSizeNormal.Height);
                if (this.ShowTooltips)
                {
                    if (this.TooltipFont != null)
                        m_MagTooltipIncrease = this.TooltipFont.Height + 6;
                    else
                        m_MagTooltipIncrease = this.Font.Height + 6;

                    increase += m_MagTooltipIncrease;
                }
                r.Height += increase;
                if (m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
                    r.Y -= increase;
            }
            else
            {

            }
            return r;
        }

        /// <summary>
        /// Called after overlay window became inactive.
        /// </summary>
        internal void OverlayInactive()
        {
            SetMouseDown(null);
            // Mouse Over destroys overlay window as well
            SetMouseOver(null);
            this.RepaintAll();
        }

        /// <summary>
        /// Stops the bubble animation effect is one is applied currently.
        /// </summary>
        public void StopBubbleEffect()
        {
            SetMouseDown(null);
            SetMouseOver(null);
        }

        private void DestroyOverlay()
        {
            if (m_Overlay == null)
                return;

            OnBubbleEnd(new EventArgs());

            m_Overlay.Hide();
            m_Overlay.AfterClose();
            m_Overlay.Dispose();
            m_Overlay = null;

            // Layout buttons without overlay
            m_ContentManager.MouseOverIndex = -1;
            this.LayoutButtons();
            if (m_MouseOverButton != null)
                m_ContentManager.MouseOverIndex = m_MouseOverButton.Parent.Buttons.IndexOf(m_MouseOverButton);
            this.Invalidate();
        }

        private BubbleButtonDisplayInfo GetBubbleButtonDisplayInfo()
        {
            m_BubbleButtonDisplayInfo.Magnified = (m_MouseOverButton != null) && m_AnimationEnabled;
            m_BubbleButtonDisplayInfo.Alignment = m_ButtonAlignment;
            return m_BubbleButtonDisplayInfo;
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsParentFormActive())
                return;
            if (m_Overlay != null && m_MagTooltipIncrease > 0)
            {
                Point p = this.PointToScreen(new Point(e.X, e.Y));
                p = m_Overlay.PointToClient(p);
                MouseEventArgs em = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
                MouseMoveMessage(em);
            }
            else
                MouseMoveMessage(e);
        }

        /// <summary>
        /// Internal processing of MouseMove event.
        /// </summary>
        /// <param name="e">Move move event arguments.</param>
        internal void MouseMoveMessage(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_IgnoreMouseMove)
            {
                m_IgnoreMouseMove = false;
                return;
            }

            bool bRefresh = false;

            Point mousePosition = new Point(e.X, e.Y);
            BubbleButton mouseOver = GetButtonAt(mousePosition.X, mousePosition.Y);
            if (mouseOver != null && mouseOver == m_IgnoreButtonMouseMove)
                return;
            else if (mouseOver == null)
                m_IgnoreButtonMouseMove = null;

            if (mouseOver != null)
            {
                if (m_MouseOverButton == null)
                {
                    CreateOverlay();
                    // Set to enable painting of magnified buttons...
                    m_MouseOverButton = mouseOver;
                    AnimateButton(mouseOver, mousePosition, true);
                    m_MouseOverButton = null;
                }

                if (m_AnimationEnabled)
                {
                    BubbleFactors factors = this.GetBubbleFactors(mouseOver.DisplayRectangle, mousePosition);
                    m_ContentManager.Factor1 = factors.Factor1;
                    m_ContentManager.Factor2 = factors.Factor2;
                    m_ContentManager.Factor3 = factors.Factor3;
                    m_ContentManager.Factor4 = factors.Factor4;
                    m_ContentManager.BubbleSize = m_ImageSizeLarge;
                    m_ContentManager.MouseOverPosition = factors.x;
                    m_ContentManager.MouseOverIndex = mouseOver.Parent.Buttons.IndexOf(mouseOver);
                    this.LayoutButtons();
                    bRefresh = true;
                    m_LastMouseOverPosition = mousePosition;
                }
            }

            if (m_MouseOverTab != null || m_TabsBounds.Contains(mousePosition))
            {
                BubbleBarTab tab = GetTabAt(mousePosition);
                SetMouseOverTab(tab);
            }

            if (m_MouseOverButton != mouseOver)
            {
                SetMouseOver(mouseOver);
                bRefresh |= true;
            }

            if (m_MouseOverButton != mouseOver && e.Button == MouseButtons.Left)
            {
                SetMouseDown(mouseOver);
                bRefresh |= true;
            }

            // Make sure that cursor did not escape while animation was going on...
            if (m_MouseOverButton != null)
            {
                Point p = Control.MousePosition;
                if (m_Overlay != null)
                    p = m_Overlay.PointToClient(p);
                else
                    p = this.PointToClient(p);
                mouseOver = GetButtonAt(p.X, p.Y);
                if (mouseOver != m_MouseOverButton)
                {
                    SetMouseOver(mouseOver);
                    bRefresh |= true;
                }
            }

            if (bRefresh)
                this.RepaintAll();
        }

        private void SetMouseOver(BubbleButton mouseOver)
        {
            if (m_MouseOverButton != null)
            {
                m_MouseOverButton.SetMouseOver(false);
                if (mouseOver == null && !m_LastMouseOverPosition.IsEmpty)
                {
                    AnimateButton(m_MouseOverButton, m_LastMouseOverPosition, false);
                    m_LastMouseOverPosition = Point.Empty;
                }
            }

            m_MouseOverButton = mouseOver;
            if (m_MouseOverButton != null)
            {
                m_MouseOverButton.SetMouseOver(true);
                CreateOverlay();
            }
            else
            {
                SetMouseDown(null);
                DestroyOverlay();
            }
            if (m_MouseOverButton != null)
                m_ContentManager.MouseOverIndex = m_MouseOverButton.Parent.Buttons.IndexOf(m_MouseOverButton);
            else
                m_ContentManager.MouseOverIndex = -1;
        }

        private void SetMouseDown(BubbleButton mouseDown)
        {
            if (m_MouseDownButton != null)
                m_MouseDownButton.SetMouseDown(false);

            if (mouseDown != null && !mouseDown.Enabled)
            {
                m_MouseDownButton = null;
                return;
            }

            m_MouseDownButton = mouseDown;
            if (m_MouseDownButton != null)
                m_MouseDownButton.SetMouseDown(true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            StopMouseLeaveTimer();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseLeaveMessage(e);
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Internal processing for MouseLeave event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        internal void MouseLeaveMessage(EventArgs e)
        {
            Point p = Point.Empty;
            if (m_MouseOverButton != null && m_Overlay != null)
                p = m_Overlay.PointToClient(Control.MousePosition);
            else
                p = this.PointToClient(Control.MousePosition);

            bool bRepaint = false;
            BubbleButton buttonAt = GetButtonAt(p.X, p.Y);

            if (m_MouseDownButton != null && buttonAt == null)
            {
                SetMouseDown(null);
                bRepaint = true;
            }

            SetMouseOverTab(null);

            if (bRepaint)
                this.RepaintAll();
            StartMouseLeaveTimer();
            m_IgnoreButtonMouseMove = null;
        }

        private Timer m_MouseLeaveTimer = null;
        private void StartMouseLeaveTimer()
        {
            if (m_Overlay == null || m_MouseOverButton == null || m_MouseLeaveTimer != null) return;

            m_MouseLeaveTimer = new Timer();
            m_MouseLeaveTimer.Interval = 500;
            m_MouseLeaveTimer.Tick += new EventHandler(MouseLeaveTimerTick);
            m_MouseLeaveTimer.Start();
        }

        private void MouseLeaveTimerTick(object sender, EventArgs e)
        {
            Point p = Point.Empty;
            if (m_MouseOverButton != null && m_Overlay != null)
                p = m_Overlay.PointToClient(Control.MousePosition);
            else
                p = this.PointToClient(Control.MousePosition);

            bool bRepaint = false;
            BubbleButton buttonAt = GetButtonAt(p.X, p.Y);

            if (buttonAt == null)
            {
                StopBubbleEffect();
                StopMouseLeaveTimer();
            }
        }

        private void StopMouseLeaveTimer()
        {
            Timer t = m_MouseLeaveTimer;
            m_MouseLeaveTimer = null;
            if (t == null) return;
            t.Stop();
            t.Dispose();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseDownMessage(e);
            base.OnMouseDown(e);
        }

        internal void OverlayMouseDownMessage(MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void MouseDownMessage(MouseEventArgs e)
        {
            BubbleButton mouseOver = GetButtonAt(e.X, e.Y);
            if (mouseOver != null)
            {
                SetMouseDown(mouseOver);
                this.RepaintAll();
            }
            else if (m_TabsBounds.Contains(e.X, e.Y))
            {
                BubbleBarTab tab = GetTabAt(new Point(e.X, e.Y));
                if (tab != null && tab != m_SelectedTab)
                    SetSelectedTab(tab, eEventSource.Mouse, true);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseUpMessage(e);
            base.OnMouseUp(e);
        }

        internal void OverlayMouseUpMessage(MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void MouseUpMessage(MouseEventArgs e)
        {
            if (m_MouseDownButton != null)
            {
                BubbleButton button = m_MouseDownButton;
                Rectangle dispRect = button.DisplayRectangle;
                Rectangle magRect = button.MagnifiedDisplayRectangle;
                SetMouseDown(null);
                SetMouseOver(null);
                m_IgnoreMouseMove = true;
                this.RepaintAll();
                if (dispRect.Contains(e.X, e.Y) || magRect.Contains(e.X, e.Y))
                    button.InvokeClick(eEventSource.Mouse, e.Button);
                m_IgnoreButtonMouseMove = button;
            }
        }

        private void ImageListDisposed(object sender, EventArgs e)
        {
            if (sender == m_Images)
            {
                this.Images = null;
            }
            else if (sender == m_ImagesLarge)
            {
                this.ImagesLarge = null;
            }
        }

        /// <summary>
        /// Called after all buttons have been removed.
        /// </summary>
        /// <param name="tab">Tab from which all buttons were removed.</param>
        internal void OnButtonsCollectionClear(BubbleBarTab tab)
        {
            StopBubbleEffect();
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        /// <summary>
        /// Called after specified button has been removed.
        /// </summary>
        /// <param name="tab">Tab from which button was removed.</param>
        /// <param name="button">Button that was removed.</param>
        internal void OnButtonRemoved(BubbleBarTab tab, BubbleButton button)
        {
            StopBubbleEffect();
            if (m_HasShortcuts && !this.IsDisposed)
                RefreshHasShortcut();
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        internal void RefreshHasShortcut()
        {
            m_HasShortcuts = false;
            foreach (BubbleBarTab t in this.Tabs)
            {
                foreach (BubbleButton b in t.Buttons)
                {
                    if (b.Shortcut != eShortcut.None)
                    {
                        m_HasShortcuts = true;
                        break;
                    }
                }
                if (m_HasShortcuts)
                    break;
            }
        }

        /// <summary>
        /// Called after new button is added to the Buttons collection.
        /// </summary>
        /// <param name="tab">Tab to which button was added.</param>
        /// <param name="button">Reference to the button added.</param>
        internal void OnButtonInserted(BubbleBarTab tab, BubbleButton button)
        {
            StopBubbleEffect();
            LayoutButtons();
            if (button.Shortcut != eShortcut.None)
            {
                m_HasShortcuts = true;
            }
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.RecalcLayout();
            InstallIMessageHandlerClient();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            UninstallIMessageHandlerClient();
            StopMouseLeaveTimer();
        }

        /// <summary>
        /// Called when Visible property of Button has changed.
        /// </summary>
        /// <param name="button">Button affected.</param>
        internal void OnButtonVisibleChanged(BubbleButton button)
        {
            LayoutButtons();
        }

        private void LayoutButtons()
        {
            if (m_SelectedTab != null)
            {
                if (m_SelectedTab.Buttons.Count > 0)
                {
                    IBlock[] buttons = new IBlock[m_SelectedTab.Buttons.Count];
                    m_SelectedTab.Buttons.CopyTo(buttons);
                    Rectangle r = m_ContentManager.Layout(GetButtonDisplayArea(), buttons, m_ButtonLayoutManager);
                    if (m_Overlay != null)
                    {
                        m_ButtonBounds.X = r.X;
                        m_ButtonBounds.Width = r.Width;
                    }
                    else
                        m_ButtonBounds = r;
                }
                else
                {
                    m_ButtonBounds = GetButtonDisplayArea();
                    m_ButtonBounds.Inflate(-m_ButtonBounds.Width / 2, 0);
                    int b = m_ButtonBounds.Bottom;
                    m_ButtonBounds.Height = m_ImageSizeNormal.Height;
                    if (m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
                        m_ButtonBounds.Y = b - m_ButtonBounds.Height;
                }
            }

            LayoutTabs();
        }

        private void LayoutTabs()
        {
            if (!BarFunctions.IsHandleValid(this))
                return;
            // Layout tabs
            if (m_TabsVisible && m_Tabs.Count > 0)
            {
                IBlock[] tabs = new IBlock[m_Tabs.Count];
                m_Tabs.CopyTo(tabs);
                Graphics g = null;
                try
                {
                    g = this.CreateGraphics();
                    m_TabLayoutManager.Graphics = g;
                    m_TabsBounds = m_TabContentManager.Layout(GetTabDisplayArea(), tabs, m_TabLayoutManager);
                }
                finally
                {
                    m_TabLayoutManager.Graphics = null;
                    if (g != null)
                        g.Dispose();
                }
            }
            else
                m_TabsBounds = Rectangle.Empty;
        }

        private Rectangle GetButtonDisplayArea()
        {
            Rectangle r = this.DisplayRectangle;
            if (m_Overlay != null)
                r = m_Overlay.DisplayRectangle;
            if (m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
                r.Height -= m_ButtonMargin;
            else if (m_ButtonAlignment == eBubbleButtonAlignment.Top)
            {
                r.Height -= m_ButtonMargin;
                r.Y += m_ButtonMargin;
            }

            return r;
        }

        /// <summary>
        /// Returns the button background rectangle for display purposes. Applies setting for the ButtonBackgroundStretch property.
        /// </summary>
        /// <returns>Background rectangle.</returns>
        private Rectangle GetButtonsBackground()
        {
            Rectangle r = m_ButtonBounds;
            if (m_ButtonBackgroundStretch)
            {
                r.Width = this.Width;
                r.X = 0;
            }
            else if (m_TabsBounds.Width + 16 > r.Width)
            {
                int diff = (m_TabsBounds.Width + 16) - r.Width;
                r.Width += diff;
                r.X -= diff / 2;
            }

            r.Height += (m_ButtonBackAreaStyle.PaddingTop + m_ButtonBackAreaStyle.PaddingBottom);
            r.Y -= m_ButtonBackAreaStyle.PaddingTop;

            if (!m_ButtonBackgroundStretch)
            {
                r.Width += (m_ButtonBackAreaStyle.PaddingLeft + m_ButtonBackAreaStyle.PaddingRight);
                r.X -= m_ButtonBackAreaStyle.PaddingLeft;
            }

            return r;
        }

        private Rectangle GetTabDisplayArea()
        {
            Rectangle r;
            Rectangle rButtonsBack = GetButtonsBackground();

            if (m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
            {
                r = new Rectangle(rButtonsBack.X, 0, rButtonsBack.Width, rButtonsBack.Y);
            }
            else
            {
                r = new Rectangle(rButtonsBack.X, rButtonsBack.Bottom, rButtonsBack.Width, this.Height - rButtonsBack.Bottom);
            }
            if (r.Height < 0)
                r.Height = 0;

            r.Width -= 16;
            r.X += 8;
            return r;
        }

        private Rectangle GetTabBounds()
        {
            Rectangle r = m_TabsBounds;
            if (!r.IsEmpty)
            {
                r.Width += 16;
                r.X -= 8;
            }
            return r;
        }

        private void AnimateButton(BubbleButton button, Point mousePosition, bool animateGrow)
        {
            if (m_AnimationTime <= 0 || m_Animation || m_SelectedTab == null || !m_AnimationEnabled)
                return;

            m_Animation = true;
            try
            {
                m_ContentManager.MouseOverIndex = m_SelectedTab.Buttons.IndexOf(button);
                bool animate = true;
                int totalSteps = m_ImageSizeLarge.Width - m_ImageSizeNormal.Width;
                int step = 1;
                int current = m_ImageSizeNormal.Width + step;
                if (!animateGrow)
                    current = m_ImageSizeLarge.Width - step;

                Rectangle displayRectangle = GetButtonDisplayArea();

                IBlock[] blocks = new IBlock[m_SelectedTab.Buttons.Count];
                m_SelectedTab.Buttons.CopyTo(blocks);

                DateTime start = DateTime.Now;
                TimeSpan stepDuration = TimeSpan.MinValue;

                while (animate)
                {
                    DateTime stepStart = DateTime.Now;

                    float multi = (float)current / (float)m_ImageSizeNormal.Width;
                    Size bubbleSize = new Size((int)((float)m_ImageSizeNormal.Width * multi), (int)((float)m_ImageSizeNormal.Height * multi));
                    BubbleFactors factors = this.GetBubbleFactors(button.DisplayRectangle, mousePosition, bubbleSize);
                    m_ContentManager.Factor1 = factors.Factor1;
                    m_ContentManager.Factor2 = factors.Factor2;
                    m_ContentManager.Factor3 = factors.Factor3;
                    m_ContentManager.Factor4 = factors.Factor4;
                    m_ContentManager.BubbleSize = bubbleSize;
                    m_ContentManager.MouseOverPosition = factors.x;
                    m_ContentManager.Layout(displayRectangle, blocks, m_ButtonLayoutManager);
                    this.RepaintAll();

                    stepDuration = DateTime.Now.Subtract(stepStart);
                    step = (int)((float)totalSteps * ((float)stepDuration.TotalMilliseconds / (float)m_AnimationTime));
                    if (step <= 0)
                    {
                        int diff = (int)(m_AnimationTime / Math.Max((float)stepDuration.TotalMilliseconds, (float)1) / totalSteps);
                        if (diff <= 0)
                            diff = (int)stepDuration.TotalMilliseconds;
                        System.Threading.Thread.Sleep(diff);
                        step = 1;
                    }
                    if (animateGrow)
                        current += step;
                    else
                        current -= step;
                    totalSteps -= step;
                    if (totalSteps <= 0 || DateTime.Now.Subtract(start).TotalMilliseconds >= m_AnimationTime)
                        break;
                }
            }
            finally
            {
                m_Animation = false;
            }
        }

        private BubbleFactors GetBubbleFactors(Rectangle buttonMouseOver, Point mousePosition)
        {
            return GetBubbleFactors(buttonMouseOver, mousePosition, m_ImageSizeLarge);
        }

        private BubbleFactors GetBubbleFactors(Rectangle buttonMouseOver, Point mousePosition, Size finalSize)
        {
            BubbleFactors factors = new BubbleFactors();

            float pp = 0;
            int xa = 0, ya = 0;
            int xDiff = Math.Max(mousePosition.X - buttonMouseOver.X, 0);
            xDiff = Math.Min(xDiff, buttonMouseOver.Width);
            int yDiff = Math.Max(mousePosition.Y - buttonMouseOver.Y, 0);
            yDiff = Math.Min(yDiff, buttonMouseOver.Height);

            if (m_Orientation == eOrientation.Horizontal)
            {
                pp = (float)(xDiff) / (float)buttonMouseOver.Width;
                xa = mousePosition.X - (int)(pp * (float)finalSize.Width);
            }
            else
            {
                pp = (float)(yDiff) / (float)buttonMouseOver.Height;
                ya = mousePosition.Y - (int)(pp * (float)finalSize.Height);
            }

            factors.x = xa;
            factors.y = ya;

            factors.Factor1 = (1 - pp) * _falloffFactor;
            factors.Factor2 = Math.Max(1 - pp, _falloffFactor);
            factors.Factor3 = Math.Max(pp, _falloffFactor);
            factors.Factor4 = pp * _falloffFactor;

            return factors;
        }

        private class BubbleFactors
        {
            public int x = 0;
            public int y = 0;
            public float Factor1 = 0;
            public float Factor2 = 0;
            public float Factor3 = 0;
            public float Factor4 = 0;
        }

        private void ApplyButtonAlignment()
        {
            m_TabContentManager.BlockSpacing = 8;

            if (m_ButtonAlignment == eBubbleButtonAlignment.Bottom)
            {
                m_ContentManager.ContentAlignment = eContentAlignment.Center;
                m_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
                m_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
                m_ContentManager.ContentOrientation = eContentOrientation.Horizontal;

                m_TabContentManager.ContentAlignment = eContentAlignment.Center;
                m_TabContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
                m_TabContentManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
                m_TabContentManager.ContentOrientation = eContentOrientation.Horizontal;
            }
            else if (m_ButtonAlignment == eBubbleButtonAlignment.Top)
            {
                m_ContentManager.ContentAlignment = eContentAlignment.Center;
                m_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                m_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
                m_ContentManager.ContentOrientation = eContentOrientation.Horizontal;

                m_TabContentManager.ContentAlignment = eContentAlignment.Center;
                m_TabContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                m_TabContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
                m_TabContentManager.ContentOrientation = eContentOrientation.Horizontal;
            }
            //			else if(m_ButtonAlignment==eBubbleButtonAlignment.Left)
            //			{
            //				m_ContentManager.ContentAlignment=eContentAlignment.Center;
            //				m_ContentManager.ContentVerticalAlignment=eContentVerticalAlignment.Bottom;
            //				m_ContentManager.ContentOrientation=eContentOrientation.Horizontal;
            //			}
        }
        #endregion

        #region Tabs Support
        /// <summary>
        /// Called after tab has been removed from the collection.
        /// </summary>
        /// <param name="tab">Tab that was removed.</param>
        internal void OnTabRemoved(BubbleBarTab tab)
        {
            if (m_SelectedTab == tab)
            {
                SelectNextVisible(tab, eEventSource.Code, false);
            }
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        /// <summary>
        /// Called after tab has been added to the collection.
        /// </summary>
        /// <param name="tab">Newly added tab.</param>
        internal void OnTabAdded(BubbleBarTab tab)
        {
            if (m_SelectedTab == null)
                m_SelectedTab = tab;
            this.LayoutButtons();
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        /// <summary>
        /// Called after all tabs are removed from the collection.
        /// </summary>
        internal void OnTabsCleared()
        {
            m_SelectedTab = null;
            this.LayoutButtons();
            if (this.DesignMode)
            {
                this.RecalcLayout();
                this.Refresh();
            }
        }

        /// <summary>
        /// Called after text of a tab has changed.
        /// </summary>
        /// <param name="tab">Tab which text has changed.</param>
        internal void OnTabTextChanged(BubbleBarTab tab)
        {
            this.RecalcLayout();
            this.Refresh();
        }

        /// <summary>
        /// Called after Visible property of the tab has changed.
        /// </summary>
        /// <param name="tab">Tab affected.</param>
        internal void OnTabVisibleChanged(BubbleBarTab tab)
        {
            if (m_SelectedTab == tab)
            {
                SelectNextVisible(tab, eEventSource.Code, false);
            }
            this.LayoutButtons();
            this.Refresh();
        }

        private void OnSelectedTabChanged()
        {
            StopBubbleEffect();
            m_IgnoreButtonMouseMove = null;
            LayoutButtons();
            this.Refresh();
        }

        internal BubbleBarTab GetMouseOverTab()
        {
            return m_MouseOverTab;
        }

        private void SelectNextVisible(BubbleBarTab reference, eEventSource source, bool bCanCancel)
        {
            BubbleBarTab sel = m_Tabs.GetNextVisibleTab(reference);
            if (sel == null)
                sel = m_Tabs.GetPreviousVisibleTab(reference);
            SetSelectedTab(sel, source, bCanCancel);
        }

        private void SetSelectedTab(BubbleBarTab tab, eEventSource source, bool bCanCancel)
        {
            if (TabChanging != null)
            {
                BubbleBarTabChangingEventArgs e = new BubbleBarTabChangingEventArgs();
                e.CurrentTab = m_SelectedTab;
                e.NewTab = tab;
                e.Source = source;
                TabChanging(this, e);
                if (e.Cancel && bCanCancel) return;
            }
            m_SelectedTab = tab;
            m_IgnoreButtonMouseMove = null;
            this.LayoutButtons();
            this.RepaintAll();
        }

        /// <summary>
        /// Sets the tab mouse is placed over.
        /// </summary>
        /// <param name="tab">Tab that mouse is currently over or null if mouse is not over any tab.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetMouseOverTab(BubbleBarTab tab)
        {
            if (m_MouseOverTab == tab)
                return;

            m_MouseOverTab = tab;

            this.Invalidate(GetTabBounds());
            this.Update();
        }
        #endregion

        #region Drag & Drop support
        private BubbleBarTab m_DragTab = null;
        private int m_DragTabOriginalIndex = -1;
        private int m_DragButtonOriginalIndex = -1;
        private BubbleButton m_DragButton = null;
        private bool m_DragInProgress = false;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DragInProgress
        {
            get { return m_DragInProgress; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StartDrag(BubbleBarTab tab)
        {
            if (m_DragInProgress)
                return;

            m_DragInProgress = true;
            m_DragTab = tab;
            Cursor.Current = Cursors.Hand;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StartDrag(BubbleButton button)
        {
            if (m_DragInProgress)
                return;

            m_DragInProgress = true;
            m_DragButton = button;
            Cursor.Current = Cursors.Hand;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DragMouseMove(Point mousePosition)
        {
            if (m_DragTab != null)
            {
                BubbleBarTab tab = GetTabAt(mousePosition);
                if (tab != null && tab != m_DragTab)
                {
                    if (m_DragTabOriginalIndex == -1)
                        m_DragTabOriginalIndex = this.Tabs.IndexOf(m_DragTab);
                    int insertPos = this.Tabs.IndexOf(tab);
                    this.Tabs.Remove(m_DragTab);
                    this.Tabs.Insert(insertPos, m_DragTab);
                    this.SelectedTab = m_DragTab;
                }
            }
            else if (m_DragButton != null)
            {
                BubbleBarTab tab = GetTabAt(mousePosition);
                if (tab != null && tab != m_DragButton.Parent)
                {
                    // Move button to the tab and select that tab
                    if (m_DragTabOriginalIndex == -1)
                        m_DragTabOriginalIndex = this.Tabs.IndexOf(m_DragButton.Parent);
                    if (m_DragButtonOriginalIndex == -1)
                        m_DragButtonOriginalIndex = m_DragButton.Parent.Buttons.IndexOf(m_DragButton);
                    this.SelectedTab = tab;
                    BubbleButton b = GetButtonAt(mousePosition);
                    if (m_DragButton.Parent != null)
                        m_DragButton.Parent.Buttons.Remove(m_DragButton);
                    if (b != null)
                        tab.Buttons.Insert(tab.Buttons.IndexOf(b), m_DragButton);
                    else
                        tab.Buttons.Add(m_DragButton);
                    this.RecalcLayout();
                    this.Refresh();
                }
                else
                {
                    BubbleButton button = GetButtonAt(mousePosition);
                    if (button != null && button != m_DragButton)
                    {
                        if (m_DragTabOriginalIndex == -1)
                            m_DragTabOriginalIndex = this.Tabs.IndexOf(m_DragButton.Parent);
                        if (m_DragButtonOriginalIndex == -1)
                            m_DragButtonOriginalIndex = m_DragButton.Parent.Buttons.IndexOf(m_DragButton);
                        BubbleBarTab parent = m_DragButton.Parent;
                        int insertIndex = parent.Buttons.IndexOf(button);
                        parent.Buttons.Remove(m_DragButton);
                        parent.Buttons.Insert(insertIndex, m_DragButton);
                        this.RecalcLayout();
                        this.Refresh();
                    }
                }
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DragMouseUp(Point mousePosition)
        {
            if (m_DragTab != null)
            {
                BubbleBarTab tab = GetTabAt(mousePosition);
                if (tab != m_DragTab)
                {
                    DragCancel();
                }
                else
                {
                    m_DragTab = null;
                    m_DragTabOriginalIndex = -1;
                }
            }
            else if (m_DragButton != null)
            {
                BubbleButton button = GetButtonAt(mousePosition);
                if (button != m_DragButton)
                    DragCancel();
                else
                {
                    m_DragButton = null;
                    m_DragTabOriginalIndex = -1;
                    m_DragButtonOriginalIndex = -1;
                }
            }

            m_DragInProgress = false;
        }

        internal void DragCancel()
        {
            if (!m_DragInProgress)
                return;

            if (m_DragTab != null)
            {
                if (m_DragTabOriginalIndex >= 0)
                {
                    this.Tabs.Remove(m_DragTab);
                    this.Tabs.Insert(m_DragTabOriginalIndex, m_DragTab);
                    this.RecalcLayout();
                    this.Refresh();
                }
                m_DragTab = null;
                m_DragTabOriginalIndex = -1;
            }
            else if (m_DragButton != null)
            {
                if (m_DragButtonOriginalIndex >= 0)
                {
                    if (m_DragButton.Parent != null)
                        m_DragButton.Parent.Buttons.Remove(m_DragButton);
                    if (m_DragTabOriginalIndex >= 0 && m_DragTabOriginalIndex != this.Tabs.IndexOf(m_DragButton.Parent))
                    {
                        this.Tabs[m_DragTabOriginalIndex].Buttons.Insert(m_DragButtonOriginalIndex, m_DragButton);
                    }
                    else
                        this.Tabs[m_DragTabOriginalIndex].Buttons.Insert(m_DragButtonOriginalIndex, m_DragButton);
                    m_DragButtonOriginalIndex = -1;
                    m_DragTabOriginalIndex = -1;
                    m_DragButton = null;
                    this.RecalcLayout();
                    this.Refresh();
                }
            }

            m_DragInProgress = false;
        }
        #endregion

        #region ISupportInitialize
        void ISupportInitialize.BeginInit()
        {
        }
        void ISupportInitialize.EndInit()
        {
            //this.RecalcLayout();
        }
        #endregion

        #region IMessageHandlerClient Implementation

        bool IMessageHandlerClient.IsModal
        {
            get
            {
                Form form = this.FindForm();
                if (form != null)
                    return form.Modal;
                return false;
            }
        }
        bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }
        bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            // Check Shortcuts
            if (System.Windows.Forms.Control.ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
            {
                int i = (int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
                if (ProcessShortcut((eShortcut)i))
                    return true;
            }
            return false;
        }
        private bool ProcessShortcut(eShortcut key)
        {
            Form form = this.FindForm();
            if (form == null || (form != Form.ActiveForm && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild != form) && !form.IsMdiContainer || Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != form)
                return false;

            if (m_HasShortcuts)
            {
                foreach (BubbleBarTab t in this.Tabs)
                {
                    foreach (BubbleButton b in t.Buttons)
                    {
                        if (b.Shortcut == key && b.Enabled)
                        {
                            b.InvokeClick(eEventSource.Keyboard, MouseButtons.None);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }
        bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }
        bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (!this.DesignMode)
            {
                // Check Shortcuts
                if (System.Windows.Forms.Control.ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
                {
                    int i = (int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
                    if (ProcessShortcut((eShortcut)i))
                        return true;
                }
            }
            return false;
        }
        bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }
        private void InstallIMessageHandlerClient()
        {
            if (!m_FilterInstalled && !this.DesignMode)
            {
                MessageHandler.RegisterMessageClient(this);
                m_FilterInstalled = true;
            }
        }
        private void UninstallIMessageHandlerClient()
        {
            if (m_FilterInstalled)
            {
                MessageHandler.UnregisterMessageClient(this);
                m_FilterInstalled = false;
            }
        }
        #endregion

        #region Licensing
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

    }

    #region Event handlers declaration
    /// <summary>
    /// Delegate for tab change events.
    /// </summary>
    public delegate void BubbleBarTabChangingEventHadler(object sender, BubbleBarTabChangingEventArgs e);
    #endregion

    #region Event Arguments
    /// <summary>
    /// Represents the event arguments tab changing events.
    /// </summary>
    public class BubbleBarTabChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Specifies the event source.
        /// </summary>
        public eEventSource Source = eEventSource.Code;
        /// <summary>
        /// Specifies newly selected tab.
        /// </summary>
        public BubbleBarTab NewTab = null;
        /// <summary>
        /// Specifies currently selected tab.
        /// </summary>
        public BubbleBarTab CurrentTab = null;
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public BubbleBarTabChangingEventArgs() { }
    }

    #endregion
}
