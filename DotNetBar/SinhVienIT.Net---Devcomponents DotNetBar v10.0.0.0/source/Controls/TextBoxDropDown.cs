#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.Editors;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Drawing.Design;
using System.Security.Permissions;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents single line text box control with the drop down button to display custom control on popup and additional custom buttons. 
    /// </summary>
    [ToolboxBitmap(typeof(TextBoxDropDown), "Controls.TextBoxDropDown.ico"), ToolboxItem(true), DefaultProperty("Text"), DefaultBindingProperty("Text"), DefaultEvent("TextChanged"), Designer("DevComponents.DotNetBar.Design.TextBoxDropDownDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class TextBoxDropDown : PopupItemControl, IInputButtonControl, ICommandSource
    {
        #region Private Variables
        private TextBoxX _TextBox = null;
        private ButtonItem _PopupItem = null;
        private static string _DropDownItemContainerName = "sysPopupItemContainer";
        private static string _DropDownControlContainerName = "sysPopupControlContainer";
        private Color _FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        private bool _FocusHighlightEnabled = false;
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
        /// Occurs when the text alignment is changed.
        /// </summary>
        [Description("Occurs when the text alignment is changed.")]
        public event EventHandler TextAlignChanged;
        /// <summary>
        /// Occurs when the value of the Modified property has changed.
        /// </summary>
        public event EventHandler ModifiedChanged;
        #endregion

        #region Constructor
         /// <summary>
        /// Initializes a new instance of the TextBoxDropDown class.
        /// </summary>
        public TextBoxDropDown()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            _TextBox = new TextBoxX();
            base.BackColor = SystemColors.Window;
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

            _TextBox.BorderStyle = BorderStyle.None;
            _TextBox.TextChanged += new EventHandler(TextBoxTextChanged);
            _TextBox.TextAlignChanged += new EventHandler(TextBoxTextAlignChanged);
            _TextBox.SizeChanged += new EventHandler(TextBoxSizeChanged);
            _TextBox.ModifiedChanged += new EventHandler(TextBoxModifiedChanged);
            _TextBox.KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
            this.Controls.Add(_TextBox);
        }
        #endregion

        #region Internal Implementation
        protected override void OnBindingContextChanged(EventArgs e)
        {
            if (_DropDownControl != null) _DropDownControl.BindingContext = this.BindingContext;
            base.OnBindingContextChanged(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F4)
            {
                if (_PopupItem.Expanded)
                    CloseDropDown();
                else
                    InvokeShowDropDown();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_InternalReadOnly) e.Handled = true;
        }

        private void TextBoxModifiedChanged(object sender, EventArgs e)
        {
            OnModifiedChanged(e);
        }
        protected virtual void OnModifiedChanged(EventArgs e)
        {
            EventHandler handler = ModifiedChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void TextBoxSizeChanged(object sender, EventArgs e)
        {
            if (!_InternalSizeUpdate)
                UpdateLayout();
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            base.Text = _TextBox.Text;
            ExecuteCommand();
        }

        private void TextBoxTextAlignChanged(object sender, EventArgs e)
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

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!this.Enabled)
                _TextBox.ResetBackColor();
            else if (_TextBox.BackColor != this.BackColor)
                _TextBox.BackColor = this.BackColor;
                
            base.OnEnabledChanged(e);
        }

        private Color _OldBackColor = Color.Empty;
        protected override void OnEnter(EventArgs e)
        {
            if (_FocusHighlightEnabled)
            {
                _OldBackColor = this.BackColor;
                this.BackColor = _FocusHighlightColor;
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (_FocusHighlightEnabled && !_OldBackColor.IsEmpty)
            {
                this.BackColor = _OldBackColor;
                if (_OldBackColor == SystemColors.Window)
                    _TextBox.ResetBackColor();
                _OldBackColor = Color.Empty;
            }
            base.OnLeave(e);
        }

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
                    //_TextBox.FocusHighlightEnabled = value;
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
                    _TextBox.FocusHighlightColor = value;
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

        internal VisualGroup ButtonGroup
        {
            get { return (_ButtonGroup); }
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
            int styleSpace = (ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(style, eSpacePart.Border));
            if (styleSpace > 0) styleSpace += 2;
            s.Height -=  styleSpace;
            styleSpace = (ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border));
            if (styleSpace > 0) styleSpace += 2;
            s.Width -= styleSpace;
            p.AvailableSize = s;
            p.ParentEnabled = this.Enabled;
            p.MouseOver = _MouseOver || this.Focused;
            if(disposeStyle) style.Dispose();
            return p;
        }

        private ElementStyle GetBackgroundStyle(out bool disposeStyle)
        {
            disposeStyle = false;
            _BackgroundStyle.SetColorScheme(this.GetColorScheme());
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
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(brush, clientRect);
            }

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);

            if (style.Custom)
            {
                SmoothingMode sm = g.SmoothingMode;
                if (this.AntiAlias)
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
            if (_TextBox.Parent != null)
                _TextBox.BackColor = this.BackColor;
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
            _ButtonGroup.RenderBounds = GetButtonsRenderBounds(style);
            _ButtonGroup.ProcessPaint(p);
            if(disposeStyle) style.Dispose();
        }

        private Rectangle GetButtonsRenderBounds(ElementStyle style)
        {
            int y = ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border);
            if (y > 0) y += 1;

            if (this.RightToLeft == RightToLeft.Yes)
            {
                return new Rectangle((ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1), y,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
            }

            return new Rectangle(this.Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, y,
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
            _TextBox.Focus();
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
            using (Graphics g = BarFunctions.CreateGraphics(this))
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
                Rectangle textBoxControlRect = ElementStyleLayout.GetInnerRect(style, this.ClientRectangle);
                if (RenderButtons)
                {
                    Rectangle buttonsRect = GetButtonsRenderBounds(style);
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        textBoxControlRect.X += buttonsRect.Right;
                        textBoxControlRect.Width -= buttonsRect.Right;
                    }
                    else
                    {
                        textBoxControlRect.Width -= (textBoxControlRect.Right - buttonsRect.X);
                    }
                }
                if (_TextBox.Multiline == false && (_TextBox.PreferredHeight < textBoxControlRect.Height))
                {
                    textBoxControlRect.Y += (textBoxControlRect.Height - _TextBox.PreferredHeight) / 2;
                }
                _TextBox.Bounds = textBoxControlRect;

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

            InvokeShowDropDown();
        }

        private void InvokeShowDropDown()
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonDropDownClick(cancelArgs);
            if (cancelArgs.Cancel) return;
            ShowDropDown();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop-down is open.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPopupOpen
        {
            get
            {
                return _PopupItem.Expanded;
            }
            set
            {
                if (value != _PopupItem.Expanded)
                {
                    if (value)
                        ShowDropDown();
                    else
                        CloseDropDown();
                }
            }
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

                if (_PopupItem.SubItems.Count == 0)
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

            ItemContainer ic = null;
            if (_PopupItem.SubItems.Contains(_DropDownItemContainerName))
                ic = _PopupItem.SubItems[_DropDownItemContainerName] as ItemContainer;
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
                ic.Dispose();
            }
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonClearClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            _TextBox.Text = "";
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
                OnDropDownControlChanged();
            }
        }
        /// <summary>
        /// Called when DropDownControl property has changed.
        /// </summary>
        protected virtual void OnDropDownControlChanged()
        {
            if (_DropDownControl != null)
            {
                _DropDownControl.BindingContext = this.BindingContext;
                if (!this.DesignMode)
                    _DropDownControl.Visible = false;
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

        internal void ProcessMouseUpOnGroup()
        {
            if (RenderButtons)
                _ButtonGroup.ProcessMouseUp(new MouseEventArgs(MouseButtons.Left, 0, -10, -10, 0));
            this.Invalidate();
        }

        /// <summary>
        /// Gets the reference to internal TextBox control. Use it to get access to the text box events and properties.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBoxX TextBox
        {
            get
            {
                return _TextBox;
            }
        }

        private bool _InternalReadOnly = false;
        internal bool InternalReadOnly
        {
            get { return _InternalReadOnly; }
            set
            {
                _InternalReadOnly = value;
            }
        }
        

        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return _TextBox.WatermarkEnabled; }
            set { _TextBox.WatermarkEnabled = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return _TextBox.WatermarkText; }
            set
            {
                if (value == null) value = "";
                _TextBox.WatermarkText = value;
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return _TextBox.WatermarkFont; }
            set { _TextBox.WatermarkFont = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return _TextBox.WatermarkColor; }
            set { _TextBox.WatermarkColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return _TextBox.WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return _TextBox.WatermarkBehavior; }
            set { _TextBox.WatermarkBehavior = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark image displayed inside of the control when Text is not set and control does not have input focus.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
        public Image WatermarkImage
        {
            get { return _TextBox.WatermarkImage; }
            set { _TextBox.WatermarkImage = value; }
        }
        /// <summary>
        /// Gets or sets the watermark image alignment.
        /// </summary>
        [DefaultValue(ContentAlignment.MiddleLeft), Category("Appearance"), Description("Indicates watermark image alignment.")]
        public ContentAlignment WatermarkImageAlignment
        {
            get { return _TextBox.WatermarkImageAlignment; }
            set { _TextBox.WatermarkImageAlignment = value; }
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

        #region TextBox Property Forwarding
        /// <summary>
        /// Gets or sets the text as it is currently displayed to the user. 
        /// </summary>
        [Category("Appearance"), DefaultValue("Indicates text as it is currently displayed to the user"), Bindable(true), RefreshProperties(RefreshProperties.Repaint), Localizable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                _TextBox.Text = value;
            }
        }


        /// <summary>
        /// Returns a string that represents the current text in text box. This method overrides ToString.
        /// </summary>
        /// <returns>A String that contains information about the current TextBox. The string includes the type, a simplified view of the input string, and the formatted input string.</returns>
        public override string ToString()
        {
            return _TextBox.ToString();
        }

        /// <summary>
        /// Gets the preferred height for a text box. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int PreferredHeight
        {
            get
            {
                int height = _TextBox.PreferredHeight;
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

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size ps = _TextBox.GetPreferredSize(proposedSize);
            ps.Height = PreferredHeight;
            return ps;
        }

        /// <summary>
        /// Gets or sets a custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.
        /// <value>A StringCollection to use with AutoCompleteSource.</value>
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Localizable(true), Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource."), Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get
            {
                return _TextBox.AutoCompleteCustomSource;
            }
            set
            {
                _TextBox.AutoCompleteCustomSource = value;
            }
        }

        /// <summary>
        /// Gets or sets an option that controls how automatic completion works for the TextBox.
        /// <value>One of the values of AutoCompleteMode. The values are Append, None, Suggest, and SuggestAppend. The default is None.</value>
        /// </summary>
        [Description("Gets or sets an option that controls how automatic completion works for the TextBox."), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0)]
        public AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return _TextBox.AutoCompleteMode;
            }
            set
            {
                _TextBox.AutoCompleteMode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the source of complete strings used for automatic completion.
        /// <value>One of the values of AutoCompleteSource. The options are AllSystemSources, AllUrl, FileSystem, HistoryList, RecentlyUsedList, CustomSource, and None. The default is None.</value>
        /// </summary>
        [DefaultValue(0x80), TypeConverter(typeof(TextBoxAutoCompleteSourceConverter)), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
        public AutoCompleteSource AutoCompleteSource
        {
            get
            {
                return _TextBox.AutoCompleteSource;
            }
            set
            {
                _TextBox.AutoCompleteSource = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the TextBox control modifies the case of characters as they are typed.
        /// <value>One of the CharacterCasing enumeration values that specifies whether the TextBox control modifies the case of characters. The default is CharacterCasing.Normal.</value>
        /// </summary>
        [Category("Behavior"), Description("Indicates whether the TextBox control modifies the case of characters as they are typed."), DefaultValue(0)]
        public CharacterCasing CharacterCasing
        {
            get
            {
                return _TextBox.CharacterCasing;
            }
            set
            {
                _TextBox.CharacterCasing = value;
            }
        }

        /// <summary>
        /// Gets or sets the character used to mask characters of a password in a single-line TextBox control.
        /// <value>The character used to mask characters entered in a single-line TextBox control. Set the value of this property to 0 (character value) if you do not want the control to mask characters as they are typed. Equals 0 (character value) by default.</value>
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint), Localizable(true), Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control."), Category("Behavior"), DefaultValue('\0')]
        public char PasswordChar
        {
            get
            {
                return _TextBox.PasswordChar;
            }
            set
            {
                _TextBox.PasswordChar = value;
            }
        }

        /// <summary>
        /// Gets or sets how text is aligned in a TextBox control.
        /// <value>One of the HorizontalAlignment enumeration values that specifies how text is aligned in the control. The default is HorizontalAlignment.Left.</value>
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Localizable(true), Description("Indicates how text is aligned in a TextBox control.")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return _TextBox.TextAlign;
            }
            set
            {
                _TextBox.TextAlign = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.
        /// <value>true if the text in the TextBox control should appear as the default password character; otherwise, false.</value>
        /// </summary>
        [Category("Behavior"), DefaultValue(false), Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character."), RefreshProperties(RefreshProperties.Repaint)]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _TextBox.UseSystemPasswordChar;
            }
            set
            {
                _TextBox.UseSystemPasswordChar = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.
        /// <value>true if the selected text does not appear highlighted when the text box control loses focus; false, if the selected text remains highlighted when the text box control loses focus. The default is true.</value>
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
        public bool HideSelection
        {
            get
            {
                return _TextBox.HideSelection;
            }
            set
            {
                _TextBox.HideSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or paste into the text box control.
        /// <value>The number of characters that can be entered into the control. The default is 32767.</value>
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the maximum number of characters the user can type or paste into the text box control."), DefaultValue(0x7fff), Localizable(true)]
        public virtual int MaxLength
        {
            get
            {
                return _TextBox.MaxLength;
            }
            set
            {
                _TextBox.MaxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates that the text box control has been modified by the user since the control was created or its contents were last set.
        /// <value>true if the control's contents have been modified; otherwise, false. The default is false.</value>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Modified
        {
            get
            {
                return _TextBox.Modified;
            }
            set
            {
                _TextBox.Modified = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only.
        /// <value>true if the text box is read-only; otherwise, false. The default is false.</value>
        /// </summary>
        [DefaultValue(false), RefreshProperties(RefreshProperties.Repaint), Category("Behavior"), Description("Gets or sets a value indicating whether text in the text box is read-only.")]
        public bool ReadOnly
        {
            get
            {
                return _TextBox.ReadOnly;
            }
            set
            {
                _TextBox.ReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// <value>A string that represents the currently selected text in the text box.</value>
        /// </summary>
        [Description("Gets or sets a value indicating the currently selected text in the control."), Browsable(false), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedText
        {
            get
            {
                return _TextBox.SelectedText;
            }
            set
            {
                _TextBox.SelectedText = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// <value>The number of characters selected in the text box.</value>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets the number of characters selected in the text box."), Category("Appearance")]
        public virtual int SelectionLength
        {
            get
            {
                return _TextBox.SelectionLength;
            }
            set
            {
                _TextBox.SelectionLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting point of text selected in the text box.
        /// <value>The starting position of text selected in the text box.</value>
        /// </summary>
        [Description("Gets or sets the starting point of text selected in the text box."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Browsable(false)]
        public int SelectionStart
        {
            get
            {
                return _TextBox.SelectionStart;
            }
            set
            {
                _TextBox.SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets the length of text in the control.Returns number of characters contained in the text of the control.
        /// </summary>
        [Browsable(false)]
        public virtual int TextLength
        {
            get
            {
                return _TextBox.TextLength;
            }
        }

        /// <summary>
        /// Appends text to the current text of a text box.
        /// </summary>
        /// <param name="text">The text to append to the current contents of the text box. </param>
        public void AppendText(string text)
        {
            _TextBox.AppendText(text);
        }

        /// <summary>
        /// Clears all text from the text box control.
        /// </summary>
        public void Clear()
        {
            _TextBox.Clear();
        }

        /// <summary>
        /// Clears information about the most recent operation from the undo buffer of the text box.
        /// </summary>
        public void ClearUndo()
        {
            _TextBox.ClearUndo();
        }

        /// <summary>
        /// Copies the current selection in the text box to the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Copy()
        {
            _TextBox.Copy();
        }
        /// <summary>
        /// Moves the current selection in the text box to the Clipboard.
        /// </summary>
        public void Cut()
        {
            _TextBox.Cut();
        }
        /// <summary>
        /// Specifies that the value of the SelectionLength property is zero so that no characters are selected in the control.
        /// </summary>
        public void DeselectAll()
        {
            _TextBox.DeselectAll();
        }
        /// <summary>
        /// Retrieves the character that is closest to the specified location within the control.
        /// </summary>
        /// <param name="pt">The location from which to seek the nearest character. </param>
        /// <returns>The character at the specified location.</returns>
        public virtual char GetCharFromPosition(Point pt)
        {
            return _TextBox.GetCharFromPosition(pt);
        }
        /// <summary>
        /// Retrieves the index of the character nearest to the specified location.
        /// </summary>
        /// <param name="pt">The location to search.</param>
        /// <returns>The zero-based character index at the specified location.</returns>
        public virtual int GetCharIndexFromPosition(Point pt)
        {
            return _TextBox.GetCharIndexFromPosition(pt);
        }
        /// <summary>
        /// Retrieves the index of the first character of a given line.
        /// </summary>
        /// <param name="lineNumber">The line for which to get the index of its first character. </param>
        /// <returns>The zero-based character index in the specified line.</returns>
        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return _TextBox.GetFirstCharIndexFromLine(lineNumber);
        }
        /// <summary>
        /// Retrieves the index of the first character of the current line.
        /// </summary>
        /// <returns>The zero-based character index in the current line.</returns>
        public int GetFirstCharIndexOfCurrentLine()
        {
            return _TextBox.GetFirstCharIndexOfCurrentLine();
        }
        /// <summary>
        /// Retrieves the line number from the specified character position within the text of the control.
        /// </summary>
        /// <param name="index">The character index position to search. </param>
        /// <returns>The zero-based line number in which the character index is located.</returns>
        public virtual int GetLineFromCharIndex(int index)
        {
            return _TextBox.GetLineFromCharIndex(index);
        }

        /// <summary>
        /// Retrieves the location within the control at the specified character index.
        /// </summary>
        /// <param name="index">The index of the character for which to retrieve the location. </param>
        /// <returns>The location of the specified character.</returns>
        public virtual Point GetPositionFromCharIndex(int index)
        {
            return _TextBox.GetPositionFromCharIndex(index);
        }

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Paste()
        {
            _TextBox.Paste();
        }

        /// <summary>
        /// Selects a range of text in the text box.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within the text box. </param>
        /// <param name="length">The number of characters to select. </param>
        public void Select(int start, int length)
        {
            _TextBox.Select(start, length);
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            _TextBox.SelectAll();
        }

        /// <summary>
        /// Undoes the last edit operation in the text box.
        /// </summary>
        public void Undo()
        {
            _TextBox.Undo();
        }

        /// <summary>
        /// Replaces the specified selection in the TextBox with the contents of the Clipboard.
        /// </summary>
        /// <param name="text">The text to replace.</param>
        public void Paste(string text)
        {
            _TextBox.Paste(text);
        }

        #endregion
    }

    #region TextBoxAutoCompleteSourceConverter
    internal class TextBoxAutoCompleteSourceConverter : EnumConverter
    {
        // Methods
        public TextBoxAutoCompleteSourceConverter(Type type)
            : base(type)
        {
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
            ArrayList values = new ArrayList();
            int count = standardValues.Count;
            for (int i = 0; i < count; i++)
            {
                if (!standardValues[i].ToString().Equals("ListItems"))
                {
                    values.Add(standardValues[i]);
                }
            }
            return new TypeConverter.StandardValuesCollection(values);
        }
    }
    #endregion
}
#endif