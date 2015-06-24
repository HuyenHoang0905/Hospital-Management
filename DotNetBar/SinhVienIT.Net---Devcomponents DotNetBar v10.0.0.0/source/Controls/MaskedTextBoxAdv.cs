#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.Editors;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(MaskedTextBoxAdv), "Controls.MaskedTextBoxAdv.ico"), System.ComponentModel.ToolboxItem(true), DefaultProperty("Mask"), DefaultBindingProperty("Text"), DefaultEvent("MaskInputRejected"), Designer("DevComponents.DotNetBar.Design.MaskedTextBoxAdvDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class MaskedTextBoxAdv : PopupItemControl, IInputButtonControl, ICommandSource
    {
        #region Private Variables
        private MaskedTextBox _MaskedTextBox = null;
        private ButtonItem _PopupItem = null;
        private static string _DropDownItemContainerName = "sysPopupItemContainer";
        private static string _DropDownControlContainerName = "sysPopupControlContainer";
        private Color _FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        private bool _FocusHighlightEnabled = false;
        private string _WatermarkText = "";
        private Font _WatermarkFont = null;
        private Color _WatermarkColor = SystemColors.GrayText;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.
        /// </summary>
        public event CancelEventHandler ButtonClearClick;
        /// <summary>
        /// Occurs when Drop-Down button that shows popup is clicked and allows you to cancel showing of the popup.
        /// </summary>
        public event CancelEventHandler ButtonDropDownClick;
        /// <summary>
        /// Occurs when ButtonCustom control is clicked.
        /// </summary>
        public event EventHandler ButtonCustomClick;
        /// <summary>
        /// Occurs when ButtonCustom2 control is clicked.
        /// </summary>
        public event EventHandler ButtonCustom2Click;
        /// <summary>
        /// Occurs after the insert mode has changed
        /// </summary>
        [Description("Occurs after the insert mode has changed")]
        public event EventHandler IsOverwriteModeChanged;
        /// <summary>
        /// Occurs after the input mask is changed.
        /// </summary>
        [Description("Occurs after the input mask is changed.")]
        public event EventHandler MaskChanged;
        /// <summary>
        /// Occurs when the user's input or assigned character does not match the corresponding format element of the input mask.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the user's input or assigned character does not match the corresponding format element of the input mask.")]
        public event MaskInputRejectedEventHandler MaskInputRejected;
        /// <summary>
        /// Occurs when the text alignment is changed.
        /// </summary>
        [Description("Occurs when the text alignment is changed.")]
        public event EventHandler TextAlignChanged;
        /// <summary>
        /// Occurs when MaskedTextBox has finished parsing the current value using the ValidatingType property.
        /// </summary>
        [Category("Focus"), Description("Occurs when MaskedTextBox has finished parsing the current value using the ValidatingType property.")]
        public event TypeValidationEventHandler TypeValidationCompleted;

 

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MaskedTextBoxAdv class.
        /// </summary>
        public MaskedTextBoxAdv()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            _MaskedTextBox = new MaskedTextBoxInternal();
            this.BackColor = SystemColors.Window;
            InitControl();
        }


        /// <summary>
        /// Initializes a new instance of the MaskedTextBoxAdv class using the specified input mask.
        /// </summary>
        /// <param name="maskedTextProvider">A custom mask language provider, derived from the MaskedTextProvider class. </param>
        public MaskedTextBoxAdv(MaskedTextProvider maskedTextProvider)
        {
            _MaskedTextBox = new MaskedTextBox(maskedTextProvider);
            InitControl();
        }

        /// <summary>
        /// Initializes a new instance of the MaskedTextBoxAdv class using the specified input mask.
        /// </summary>
        /// <param name="mask">Initializes a new instance of the MaskedTextBox class using the specified input mask.</param>
        public MaskedTextBoxAdv(string mask)
        {
            _MaskedTextBox = new MaskedTextBox(mask);
            InitControl();
        }

        private void InitControl()
        {
            _BackgroundStyle.SetColorScheme(this.ColorScheme);
            _BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);

            _ButtonCustom = new InputButtonSettings(this);
            _ButtonCustom2 = new InputButtonSettings(this);
            _ButtonClear = new InputButtonSettings(this);
            _ButtonDropDown = new InputButtonSettings(this);
            CreateButtonGroup();

            _MaskedTextBox.BorderStyle = BorderStyle.None;
            _MaskedTextBox.TextChanged += new EventHandler(MaskedTextBoxTextChanged);
            _MaskedTextBox.IsOverwriteModeChanged += new EventHandler(MaskedTextBoxIsOverwriteModeChanged);
            _MaskedTextBox.MaskChanged += new EventHandler(MaskedTextBoxMaskChanged);
            _MaskedTextBox.MaskInputRejected += new MaskInputRejectedEventHandler(MaskedTextBoxMaskInputRejected);
            _MaskedTextBox.TextAlignChanged += new EventHandler(MaskedTextBoxTextAlignChanged);
            _MaskedTextBox.TypeValidationCompleted += new TypeValidationEventHandler(MaskedTextBoxTypeValidationCompleted);
            _MaskedTextBox.SizeChanged += new EventHandler(MaskedTextBoxSizeChanged);
            this.Controls.Add(_MaskedTextBox);
        }

        private void MaskedTextBoxSizeChanged(object sender, EventArgs e)
        {
            if (!_InternalSizeUpdate)
                UpdateLayout();
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as background color to highlight text box when it has input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
        public bool FocusHighlightEnabled
        {
            get { return _FocusHighlightEnabled; }
            set
            {
                if (_FocusHighlightEnabled != value)
                {
                    _FocusHighlightEnabled = value;
                    if (this.Focused)
                        this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color used as background color to highlight text box when it has input focus and focus highlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
        public Color FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set
            {
                if (_FocusHighlightColor != value)
                {
                    _FocusHighlightColor = value;
                    if (this.Focused)
                        this.Invalidate(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return !_FocusHighlightColor.Equals(ColorScheme.GetColor(0xFFFF88));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        }

        private void MaskedTextBoxTypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            OnTypeValidationCompleted(e);
        }

        /// <summary>
        /// Raises TypeValidationCompleted event.
        /// </summary>
        protected virtual void OnTypeValidationCompleted(TypeValidationEventArgs e)
        {
            TypeValidationEventHandler eh = TypeValidationCompleted;
            if (eh != null)
                eh(this, e);
        }

        private void MaskedTextBoxTextAlignChanged(object sender, EventArgs e)
        {
            OnTextAlignChanged(e);
        }
        /// <summary>
        /// Raises the TextAlignChanged event.
        /// </summary>
        protected virtual void OnTextAlignChanged(EventArgs e)
        {
            EventHandler eh = TextAlignChanged;
            if (eh != null)
                eh(this, e);
        }

        private void MaskedTextBoxMaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            OnMaskInputRejected(e);
        }
        /// <summary>
        /// Raises MaskInputRejected event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMaskInputRejected(MaskInputRejectedEventArgs e)
        {
            MaskInputRejectedEventHandler eh = MaskInputRejected;
            if (eh != null)
                eh(this, e);
        }

        private void MaskedTextBoxMaskChanged(object sender, EventArgs e)
        {
            OnMaskChanged(e);
        }

        /// <summary>
        /// Raises the MaskChanged event.
        /// </summary>
        protected virtual void OnMaskChanged(EventArgs e)
        {
            EventHandler eh = MaskChanged;
            if (eh != null)
                eh(this, e);
        }

        void MaskedTextBoxIsOverwriteModeChanged(object sender, EventArgs e)
        {
            OnIsOverwriteModeChanged(e);
        }

        /// <summary>
        /// Raises IsOverwriteModeChanged event.
        /// </summary>
        protected virtual void OnIsOverwriteModeChanged(EventArgs e)
        {
            EventHandler eh = IsOverwriteModeChanged;
            if (eh != null)
                eh(this, e);
        }

        private void MaskedTextBoxTextChanged(object sender, EventArgs e)
        {
            base.Text = _MaskedTextBox.Text;
            //OnTextChanged(e);
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
            _ButtonGroup.InvalidateArrange();
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (_BackgroundStyle != null) _BackgroundStyle.StyleChanged -= new EventHandler(VisualPropertyChanged);
            base.Dispose(disposing);
        }

        private InputButtonSettings _ButtonDropDown = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that shows drop-down when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that shows drop-down when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonDropDown
        {
            get
            {
                return _ButtonDropDown;
            }
        }

        private InputButtonSettings _ButtonClear = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that clears the content of the control when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that clears the content of the control when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonClear
        {
            get
            {
                return _ButtonClear;
            }
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
            UpdateButtons();
        }

        private VisualGroup _ButtonGroup = null;
        private void UpdateButtons()
        {
            RecreateButtons();
            _ButtonGroup.InvalidateArrange();
            this.Invalidate();
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

            if (_ButtonClear.Visible)
            {
                VisualItem button = CreateButton(_ButtonClear);
                if (_ButtonClear.ItemReference != null)
                    _ButtonClear.ItemReference.Click -= new EventHandler(ClearButtonClick);
                _ButtonClear.ItemReference = button;
                button.MouseUp += new MouseEventHandler(ClearButtonClick);
                list.Add(_ButtonClear, button);
            }

            if (_ButtonDropDown.Visible)
            {
                VisualItem button = CreateButton(_ButtonDropDown);
                if (_ButtonDropDown.ItemReference != null)
                {
                    _ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
                }
                _ButtonDropDown.ItemReference = button;
                button.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
                list.Add(_ButtonDropDown, button);
            }

            return list;
        }

        protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
        {
            VisualItem item = null;

            if (buttonSettings == _ButtonDropDown)
            {
                item = new VisualDropDownButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }
            else
            {
                item = new VisualCustomButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }

            VisualButton button = item as VisualButton;
            button.ClickAutoRepeat = false;

            if (buttonSettings == _ButtonClear)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.DateReset.png");
            }

            return item;
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

        internal VisualGroup ButtonGroup
        {
            get { return (_ButtonGroup); }
        }

        private void ButtonGroupRenderInvalid(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ButtonGroupArrangeInvalid(object sender, EventArgs e)
        {
            Invalidate();
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
            ElementStyle style = GetBackgroundStyle(out disposeStyle);
            s.Height -= (ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(style, eSpacePart.Border)) + 2;
            s.Width -= (ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border)) + 2;
            p.AvailableSize = s;
            p.ParentEnabled = this.Enabled;
            p.MouseOver = _MouseOver || this.Focused;

            if(disposeStyle) style.Dispose();

            return p;
        }

        private ElementStyle GetBackgroundStyle(out bool disposeStyle)
        {
            _BackgroundStyle.SetColorScheme(this.GetColorScheme());
            disposeStyle = false;
            return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Rectangle clientRect = this.ClientRectangle;
            bool enabled = this.Enabled;

            if (!this.Enabled)
                e.Graphics.FillRectangle(SystemBrushes.Control, clientRect);
            else
            {
                using(SolidBrush brush=new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(brush, clientRect);
            }

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);

            if (style.Custom)
            {
                SmoothingMode sm = g.SmoothingMode;
                if(this.AntiAlias)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, g, clientRect);
                if (!enabled)
                {
                    ElementStyleDisplay.PaintBorder(displayInfo);
                }
                else
                    ElementStyleDisplay.Paint(displayInfo);
                if (this.AntiAlias)
                    g.SmoothingMode = sm;
            }

            PaintButtons(g);

            if(disposeStyle) style.Dispose();
        }


        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return this.BackColor != SystemColors.Window;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _MaskedTextBox.BackColor = this.BackColor;
        }

        private void PaintButtons(Graphics g)
        {
            PaintInfo p = CreatePaintInfo(g);

            if (!_ButtonGroup.IsLayoutValid)
            {
                UpdateLayout(p);
            }

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);
            //p.RenderOffset = new Point(this.Width - ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border));
            _ButtonGroup.RenderBounds = GetButtonsRenderBounds(style);
            _ButtonGroup.ProcessPaint(p);

            if(disposeStyle) style.Dispose();
        }

        private Rectangle GetButtonsRenderBounds(ElementStyle style)
        {
            if (this.RightToLeft == RightToLeft.Yes)
            {
                return new Rectangle((ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1), ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
            }
            
            return new Rectangle(this.Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
        }

        protected override void OnResize(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            UpdateLayout();
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _MaskedTextBox.Focus();
            base.OnGotFocus(e);
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            UpdateLayout();
            this.Invalidate();
            base.OnRightToLeftChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            UpdateLayout();
            this.Invalidate();
            base.OnFontChanged(e);
        }

        private void UpdateLayout()
        {
            using(Graphics g = BarFunctions.CreateGraphics(this))
                UpdateLayout(CreatePaintInfo(g));
        }

        private bool _InternalSizeUpdate = false;
        private void UpdateLayout(PaintInfo p)
        {
            if (_InternalSizeUpdate) return;

            _InternalSizeUpdate = true;

            try
            {
                if (!_ButtonGroup.IsLayoutValid)
                {
                    _ButtonGroup.PerformLayout(p);
                }

                bool disposeStyle = false;
                ElementStyle style = GetBackgroundStyle(out disposeStyle);
                Rectangle maskedControlRect = ElementStyleLayout.GetInnerRect(style, this.ClientRectangle);
                if (RenderButtons)
                {
                    Rectangle buttonsRect = GetButtonsRenderBounds(style);
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        maskedControlRect.X += buttonsRect.Right;
                        maskedControlRect.Width -= buttonsRect.Right;
                    }
                    else
                    {
                        maskedControlRect.Width -= (maskedControlRect.Right - buttonsRect.X);
                    }
                }
                if (_MaskedTextBox.PreferredHeight < maskedControlRect.Height)
                {
                    maskedControlRect.Y += (maskedControlRect.Height - _MaskedTextBox.PreferredHeight) / 2;
                }
                _MaskedTextBox.Bounds = maskedControlRect;

                if(disposeStyle) style.Dispose();
            }
            finally
            {
                _InternalSizeUpdate = false;
            }
        }

        private Control _PreviousDropDownControlParent = null;
        private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 250)
            {
                _CloseTime = DateTime.MinValue;
                return;
            }

            ShowDropDown();
        }

        private bool _ShowingPopup = false;
        /// <summary>
        /// Shows drop-down popup. Note that popup will be shown only if there is a DropDownControl assigned or DropDownItems collection has at least one item.
        /// </summary>
        public void ShowDropDown()
        {

            if (_DropDownControl == null && _PopupItem.SubItems.Count == 0 || _ShowingPopup || _PopupItem.Expanded)
                return;

            _ShowingPopup = true;
            try
            {
                ControlContainerItem cc = null;
                ItemContainer ic = null;
                if (_DropDownControl != null)
                {
                    ic = new ItemContainer();
                    ic.Name = _DropDownItemContainerName;
                    cc = new ControlContainerItem(_DropDownControlContainerName);
                    ic.SubItems.Add(cc);
                    _PopupItem.SubItems.Insert(0, ic);
                }

                CancelEventArgs cancelArgs = new CancelEventArgs();
                OnButtonDropDownClick(cancelArgs);
                if (cancelArgs.Cancel || _PopupItem.SubItems.Count == 0)
                {
                    if (ic != null)
                        _PopupItem.SubItems.Remove(ic);
                    return;
                }

                if (_DropDownControl != null)
                {
                    _PreviousDropDownControlParent = _DropDownControl.Parent;
                    cc.Control = _DropDownControl;
                }

                _PopupItem.SetDisplayRectangle(this.ClientRectangle);
                if (this.RightToLeft == RightToLeft.No)
                {
                    Point pl = new Point(this.Width - _PopupItem.PopupSize.Width, this.Height);
                    ScreenInformation screen = BarFunctions.ScreenFromControl(this);
                    Point ps = PointToScreen(pl);
                    if (screen != null && screen.WorkingArea.X > ps.X)
                    {
                        pl.X = 0;
                    }
                    _PopupItem.PopupLocation = pl;
                }

                _PopupItem.Expanded = !_PopupItem.Expanded;
            }
            finally
            {
                _ShowingPopup = false;
            }
        }

        /// <summary>
        /// Closes the drop-down popup if it is open.
        /// </summary>
        public void CloseDropDown()
        {
            if (_PopupItem.Expanded) _PopupItem.Expanded = false;
        }

        private DateTime _CloseTime = DateTime.MinValue;
        private void DropDownPopupClose(object sender, EventArgs e)
        {
            _CloseTime = DateTime.Now;
            if (_PopupItem.SubItems.Contains(_DropDownItemContainerName))
            {
                ItemContainer ic = _PopupItem.SubItems[_DropDownItemContainerName] as ItemContainer;
                if (ic != null)
                {
                    ControlContainerItem cc = ic.SubItems[_DropDownControlContainerName] as ControlContainerItem;
                    if (cc != null)
                    {
                        cc.Control = null;
                        ic.SubItems.Remove(cc);
                        if (_DropDownControl != null)
                        {
                            _DropDownControl.Parent = _PreviousDropDownControlParent;
                            _PreviousDropDownControlParent = null;
                        }
                    }
                    _PopupItem.SubItems.Remove(ic);
                }
            }
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonClearClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            _MaskedTextBox.Text = "";
        }

        /// <summary>
        /// Raises the ButtonClearClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonClearClick(CancelEventArgs e)
        {
            if (ButtonClearClick != null)
                ButtonClearClick(this, e);
        }

        /// <summary>
        /// Raises the ButtonDropDownClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonDropDownClick(CancelEventArgs e)
        {
            if (ButtonDropDownClick != null)
                ButtonDropDownClick(this, e);
        }

        private Control _DropDownControl = null;
        /// <summary>
        /// Gets or sets the reference of the control that will be displayed on popup that is shown when drop-down button is clicked.
        /// </summary>
        [DefaultValue(null), Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
        public Control DropDownControl
        {
            get { return _DropDownControl; }
            set
            {
                _DropDownControl = value;
            }
        }

        protected override PopupItem CreatePopupItem()
        {
            ButtonItem button = new ButtonItem("sysPopupProvider");
            button.PopupFinalized += new EventHandler(DropDownPopupClose);
            _PopupItem = button;
            return button;
        }

        /// <summary>
        /// Gets the collection of BaseItem derived items displayed on popup menu.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection DropDownItems
        {
            get { return _PopupItem.SubItems; }
        }

        protected override void RecalcSize()
        {
        }

        public override void PerformClick()
        {
        }

        private bool RenderButtons
        {
            get
            {
                return _ButtonCustom != null && _ButtonCustom.Visible || _ButtonCustom2 != null && _ButtonCustom2.Visible ||
                    _ButtonDropDown != null && _ButtonDropDown.Visible || _ButtonClear != null && _ButtonClear.Visible;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseMove(e);
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseLeave();
            }
            base.OnMouseLeave(e);
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

        protected override void OnForeColorChanged(EventArgs e)
        {
            if (_MaskedTextBox != null) _MaskedTextBox.ForeColor = this.ForeColor;
            base.OnForeColorChanged(e);
        }

        /// <summary>
        /// Gets the reference to internal MaskedTextBox control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MaskedTextBox MaskedTextBox
        {
            get
            {
                return _MaskedTextBox;
            }
        }

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return _WatermarkText; }
            set
            {
                if (value == null) value = "";
                _WatermarkText = value;
                MarkupTextChanged();
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return _WatermarkFont; }
            set { _WatermarkFont = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
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

        private TextMarkup.BodyElement _TextMarkup = null;
        private void MarkupTextChanged()
        {
            _TextMarkup = null;

            if (!TextMarkup.MarkupParser.IsMarkup(ref _WatermarkText))
                return;

            _TextMarkup = TextMarkup.MarkupParser.Parse(_WatermarkText);
            ResizeMarkup();
        }

        private void ResizeMarkup()
        {
            if (_TextMarkup != null)
            {
                using (Graphics g = this.CreateGraphics())
                {
                    MarkupDrawContext dc = GetMarkupDrawContext(g);
                    _TextMarkup.Measure(GetWatermarkBounds().Size, dc);
                    Size sz = _TextMarkup.Bounds.Size;
                    _TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, sz), dc);
                }
            }
        }

        private Rectangle GetWatermarkBounds()
        {
            Rectangle r = this.ClientRectangle;
            r.Inflate(-1, 0);
            return r;
        }

        private MarkupDrawContext GetMarkupDrawContext(Graphics g)
        {
            return new MarkupDrawContext(g, (_WatermarkFont == null ? this.Font : _WatermarkFont), _WatermarkColor, this.RightToLeft == RightToLeft.Yes);
        }
        #endregion

        #region Masked Edit Property Forwarding
        /// <summary>
        /// Gets or sets a value indicating whether PromptChar can be entered as valid data by the user. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether PromptChar can be entered as valid data by the user")]
        public bool AllowPromptAsInput
        {
            get
            {
                return _MaskedTextBox.AllowPromptAsInput;
            }
            set
            {
                _MaskedTextBox.AllowPromptAsInput=value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the MaskedTextBox control accepts characters outside of the ASCII character set.
        /// <remarks>true if only ASCII is accepted; false if the MaskedTextBox control can accept any arbitrary Unicode character. The default is false.</remarks>
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether the MaskedTextBox control accepts characters outside of the ASCII character set"), RefreshProperties(RefreshProperties.Repaint)]
        public bool AsciiOnly
        {
            get
            {
                return _MaskedTextBox.AsciiOnly;
            }
            set
            {
                _MaskedTextBox.AsciiOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the masked text box control raises the system beep for each user key stroke that it rejects.
        /// </summary>
        [Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects"), DefaultValue(false), Category("Behavior")]
        public bool BeepOnError
        {
            get
            {
                return _MaskedTextBox.BeepOnError;
            }
            set
            {
                _MaskedTextBox.BeepOnError = value;
            }
        }

        /// <summary>
        /// Gets or sets the culture information associated with the masked text box.
        /// </summary>
        [Description("Indicates culture information associated with the masked text box."), Category("Behavior"), RefreshProperties(RefreshProperties.Repaint)]
        public CultureInfo Culture
        {
            get
            {
                return _MaskedTextBox.Culture;
            }
            set
            {
                _MaskedTextBox.Culture = value;
            }
        }
        /// <summary>
        /// Gets whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCulture()
        {
            return !CultureInfo.CurrentCulture.Equals(this.Culture);
        }

        /// <summary>
        /// Gets or sets a value that determines whether literals and prompt characters are copied to the clipboard.
        /// </summary>
        [DefaultValue(2), Category("Behavior"), Description("Indicates a value that determines whether literals and prompt characters are copied to the clipboard"), RefreshProperties(RefreshProperties.Repaint)]
        public MaskFormat CutCopyMaskFormat
        {
            get { return _MaskedTextBox.CutCopyMaskFormat; }
            set
            {
                _MaskedTextBox.CutCopyMaskFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the IFormatProvider to use when performing type validation.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IFormatProvider FormatProvider
        {
            get
            {
                return _MaskedTextBox.FormatProvider;
            }
            set
            {
                _MaskedTextBox.FormatProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the prompt characters in the input mask are hidden when the masked text box loses focus.
        /// </summary>
        [Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus."), RefreshProperties(RefreshProperties.Repaint), DefaultValue(false), Category("Behavior")]
        public bool HidePromptOnLeave
        {
            get
            {
                return _MaskedTextBox.HidePromptOnLeave;
            }
            set
            {
                _MaskedTextBox.HidePromptOnLeave = value;
            }
        }

        /// <summary>
        /// Gets or sets the text insertion mode of the masked text box control.
        /// </summary>
        [DefaultValue(0), Description("Indicates text insertion mode of the masked text box control."), Category("Behavior")]
        public InsertKeyMode InsertKeyMode
        {
            get
            {
                return _MaskedTextBox.InsertKeyMode;
            }
            set
            {
                _MaskedTextBox.InsertKeyMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the input mask to use at run time. The default value is the empty string which allows any input
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint), Editor("DevComponents.DotNetBar.Design.MaskAdvPropertyEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor)), Description("Indicates input mask to use at run time."), MergableProperty(false), Category("Behavior"), DefaultValue(""), Localizable(true)]
        public string Mask
        {
            get
            {
                return _MaskedTextBox.Mask;
            }
            set
            {
                _MaskedTextBox.Mask = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all required inputs have been entered into the input mask. Returns true if all required input has been entered into the mask; otherwise, false.
        /// </summary>
        [Browsable(false)]
        public bool MaskCompleted
        {
            get
            {
                return _MaskedTextBox.MaskCompleted;
            }
        }

        /// <summary>
        /// Gets a clone of the mask provider associated with this instance of the masked text box control. Returns masking language provider of type MaskedTextProvider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public MaskedTextProvider MaskedTextProvider
        {
            get
            {
                return _MaskedTextBox.MaskedTextProvider;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all required and optional inputs have been entered into the input mask. Returns true if all required and optional inputs have been entered; otherwise, false. 
        /// </summary>
        [Browsable(false)]
        public bool MaskFull
        {
            get
            {
                return _MaskedTextBox.MaskFull;
            }
        }

        /// <summary>
        /// Gets or sets the character to be displayed in substitute for user input.
        /// </summary>
        [DefaultValue('\0'), Category("Behavior"), RefreshProperties(RefreshProperties.Repaint), Description("Indicates character to be displayed in substitute for user input.")]
        public char PasswordChar
        {
            get
            {
                return _MaskedTextBox.PasswordChar;
            }
            set
            {
                _MaskedTextBox.PasswordChar = value;
            }
        }

        /// <summary>
        /// Gets or sets the character used to represent the absence of user input in MaskedTextBox. Returns character used to prompt the user for input. The default is an underscore (_). 
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue('_'), RefreshProperties(RefreshProperties.Repaint), Description("Indicates character used to represent the absence of user input in MaskedTextBox.")]
        public char PromptChar
        {
            get
            {
                return _MaskedTextBox.PromptChar;
            }
            set
            {
                _MaskedTextBox.PromptChar = value;
            }
        }

        /// <summary>
        /// Gets or sets whether text in control is read only. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether text in control is read only."), Category("Behavior")]
        public bool ReadOnly
        {
            get
            {
                return _MaskedTextBox.ReadOnly;
            }
            set
            {
                _MaskedTextBox.ReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parsing of user input should stop after the first invalid character is reached. Returns true if processing of the input string should be terminated at the first parsing error; otherwise, false if processing should ignore all errors. The default is false.
        /// </summary>
        [Description("Indicates whether the parsing of user input should stop after the first invalid character is reached."), DefaultValue(false), Category("Behavior")]
        public bool RejectInputOnFirstFailure
        {
            get
            {
                return _MaskedTextBox.RejectInputOnFirstFailure;
            }
            set
            {
                _MaskedTextBox.RejectInputOnFirstFailure = value;
            }
        }


        /// <summary>
        /// Gets or sets a value that determines how an input character that matches the prompt character should be handled. Returns true if the prompt character entered as input causes the current editable position in the mask to be reset; otherwise, false to indicate that the prompt character is to be processed as a normal input character. The default is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates value that determines how an input character that matches the prompt character should be handled.")]
        public bool ResetOnPrompt
        {
            get
            {
                return _MaskedTextBox.ResetOnPrompt;
            }
            set
            {
                _MaskedTextBox.ResetOnPrompt = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines how a space input character should be handled. Returns true if the space input character causes the current editable position in the mask to be reset; otherwise, false to indicate that it is to be processed as a normal input character. The default is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates value that determines how a space input character should be handled.")]
        public bool ResetOnSpace
        {
            get
            {
                return _MaskedTextBox.ResetOnSpace;
            }
            set
            {
                _MaskedTextBox.ResetOnSpace = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual string SelectedText
        {
            get
            {
                return _MaskedTextBox.SelectedText;
            }
            set
            {
                _MaskedTextBox.SelectedText = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectionLength
        {
            get
            {
                return _MaskedTextBox.SelectionLength;
            }
            set
            {
                _MaskedTextBox.SelectionLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting point of text selected in the text box.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int SelectionStart
        {
            get
            {
                return _MaskedTextBox.SelectionStart;
            }
            set
            {
                _MaskedTextBox.SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the defined shortcuts are enabled.
        /// </summary>
        [Description("Indicates whether the defined shortcuts are enabled."), Category("Behavior"), DefaultValue(true)]
        public virtual bool ShortcutsEnabled
        {
            get
            {
                return _MaskedTextBox.ShortcutsEnabled;
            }
            set
            {
                _MaskedTextBox.ShortcutsEnabled = value;
            }
        }
 
        [Description("MaskedTextBoxSkipLiterals"), Category("CatBehavior"), DefaultValue(true)]
        public bool SkipLiterals
        {
            get
            {
                return _MaskedTextBox.SkipLiterals;
            }
            set
            {
                _MaskedTextBox.SkipLiterals = value;
            }
        }

        /// <summary>
        /// Gets or sets the text as it is currently displayed to the user. 
        /// </summary>
        [Category("Appearance"), DefaultValue("Indicates text as it is currently displayed to the user"), Bindable(true), RefreshProperties(RefreshProperties.Repaint), Editor("System.Windows.Forms.Design.MaskedTextBoxTextEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                _MaskedTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets how text is aligned in a masked text box control.
        /// </summary>
        [DefaultValue(0), Localizable(true), Description("Indicates how text is aligned in a masked text box control."), Category("Appearance")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return _MaskedTextBox.TextAlign;
            }
            set
            {
                _MaskedTextBox.TextAlign = value;
            }
        }

        /// <summary>
        /// Gets the length of the displayed text. 
        /// </summary>
        [Browsable(false)]
        public virtual int TextLength
        {
            get
            {
                return _MaskedTextBox.TextLength;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether literals and prompt characters are included in the formatted string.
        /// </summary>
        [Category("Behavior"), RefreshProperties(RefreshProperties.Repaint), DefaultValue(2), Description("Indicates a value that determines whether literals and prompt characters are included in the formatted string.")]
        public MaskFormat TextMaskFormat
        {
            get
            {
                return _MaskedTextBox.TextMaskFormat;
            }
            set
            {
                _MaskedTextBox.TextMaskFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operating system-supplied password character should be used.
        /// </summary>
        [DefaultValue(false), RefreshProperties(RefreshProperties.Repaint), Category("Behavior"), Description("Indicates whether the operating system-supplied password character should be used")]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _MaskedTextBox.UseSystemPasswordChar;
            }
            set
            {
                _MaskedTextBox.UseSystemPasswordChar = value;
            }
        }

        /// <summary>
        /// Gets or sets the data type used to verify the data input by the user. Returns Type representing the data type used in validation. The default is null.
        /// </summary>
        [DefaultValue((string)null), Browsable(false)]
        public Type ValidatingType
        {
            get
            {
                return _MaskedTextBox.ValidatingType;
            }
            set
            {
                _MaskedTextBox.ValidatingType = value;
            }
        }

        /// <summary>
        /// Converts the user input string to an instance of the validating type.
        /// </summary>
        /// <returns>If successful, an Object of the type specified by the ValidatingType property; otherwise, null to indicate conversion failure.</returns>
        public object ValidateText()
        {
            return _MaskedTextBox.ValidateText();
        }

        /// <summary>
        /// Returns a string that represents the current masked text box. This method overrides ToString.
        /// </summary>
        /// <returns>A String that contains information about the current MaskedTextBox. The string includes the type, a simplified view of the input string, and the formatted input string.</returns>
        public override string ToString()
        {
            return _MaskedTextBox.ToString();
        }

        /// <summary>
        /// Gets the preferred height for a masked text box. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int PreferredHeight
        {
            get
            {
                int height = _MaskedTextBox.PreferredHeight;
                bool disposeStyle = false;
                ElementStyle style = GetBackgroundStyle(out disposeStyle);
                height += ElementStyleLayout.VerticalStyleWhiteSpace(style);
                if (disposeStyle)
                {
                    style.Dispose();
                }
                return height;
            }
        }

        public override Size  GetPreferredSize(Size proposedSize)
        {
            Size ps = _MaskedTextBox.GetPreferredSize(proposedSize);
            ps.Height = PreferredHeight;
 	        return ps;
        }
        #endregion

        #region Masked Edit Custom
        private class MaskedTextBoxInternal : MaskedTextBox
        {
            private bool _IgnoreFocus = false;
            private bool _Focused = false;
            private Color _LastBackColor = Color.Empty;

            private bool RenderWatermark
            {
                get
                {
                    MaskedTextBoxAdv parent = MaskedTextBoxAdv;
                    if (parent == null) return false;

                    if (!parent.WatermarkEnabled)
                        return false;
                    if (parent.WatermarkBehavior == eWatermarkBehavior.HideOnFocus)
                        return !_Focused && this.Enabled && this.Text != null && this.Text.Length == 0 && parent.WatermarkText.Length > 0;
                    else
                        return this.Enabled && this.Text != null && this.Text.Length == 0 && parent.WatermarkText.Length > 0;
                }
            }

            private void DrawWatermark()
            {
                MaskedTextBoxAdv parent = MaskedTextBoxAdv;
                if (parent == null) return;

                using (Graphics g = this.CreateGraphics())
                {
                    if (parent._TextMarkup != null)
                    {
                        MarkupDrawContext dc = parent.GetMarkupDrawContext(g);
                        parent._TextMarkup.Render(dc);
                    }
                    else
                    {
                        eTextFormat format = eTextFormat.Left;

                        if (this.RightToLeft == RightToLeft.Yes) format |= eTextFormat.RightToLeft;
                        if (this.TextAlign == HorizontalAlignment.Left)
                            format |= eTextFormat.Left;
                        else if (this.TextAlign == HorizontalAlignment.Right)
                            format |= eTextFormat.Right;
                        else if (this.TextAlign == HorizontalAlignment.Center)
                            format |= eTextFormat.HorizontalCenter;
                        format |= eTextFormat.EndEllipsis;
                        format |= eTextFormat.WordBreak;
                        TextDrawing.DrawString(g, parent.WatermarkText, (parent.WatermarkFont == null ? this.Font : parent.WatermarkFont),
                            parent.WatermarkColor, parent.GetWatermarkBounds(), format);
                    }
                }
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);

                MaskedTextBoxAdv parent = MaskedTextBoxAdv;
                if (parent == null) return;
                if (parent.WatermarkText.Length > 0)
                    this.Invalidate();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                MaskedTextBoxAdv parent = MaskedTextBoxAdv;
                if (parent == null) return;

                switch (m.Msg)
                {
                    case (int)WinApi.WindowsMessages.WM_SETFOCUS:
                        {
                            if (_IgnoreFocus)
                            {
                                _IgnoreFocus = false;
                            }
                            else
                            {
                                _Focused = true;
                                if (parent.FocusHighlightEnabled && this.Enabled)
                                {
                                    _LastBackColor = this.BackColor;
                                    this.BackColor = parent.FocusHighlightColor;
                                    parent.BackColor = parent.FocusHighlightColor;
                                    parent.Invalidate(true);
                                }
                            }
                            break;
                        }
                    case (int)WinApi.WindowsMessages.WM_KILLFOCUS:
                        {
                            if (!_Focused)
                            {
                                _IgnoreFocus = true;
                            }
                            else
                            {
                                _Focused = false;
                                if (this.Text == null || this.Text.Length == 0)
                                    parent.Invalidate(true);
                                if (parent.FocusHighlightEnabled && !_LastBackColor.IsEmpty)
                                {
                                    this.BackColor = _LastBackColor;
                                    parent.BackColor = _LastBackColor;
                                    parent.Invalidate(true);
                                }
                            }
                            break;
                        }
                    case (int)WinApi.WindowsMessages.WM_PAINT:
                        {
                            if (RenderWatermark)
                                DrawWatermark();
                            break;
                        }
                }
            }

            private MaskedTextBoxAdv MaskedTextBoxAdv
            {
                get
                {
                    return Parent as MaskedTextBoxAdv;
                }
            }
        }
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion
    }
}
#endif