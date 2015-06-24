#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using System.Drawing;

namespace DevComponents.Editors
{
    /// <summary>
    /// Represents an control that allows numeric input.
    /// </summary>
    [ToolboxItem(false), DefaultEvent("ValueChanged")]
    public abstract class NumericInputBase : VisualControlBase, ICommandSource
    {
        #region Private Variables
        private VisualInputGroup _InputGroup = null;
        private ButtonItem _PopupItem = null;
        private static string _DropDownControlContainerName = "sysPopupControlContainer";
        private static string _DropDownItemContainerName = "sysPopupItemContainer";
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the Value or IsEmpty property changes.
        /// <remarks>
        /// This event is not raised when the entered date is earlier than MinDateTime or later than MaxDateTime.
        /// </remarks>
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Occurs when the Value or IsEmpty property changes. This event occurs at the same time and has same function as ValueChanged event. It is provided for binding support.
        /// </summary>
        public event EventHandler ValueObjectChanged;
        /// <summary>
        /// Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.
        /// </summary>
        public event CancelEventHandler ButtonClearClick;
        /// <summary>
        /// Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.
        /// </summary>
        public event CancelEventHandler ButtonDropDownClick;

        /// <summary>
        /// Occurs when ShowCheckBox property is set to true and user changes the lock status of the control by clicking the check-box.
        /// </summary>
        public event EventHandler LockUpdateChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericInputBase class.
        /// </summary>
        public NumericInputBase()
            : base()
        {
            _ButtonClear = new InputButtonSettings(this);
            _ButtonDropDown = new InputButtonSettings(this);
            _ButtonFreeText = new InputButtonSettings(this);

            _InputGroup = this.RootVisualItem as VisualInputGroup;
            _InputGroup.AutoOverwrite = false;
            this.InputHorizontalAlignment = eHorizontalAlignment.Right;
        }
        #endregion

        #region Internal Implementation
        protected virtual void UpdateInputFieldAlignment()
        {
            
        }
        protected override void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
        {
            UpdateInputFieldAlignment();
            base.OnInputButtonSettingsChanged(inputButtonSettings);
        }
        protected override void OnInputHorizontalAlignmentChanged(eHorizontalAlignment oldValue, eHorizontalAlignment newValue)
        {
            UpdateInputFieldAlignment();
            base.OnInputHorizontalAlignmentChanged(oldValue, newValue);
        }
        /// <summary>
        /// Gets or sets whether input part of the control is read-only. When set to true the input part of the control becomes
        /// read-only and does not allow the typing. However, drop-down part if visible still allows user to possibly change the value of the control
        /// through the method you can provide on drop-down.
        /// Use this property to allow change of the value through drop-down button only.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether input part of the control is read-only.")]
        public bool IsInputReadOnly
        {
            get { return _InputGroup.IsReadOnly; }
            set
            {
                _InputGroup.IsReadOnly = value;
            }
        }

