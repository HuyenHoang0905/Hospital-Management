using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;
using DevComponents.DotNetBar.TextMarkup;
using System.Reflection;
using DevComponents.Editors;
using System.Collections;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(TextBoxX), "Controls.TextBoxX.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.TextBoxXDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class TextBoxX : TextBox, INonClientControl
#if FRAMEWORK20
            , IInputButtonControl
#endif
    {
        #region Private Variables
        private NonClientPaintHandler m_NCPainter = null;
        private string m_WatermarkText = "";
        private bool m_Focused = false;
        private Font m_WatermarkFont = null;
        private Color m_WatermarkColor = SystemColors.GrayText;
        private ElementStyle m_BorderStyle = null;
        private int m_LastFocusWindow;
        private string m_OriginalText;
        private bool m_IsTextBoxItem = false;
        private Color m_FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        private bool m_FocusHighlightEnabled = false;
        private Color m_LastBackColor = Color.Empty;
        #endregion

        #region Events
#if FRAMEWORK20
        /// <summary>
        /// Occurs when ButtonCustom control is clicked.
        /// </summary>
        public event EventHandler ButtonCustomClick;

        /// <summary>
        /// Occurs when ButtonCustom2 control is clicked.
        /// </summary>
        public event EventHandler ButtonCustom2Click;
#endif
        #endregion

        #region Constructor
        public TextBoxX()
        {
#if FRAMEWORK20
            _ButtonCustom = new InputButtonSettings(this);
            _ButtonCustom2 = new InputButtonSettings(this);
            CreateButtonGroup();
#endif

            m_BorderStyle = new ElementStyle();
            m_BorderStyle.StyleChanged += new EventHandler(BorderStyle_StyleChanged);
            m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
            base.BorderStyle = BorderStyle.None;
            this.AutoSize = false;
        }

        internal TextBoxX(bool isTextBoxItem) : this()
        {
            m_IsTextBoxItem = isTextBoxItem;
        }

        protected override void Dispose(bool disposing)
        {
            if (m_NCPainter != null)
            {
                m_NCPainter.Dispose();
                //m_NCPainter = null;
            }
            if (m_BorderStyle != null) m_BorderStyle.StyleChanged -= new EventHandler(BorderStyle_StyleChanged);
            base.Dispose(disposing);
        }
        #endregion

        #region Windows Messages Handling
        private bool m_IgnoreFocus = false;
        private bool _MouseOverButtons = false;
        protected override void WndProc(ref Message m)
        {
            if (m_NCPainter != null)
            {
                bool callBase = m_NCPainter.WndProc(ref m);

                if (callBase)
                    base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }

#if FRAMEWORK20
            if (RenderButtons)
            {
                switch (m.Msg)
                {
                    case (int)WinApi.WindowsMessages.WM_NCMOUSEMOVE:
                    case (int)WinApi.WindowsMessages.WM_NCHITTEST:
                        {
                            Point p = PointToClient(new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam)));
                            if (_ButtonGroup.RenderBounds.Contains(p))
                            {
                                m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.ClientArea);
                                return;
                            }
                            break;
                        }
                }
            }
