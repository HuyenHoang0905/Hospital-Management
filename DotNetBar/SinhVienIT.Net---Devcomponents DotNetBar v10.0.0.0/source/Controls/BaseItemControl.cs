using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the class for the BaseItem non-popup based control host.
    /// </summary>
    [ToolboxItem(false)]
    public abstract class BaseItemControl : Control
    {
        #region Private Variables
        private BaseItem m_Item = null;
        private ElementStyle m_BackgroundStyle = null;
        private ColorScheme m_ColorScheme = null;
        private bool m_DesignModeInternal = false;
        private bool m_AntiAlias = true;
        #endregion

        #region Constructor Dispose
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public BaseItemControl()
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
            this.SetStyle(ControlStyles.StandardDoubleClick, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
            m_BackgroundStyle = new ElementStyle();
            m_BackgroundStyle.SetColorScheme(m_ColorScheme);
            m_BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
        }
        #endregion

        #region Internal Implementation
#if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_Item != null)
                m_Item.UpdateBindings();
        }
#endif

        protected bool GetDesignMode()
        {
            if (!m_DesignModeInternal)
                return this.DesignMode;
            return m_DesignModeInternal;
        }

        protected virtual ElementStyle GetBackgroundStyle()
        {
            return m_BackgroundStyle;
        }

        internal void SetDesignMode(bool mode)
        {
            m_DesignModeInternal = mode;
            if(m_Item!=null)
                m_Item.SetDesignMode(mode);
        }

        /// <summary>
        /// Gets or sets the instance of BaseItem object hosted by this control.
        /// </summary>
        protected virtual BaseItem HostItem
        {
            get { return m_Item; }
            set
            {
                m_Item = value;
                if (m_Item != null)
                {
                    m_Item.Displayed = true;
                    m_Item.ContainerControl = this;
                }
            }
        }

        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrieved from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            if (m_Item != null && BarFunctions.IsOffice2007Style(m_Item.EffectiveStyle))
            {
                BaseRenderer r = GetRenderer();
                if (r is Office2007Renderer)
                    return ((Office2007Renderer)r).ColorTable.LegacyColors;
            }
            return m_ColorScheme;
        }

        private bool _CallBasePaintBackground = true;
        /// <summary>
        /// Gets or sets whether during painting OnPaintBackground on base control is called when BackColor=Transparent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CallBasePaintBackground
        {
            get { return _CallBasePaintBackground; }
            set
            {
                _CallBasePaintBackground = value;
            }
        }

        internal void InternalPaint(PaintEventArgs e)
        {
            OnPaint(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.BackColor.IsEmpty || this.BackColor == Color.Transparent) && _CallBasePaintBackground)
            {
                base.OnPaintBackground(e);
            }

            if (m_BackgroundStyle != null)
                m_BackgroundStyle.SetColorScheme(this.GetColorScheme());

            PaintBackground(e);
            PaintControl(e);

            if (this.Focused && /*this.ShowFocusCues &&*/ this.FocusCuesEnabled)
            {
                PaintFocusCues(e);
            }

            base.OnPaint(e);
        }

        private bool _FocusCuesEnabled = true;
        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether control displays focus cues when focused.")]
        public virtual bool FocusCuesEnabled
        {
            get { return _FocusCuesEnabled; }
            set
            {
                _FocusCuesEnabled = value;
                if (this.Focused) this.Invalidate();
            }
        }

        /// <summary>
        /// Paints the control focus cues.
        /// </summary>
        /// <param name="e">Paint event information.</param>
        protected virtual void PaintFocusCues(PaintEventArgs e)
        {
            ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
        }

        protected virtual void PaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = this.ClientRectangle;
            ElementStyle style = this.GetBackgroundStyle();

            if (!this.BackColor.IsEmpty && this.BackColor != Color.Transparent)
            {
                DisplayHelp.FillRectangle(g, r, this.BackColor);
            }

            if (this.BackgroundImage != null)
                base.OnPaintBackground(e);

            if (style.Custom)
            {
                SmoothingMode sm = g.SmoothingMode;
                if (m_AntiAlias)
                    g.SmoothingMode = SmoothingMode.HighQuality;
                ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, e.Graphics, r);
                ElementStyleDisplay.Paint(displayInfo);
                if (m_AntiAlias)
                    g.SmoothingMode = sm;
            }
        }

        protected virtual void PaintControl(PaintEventArgs e)
        {
            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;
            Graphics g = e.Graphics;

            if (m_AntiAlias)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            ItemPaintArgs pa = GetItemPaintArgs(g);
            pa.ClipRectangle = e.ClipRectangle;

            if (m_Item != null) m_Item.Paint(pa);

            g.SmoothingMode = sm;
            g.TextRenderingHint = th;
        }

        /// <summary>
        /// Creates the Graphics object for the control.
        /// </summary>
        /// <returns>The Graphics object for the control.</returns>
        public new Graphics CreateGraphics()
        {
            Graphics g = base.CreateGraphics();
            if (m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                #if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
                #endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

        protected virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
            return pa;
        }

        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
                m_DefaultRenderer = new Rendering.Office2007Renderer();

            return m_Renderer;
        }

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        [Browsable(false), DefaultValue(eRenderMode.Global)]
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set { m_Renderer = value; }
        }

        /// <summary>
        /// Specifies the background style of the control.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets bar background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return m_BackgroundStyle; }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            m_BackgroundStyle.StyleChanged -= new EventHandler(this.VisualPropertyChanged);
            m_BackgroundStyle = new ElementStyle();
            m_BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            OnBackgroundStyleChanged();
            this.Invalidate();
        }

        protected virtual void OnBackgroundStyleChanged()
        {
        }

        private void VisualPropertyChanged(object sender, EventArgs e)
        {
            OnVisualPropertyChanged();
        }

        /// <summary>
        /// Called when visual property of the control has changed so the control can be updated.
        /// </summary>
        protected virtual void OnVisualPropertyChanged()
        {
            if (this.GetDesignMode() ||
                this.Parent != null && this.Parent.Site != null && this.Parent.Site.DesignMode)
            {
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets Bar Color Scheme.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Appearance"), Description("Gets or sets Bar Color Scheme."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorScheme ColorScheme
        {
            get { return m_ColorScheme; }
            set
            {
                if (value == null)
                    throw new ArgumentException("NULL is not a valid value for this property.");
                m_ColorScheme = value;
                if (this.Visible)
                    this.Refresh();
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColorScheme()
        {
            return m_ColorScheme.SchemeChanged;
        }

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                if (m_AntiAlias != value)
                {
                    m_AntiAlias = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            //if (this.Parent != null && (this.Images != null || this.ImagesLarge != null || this.ImagesMedium != null))
            //{
            //    foreach (BaseItem panel in m_BaseItemContainer.SubItems)
            //    {
            //        foreach (BaseItem item in panel.SubItems)
            //        {
            //            if (item is ImageItem)
            //                ((ImageItem)item).OnImageChanged();
            //        }
            //    }
            //}

            if (this.DesignMode && m_Item!=null)
                m_Item.SetDesignMode(this.DesignMode);
        }

        /// <summary>
        /// Forces the button to perform internal layout.
        /// </summary>
        public virtual void RecalcLayout()
        {
            if (m_Item == null) return;

            Rectangle r = GetItemBounds();
            m_Item.Bounds = r;
            this.RecalcSize();
            m_Item.Bounds = r;
            this.Invalidate();
        }

        /// <summary>
        /// Recalculates the size of the internal item.
        /// </summary>
        protected virtual void RecalcSize()
        {
            m_Item.RecalcSize();
        }

        protected virtual Rectangle GetItemBounds()
        {
            Rectangle r = this.ClientRectangle;
            r.X += ElementStyleLayout.LeftWhiteSpace(this.BackgroundStyle);
            r.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(this.BackgroundStyle);
            r.Y += ElementStyleLayout.TopWhiteSpace(this.BackgroundStyle);
            r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(this.BackgroundStyle);

            return r;
        }

        protected override void OnResize(EventArgs e)
        {
            RecalcLayout();
            base.OnResize(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            m_Item.Text = this.Text;
            this.RecalcLayout();
            base.OnTextChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            BarUtilities.InvalidateFontChange(m_Item);
            this.RecalcLayout();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            m_Item.Enabled = this.Enabled;
            base.OnEnabledChanged(e);
        }

        private bool m_MouseFocus = false;
        protected override void OnGotFocus(EventArgs e)
        {
            if (!m_MouseFocus)
                m_Item.OnGotFocus();
            m_MouseFocus = false;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            m_Item.OnLostFocus();
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            m_Item.InternalMouseEnter();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_Item.InternalMouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_Item.InternalMouseLeave();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            m_Item.InternalMouseHover();
            base.OnMouseHover(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!this.Focused && this.GetStyle(ControlStyles.Selectable))
                {
                    m_MouseFocus = true;
                    this.Focus();
                }
            }

            m_Item.InternalMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_Item.InternalMouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnClick(EventArgs e)
        {
            m_Item.InternalClick(Control.MouseButtons, this.PointToClient(Control.MousePosition));
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            m_Item.InternalDoubleClick(Control.MouseButtons, this.PointToClient(Control.MousePosition));
            base.OnDoubleClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            m_Item.InternalKeyDown(e);
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Gets/Sets the visual style for the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Specifies the visual style of the control."), DefaultValue(eDotNetBarStyle.Office2007)]
        public virtual eDotNetBarStyle Style
        {
            get
            {
                return m_Item.Style;
            }
            set
            {
                m_Item.Style = value;
                if(!BarFunctions.IsOffice2007Style(value))
                    this.GetColorScheme().Style = value;
                this.RecalcLayout();
            }
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, m_Item.Text))
            {
                if (ProcessAccelerator())
                    return true;
            }
            return base.ProcessMnemonic(charCode);
        }

        protected virtual bool ProcessAccelerator()
        {
            m_Item.RaiseClick(eEventSource.Keyboard);
            return true;
        }
        #endregion

        #region Layout Support
        private int m_UpdateSuspendCount = 0;
        /// <summary>
        /// Indicates to control that all further update operations should not result in layout and refresh of control content.
        /// Use this method to optimize the addition of new items to the control. This method supports nested calls meaning
        /// that multiple calls are allowed but they must be ended with appropriate number of EndUpdate calls.
        /// IsUpdateSuspended property returns whether update is suspended.
        /// </summary>
        public void BeginUpdate()
        {
            m_UpdateSuspendCount++;
        }

        /// <summary>
        /// Indicates that update operation is complete and that control should perform layout and refresh to show changes. See BeginUpdate
        /// for more details.
        /// </summary>
        public void EndUpdate()
        {
            EndUpdate(true);
        }

        /// <summary>
        /// Indicates that update operation is complete and that control should perform layout and refresh to show changes. See BeginUpdate
        /// for more details.
        /// </summary>
        public void EndUpdate(bool callRecalcLayout)
        {
            if (m_UpdateSuspendCount > 0)
            {
                m_UpdateSuspendCount--;
                if (m_UpdateSuspendCount == 0 && callRecalcLayout)
                    this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets whether control layout is suspended becouse of the call to BeginUpdate method.
        /// </summary>
        [Browsable(false)]
        public bool IsUpdateSuspended
        {
            get { return m_UpdateSuspendCount > 0; }
        }
        #endregion
    }
}