        private bool _ShowCheckBox = false;
        /// <summary>
        /// Gets or sets a value indicating whether a check box is displayed to the left of the input value.
        /// Set to true if a check box is displayed to the left of the input value; otherwise, false. The default is false.
        /// <remarks>
        /// When the ShowCheckBox property is set to true, a check box is displayed to the left of the input in the control. When the check box is selected, the value can be updated. When the check box is cleared, the value is unable to be changed.
        /// You can handle the LockUpdateChanged event to be notified when this check box is checked and unchecked. Use LockUpdateChecked property 
        /// to get or sets whether check box is checked.
        /// </remarks>
        /// </summary>
        [DefaultValue(false), Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
        public bool ShowCheckBox
        {
            get { return _ShowCheckBox; }
            set
            {
                _ShowCheckBox = value;
                OnShowCheckBoxChanged();
            }
        }

        protected virtual void OnShowCheckBoxChanged()
        {
            UpdateInputFieldAlignment();

            LockUpdateCheckBox checkBox = LockUpdateCheckBox;
            if (checkBox != null)
            {
                _InputGroup.Items.Remove(checkBox);
                checkBox.CheckedChanged -= new EventHandler(LockCheckedChanged);
            }

            if (this.ShowCheckBox)
            {
                if (!(_InputGroup.Items[0] is LockUpdateCheckBox))
                {
                    checkBox = new LockUpdateCheckBox();
                    checkBox.ItemType = eSystemItemType.SystemButton;
                    checkBox.CheckedChanged += new EventHandler(LockCheckedChanged);
                    _InputGroup.Items.Insert(0, checkBox);
                    checkBox.Checked = _LockUpdateChecked;
                }
            }
        }

        private LockUpdateCheckBox LockUpdateCheckBox
        {
            get
            {
                if (_InputGroup.Items[0] is LockUpdateCheckBox)
                    return (LockUpdateCheckBox)_InputGroup.Items[0];
                return null;
            }
        }

        private void LockCheckedChanged(object sender, EventArgs e)
        {
            LockUpdateCheckBox checkBox = LockUpdateCheckBox;
            if (checkBox != null)
                _LockUpdateChecked = checkBox.Checked;

            OnLockUpdateChanged(e);
        }

        private bool _LockUpdateChecked = true;
        /// <summary>
        /// Gets or sets whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
        public bool LockUpdateChecked
        {
            get { return _LockUpdateChecked; }
            set
            {
                if (_LockUpdateChecked != value)
                {
                    _LockUpdateChecked = value;
                    LockUpdateCheckBox checkBox = LockUpdateCheckBox;
                    if (checkBox != null)
                        checkBox.Checked = _LockUpdateChecked;
                }
            }
        }

        /// <summary>
        /// Raises the LockUpdateChanged event.
        /// </summary>
        /// <param name="e">Provides event data./</param>
        protected virtual void OnLockUpdateChanged(EventArgs e)
        {
            if (LockUpdateChanged != null)
                LockUpdateChanged(this, e);
            if (_FreeTextEntryBox != null) _FreeTextEntryBox.ReadOnly = !this.LockUpdateChecked;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a spin button control (up-down control) is used to adjust the current value. The default is false. 
        /// <remarks>
        /// When the ShowUpDown property is set to true, a spin button control is shown to adjust value of currently focused input item. 
        /// The value can be adjusted by using the up and down buttons to change the value.
        /// </remarks>
        /// </summary>
        private bool _ShowUpDown = false;
        [DefaultValue(false)]
        public bool ShowUpDown
        {
            get { return _ShowUpDown; }
            set
            {
                _ShowUpDown = value;
                OnShowUpDownChanged();
            }
        }

        protected override void RecreateButtons()
        {
            base.RecreateButtons();
            OnShowUpDownChanged();
            OnShowCheckBoxChanged();
        }

        protected virtual void OnShowUpDownChanged()
        {
            UpdateInputFieldAlignment();

            for (int i = _InputGroup.Items.Count - 1; i >= 0; i--)
            {
                if (_InputGroup.Items[i] is VisualUpDownButton)
                {
                    VisualUpDownButton upDownButton = (VisualUpDownButton)_InputGroup.Items[i];
                    _InputGroup.Items.RemoveAt(i);
                    //upDownButton.UpClick -= new EventHandler(UpDownButtonUpClick);
                    //upDownButton.DownClick -= new EventHandler(UpDownButtonDownClick);
                    break;
                }
            }

            if (this.ShowUpDown)
            {
                VisualUpDownButton upDownButton = new VisualUpDownButton();
                upDownButton.Alignment = eItemAlignment.Right;
                upDownButton.AutoChange = eUpDownButtonAutoChange.FocusedItem;
                upDownButton.ItemType = eSystemItemType.SystemButton;
                //upDownButton.UpClick += new EventHandler(UpDownButtonUpClick);
                //upDownButton.DownClick += new EventHandler(UpDownButtonDownClick);
                upDownButton.Enabled = this.LockUpdateChecked;
                _InputGroup.Items.Add(upDownButton);
            }
        }

        //private void UpDownButtonDownClick(object sender, EventArgs e)
        //{
        //    if (this.FreeTextEntryMode && _FreeTextEntryBox.Focused)
        //        DecreaseValue();
        //}
        //private void UpDownButtonUpClick(object sender, EventArgs e)
        //{
        //    if (this.FreeTextEntryMode && _FreeTextEntryBox.Focused)
        //        IncreaseValue();
        //}

        /// <summary>
        /// Decreases value of the control.
        /// </summary>
        public abstract void DecreaseValue();
        /// <summary>
        /// Increases the value of the control.
        /// </summary>
        public abstract void IncreaseValue();

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

        private InputButtonSettings _ButtonFreeText = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that switches the control into the free-text entry mode when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonFreeText
        {
            get
            {
                return _ButtonFreeText;
            }
        }


        protected override VisualItem CreateButton(InputButtonSettings buttonSettings)
        {
            VisualItem item = null;
            if (buttonSettings == _ButtonDropDown)
            {
                item = new VisualDropDownButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }
            else
                item = base.CreateButton(buttonSettings);

            VisualButton button = item as VisualButton;
            button.ClickAutoRepeat = false;

            if (buttonSettings == _ButtonClear)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.DateReset.png");
            }
            else if (buttonSettings == _ButtonFreeText)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.FreeText.png");
                button.Checked = buttonSettings.Checked;
            }

            return item;
        }

