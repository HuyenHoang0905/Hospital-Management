using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Panel control with title bar that can be expanded or collapsed.
    /// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ExpandablePanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxBitmap(typeof(ExpandablePanel), "ExpandablePanel.ico")]
    public class ExpandablePanel:PanelEx
    {
        #region Private Variables, Events & Constructor
        /// <summary>
        /// Occurs before Expanded property is changed. You can cancel change of this property by setting Cancel=true on the event arguments.
        /// </summary>
        public event ExpandChangeEventHandler ExpandedChanging;
        /// <summary>
        /// Occurs after Expanded property has changed. You can handle ExpandedChanging event and have opportunity to cancel the change.
        /// </summary>
        public event ExpandChangeEventHandler ExpandedChanged;

        private PanelExTitle m_TitleBar = new PanelExTitle();
        private bool m_Expanded = true;
        private int m_AnimationTime = 100;      
        private Rectangle m_ExpandedBounds = Rectangle.Empty;
        private Image m_ButtonImageExpand = null;
        private Image m_ButtonImageCollapse = null;
        private eCollapseDirection m_CollapseDirection = eCollapseDirection.BottomToTop;
        private Image m_Office2007ButtonImageExpand = null;
        private Image m_Office2007ButtonImageCollapse = null;
        private Image m_Office2007ButtonImageExpandMouseOver = null;
        private Image m_Office2007ButtonImageCollapseMouseOver = null;
        private PanelEx m_VerticalExpandPane = null;
        private bool m_ExpandOnTitleClick = false;

        public ExpandablePanel()
		{
            m_TitleBar.Text = "Title Bar";
            m_TitleBar.Location = new Point(0, 0);
            m_TitleBar.Size = new Size(24, 26);
            m_TitleBar.Dock = DockStyle.Top;
            this.Controls.Add(m_TitleBar);
            //m_TitleBar.ApplyPanelStyle();
            m_TitleBar.ColorScheme = this.ColorScheme;
            m_TitleBar.ExpandedClick += new EventHandler(m_TitleBar_ExpandedClick);
            m_TitleBar.Click += new EventHandler(TitleBarClick);
			UpdateExpandButtonImage();
        }

        #endregion

        #region Internal Implementation
        private void TitleBarClick(object sender, EventArgs e)
        {
            if (m_ExpandOnTitleClick && !m_TitleBar.ExpandChangeButton.DisplayRectangle.Contains(m_TitleBar.PointToClient(Control.MousePosition)))
            {
                SetExpanded(!this.Expanded, eEventSource.Mouse);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            m_TitleBar.SendToBack();
        }

		void m_TitleBar_ExpandedClick(object sender, EventArgs e)
        {
            SetExpanded(!this.Expanded, eEventSource.Mouse);
        }

        private void SetExpanded(bool expanded, eEventSource action)
        {
            ExpandedChangeEventArgs e = new ExpandedChangeEventArgs(action, expanded);
            InvokeExpandedChanging(e);
            if (e.Cancel)
                return;
            m_Expanded = expanded;
            OnExpandedChanged();
            InvokeExpandedChanged(e);
        }

        private DockStyle GetControlDock(Control c)
        {
            if (c.Dock == DockStyle.None || c.Dock == DockStyle.Fill)
            {
                return DockStyle.Top;
            }
            return c.Dock;
        }

        private Rectangle GetExpandedBounds()
        {
            Rectangle r = m_ExpandedBounds;
            DockStyle dock = GetControlDock(m_TitleBar);
            if (dock == DockStyle.Left)
            {
                r = new Rectangle(this.Bounds.Location, m_ExpandedBounds.Size);
            }
            else if (dock == DockStyle.Right)
            {
                r = new Rectangle(this.Bounds.X-(m_ExpandedBounds.Width-this.Width),this.Bounds.Y,m_ExpandedBounds.Width,m_ExpandedBounds.Height);
            }
            else if (dock == DockStyle.Top)
            {
                if (m_CollapseDirection == eCollapseDirection.LeftToRight)
                    r = new Rectangle(this.Bounds.X - (m_ExpandedBounds.Width - this.Bounds.Width), this.Bounds.Y, m_ExpandedBounds.Width, m_ExpandedBounds.Height);
                else if (m_CollapseDirection == eCollapseDirection.TopToBottom)
                    r = new Rectangle(this.Bounds.X, m_ExpandedBounds.Y, m_ExpandedBounds.Width, m_ExpandedBounds.Height);
                else
                    r = new Rectangle(this.Bounds.Location, m_ExpandedBounds.Size);
            }
            else if (dock == DockStyle.Bottom)
            {
                r = new Rectangle(this.Bounds.X, this.Bounds.Y-(m_ExpandedBounds.Height-this.Height), m_ExpandedBounds.Width,m_ExpandedBounds.Height);
            }

            return r;
        }

        private void OnExpandedChanged()
        {
            if (this.Expanded)
            {
                m_TitleBar.RenderText = true;
                this.RenderText = true;
                if (!m_ExpandedBounds.IsEmpty)
                {
                    if (m_VerticalExpandPane != null)
                        DisposeVerticalExpandPane();
                    // Show internal controls
                    this.SuspendLayout();
                    foreach (Control c in this.Controls)
                    {
                        if (c is ContextMenuBar) continue;
                        if(c!=this.TitlePanel)
                            c.Visible = true;
                    }
                    this.ResumeLayout();

                    Rectangle targetRect = GetExpandedBounds();
                    if (this.AnimationTime == 0 || this.DesignMode)
                    {
                        TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, targetRect);
                    }
                    else
                    {
                        Rectangle controlRect = this.Bounds;
                        BarFunctions.AnimateControl(this, true, m_AnimationTime, controlRect, targetRect);
                    }
                    TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, Rectangle.Empty);
                }
            }
            else
            {
                TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, this.Bounds);
                Point p = this.PointToScreen(Point.Empty);
                if (this.Parent != null)
                    p = this.Parent.PointToClient(p);
                else
                    p = Point.Empty;

                Rectangle targetRect = new Rectangle(p, new Size(m_TitleBar.Width + this.DockPadding.Left+this.DockPadding.Right,
                    Math.Max(m_TitleBar.Height + this.DockPadding.Top + this.DockPadding.Bottom, m_TitleBar.ExpandChangeButton.HeightInternal)));
                if (m_CollapseDirection == eCollapseDirection.RightToLeft)
                {
                    targetRect = new Rectangle(p, new Size(Math.Max(m_TitleBar.Height + this.DockPadding.Top + this.DockPadding.Bottom, m_TitleBar.ExpandChangeButton.WidthInternal + 6), this.Height));
                }
                else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
                {
                    Size s = new Size(Math.Max(m_TitleBar.Height + this.DockPadding.Top + this.DockPadding.Bottom, m_TitleBar.ExpandChangeButton.WidthInternal + 6), this.Height);
                    if (!p.IsEmpty) p.X += this.Width - s.Width;
                    targetRect = new Rectangle(p, s); 
                }
                else if (m_CollapseDirection == eCollapseDirection.TopToBottom)
                {
                    Size s = new Size(m_TitleBar.Width + this.DockPadding.Left + this.DockPadding.Right,
                        Math.Max(m_TitleBar.Height + this.DockPadding.Top + this.DockPadding.Bottom, m_TitleBar.ExpandChangeButton.HeightInternal));
                    if (!p.IsEmpty) p.Y += this.Height - s.Height;
                    targetRect = new Rectangle(p, s);
                }

                if (this.AnimationTime == 0 || this.DesignMode)
                {
                    TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, targetRect);
                }
                else
                {
                    Rectangle controlRect = this.Bounds;
                    BarFunctions.AnimateControl(this, true, m_AnimationTime, controlRect, targetRect);
                }
                if (m_CollapseDirection == eCollapseDirection.LeftToRight || m_CollapseDirection == eCollapseDirection.RightToLeft)
                {
                    m_TitleBar.RenderText = false;
                    this.RenderText = false;
                }

                // Hide internal controls
                this.SuspendLayout();
                foreach (Control c in this.Controls)
                {
                    if (c is ContextMenuBar) continue;
                    if (c != this.TitlePanel)
                        c.Visible = false;
                }
                this.ResumeLayout();

                m_TitleBar.SendToBack();
                if (m_CollapseDirection == eCollapseDirection.LeftToRight || m_CollapseDirection == eCollapseDirection.RightToLeft)
                    SetupExpandVerticalPane();
            }
            UpdateExpandButtonImage();
            this.Refresh();
        }

        private void DisposeVerticalExpandPane()
        {
            if (m_VerticalExpandPane != null)
            {
                this.Controls.Remove(m_VerticalExpandPane);
                m_VerticalExpandPane.Dispose();
                m_VerticalExpandPane = null;
            }
        }

        private void UpdateExpandButtonImage()
        {
            Image img = null;
            Image imgMouseOver = null;

            if (this.Expanded)
            {
                if (m_ButtonImageCollapse != null)
                    img = m_ButtonImageCollapse;
                else
                {
                    if (BarFunctions.IsOffice2007Style(this.ColorSchemeStyle))
                    {
                        if (m_CollapseDirection == eCollapseDirection.BottomToTop || m_CollapseDirection == eCollapseDirection.RightToLeft)
                        {
                            img = m_Office2007ButtonImageCollapse;
                            imgMouseOver = m_Office2007ButtonImageCollapseMouseOver;
                        }
                        else
                        {
                            img = m_Office2007ButtonImageExpand;
                            imgMouseOver = m_Office2007ButtonImageExpandMouseOver;
                        }
                    }
                    else
                    {
                        if(m_CollapseDirection == eCollapseDirection.BottomToTop)
                            img = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
                        else if(m_CollapseDirection == eCollapseDirection.TopToBottom)
                            img = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
                        else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
                        {
                            Bitmap b = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
                            b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            img = b;
                        }
                        else if (m_CollapseDirection == eCollapseDirection.RightToLeft)
                        {
                            Bitmap b = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
                            b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            img = b;
                        }
                    }
                }
            }
            else
            {
                if (m_ButtonImageExpand != null)
                    img = m_ButtonImageExpand;
                else
                {
                    if (BarFunctions.IsOffice2007Style(this.ColorSchemeStyle))
                    {
                        if (m_CollapseDirection == eCollapseDirection.BottomToTop || m_CollapseDirection == eCollapseDirection.RightToLeft)
                        {
                            img = m_Office2007ButtonImageExpand;
                            imgMouseOver = m_Office2007ButtonImageExpandMouseOver;
                        }
                        else
                        {
                            img = m_Office2007ButtonImageCollapse;
                            imgMouseOver = m_Office2007ButtonImageCollapseMouseOver;
                        }
                    }
                    else
                    {
                        if (m_CollapseDirection == eCollapseDirection.BottomToTop)
                            img = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
                        else if(m_CollapseDirection == eCollapseDirection.TopToBottom)
                            img = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
                        else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
                        {
                            Bitmap b = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
                            b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            img = b;
                        }
                        else if (m_CollapseDirection == eCollapseDirection.RightToLeft)
                        {
                            Bitmap b = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
                            b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            img = b;
                        }
                    }
                }
            }

            m_TitleBar.ExpandChangeButton.Image = img;
            m_TitleBar.ExpandChangeButton.HoverImage = imgMouseOver;

            m_TitleBar.UpdateButtonPosition();
            m_TitleBar.Invalidate();
        }

        private void UpdateOffice2007Images()
        {
            if (m_Office2007ButtonImageCollapse != null)
                m_Office2007ButtonImageCollapse.Dispose();
            if (m_Office2007ButtonImageExpand != null)
                m_Office2007ButtonImageExpand.Dispose();
            if (m_Office2007ButtonImageCollapseMouseOver != null)
                m_Office2007ButtonImageCollapseMouseOver.Dispose();
            if (m_Office2007ButtonImageExpandMouseOver != null)
                m_Office2007ButtonImageExpandMouseOver.Dispose();

            bool vertImage = (m_CollapseDirection == eCollapseDirection.TopToBottom) || (m_CollapseDirection == eCollapseDirection.BottomToTop);

            m_Office2007ButtonImageCollapse = UIGraphics.CreateExpandButtonImage(true, this.ColorScheme.PanelText, vertImage);
            m_Office2007ButtonImageCollapseMouseOver = UIGraphics.CreateExpandButtonImage(true, this.ColorScheme.ItemHotText, vertImage);
            m_Office2007ButtonImageExpand = UIGraphics.CreateExpandButtonImage(false, this.ColorScheme.PanelText, vertImage);
            m_Office2007ButtonImageExpandMouseOver = UIGraphics.CreateExpandButtonImage(false, this.ColorScheme.ItemHotText, vertImage);
        }

        private void InvokeExpandedChanging(ExpandedChangeEventArgs e)
        {
            if (ExpandedChanging != null)
                ExpandedChanging(this, e);
        }
        private void InvokeExpandedChanged(ExpandedChangeEventArgs e)
        {
            if (ExpandedChanged != null)
                ExpandedChanged(this, e);
        }

        /// <summary>
        /// Called after either ColorScheme or ColorSchemeStyle has changed. If you override make sure that you call base implementation so default
        /// processing can occur.
        /// </summary>
        protected override void OnColorSchemeChanged()
        {
            base.OnColorSchemeChanged();
            m_TitleBar.ColorScheme = GetColorScheme();
            UpdateAutoGeneratedImages();
        }

        private void UpdateAutoGeneratedImages()
        {
            DisposeAutoGeneratedImages();
            if (BarFunctions.IsOffice2007Style(this.ColorSchemeStyle))
                UpdateOffice2007Images();
                
            UpdateExpandButtonImage();
        }

        private void DisposeAutoGeneratedImages()
        {
            if (m_Office2007ButtonImageCollapse != null)
            {
                m_Office2007ButtonImageCollapse.Dispose();
                m_Office2007ButtonImageCollapse = null;
            }
            if (m_Office2007ButtonImageCollapseMouseOver != null)
            {
                m_Office2007ButtonImageCollapseMouseOver.Dispose();
                m_Office2007ButtonImageCollapseMouseOver = null;
            }
            if (m_Office2007ButtonImageExpand != null)
            {
                m_Office2007ButtonImageExpand.Dispose();
                m_Office2007ButtonImageExpand = null;
            }
            if (m_Office2007ButtonImageExpandMouseOver != null)
            {
                m_Office2007ButtonImageExpandMouseOver.Dispose();
                m_Office2007ButtonImageExpandMouseOver = null;
            }
        }

        private int GetTitleHeight()
        {
            if (m_TitleBar.Dock == DockStyle.Left || m_TitleBar.Dock == DockStyle.Right)
                return m_TitleBar.Width;
            else
                return m_TitleBar.Height;
        }

        private void SetTitleHeight(int value)
        {
            //if(m_CollapseDirection == eCollapseDirection.LeftToRight || m_CollapseDirection == eCollapseDirection.RightToLeft)
            if (m_TitleBar.Dock == DockStyle.Left || m_TitleBar.Dock == DockStyle.Right)
            {
                m_TitleBar.Width = value;
                //if (!this.Expanded)
                //    TypeDescriptor.GetProperties(this)["Width"].SetValue(this, value);
            }
            else
            {
                m_TitleBar.Height = value;
                //if (!this.Expanded)
                //    TypeDescriptor.GetProperties(this)["Height"].SetValue(this, value);
            }
        }

        private void OnCollapseDirectionChanged()
        {
            UpdateAutoGeneratedImages();
        }

        protected override void Dispose(bool disposing)
        {
            DisposeAutoGeneratedImages();
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_ButtonImageCollapse);
                BarUtilities.DisposeImage(ref m_ButtonImageExpand);
            }
            base.Dispose(disposing);
        }

        private void SetupExpandVerticalPane()
        {
            m_VerticalExpandPane = new PanelEx();
            m_VerticalExpandPane.ColorSchemeStyle = this.ColorSchemeStyle;
            m_VerticalExpandPane.ColorScheme = this.ColorScheme;
            m_VerticalExpandPane.ApplyButtonStyle();
            m_VerticalExpandPane.Style.VerticalText = true;
            m_VerticalExpandPane.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
            m_VerticalExpandPane.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
            m_VerticalExpandPane.Style.ResetBackColor2();
            m_VerticalExpandPane.StyleMouseOver.VerticalText = true;
            m_VerticalExpandPane.StyleMouseDown.VerticalText = true;
            m_VerticalExpandPane.Dock = DockStyle.Fill;
            m_VerticalExpandPane.Text = TitlePanel.Text;
            
            this.Controls.Add(m_VerticalExpandPane);
            if(this.TitleStyle.Font!=null)
                m_VerticalExpandPane.Font = this.TitleStyle.Font;
            else if (TitlePanel.Font != null)
                m_VerticalExpandPane.Font = TitlePanel.Font;
            m_VerticalExpandPane.Click += new EventHandler(VerticalExpandPaneClick);
        }

        private void VerticalExpandPaneClick(object sender, EventArgs e)
        {
            this.Expanded = !this.Expanded;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the panel is collapsed/expanded when title bar is clicked. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Expand"), Description("Indicates whether panel is collapsed/expanded when title bar is clicked.")]
        public bool ExpandOnTitleClick
        {
            get { return m_ExpandOnTitleClick; }
            set { m_ExpandOnTitleClick = value; }
        }

        /// <summary>
        /// Gets or sets the collapse/expand direction for the control. Default value causes the control to collapse from bottom to top.
        /// </summary>
        [Browsable(true), DefaultValue(eCollapseDirection.BottomToTop), Category("Expand"), Description("Indicates collapse/expand direction for the control.")]
        public eCollapseDirection CollapseDirection
        {
            get { return m_CollapseDirection; }
            set
            {
                m_CollapseDirection = value;
                OnCollapseDirectionChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether panel is expanded or not. Default value is true.
        /// </summary>
        [Browsable(true), Category("Expand"), DefaultValue(true), Description("Indicates whether panel is expaned or not. Default value is true.")]
        public bool Expanded
        {
            get { return m_Expanded; }
            set
            {
                if (m_Expanded != value)
                    SetExpanded(value, eEventSource.Code);
            }
        }

        /// <summary>
        /// Gets or sets animation time in milliseconds. Default value is 100 miliseconds. You can set this to 0 (zero) to disable animation.
        /// </summary>
        [Browsable(true), DefaultValue(100), Category("Expand"), Description("Indicates animation time in milliseconds, set to 0 to disable animation.")]
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
        /// Gets or sets the bounds of panel when expanded. This value is managed automatically by control based on the starting designer size and value
        /// of Expanded property.
        /// </summary>
        [Browsable(false)]
        public Rectangle ExpandedBounds
        {
            get { return m_ExpandedBounds; }
            set { m_ExpandedBounds = value; }
        }

        /// <summary>
        /// Used for design time support.
        /// </summary>
        /// <returns>true if property should be serialized.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeExpandedBounds()
        {
            return !m_ExpandedBounds.IsEmpty;
        }

		/// <summary>
		/// Gets or sets whether expand button is visible or not. Default value is true.
		/// </summary>
		[Browsable(true), DefaultValue(true), Category("Expand Button"), Description("Indicates whether expand button is visible or not.")]
		public bool ExpandButtonVisible
		{
			get { return m_TitleBar.ExpandButtonVisible; }
			set
			{ 
				m_TitleBar.ExpandButtonVisible = value;
			}
		}

		/// <summary>
		/// Returns bounds of expand button. Bounds are relative to the TitlePanel coordinates.
		/// </summary>
		[Browsable(false)]
		public Rectangle ExpandButtonBounds
		{
			get
			{
				return m_TitleBar.ExpandChangeButton.DisplayRectangle;
			}
		}

        /// <summary>
        /// Gets or sets image that is used on title bar button to collapse panel. Default value is null which indicates
        /// that system default image is used.
        /// </summary>
        [Browsable(true),DefaultValue(null),Category("Title"),Description("Indicates image used on title bar to collapse panel.")]
        public Image ButtonImageCollapse
        {
            get { return m_ButtonImageCollapse; }
            set
            {
                m_ButtonImageCollapse = value;
                this.UpdateExpandButtonImage();
            }
        }

        /// <summary>
        /// Gets or sets image that is used on title bar button to expand panel. Default value is null which indicates
        /// that system default image is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Title"), Description("Indicates image used on title bar to expand panel.")]
        public Image ButtonImageExpand
        {
            get { return m_ButtonImageExpand; }
            set
            {
                m_ButtonImageExpand = value;
                this.UpdateExpandButtonImage();
            }
        }

        /// <summary>
        /// Gets or sets the text for the title of the panel.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Title"), Description("Indicates text for the title of the panel."), Localizable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string TitleText
        {
            get { return m_TitleBar.Text; }
            set
            {
                m_TitleBar.Text = value;
                if (m_VerticalExpandPane != null)
                    m_VerticalExpandPane.Text = value;
                    
            }
        }

        /// <summary>
        /// Gets or sets the title style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Title"), Description("Gets or sets title style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemStyle TitleStyle
        {
            get
            {
                return m_TitleBar.Style;
            }
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetTitleStyle()
        {

            m_TitleBar.ResetStyle();
        }

        /// <summary>
        /// Gets or sets the title style when mouse hovers over the title.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Title"), Description("Gets or sets the title style when mouse hovers over the title."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemStyle TitleStyleMouseOver
        {
            get
            {
                return m_TitleBar.StyleMouseOver;
            }
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetTitleStyleMouseOver()
        {
            m_TitleBar.ResetStyleMouseOver();
        }

        /// <summary>
        /// Gets or sets the title style when mouse button is pressed on the title.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Title"), Description("Gets or sets the Title style when mouse button is pressed on the title."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemStyle TitleStyleMouseDown
        {
            get
            {
                return m_TitleBar.StyleMouseDown;
            }
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetTitleStyleMouseDown()
        {
            m_TitleBar.ResetStyleMouseDown();
        }

        /// <summary>
        /// Gets or sets the height of the title portion of the panel. Height must be greater than 0. Default is 26.
        /// </summary>
        [Browsable(true), DefaultValue(26), Category("Title"), Description("Indicates height of the title portion of the panel.")]
        public int TitleHeight
        {
            get { return GetTitleHeight(); }
            set { SetTitleHeight(value); }
        }

        /// <summary>
        /// Gets reference to Panel control used as title bar.
        /// </summary>
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelEx TitlePanel
        {
            get { return m_TitleBar; }
        }

        /// <summary>
        /// Gets reference to the title bar expand button.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonItem ExpandButton
        {
            get { return m_TitleBar.ExpandChangeButton; }
        }
        /// <summary>
        /// Gets or sets alignment of the expand button.
        /// </summary>
        [DefaultValue(eTitleButtonAlignment.Right), Category("Expand Button"), Description("Indicates the alignment of expand button.")]
        public eTitleButtonAlignment ExpandButtonAlignment
        {
            get { return m_TitleBar.ButtonAlignment; }
            set
            {
                m_TitleBar.ButtonAlignment = value;
            }
        }
        

        /// <summary>
        /// Gets the reference to the panel used as button when control is collapsed to the left or right.
        /// </summary>
        [Browsable(false)]
        public PanelEx VerticalExpandPanel
        {
            get { return m_VerticalExpandPane; }
        }

        /// <summary>
        /// Called when AntiAlias property has changed.
        /// </summary>
        protected override void OnAntiAliasChanged()
        {
            m_TitleBar.AntiAlias = this.AntiAlias;
        }
        #endregion
    }
}
