#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors
{
    [ToolboxItem(false)]
    public class VisualControlBase : PopupItemControl, IInputButtonControl, ISupportInitialize
    {
        #region Private Variables
        private VisualItem _RootVisual = null;
        private bool _FocusHighlightEnabled = false;
        private Color _LastBackColor = Color.Empty;
        private static Color _DefaultHighlightColor = Color.FromArgb(0xFF, 0xFF, 0x88);
        private Color _FocusHighlightColor = _DefaultHighlightColor;
        private bool _MouseOver = false;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when ButtonCustom control is clicked.
        /// </summary>
        public event EventHandler ButtonCustomClick;

        /// <summary>
        /// Occurs when ButtonCustom2 control is clicked.
        /// </summary>
        public event EventHandler ButtonCustom2Click;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public VisualControlBase()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(SystemOptions.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.Selectable, true);

            this.IsAccessible = true;

            _Colors = new InputControlColors();
            _Colors.ColorChanged += new EventHandler(ColorsColorChanged);

            _ButtonCustom = new InputButtonSettings(this);
            _ButtonCustom2 = new InputButtonSettings(this);
            _RootVisual = CreateRootVisual();
            _RootVisual.ArrangeInvalid += new EventHandler(VisualArrangeInvalid);
            _RootVisual.RenderInvalid += new EventHandler(VisualRenderInvalid);

            _BackgroundStyle.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
            _BackgroundStyle.SetColorScheme(this.ColorScheme);
            _BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);

            ApplyFieldNavigation();
        }
        #endregion

        #region Internal Implementation

        /// <summary>
        /// Resets the input position so the new input overwrites current value.
        /// </summary>
        public void ResetInputPosition()
        {
            if (_RootVisual is VisualInputBase)
                ((VisualInputBase)_RootVisual).ResetInputPosition();
            else if (_RootVisual is VisualInputGroup)
                ((VisualInputGroup)_RootVisual).ResetInputPosition();
        }

        private void ColorsColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected virtual PaintInfo CreatePaintInfo(PaintEventArgs e)
        {
            PaintInfo p = new PaintInfo();
            p.Graphics = e.Graphics;
            p.DefaultFont = this.Font;
            p.ForeColor = this.ForeColor;
            p.RenderOffset = new System.Drawing.Point();
            p.WatermarkColor = _WatermarkColor;
            p.WatermarkEnabled = _WatermarkEnabled;
            p.WatermarkFont = _WatermarkFont;
            p.AvailableSize = this.ClientRectangle.Size;
            p.ParentEnabled = this.Enabled;
            p.MouseOver = _MouseOver || this.Focused;
            p.Colors = _Colors;
            if (!_DisabledForeColor.IsEmpty) p.DisabledForeColor = _DisabledForeColor;
            return p;
        }

        private bool _RenderControlButtons = true;
        /// <summary>
        /// Gets or sets whether control system buttons are rendered. Default value is true.
        /// </summary>
        internal bool RenderControlButtons
        {
            get { return _RenderControlButtons; }
            set
            {
                _RenderControlButtons = value;
                OnRenderControlButtonsChanged();
                
            }
        }
        private void OnRenderControlButtonsChanged()
        {
            // Update IsRendered on system items.
            if (_RootVisual is VisualGroup)
            {
                VisualGroup group = (VisualGroup)_RootVisual;
                VisualItem[] items = new VisualItem[group.Items.Count];
                group.Items.CopyTo(items);
                foreach (VisualItem item in items)
                {
                    if (item.ItemType == eSystemItemType.SystemButton || item is VisualUpDownButton || item is LockUpdateCheckBox)
                    {
                        item.IsRendered = _RenderControlButtons;
                    }
                }
            }
            this.Invalidate();
        }
        
        private bool _CallBasePaintBackground = true;
        /// <summary>
        /// Gets or sets whether during painting OnPaintBackground on base control is called when BackColor=Transparent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool CallBasePaintBackground
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
            Rectangle clientRect = this.ClientRectangle;

            PaintInfo p = CreatePaintInfo(e);
            bool enabled=this.Enabled;

            if (!enabled)
            {
                Color c = _DisabledBackColor;
                if (c.IsEmpty) c = SystemColors.Control;
                using (SolidBrush brush = new SolidBrush(c))
                    e.Graphics.FillRectangle(brush, clientRect);
            }
            else if (this.BackColor != Color.Transparent)
                e.Graphics.FillRectangle(SystemBrushes.Window, clientRect);
            else if (this.BackColor == Color.Transparent && _CallBasePaintBackground)
                base.OnPaintBackground(e);

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);
            if (style.Custom)
            {
                ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, e.Graphics, clientRect);
                if (!enabled)
                {
                    ElementStyleDisplay.PaintBorder(displayInfo);
                }
                else
                    ElementStyleDisplay.Paint(displayInfo);
                clientRect.X += ElementStyleLayout.LeftWhiteSpace(style);
                clientRect.Y += ElementStyleLayout.TopWhiteSpace(style);
                clientRect.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style);
                clientRect.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);
                p.RenderOffset = clientRect.Location;
                p.AvailableSize = clientRect.Size;
            }

            if (_FocusHighlightEnabled && this.Focused && !_FocusHighlightColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(_FocusHighlightColor))
                    e.Graphics.FillRectangle(brush, clientRect);
            }

            if (!_RootVisual.IsLayoutValid)
            {
                if (_RootVisual is VisualGroup)
                    ((VisualGroup)_RootVisual).HorizontalItemAlignment = _InputHorizontalAlignment;
                _RootVisual.PerformLayout(p);
            }

            if (SupportsFreeTextEntry)
            {
                if (!IsFreeTextEntryVisible)
                    HideFreeTextBoxEntry();
                else
                {
                    Control textBox = GetFreeTextBox();
                    Rectangle r = GetFreeTextBounds(textBox.PreferredSize, clientRect);
                    if (textBox.Bounds != r)
                        textBox.Bounds = r;
                    p.RenderSystemItemsOnly = true;
                }
            }
            
            if (_InputHorizontalAlignment != eHorizontalAlignment.Left)
            {
                if (_InputHorizontalAlignment == eHorizontalAlignment.Right)
                    p.RenderOffset = new Point(clientRect.Width - _RootVisual.Size.Width,
                        (clientRect.Height - _RootVisual.Size.Height) / 2);
                else
                    p.RenderOffset = new Point((clientRect.Width - _RootVisual.Size.Width) / 2,
                        (clientRect.Height - _RootVisual.Size.Height) / 2);
            }
            else
                p.RenderOffset = new Point(0, (clientRect.Height - _RootVisual.Size.Height) / 2);

            if (this.WatermarkEnabled && this.WatermarkText.Length > 0 && this.IsWatermarkRendered)
            {
                Rectangle watermarkBounds = clientRect;
                watermarkBounds.Inflate(-1, -1);
                DrawWatermark(p, watermarkBounds);
            }
            else
                _RootVisual.ProcessPaint(p);

            base.OnPaint(e);

            if(disposeStyle) style.Dispose();
        }

        private ElementStyle GetBackgroundStyle(out bool disposeStyle)
        {
            disposeStyle = false;
            _BackgroundStyle.SetColorScheme(this.ColorScheme);
            return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
        }

        protected virtual void DrawWatermark(PaintInfo p, Rectangle r)
        {
            if (this.WatermarkText.Length == 0) return;
            Font font = p.DefaultFont;
            if (this.WatermarkFont != null) font = this.WatermarkFont;

            eTextFormat format = eTextFormat.Default;
            if (_WatermarkAlignment == eTextAlignment.Center)
                format |= eTextFormat.HorizontalCenter;
            else if (_WatermarkAlignment == eTextAlignment.Right)
                format |= eTextFormat.Right;

            TextDrawing.DrawString(p.Graphics, this.WatermarkText, font, this.WatermarkColor, r, format);
        }

        /// <summary>
        /// Gets whether watermark text should be rendered.
        /// </summary>
        protected virtual bool IsWatermarkRendered
        {
            get
            {
                return false;
            }
        }

        protected override bool IsInputChar(char charCode)
        {
            return true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            _RootVisual.ProcessKeyDown(e);
            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            _RootVisual.ProcessKeyPress(e);
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _RootVisual.ProcessKeyUp(e);
            base.OnKeyUp(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!IsFreeTextEntryVisible && _RootVisual.ProcessCmdKey(ref msg, keyData))
                return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Focused)
            {
                if (this.Parent is MenuPanel)
                    this.Focus();
                else
                    this.Select();
            }
            _RootVisual.ProcessMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            _RootVisual.ProcessMouseWheel(e);
            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _RootVisual.ProcessMouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _RootVisual.ProcessMouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _MouseOver = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _MouseOver = false;
            _RootVisual.ProcessMouseLeave();
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            _RootVisual.ProcessClick();
            base.OnClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            _RootVisual.ProcessMouseClick(e);
            base.OnMouseClick(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _RootVisual.ProcessGotFocus();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _RootVisual.ProcessLostFocus();
            base.OnLostFocus(e);
        }

        private bool _IsKeyboardFocusWithin = false;
        /// <summary>
        /// Gets whether keyboard focus is within the control.
        /// </summary>
        [Browsable(false)]
        public bool IsKeyboardFocusWithin
        {
            get { return _IsKeyboardFocusWithin; }
            internal set
            {
                _IsKeyboardFocusWithin = value;
                OnIsKeyboardFocusWithinChanged();
            }
        }

        protected virtual void OnIsKeyboardFocusWithinChanged()
        {
            if (SupportsFreeTextEntry)
            {
                if (!IsKeyboardFocusWithin) _RootVisual.ProcessLostFocus();
            }
            if (FocusHighlightEnabled) this.Invalidate();

        }
        

        protected override void OnEnter(EventArgs e)
        {
            this.IsKeyboardFocusWithin = true;
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.IsKeyboardFocusWithin = false;
            base.OnLeave(e);
        }

        protected virtual VisualItem CreateRootVisual()
        {
            return null;
        }

        /// <summary>
        /// Gets the reference to internal visual item used as the root visual for the control. Using this property is in all cases not necessary except for some
        /// very advanced usage scenarios.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual VisualItem RootVisualItem
        {
            get { return _RootVisual; }
        }

        private void VisualArrangeInvalid(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void VisualRenderInvalid(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private eHorizontalAlignment _InputHorizontalAlignment = eHorizontalAlignment.Left;
        /// <summary>
        /// Gets or sets the input field alignment inside the control
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalAlignment.Left), Category("Appearance"), Description("Indicates alignment of input fields inside of the control.")]
        public virtual eHorizontalAlignment InputHorizontalAlignment
        {
            get { return _InputHorizontalAlignment; }
            set
            {
                if (_InputHorizontalAlignment != value)
                {
                    eHorizontalAlignment oldValue = _InputHorizontalAlignment;
                    _InputHorizontalAlignment = value;
                    _RootVisual.InvalidateArrange();
                    OnInputHorizontalAlignmentChanged(oldValue, value);
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// Called when InputHorizontalAlignment property value has changed.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New Value.</param>
        protected virtual void OnInputHorizontalAlignmentChanged(eHorizontalAlignment oldValue, eHorizontalAlignment newValue)
        {
        }

        private Font _WatermarkFont = null;
        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public virtual Font WatermarkFont
        {
            get { return _WatermarkFont; }
            set { _WatermarkFont = value; this.Invalidate(); }
        }

        private Color _WatermarkColor = SystemColors.GrayText;
        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public virtual Color WatermarkColor
        {
            get { return _WatermarkColor; }
            set { _WatermarkColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return _WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed if set for the input items. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed if set for the input items.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; this.Invalidate(); }
        }

        private string _WatermarkText = "";
        /// <summary>
        /// Gets or sets the watermark text displayed on the input control when control is empty.
        /// </summary>
        [DefaultValue(""), Description("Indicates watermark text displayed on the input control when control is empty."), Category("Watermark"), Localizable(true)]
        public string WatermarkText
        {
            get { return _WatermarkText; }
            set
            {
                if (value != null)
                {
                    _WatermarkText = value;
                    this.Invalidate();
                }
            }
        }

        private eTextAlignment _WatermarkAlignment = eTextAlignment.Left;
        /// <summary>
        /// Gets or sets the watermark text alignment. Default value is left.
        /// </summary>
        [Browsable(true), DefaultValue(eTextAlignment.Left), Description("Indicates watermark text alignment."), Category("Watermark")]
        public eTextAlignment WatermarkAlignment
        {
            get { return _WatermarkAlignment; }
            set
            {
                if (_WatermarkAlignment != value)
                {
                    _WatermarkAlignment = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as background color to highlight text box when it has input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
        public virtual bool FocusHighlightEnabled
        {
            get { return _FocusHighlightEnabled; }
            set
            {
                if (_FocusHighlightEnabled != value)
                {
                    _FocusHighlightEnabled = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color used as background color to highlight text box when it has input focus and focus highlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
        public virtual Color FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set
            {
                if (_FocusHighlightColor != value)
                {
                    _FocusHighlightColor = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return !_FocusHighlightColor.Equals(_DefaultHighlightColor);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            FocusHighlightColor = _DefaultHighlightColor;
        }

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
            RecreateButtons();
        }

        protected virtual void RecreateButtons()
        {
            VisualItem[] buttons = CreateOrderedButtonList();
            if (_RootVisual is VisualGroup)
            {
                // Remove all system buttons that are already in the list
                VisualGroup group = _RootVisual as VisualGroup;
                VisualItem[] items = new VisualItem[group.Items.Count];
                group.Items.CopyTo(items);
                foreach (VisualItem item in items)
                {
                    if (item.ItemType == eSystemItemType.SystemButton)
                    {
                        group.Items.Remove(item);
                        if(item == _ButtonCustom.ItemReference)
                            item.Click -= new EventHandler(CustomButtonClick);
                        else if(item == _ButtonCustom2.ItemReference)
                            item.Click -= new EventHandler(CustomButton2Click);
                        item.IsRendered = _RenderControlButtons;
                    }
                }

                // Add new buttons to the list
                group.Items.AddRange(buttons);
            }
        }

        private void CustomButtonClick(object sender, EventArgs e)
        {
            OnButtonCustomClick(e);
        }

        protected virtual void OnButtonCustomClick(EventArgs e)
        {
            if (ButtonCustomClick != null)
                ButtonCustomClick(this, e);
        }

        private void CustomButton2Click(object sender, EventArgs e)
        {
            OnButtonCustom2Click(e);
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
                    _ButtonCustom.ItemReference.Click -= new EventHandler(CustomButtonClick);
                _ButtonCustom.ItemReference = button;
                button.Click += new EventHandler(CustomButtonClick);
                button.Enabled = _ButtonCustom.Enabled;
                list.Add(_ButtonCustom, button);
            }

            if (_ButtonCustom2.Visible)
            {
                VisualItem button = CreateButton(_ButtonCustom2);
                if (_ButtonCustom.ItemReference != null)
                    _ButtonCustom.ItemReference.Click -= new EventHandler(CustomButton2Click);
                _ButtonCustom2.ItemReference = button;
                button.Click += new EventHandler(CustomButton2Click);
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
            button.Alignment = eItemAlignment.Right;
            button.ItemType = eSystemItemType.SystemButton;
            button.Enabled = buttonSettings.Enabled;
            button.Shortcut = buttonSettings.Shortcut;
        }

        protected override void OnResize(EventArgs e)
        {
            AutoAdjustHeight();
            _RootVisual.InvalidateArrange();
            base.OnResize(e);
        }

        protected override PopupItem CreatePopupItem()
        {
            return new ButtonItem();
        }

        protected override void RecalcSize()
        {
        }

        public override void PerformClick()
        {
        }

        private ElementStyle _BackgroundStyle = new ElementStyle();
        /// <summary>
        /// Specifies the background style of the control.
        /// </summary>
        [Category("Style"), Description("Gets or sets control background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return _BackgroundStyle; }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _BackgroundStyle.StyleChanged -= new EventHandler(this.VisualPropertyChanged);
            _BackgroundStyle = new ElementStyle();
            _BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            this.Invalidate();
        }

        private void VisualPropertyChanged(object sender, EventArgs e)
        {
            OnVisualPropertyChanged();
        }

        protected virtual void OnVisualPropertyChanged()
        {
            _RootVisual.InvalidateArrange();
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (_BackgroundStyle != null) _BackgroundStyle.StyleChanged -= VisualPropertyChanged;
            _BackgroundStyle.Dispose();
            base.Dispose(disposing);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        /// Gets the preferred height of the control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PreferredHeight
        {
            get
            {
                return GetPreferredHeight();
            }
        }

        private int _PreferredHeight = -1;
        protected virtual int GetPreferredHeight()
        {
            if (_PreferredHeight > -1)
            {
                return _PreferredHeight;
            }
            int h = this.Font.Height + ((_AutoBorderSize != 0) ? _AutoBorderSize : ((SystemInformation.BorderSize.Height * 4) + 3));
            _PreferredHeight = h;
            return h;
        }
        private int _AutoBorderSize;
        internal int AutoBorderSize
        {
            get { return _AutoBorderSize; }
            set
            {
                _AutoBorderSize = value;
                _PreferredHeight = -1;
            }
        }
        
        protected override void OnFontChanged(EventArgs e)
        {
            _PreferredHeight = -1;
            this.Height = PreferredHeight;
            base.OnFontChanged(e);
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(80, GetPreferredHeight());
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size size = this.DefaultSize;
            if (proposedSize.Width > 0 && proposedSize.Width < size.Width)
                size.Width = proposedSize.Width;
            return this.DefaultSize;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            AutoAdjustHeight();
            base.OnHandleCreated(e);
        }

        private void AutoAdjustHeight()
        {
            if (this.Height != PreferredHeight)
                this.Height = PreferredHeight;
        }

        private eInputFieldNavigation _FieldNavigation = eInputFieldNavigation.All;
        /// <summary>
        /// Gets or sets the keys used to navigate between the input fields provided by this control.
        /// </summary>
        [DefaultValue(eInputFieldNavigation.All), Description("Indicates keys used to navigate between the input fields provided by this control")]
        public eInputFieldNavigation FieldNavigation
        {
            get { return _FieldNavigation; }
            set
            {
                if (_FieldNavigation != value)
                {
                    _FieldNavigation = value;
                    ApplyFieldNavigation();
                }
            }
        }

        protected virtual void ApplyFieldNavigation()
        {
            if (_RootVisual is VisualInputGroup)
            {
                VisualInputGroup group = _RootVisual as VisualInputGroup;
                group.ArrowNavigationEnabled = ((_FieldNavigation & eInputFieldNavigation.Arrows) == eInputFieldNavigation.Arrows);
                group.TabNavigationEnabled = ((_FieldNavigation & eInputFieldNavigation.Tab) == eInputFieldNavigation.Tab);
                group.EnterNavigationEnabled = ((_FieldNavigation & eInputFieldNavigation.Enter) == eInputFieldNavigation.Enter);
            }
        }

        protected virtual bool IsNull(object value)
        {
            if (value == null || value is DBNull) return true;
            return false;
        }

        private Color _DisabledBackColor = Color.Empty;
        /// <summary>
        /// Gets or sets the control background color when control is disabled. Default value is an empty color which indicates that background is not changed when control is disabled.
        /// </summary>
        [Description("Indicates control background color when control is disabled"), Category("Appearance")]
        public Color DisabledBackColor
        {
            get { return _DisabledBackColor; }
            set
            {
                if (_DisabledBackColor != value)
                {
                    _DisabledBackColor = value;
                    if (!this.Enabled) this.Invalidate();
                }
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDisabledBackColor()
        {
            return !_DisabledBackColor.IsEmpty;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDisabledBackColor()
        {
            DisabledBackColor = Color.Empty;
        }

        private Color _DisabledForeColor = Color.Empty;
        /// <summary>
        /// Gets or sets the control text color when control is disabled. Default value is an empty color which indicates that background is not changed when control is disabled.
        /// </summary>
        [Description("Indicates control background color when control is disabled"), Category("Appearance")]
        public Color DisabledForeColor
        {
            get { return _DisabledForeColor; }
            set
            {
                if (_DisabledForeColor != value)
                {
                    _DisabledForeColor = value;
                    if (!this.Enabled) this.Invalidate();
                }
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDisabledForeColor()
        {
            return !_DisabledForeColor.IsEmpty;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDisabledForeColor()
        {
            DisabledForeColor = Color.Empty;
        }


        private InputControlColors _Colors = null;
        /// <summary>
        /// Gets the system colors used by the control.
        /// </summary>
        [Category("Colors"), Description("System colors used by the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputControlColors Colors
        {
            get { return _Colors; }
        }

        /// <summary>
        /// Selects next input field if possible.
        /// </summary>
        /// <returns>true if next input field was selected otherwise false.</returns>
        public bool SelectNextInputField()
        {
            VisualInputGroup group = _RootVisual as VisualInputGroup;
            if (group != null) return group.SelectNextInput();
            return false;
        }
        /// <summary>
        /// Selects previous input field if possible.
        /// </summary>
        /// <returns>true if previous input field was selected otherwise false.</returns>
        public bool SelectPreviousInputField()
        {
            VisualInputGroup group = _RootVisual as VisualInputGroup;
            if (group != null) return group.SelectPreviousInput();
            return false;
        }
        #endregion

        #region Free-Text Entry Support
        protected virtual Rectangle GetFreeTextBounds(Size preferedSize, Rectangle clientRect)
        {
            Rectangle r = Rectangle.Empty;
            if (_RootVisual == null) return r;

            r = clientRect; // _RootVisual.RenderBounds;
            VisualGroup group = _RootVisual as VisualGroup;
            foreach (VisualItem item in group.Items)
            {
                if (item.ItemType == eSystemItemType.SystemButton)
                {
                    DisplayHelp.ExcludeEdgeRect(ref r, item.RenderBounds);
                }
            }

            if (r.Width != clientRect.Width)
            {
                r.Inflate(-1, 0);
            }

            if (preferedSize.Height > 0 && preferedSize.Height < r.Height)
            {
                r.Y += (r.Height - preferedSize.Height) / 2;
                r.Height = preferedSize.Height;
            }

            return r;
        }

        protected virtual bool SupportsFreeTextEntry
        {
            get
            {
                return false;
            }
        }

        protected virtual Control GetFreeTextBox()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual bool IsFreeTextEntryVisible
        {
            get
            {
                return false;
            }
        }

        protected virtual void HideFreeTextBoxEntry()
        {
            
        }
        #endregion

        #region Property Hiding
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        [Browsable(false)]
        public override bool DisabledImagesGrayScale
        {
            get
            {
                return base.DisabledImagesGrayScale;
            }
            set
            {
                base.DisabledImagesGrayScale = value;
            }
        }

        [Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }

        [Browsable(false)]
        public override eDotNetBarStyle Style
        {
            get
            {
                return base.Style;
            }
            set
            {
                base.Style = value;
            }
        }

        [Browsable(false)]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }
        #endregion

        #region ISupportInitialize
        private bool _Initializing = false;
        protected virtual bool IsInitializing
        {
            get
            {
                return _Initializing;
            }
        }
        /// <summary>
        /// ISupportInitialize.BeginInit implementation. While initialization is in progress ValueChanged events will not be fired.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void BeginInit()
        {
            _Initializing = true;
        }

        /// <summary>
        /// ISupportInitialize.EndInit implementation.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void EndInit()
        {
            _Initializing = false;
        }
        #endregion
    }

    #region Free Text Box Entry
    internal class FreeTextEntryBox : TextBox
    {
        private bool _HideOnLostFocus;
        public event EventHandler RevertValue;
        public event EventHandler ApplyValue;
        public event EventHandler IncreaseValue;
        public event EventHandler DecreaseValue;
        /// <summary>
        /// Raises DecreaseValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnDecreaseValue(EventArgs e)
        {
            EventHandler handler = DecreaseValue;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Raises IncreaseValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnIncreaseValue(EventArgs e)
        {
            EventHandler handler = IncreaseValue;
            if (handler != null)
                handler(this, e);
        }
        public FreeTextEntryBox()
        {
            //this.MaxLength = 15;
        }

        private void OnApplyValue(EventArgs e)
        {
            if (ApplyValue != null) ApplyValue(this, e);
        }

        private void OnRevertValue(EventArgs e)
        {
            if (RevertValue != null) RevertValue(this, e);
        }

        public void HideOnLostFocus()
        {
            if (this.Focused)
                _HideOnLostFocus = true;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            if (_HideOnLostFocus)
            {
                this.Visible = false;
                _HideOnLostFocus = false;
            }
            base.OnLostFocus(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnApplyValue(e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                OnRevertValue(e);
            }
            else if (e.KeyCode == Keys.Up)
            {
                OnIncreaseValue(e);
            }
            else if (e.KeyCode == Keys.Down)
            {
                OnDecreaseValue(e);
            }
            //else
            //{
            //    char c = BarFunctions.GetCharForKeyValue(e.KeyValue);

            //    if (c != '.' && !char.IsDigit(c) && !char.IsControl(c))
            //        e.SuppressKeyPress = true;
            //}

            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this.ReadOnly)
            {
                if (e.Delta > 0)
                {
                    OnIncreaseValue(EventArgs.Empty);
                }
                else
                {
                    OnDecreaseValue(EventArgs.Empty);
                }
            }
            base.OnMouseWheel(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            OnApplyValue(e);
            base.OnLeave(e);
        }

    }
    #endregion

    #region FreeTextEntryConversion Event Handler and Arguments
    /// <summary>
    /// Defines data for ConvertFreeTextEntry event.
    /// </summary>
    public class FreeTextEntryConversionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the string value that was entered by the user.
        /// </summary>
        public readonly string ValueEntered;

        /// <summary>
        /// Gets or sets the converted ValueEntered into the control's value type. For example for IpAddressInput the value set here
        /// must be of string type and in IP address format. For IntegerInput control the value set here must be an int type. For DateTimeInput
        /// control value set here must be DateTime type.
        /// If you provide ControlValue set ValueConverted=true to indicate so.
        /// </summary>
        public object ControlValue = null;

        /// <summary>
        /// Gets or sets whether ValueEntered has been converted to ControlValue. Set to true to indicate that you have performed conversion.
        /// </summary>
        public bool IsValueConverted = false;


        public FreeTextEntryConversionEventArgs(string valueEntered)
        {
            this.ValueEntered = valueEntered;
        }
    }
    /// <summary>
    /// Defines delegate for ConvertFreeTextEntry event.
    /// </summary>
    /// <param name="sender">Source of event.</param>
    /// <param name="ea">Provides event data.</param>
    public delegate void FreeTextEntryConversionEventHandler(object sender, FreeTextEntryConversionEventArgs e);
    #endregion
}
#endif