        private string _DisplayFormat = "";
        /// <summary>
        /// Gets or sets the Numeric String Format that is used to format the numeric value entered for display purpose. Read more about
        /// available formats in MSDN under "Standard Numeric Format Strings" and "Custom Numeric Format Strings" topics.
        /// <remarks>
        /// The format specified here indicates the format for display purpose only, not for the input purpose.
        /// For example to display the number in system Currency format set the DisplayFormat to 'C'.
        /// </remarks>
        /// </summary>
        [DefaultValue(""), Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
        public string DisplayFormat
        {
            get { return _DisplayFormat; }
            set
            {
                if (value != null)
                {
                    int i = 12;
                    string s = i.ToString(value);
                    _DisplayFormat = value;
                    OnDisplayFormatChanged();
                    this.Invalidate();
                    RootVisualItem.InvalidateArrange();   
                }
            }
        }

        protected abstract void OnDisplayFormatChanged();

        /// <summary>
        /// Gets or sets whether empty null/nothing state of the control is allowed. Default value is true which means that IsEmpty property
        /// may return true if input value is resets or ValueObject set to null/nothing.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether empty null/nothing state of the control is allowed.")]
        public bool AllowEmptyState
        {
            get { return _InputGroup.AllowEmptyState; }
            set { _InputGroup.AllowEmptyState = value; this.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets whether control is empty i.e. it does not hold a valid value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return _InputGroup.IsEmpty; }
            set { _InputGroup.IsEmpty = value; }
        }

        protected override System.Collections.SortedList CreateSortedButtonList()
        {
            SortedList list = base.CreateSortedButtonList();
            if (_ButtonClear.Visible)
            {
                VisualItem button = CreateButton(_ButtonClear);
                if (_ButtonClear.ItemReference != null)
                    _ButtonClear.ItemReference.Click -= new EventHandler(ClearButtonClick);
                _ButtonClear.ItemReference = button;
                button.Click += new EventHandler(ClearButtonClick);
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
            if (_ButtonFreeText.Visible)
            {
                VisualItem button = CreateButton(_ButtonFreeText);
                if (_ButtonFreeText.ItemReference != null)
                    _ButtonFreeText.ItemReference.Click -= new EventHandler(FreeTextButtonClick);
                _ButtonFreeText.ItemReference = button;
                button.Click += FreeTextButtonClick;
                list.Add(_ButtonFreeText, button);
            }

            return list;
        }

        private Control _PreviousDropDownControlParent = null;
        private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150)
            {
                _CloseTime = DateTime.MinValue;
                return;
            }

            ShowDropDown();
        }

        /// <summary>
        /// Shows drop-down popup. Note that popup will be shown only if there is a DropDownControl assigned or DropDownItems collection has at least one item.
        /// </summary>
        public void ShowDropDown()
        {
            if (_DropDownControl == null && _PopupItem.SubItems.Count == 0)
                return;
            if (_PopupItem.Expanded)
            {
                _PopupItem.Expanded = false;
                return;
            }

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

            _PreviousDropDownControlParent = _DropDownControl.Parent;
            cc.Control = _DropDownControl;

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

        private void ClearButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonClearClick(cancelArgs);
            if (cancelArgs.Cancel) return;
            this.IsEmpty = true;
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

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (IsInitializing) return;

            if (ValueChanged != null)
                ValueChanged(this, e);
            if (ValueObjectChanged != null)
                ValueObjectChanged(this, e);
            ExecuteCommand();
        }