#endif

            switch (m.Msg)
            {
                case (int)WinApi.WindowsMessages.WM_SETFOCUS:
                    {
                        if (m_IgnoreFocus)
                        {
                            m_IgnoreFocus = false;
                        }
                        else
                        {
                            m_Focused = true;
                            m_LastFocusWindow = m.WParam.ToInt32();
                            m_OriginalText = this.Text;
                            if (this.FocusHighlightEnabled && this.Enabled)
                            {
                                m_LastBackColor = this.BackColor;
                                this.BackColor = this.FocusHighlightColor;
                                this.InvalidateNonClient();
                            }
                        }
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_KILLFOCUS:
                    {
                        if (!m_Focused)
                        {
                            m_IgnoreFocus = true;
                        }
                        else
                        {
                            m_Focused = false;
                            if (this.Text.Length == 0)
                                this.Invalidate();
                            if (this.FocusHighlightEnabled && !m_LastBackColor.IsEmpty)
                            {
                                this.BackColor = m_LastBackColor;
                                this.InvalidateNonClient();
                            }
                        }
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_PAINT:
                    {
                        if (RenderWatermark)
                            DrawWatermark();
                        if (this.Parent is ItemControl)
                            ((ItemControl)this.Parent).UpdateKeyTipsCanvas();
                        break;
                    }
            }
        }

        private bool RenderWatermark
        {
            get
            {
                if (!_WatermarkEnabled)
                    return false;
                if (_WatermarkBehavior == eWatermarkBehavior.HideOnFocus)
                    return !m_Focused && this.Enabled && this.Text != null && this.Text.Length == 0 && (m_WatermarkText.Length > 0 || _WatermarkImage != null);
                else
                    return this.Enabled && this.Text != null && this.Text.Length == 0 && (m_WatermarkText.Length > 0 || _WatermarkImage != null);
            }
        }
        #endregion

        #region Internal Implementation
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!this.Multiline && _PreventEnterBeep && e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        private bool _PreventEnterBeep = false;
        /// <summary>
        /// Gets or sets whether control prevents Beep sound when Enter key is pressed.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Gets or sets whether control prevents Beep sound when Enter key is pressed.")]
        public bool PreventEnterBeep
        {
            get { return _PreventEnterBeep; }
            set
            {
                _PreventEnterBeep = value;
            }
        }

        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as background color to highlight text box when it has input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
        public bool FocusHighlightEnabled
        {
            get { return m_FocusHighlightEnabled; }
            set
            {
                if (m_FocusHighlightEnabled != value)
                {
                    m_FocusHighlightEnabled = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color used as background color to highlight text box when it has input focus and focus highlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
        public Color FocusHighlightColor
        {
            get { return m_FocusHighlightColor; }
            set
            {
                if (m_FocusHighlightColor != value)
                {
                    m_FocusHighlightColor = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return !m_FocusHighlightColor.Equals(ColorScheme.GetColor(0xFFFF88));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        }

        internal void ReleaseFocus()
        {
            if (this.Focused && m_LastFocusWindow != 0)
            {
                int focus = m_LastFocusWindow;
                m_LastFocusWindow = 0;
                Control ctrl = Control.FromChildHandle(new System.IntPtr(focus));
                if (ctrl != this)
                {
                    Control p = this.Parent;
                    while (p != null)
                    {
                        if (p == ctrl)
                        {
                            if (ctrl is MenuPanel)
                                ctrl.Focus();
                            return;
                        }
                        p = p.Parent;
                    }

                    if (ctrl != null)
                        ctrl.Focus();
                    else
                    {
                        NativeFunctions.SetFocus(focus);
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (m_IsTextBoxItem)
            {
                /*if (e.KeyCode == Keys.Enter)
                    ReleaseFocus();
                else*/ if (e.KeyCode == Keys.Escape)
                {
                    this.Text = m_OriginalText;
                    this.SelectionStart = 0;
                    ReleaseFocus();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.A) == Keys.A && Control.ModifierKeys == Keys.ControlKey)
            {
                this.SelectAll();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            m_LastFocusWindow = 0;
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Gets or sets the scrollbar skinning type when control is using Office 2007 style.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eScrollBarSkin ScrollbarSkin
        {
            get { return m_NCPainter.SkinScrollbars; }
            set { m_NCPainter.SkinScrollbars = value; }
        }

        /// <summary>
        /// Specifies the control border style. Default value has Class property set so the system style for the control is used.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Specifies the control border style. Default value has Class property set so the system style for the control is used."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle Border
        {
            get { return m_BorderStyle; }
        }

        private void BorderStyle_StyleChanged(object sender, EventArgs e)
        {
            InvalidateNonClient();
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            InvalidateNonClient();
            base.OnReadOnlyChanged(e);
        }

        /// <summary>
        /// Invalidates non-client area of the text box as response to the border changes.
        /// </summary>
        public void InvalidateNonClient()
        {
            if (!BarFunctions.IsHandleValid(this)) return;
            NativeFunctions.SetWindowPos(this.Handle, 0, 0, 0, 0, 0, NativeFunctions.SWP_FRAMECHANGED |
                                NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE | NativeFunctions.SWP_NOZORDER);
            SetAutoHeight();
            const int RDW_INVALIDATE = 0x0001;
            const int RDW_FRAME = 0x0400;
            NativeFunctions.RECT r = new NativeFunctions.RECT(0, 0, this.Width, this.Height);
            NativeFunctions.RedrawWindow(this.Handle, ref r, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME);
        }

        private TextMarkup.BodyElement m_TextMarkup = null;
        private void MarkupTextChanged()
        {
            m_TextMarkup = null;

            if (!TextMarkup.MarkupParser.IsMarkup(ref m_WatermarkText))
                return;

            m_TextMarkup = TextMarkup.MarkupParser.Parse(m_WatermarkText);
            ResizeMarkup();
        }

        private void ResizeMarkup()
        {
            if (m_TextMarkup != null)
            {
                using (Graphics g = this.CreateGraphics())
                {
                    MarkupDrawContext dc = GetMarkupDrawContext(g);
                    m_TextMarkup.Measure(GetWatermarkBounds().Size, dc);
                    Size sz = m_TextMarkup.Bounds.Size;
                    m_TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, sz), dc);
                }
            }
        }

        private Rectangle GetWatermarkBounds()
        {
            Rectangle r = this.ClientRectangle;
            r.Inflate(-1, 0);
            return r;
        }

        private void DrawWatermark()
        {
            using (Graphics g = this.CreateGraphics())
            {
                Rectangle bounds = GetWatermarkBounds();
                if (_WatermarkImage != null)
                {
                    Rectangle imageBounds = new Rectangle(Point.Empty, _WatermarkImage.Size);
                    if (_WatermarkImageAlignment == ContentAlignment.BottomCenter)
                        imageBounds.Location = new Point(bounds.X + (bounds.Width - imageBounds.Width) / 2, bounds.Bottom - imageBounds.Height);
                    else if (_WatermarkImageAlignment == ContentAlignment.BottomLeft)
                        imageBounds.Location = new Point(bounds.X, bounds.Bottom - imageBounds.Height);
                    else if (_WatermarkImageAlignment == ContentAlignment.BottomRight)
                        imageBounds.Location = new Point(bounds.Right - imageBounds.Width, bounds.Bottom - imageBounds.Height);
                    else if (_WatermarkImageAlignment == ContentAlignment.MiddleCenter)
                        imageBounds.Location = new Point(bounds.X + (bounds.Width - imageBounds.Width) / 2, bounds.Y + (bounds.Height - imageBounds.Height) / 2);
                    else if (_WatermarkImageAlignment == ContentAlignment.MiddleLeft)
                        imageBounds.Location = new Point(bounds.X, bounds.Y + (bounds.Height - imageBounds.Height) / 2);
                    else if (_WatermarkImageAlignment == ContentAlignment.MiddleRight)
                        imageBounds.Location = new Point(bounds.Right - imageBounds.Width, bounds.Y + (bounds.Height - imageBounds.Height) / 2);
                    else if (_WatermarkImageAlignment == ContentAlignment.TopCenter)
                        imageBounds.Location = new Point(bounds.X + (bounds.Width - imageBounds.Width) / 2, bounds.Y);
                    else if (_WatermarkImageAlignment == ContentAlignment.TopLeft)
                        imageBounds.Location = new Point(bounds.X, bounds.Y);
                    else if (_WatermarkImageAlignment == ContentAlignment.TopRight)
                        imageBounds.Location = new Point(bounds.Right - imageBounds.Width, bounds.Y);
                    g.DrawImage(_WatermarkImage, imageBounds);

                    if (_WatermarkImageAlignment == ContentAlignment.BottomLeft || _WatermarkImageAlignment == ContentAlignment.MiddleLeft || _WatermarkImageAlignment == ContentAlignment.TopLeft)
                    {
                        bounds.X = imageBounds.Right;
                        bounds.Width = Math.Max(0, bounds.Width - imageBounds.Width);
                    }
                    else if (_WatermarkImageAlignment == ContentAlignment.BottomRight || _WatermarkImageAlignment == ContentAlignment.MiddleRight || _WatermarkImageAlignment == ContentAlignment.TopRight)
                    {
                        bounds.Width = Math.Max(0, bounds.Width - imageBounds.Width);
                    }
                }

                if (bounds.Width <= 0) return;

                if (m_TextMarkup != null)
                {
                    MarkupDrawContext dc = GetMarkupDrawContext(g);
                    m_TextMarkup.Render(dc);
                }
                else
                {
                    eTextFormat tf = eTextFormat.Left;
                    if (this.Multiline)
                    {
                        if (this.TextAlign == HorizontalAlignment.Center)
                            tf |= eTextFormat.VerticalCenter;
                        else if (TextAlign == HorizontalAlignment.Right)
                            tf |= eTextFormat.Bottom;
                    }

                    if (this.RightToLeft == RightToLeft.Yes) tf |= eTextFormat.RightToLeft;
                    if (this.TextAlign == HorizontalAlignment.Left)
                        tf |= eTextFormat.Left;
                    else if (this.TextAlign == HorizontalAlignment.Right)
                        tf |= eTextFormat.Right;
                    else if (this.TextAlign == HorizontalAlignment.Center)
                        tf |= eTextFormat.HorizontalCenter;
                    tf |= eTextFormat.EndEllipsis;
                    tf |= eTextFormat.WordBreak;
                    TextDrawing.DrawString(g, m_WatermarkText, (m_WatermarkFont == null ? this.Font : m_WatermarkFont),
                        m_WatermarkColor, bounds, tf);
                }
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            SetAutoHeight();
            base.OnFontChanged(e);
        }
        /// <summary>
        /// Calculates and sets the text-box height based on font and style. This method is used internally and should not be used.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetAutoHeight()
        {
            if (!this.AutoSize && this.Multiline == false && this.BorderStyle == BorderStyle.None && !this.IsDisposed && this.Parent!=null && !m_IsTextBoxItem)
            {
                int h = this.FontHeight;
                if (this.Font != null) h = Math.Max(h, this.Font.Height);
                // Adjust for DPI other than 96
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    if (graphics.DpiX != 96f)
                    {
                        h = (int)Math.Ceiling(h * (graphics.DpiX / 96f));
                    }
                }
                bool disposeStyle = false;
                ElementStyle style = GetBorderStyle(out disposeStyle);
                if (style != null)
                {
                    if (style.PaintTopBorder)
                    {
                        if (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal)
                            h += Math.Max(style.BorderTopWidth, style.CornerDiameter / 2 + 1);
                        else
                            h += style.BorderTopWidth;
                    }

                    if (style.PaintBottomBorder)
                    {
                        if (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal)
                            h += Math.Max(style.BorderBottomWidth, style.CornerDiameter / 2 + 1);
                        else
                            h += style.BorderBottomWidth;
                    }
                    h += style.PaddingTop + style.PaddingBottom;
                    if(disposeStyle) style.Dispose();
                }
                #if (FRAMEWORK20)
                if (_ButtonCustom != null && _ButtonCustom.Visible && _ButtonCustom.Image != null)
                {
                    h = Math.Max(h, _ButtonCustom.Image.Height + 6);
                }
                if (_ButtonCustom2 != null && _ButtonCustom2.Visible && _ButtonCustom2.Image != null)
                {
                    h = Math.Max(h, _ButtonCustom2.Image.Height + 6);
                }
                #endif
                this.Height = h;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            ResizeMarkup();
            #if (FRAMEWORK20)
            _ButtonGroup.InvalidateArrange();
            #endif
            base.OnResize(e);
        }

        private MarkupDrawContext GetMarkupDrawContext(Graphics g)
        {
            return new MarkupDrawContext(g, (m_WatermarkFont == null ? this.Font : m_WatermarkFont), m_WatermarkColor, this.RightToLeft == RightToLeft.Yes);
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            if(m_WatermarkText.Length>0)
                this.Invalidate();  
            base.OnTextAlignChanged(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (m_WatermarkText.Length > 0)
                this.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (m_WatermarkText.Length > 0)
                this.Invalidate();
            base.OnTextChanged(e);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BorderStyle BorderStyle
        {
            get {return base.BorderStyle;}
            set {}
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

            return m_DefaultRenderer;
        }

        /// <summary>
        /// Gets or sets the rendering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
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

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; this.Invalidate(); }
        }

        private Image _WatermarkImage = null;
        /// <summary>
        /// Gets or sets the watermark image displayed inside of the control when Text is not set and control does not have input focus.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
        public Image WatermarkImage
        {
            get { return _WatermarkImage; }
            set { _WatermarkImage = value; this.Invalidate(); }
        }
        private ContentAlignment _WatermarkImageAlignment = ContentAlignment.MiddleLeft;
        /// <summary>
        /// Gets or sets the watermark image alignment.
        /// </summary>
        [DefaultValue(ContentAlignment.MiddleLeft), Category("Appearance"), Description("Indicates watermark image alignment.")]
        public ContentAlignment WatermarkImageAlignment
        {
            get { return _WatermarkImageAlignment; }
            set { _WatermarkImageAlignment = value; this.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return m_WatermarkText; }
            set
            {
                if (value == null) value = "";
                m_WatermarkText = value;
                MarkupTextChanged();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return m_WatermarkFont; }
            set { m_WatermarkFont = value; this.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return m_WatermarkColor; }
            set { m_WatermarkColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return m_WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        private eWatermarkBehavior _WatermarkBehavior = eWatermarkBehavior.HideOnFocus;
        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return _WatermarkBehavior; }
            set { _WatermarkBehavior = value; this.Invalidate(); }
        }

        private ElementStyle GetBorderStyle(out bool disposeStyle)
        {
            disposeStyle = false;
            m_BorderStyle.SetColorScheme(this.GetColorScheme());
            return ElementStyleDisplay.GetElementStyle(m_BorderStyle, out disposeStyle);
        }

#if FRAMEWORK20
        private InputButtonSettings _ButtonCustom = null;
        /// <summary>
        /// Gets the object that describes the settings for the custom button that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom
        {
            get
            {
                return _ButtonCustom;
            }
        }

        private InputButtonSettings _ButtonCustom2 = null;
        /// <summary>
        /// Gets the object that describes the settings for the custom button that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom2
        {
            get
            {
                return _ButtonCustom2;
            }
        }


        void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
        {
            OnInputButtonSettingsChanged(inputButtonSettings);
        }

        protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
        {
            UpdateButtons();
        }

        private VisualGroup _ButtonGroup = null;
        private void UpdateButtons()
        {
            RecreateButtons();
            _ButtonGroup.InvalidateArrange();
            this.InvalidateNonClient();
        }

        protected virtual void RecreateButtons()
        {
            VisualItem[] buttons = CreateOrderedButtonList();
            // Remove all system buttons that are already in the list
            VisualGroup group = _ButtonGroup;
            VisualItem[] items = new VisualItem[group.Items.Count];
            group.Items.CopyTo(items);
            foreach (VisualItem item in items)
            {
                if (item.ItemType == eSystemItemType.SystemButton)
                {
                    group.Items.Remove(item);
                    if (item == _ButtonCustom.ItemReference)
                        item.MouseUp -= new MouseEventHandler(CustomButtonClick);
                    else if (item == _ButtonCustom2.ItemReference)
                        item.MouseUp -= new MouseEventHandler(CustomButton2Click);
                }
            }

            // Add new buttons to the list
            group.Items.AddRange(buttons);
        }

        private void CustomButtonClick(object sender, MouseEventArgs e)
        {
            if (_ButtonCustom.ItemReference.RenderBounds.Contains(e.X, e.Y))
                OnButtonCustomClick(e);
        }

        /// <summary>
        /// Invokes ButtonCustomClick event.
        /// </summary>
        public void PerformButtonCustomClick()
        {
            OnButtonCustomClick(EventArgs.Empty);
        }

        protected virtual void OnButtonCustomClick(EventArgs e)
        {
            if (ButtonCustomClick != null)
                ButtonCustomClick(this, e);
        }

        private void CustomButton2Click(object sender, MouseEventArgs e)
        {
            if (_ButtonCustom2.ItemReference.RenderBounds.Contains(e.X, e.Y))
                OnButtonCustom2Click(e);
        }

        /// <summary>
        /// Invokes ButtonCustomClick2 event.
        /// </summary>
        public void PerformButtonCustom2Click()
        {
            OnButtonCustom2Click(EventArgs.Empty);
        }

        protected virtual void OnButtonCustom2Click(EventArgs e)
        {
            if (ButtonCustom2Click != null)
                ButtonCustom2Click(this, e);
        }

        private VisualItem[] CreateOrderedButtonList()
        {
            SortedList list = CreateSortedButtonList();

            VisualItem[] items = new VisualItem[list.Count];
            list.Values.CopyTo(items, 0);

            return items;
        }

        protected virtual SortedList CreateSortedButtonList()
        {
            SortedList list = new SortedList(4);
            if (_ButtonCustom.Visible)
            {
                VisualItem button = CreateButton(_ButtonCustom);
                if (_ButtonCustom.ItemReference != null)
                    _ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButtonClick);
                _ButtonCustom.ItemReference = button;
                //button.Click += new EventHandler(CustomButtonClick);
                button.MouseUp += new MouseEventHandler(CustomButtonClick);
                button.Enabled = _ButtonCustom.Enabled;
                list.Add(_ButtonCustom, button);
            }

            if (_ButtonCustom2.Visible)
            {
                VisualItem button = CreateButton(_ButtonCustom2);
                if (_ButtonCustom.ItemReference != null)
                    _ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButton2Click);
                _ButtonCustom2.ItemReference = button;
                //button.Click += new EventHandler(CustomButton2Click);
                button.MouseUp += new MouseEventHandler(CustomButton2Click);
                button.Enabled = _ButtonCustom2.Enabled;
                list.Add(_ButtonCustom2, button);
            }

            return list;
        }

        protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
        {
            VisualCustomButton button = new VisualCustomButton();
            ApplyButtonSettings(buttonSettings, button);
            return button;
        }

        protected virtual void ApplyButtonSettings(InputButtonSettings buttonSettings, VisualButton button)
        {
            button.Text = buttonSettings.Text;
            button.Image = buttonSettings.Image;
            button.ItemType = eSystemItemType.SystemButton;
            button.Enabled = buttonSettings.Enabled;
        }

        private void CreateButtonGroup()
        {
            VisualGroup group = new VisualGroup();
            group.HorizontalItemSpacing = 1;
            group.ArrangeInvalid += new EventHandler(ButtonGroupArrangeInvalid);
            group.RenderInvalid += new EventHandler(ButtonGroupRenderInvalid);
            _ButtonGroup = group;
        }

        private void ButtonGroupRenderInvalid(object sender, EventArgs e)
        {
            InvalidateNonClient();
        }

        private void ButtonGroupArrangeInvalid(object sender, EventArgs e)
        {
            InvalidateNonClient();
        }

        private bool _MouseOver = false;
        private PaintInfo CreatePaintInfo(Graphics g)
        {
            PaintInfo p = new PaintInfo();
            p.Graphics = g;
            p.DefaultFont = this.Font;
            p.ForeColor = this.ForeColor;
            p.RenderOffset = new System.Drawing.Point();
            Size s = this.Size;
            bool disposeStyle = false;
            ElementStyle style = GetBorderStyle(out disposeStyle);
            s.Height -= (ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(style, eSpacePart.Border)) + 2;
            s.Width -= (ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border)) + 2;
            p.AvailableSize = s;
            p.ParentEnabled = this.Enabled;
            p.MouseOver = _MouseOver || this.Focused;
            if(disposeStyle) style.Dispose();
            return p;
        }

        private bool RenderButtons
        {
            get
            {
                return (this.ScrollBars == ScrollBars.None) && (_ButtonCustom != null && _ButtonCustom.Visible || _ButtonCustom2 != null && _ButtonCustom2.Visible);
            }
        }

        private void PaintButtons(Graphics g)
        {
            PaintInfo p = CreatePaintInfo(g);
            if (!_ButtonGroup.IsLayoutValid)
            {
                _ButtonGroup.PerformLayout(p);
            }
            bool disposeStyle = false;
            ElementStyle style = GetBorderStyle(out disposeStyle);
            //p.RenderOffset = new Point(this.Width - ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border));
            _ButtonGroup.RenderBounds = new Rectangle(this.Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
            _ButtonGroup.ProcessPaint(p);
            if(disposeStyle) style.Dispose();
        }

        private Cursor _ControlCursor = null;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseMove(e);
                if (_ButtonGroup.RenderBounds.Contains(e.X, e.Y))
                {
                    if (_ControlCursor == null)
                    {
                        _InternalCursorChange = true;
                        _ControlCursor = this.Cursor;
                        this.Cursor = Cursors.Arrow;
                        _InternalCursorChange = false;
                    }
                }
                else if (_ControlCursor != null)
                {
                    _InternalCursorChange = true;
                    this.Cursor = _ControlCursor;
                    _ControlCursor = null;
                    _InternalCursorChange = false;
                }
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            _MouseOver = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _MouseOver = false;
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseLeave();
                if (_ControlCursor != null)
                {
                    _InternalCursorChange = true;
                    this.Cursor = _ControlCursor;
                    _ControlCursor = null;
                    _InternalCursorChange = false;
                }
            }
            base.OnMouseLeave(e);
        }
        private bool _InternalCursorChange = false;
        protected override void OnCursorChanged(EventArgs e)
        {
            if (_ControlCursor != null && !_InternalCursorChange)
                _ControlCursor = this.Cursor;
            base.OnCursorChanged(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (RenderButtons)
                _ButtonGroup.ProcessMouseDown(e);
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (RenderButtons)
                _ButtonGroup.ProcessMouseUp(e);
            base.OnMouseUp(e);
        }
#endif
        #endregion

        #region INonClientControl Members
        void INonClientControl.BaseWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        //CreateParams INonClientControl.ControlCreateParams
        //{
        //    get { return base.CreateParams; }
        //}

        ItemPaintArgs INonClientControl.GetItemPaintArgs(System.Drawing.Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false; // m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
            return pa;
        }

        ElementStyle INonClientControl.BorderStyle
        {
            get { bool disposeStyle = false; return GetBorderStyle(out disposeStyle); }
        }

        void INonClientControl.PaintBackground(PaintEventArgs e)
        {
            if (this.Parent == null) return;
            Type t = typeof(Control);
            MethodInfo mi = t.GetMethod("PaintTransparentBackground", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(PaintEventArgs), typeof(Rectangle) }, null);
            if (mi != null)
            {
                mi.Invoke(this, new object[] {e, new Rectangle(0,0,this.Width, this.Height)});
            }
        }

        private ColorScheme m_ColorScheme = null;
        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrived from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            BaseRenderer r = GetRenderer();
            if (r is Office2007Renderer)
                return ((Office2007Renderer)r).ColorTable.LegacyColors;
            if (m_ColorScheme == null)
                m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
            return m_ColorScheme;
        }

        IntPtr INonClientControl.Handle
        {
            get { return this.Handle; }
        }

        int INonClientControl.Width
        {
            get { return this.Width; }
        }

        int INonClientControl.Height
        {
            get { return this.Height; }
        }

        bool INonClientControl.IsHandleCreated
        {
            get { return this.IsHandleCreated; }
        }

        Point INonClientControl.PointToScreen(Point client)
        {
            return this.PointToScreen(client);
        }

        Color INonClientControl.BackColor
        {
            get { return this.BackColor; }
        }

        void INonClientControl.AdjustClientRectangle(ref Rectangle r) 
        {
#if FRAMEWORK20
            if (RenderButtons)
            {
                if (!_ButtonGroup.IsLayoutValid)
                {
                    using (Graphics g = this.CreateGraphics())
                    {
                        PaintInfo p = CreatePaintInfo(g);
                        _ButtonGroup.PerformLayout(p);
                    }
                }
                r.Width -= _ButtonGroup.Size.Width + 1;
            }
#endif
        }
        
        void INonClientControl.AdjustBorderRectangle(ref Rectangle r) { }

        void INonClientControl.RenderNonClient(Graphics g)
        {
#if FRAMEWORK20
            if (RenderButtons)
                PaintButtons(g);
#endif
        }
        #endregion
    }
}