        /// <summary>
        /// Gets or sets the input field alignment inside the control
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalAlignment.Right), Category("Appearance"), Description("Indicates alignment of input fields inside of the control.")]
        public override eHorizontalAlignment InputHorizontalAlignment
        {
            get
            {
                return base.InputHorizontalAlignment;
            }
            set
            {
                base.InputHorizontalAlignment = value;
            }
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

        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input is enabled. When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether auto-overwrite functionality for input is enabled.")]
        public bool AutoOverwrite
        {
            get { return _InputGroup.AutoOverwrite; }
            set { _InputGroup.AutoOverwrite = value; }
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


        #region Free-Text Entry Support
        private Keys _FreeTextEntryToggleKey = Keys.None;
        /// <summary>
        /// Gets or sets the keyboard key that will toggle FreeTextEntryMode. Default value is Keys.None.
        /// </summary>
        [DefaultValue(Keys.None), Category("Free-Text"), Description("")]
        public Keys FreeTextEntryToggleKey
        {
            get { return _FreeTextEntryToggleKey; }
            set { _FreeTextEntryToggleKey = value; }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_FreeTextEntryToggleKey!= Keys.None && keyData == _FreeTextEntryToggleKey)
            {
                this.FreeTextEntryMode = !this.FreeTextEntryMode;
                if (this.FreeTextEntryMode && _FreeTextEntryBox != null) _FreeTextEntryBox.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the
        /// value control expects.
        /// </summary>
        [Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects."), Category("Free-Text")]
        public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry;
        /// <summary>
        /// Occurs when Free-Text button is clicked and allows you to cancel its default action.
        /// </summary>
        public event CancelEventHandler ButtonFreeTextClick;

        private void FreeTextButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonFreeTextClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            FreeTextEntryMode = !FreeTextEntryMode;
            _ButtonFreeText.Checked = FreeTextEntryMode;
            if (FreeTextEntryMode && _FreeTextEntryBox != null && !_FreeTextEntryBox.Focused)
                _FreeTextEntryBox.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
        {
            CancelEventHandler handler = ButtonFreeTextClick;
            if (handler != null) handler(this, e);
        }

        private bool _AutoResolveFreeTextEntries = true;
        /// <summary>
        /// Gets or sets whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Free-Text"), Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
        public bool AutoResolveFreeTextEntries
        {
            get { return _AutoResolveFreeTextEntries; }
            set
            {
                _AutoResolveFreeTextEntries = value;
            }
        }

        private bool _AutoOffFreeTextEntry = false;
        /// <summary>
        /// Gets or sets whether free-text entry is automatically turned off when control loses input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Free-Text"), Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
        public bool AutoOffFreeTextEntry
        {
            get { return _AutoOffFreeTextEntry; }
            set
            {
                _AutoOffFreeTextEntry = value;
            }
        }

        private bool _FreeTextEntryMode = false;
        /// <summary>
        /// Gets or sets whether control input is in free-text input mode. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Free-Text"), Description("Indicates whether control input is in free-text input mode.")]
        public bool FreeTextEntryMode
        {
            get { return _FreeTextEntryMode; }
            set
            {
                _FreeTextEntryMode = value;
                OnFreeTextEntryModeChanged();
            }
        }

        private void OnFreeTextEntryModeChanged()
        {
            if (!_FreeTextEntryMode)
            {
                if (_FreeTextEntryBox != null)
                {
                    _FreeTextEntryBox.ApplyValue -= ApplyFreeTextValue;
                    _FreeTextEntryBox.RevertValue -= RevertFreeTextValue;
                    _FreeTextEntryBox.LostFocus -= FreeTextLostFocus;
                    this.Controls.Remove(_FreeTextEntryBox);
                    _FreeTextEntryBox.Dispose();
                    _FreeTextEntryBox = null;
                }
            }
            else
            {
                UpdateFreeTextBoxVisibility();
            }
            if (_ButtonFreeText != null) _ButtonFreeText.Checked = _FreeTextEntryMode;
        }

        protected override void OnIsKeyboardFocusWithinChanged()
        {
            if (_FreeTextEntryMode)
            {
                UpdateFreeTextBoxVisibility();
                if (this.IsKeyboardFocusWithin)
                {
                    Control textBox = GetFreeTextBox();
                    if (!textBox.Focused)
                        textBox.Focus();
                }
            }
            base.OnIsKeyboardFocusWithinChanged();
        }

        private void UpdateFreeTextBoxVisibility()
        {
            FreeTextEntryBox textBox = (FreeTextEntryBox)GetFreeTextBox();
            if (this.IsKeyboardFocusWithin)
            {
                textBox.Visible = true;
                textBox.Text = this.Text;
                RootVisualItem.InvalidateArrange();
                this.Invalidate();
            }
            else
            {
                if (textBox.Visible)
                {
                    if (textBox.Focused)
                        textBox.HideOnLostFocus();
                    else
                        textBox.Visible = false;
                    RootVisualItem.InvalidateArrange();
                    this.Invalidate();
                }
            }
        }

        protected override bool SupportsFreeTextEntry
        {
            get
            {
                return true;
            }
        }

        internal FreeTextEntryBox _FreeTextEntryBox = null;
        protected override Control GetFreeTextBox()
        {
            if (_FreeTextEntryBox == null)
            {
                _FreeTextEntryBox = new FreeTextEntryBox();
                _FreeTextEntryBox.ApplyValue += ApplyFreeTextValue;
                _FreeTextEntryBox.RevertValue += RevertFreeTextValue;
                _FreeTextEntryBox.LostFocus += FreeTextLostFocus;
                _FreeTextEntryBox.IncreaseValue += FreeTextIncreaseValue;
                _FreeTextEntryBox.DecreaseValue += new EventHandler(FreeTextDecreaseValue);
                _FreeTextEntryBox.BorderStyle = BorderStyle.None;
                _FreeTextEntryBox.ReadOnly = !this.LockUpdateChecked;
                if (this.InputHorizontalAlignment == eHorizontalAlignment.Right)
                    _FreeTextEntryBox.TextAlign = HorizontalAlignment.Right;
                else if (this.InputHorizontalAlignment == eHorizontalAlignment.Center)
                    _FreeTextEntryBox.TextAlign = HorizontalAlignment.Center;
                else if (this.InputHorizontalAlignment == eHorizontalAlignment.Left)
                    _FreeTextEntryBox.TextAlign = HorizontalAlignment.Left;

                this.Controls.Add(_FreeTextEntryBox);
            }

            return _FreeTextEntryBox;
        }

        void FreeTextDecreaseValue(object sender, EventArgs e)
        {
            ApplyFreeTextValue();
            DecreaseValue();
        }

        void FreeTextIncreaseValue(object sender, EventArgs e)
        {
            ApplyFreeTextValue();
            IncreaseValue();
        }

        private void RevertFreeTextValue(object sender, EventArgs e)
        {
            if (_FreeTextEntryBox != null) _FreeTextEntryBox.Text = this.Text;
        }

        protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
        {
            FreeTextEntryConversionEventHandler handler = this.ConvertFreeTextEntry;
            if (handler != null) handler(this, e);
        }

        private void ApplyFreeTextValue(object sender, EventArgs e)
        {
            ApplyFreeTextValue();
        }

        protected virtual void ApplyFreeTextValue()
        {
            
        }

        private void FreeTextLostFocus(object sender, EventArgs e)
        {
            if (_AutoOffFreeTextEntry && !this.IsKeyboardFocusWithin)
                this.FreeTextEntryMode = false;
        }

        protected override void HideFreeTextBoxEntry()
        {
            if (_FreeTextEntryBox != null) _FreeTextEntryBox.Visible = false;
        }

        protected override bool IsFreeTextEntryVisible
        {
            get
            {
                return _FreeTextEntryMode && this.IsKeyboardFocusWithin;
            }
        }
        #endregion
    }
}
#endif

