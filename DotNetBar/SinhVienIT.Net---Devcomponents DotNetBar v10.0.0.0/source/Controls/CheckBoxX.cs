using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the Check Box control with extended styles.
    /// </summary>
    [ToolboxBitmap(typeof(CheckBoxX), "Controls.CheckBoxX.ico"), ToolboxItem(true), DefaultEvent("CheckedChanged"), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.CheckBoxXDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class CheckBoxX : BaseItemControl, ICommandSource
    {
        #region Private Variables
        private const string DefaultCheckValueUnchecked = "N";
        private const string DefaultCheckValueChecked = "Y";
        private const object DefaultCheckValueIndeterminate = null;

        private CheckBoxItem m_CheckBox = null;
        private object m_CheckValue = DefaultCheckValueUnchecked;

        private object m_CheckValueUnchecked = DefaultCheckValueUnchecked;
        private object m_CheckValueChecked = DefaultCheckValueChecked;
        private object m_CheckValueIndeterminate = DefaultCheckValueIndeterminate;
        private bool m_ConsiderEmptyStringAsNull = true;
        #endregion

        #region Events
        /// <summary>
        /// Occurs before Checked property is changed and allows you to cancel the change.
        /// </summary>
        public event CheckBoxXChangeEventHandler CheckedChanging;
        
        /// <summary>
        /// Occurs after Checked property is changed with extended information.
        /// </summary>
        public event CheckBoxXChangeEventHandler CheckedChangedEx;

        /// <summary>
        /// Occurs after Checked property is changed. This event is provided for the Windows Forms data binding support. You can use CheckedChangedEx to get extended information about the changed.
        /// </summary>
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Occurs after Checked property is changed. This event is provided for the Windows Forms data binding support. You can use CheckedChangedEx to get extended information about the changed.
        /// </summary>
        public event EventHandler CheckValueChanged;
        #endregion

        #region Constructor, Dispose
        public CheckBoxX()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            m_CheckBox = new CheckBoxItem();
            m_CheckBox.VerticalPadding = 0;
            m_CheckBox.Style = eDotNetBarStyle.Office2007;
            m_CheckBox.CheckedChanging += new CheckBoxChangeEventHandler(OnCheckedChanging);
            m_CheckBox.CheckedChanged += new CheckBoxChangeEventHandler(OnCheckedChanged);
            this.HostItem = m_CheckBox;
        }

        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets the size of the check or radio sign. Default value is 13x13. Minimum value is 6x6.
        /// </summary>
        [Category("Appearance"), Description("Indicates size of the check or radio sign. Default value is 13x13.")]
        public Size CheckSignSize
        {
            get { return m_CheckBox.CheckSignSize; }
            set { m_CheckBox.CheckSignSize = value; this.RecalcLayout(); this.Invalidate(); }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCheckSignSize()
        {
            return m_CheckBox.ShouldSerializeCheckSignSize();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCheckSignSize()
        {
            this.CheckSignSize = new Size(13, 13);
        }

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for controls Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the control instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for controls Text property.")]
        public bool EnableMarkup
        {
            get { return m_CheckBox.EnableMarkup; }
            set
            {
                m_CheckBox.EnableMarkup = value;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Focused)
                this.Select();
            base.OnMouseDown(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            if (this.CheckBoxStyle == eCheckBoxStyle.RadioButton && Control.MouseButtons == MouseButtons.None && WinApi.GetKeyState(9) < 0 && this.AutoCheck)
            {
                m_CheckBox.SetChecked(true, eEventSource.Keyboard);
            }
            base.OnEnter(e);
        }

        /// <summary>
        /// Invokes the CheckedChanged event.
        /// </summary>
        private void OnCheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            CheckBoxX previous = null;
            if (m_CheckBox.CheckBoxStyle == eCheckBoxStyle.RadioButton && m_CheckBox.Checked && this.Parent != null)
            {
                foreach (Control c in this.Parent.Controls)
                {
                    if (c == this)
                    {
                        c.TabStop = true;
                        continue;
                    }
                    CheckBoxX b = c as CheckBoxX;
                    if (b != null && b.Checked && b.CheckBoxStyle == eCheckBoxStyle.RadioButton)
                    {
                        b.Checked = false;
                        b.TabStop = false;
                        previous = b;
                    }
                }
            }

            if (this.Command != null)
                this.Command.Checked = this.Checked;

            CheckBoxXChangeEventArgs e1 = new CheckBoxXChangeEventArgs(previous, this, e.EventSource);

            if (CheckedChangedEx != null)
                CheckedChangedEx(this, e1);

            if (CheckedChanged != null)
                CheckedChanged(this, new EventArgs());

            if (GetCheckState(m_CheckValue) != this.CheckState)
                m_CheckValue = GetCheckValue(this.CheckState);

            if (CheckValueChanged != null)
                CheckValueChanged(this, new EventArgs());

            ExecuteCommand();
        }

        /// <summary>
        /// Invokes CheckedChanging event.
        /// </summary>
        private void OnCheckedChanging(object sender, CheckBoxChangeEventArgs e)
        {
            CheckBoxXChangeEventArgs e1 = new CheckBoxXChangeEventArgs(null, this, e.EventSource);

            if (m_CheckBox.CheckBoxStyle == eCheckBoxStyle.RadioButton && !m_CheckBox.Checked && this.Parent != null)
            {
                CheckBoxX b = null;
                foreach (Control c in this.Parent.Controls)
                {
                    if (c == this)
                        continue;
                    b = c as CheckBoxX;
                    if (b != null && b.Checked && b.CheckBoxStyle == eCheckBoxStyle.RadioButton)
                    {
                        break;
                    }
                }
                e1 = new CheckBoxXChangeEventArgs(b, this, e.EventSource);
            }

            if (CheckedChanging != null)
            {
                CheckedChanging(this, e1);
                e.Cancel = e1.Cancel;
            }
        }

        /// <summary>
        /// Gets or sets the appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.
        /// </summary>
        [Browsable(true), DefaultValue(eCheckBoxStyle.CheckBox), Category("Appearance"), Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
        public eCheckBoxStyle CheckBoxStyle
        {
            get { return m_CheckBox.CheckBoxStyle; }
            set
            {
                m_CheckBox.CheckBoxStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the check box position relative to the text. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eCheckBoxPosition.Left), Category("Appearance"), Description("Indicates the check box position relative to the text.")]
        public eCheckBoxPosition CheckBoxPosition
        {
            get { return m_CheckBox.CheckBoxPosition; }
            set
            {
                m_CheckBox.CheckBoxPosition = value;
            }
        }

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(true), Bindable(true), DevCoBrowsable(true), Category("Appearance"), Description("Indicates whether item is checked or not."), DefaultValue(false)]
        public virtual bool Checked
        {
            get
            {
                return m_CheckBox.Checked;
            }
            set
            {
                m_CheckBox.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets whether text assigned to the check box is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether text assigned to the check box is visible.")]
        public bool TextVisible
        {
            get { return m_CheckBox.TextVisible; }
            set
            {
                m_CheckBox.TextVisible = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the text color. Default value is Color.Empty which indicates that default color is used.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates text color.")]
        public Color TextColor
        {
            get { return m_CheckBox.TextColor; }
            set
            {
                m_CheckBox.TextColor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !m_CheckBox.TextColor.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && this.Enabled && this.AutoCheck)
            {
                if (this.CheckBoxStyle == eCheckBoxStyle.CheckBox)
                    SetChecked(!this.Checked, eEventSource.Keyboard);
                else if (!this.Checked)
                    SetChecked(true, eEventSource.Keyboard);
            }
            base.OnKeyDown(e);
        }

        private void SetChecked(bool newValue, eEventSource source)
        {
            m_CheckBox.SetChecked(newValue, source);
        }

        private void SetChecked(CheckState newValue, eEventSource source)
        {
            m_CheckBox.SetChecked(newValue, source);
        }

        /// <summary>
        /// Gets or sets the text associated with the control.
        /// </summary>
        [Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance"), Description("Indicates text associated with this button.."), Localizable(true), DefaultValue("")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

#if FRAMEWORK20
        [Localizable(true), Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!BarFunctions.IsHandleValid(this))
                return base.GetPreferredSize(proposedSize);
            if (this.Text.Length == 0)
                return base.GetPreferredSize(proposedSize);

            m_CheckBox.RecalcSize();
            Size s = m_CheckBox.Size;
            s.Width += 2;
            s.Height += 2;
            if (!this.TextVisible) s.Width += 2;
            m_CheckBox.Bounds = GetItemBounds();
            return s;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized to display its entire contents. You can set MaximumSize.Width property to set the maximum width used by the control.
        /// </summary>
        [Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    AdjustSize();
                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void AdjustSize()
        {
            if (this.AutoSize)
            {
                this.Size = base.PreferredSize;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.AdjustSize();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.AutoSize)
                this.AdjustSize();
        }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether the CheckBox will allow three check states rather than two. If the ThreeState property is set to true
        /// CheckState property should be used instead of Checked property to set the extended state of the control.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false), Description("Indicates whether the CheckBox will allow three check states rather than two.")]
        public bool ThreeState
        {
            get { return m_CheckBox.ThreeState; }
            set { m_CheckBox.ThreeState = value; }
        }

        /// <summary>
        /// Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state. 
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(CheckState.Unchecked), Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
        public CheckState CheckState
        {
            get { return m_CheckBox.CheckState; }
            set { m_CheckBox.CheckState = value; }
        }

        /// <summary>
        /// Gets the underlying CheckBoxItem
        /// </summary>
        internal CheckBoxItem CheckBoxItem
        {
            get { return (m_CheckBox); }
        }

        private bool _EnableMnemonicWithAltKeyOnly = false;
        /// <summary>
        /// Gets or sets whether mnemonic character assigned to control is processed only if Alt key is pressed. Default value is false which indicate that Alt key is not required.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether mnemonic character assigned to control is processed only if Alt key is pressed")]
        public bool EnableMnemonicWithAltKeyOnly
        {
            get { return _EnableMnemonicWithAltKeyOnly; }
            set
            {
                _EnableMnemonicWithAltKeyOnly = value;
            }
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (AutoCheck && CanSelect && IsMnemonic(charCode, this.Text) && this.Enabled &&
                (!_EnableMnemonicWithAltKeyOnly || Control.ModifierKeys == Keys.Alt || this.Focused))
            {
                if (Focus())
                {
                    if (this.CheckBoxStyle == eCheckBoxStyle.CheckBox)
                        SetChecked(!this.Checked, eEventSource.Keyboard);
                    else if (!this.Checked)
                        SetChecked(true, eEventSource.Keyboard);
                    return true;
                }
            }
            return base.ProcessMnemonic(charCode);
        }
        #endregion

        #region Data-Binding Support
        /// <summary>
        /// Gets or set whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
        public bool AutoCheck
        {
            get { return m_CheckBox.AutoCheck; }
            set
            {
                m_CheckBox.AutoCheck = value;
            }
        }

        [DefaultValue("N"), Bindable(true), RefreshProperties(RefreshProperties.All), TypeConverter(typeof(StringConverter))]
        public object CheckValue
        {
            get { return m_CheckValue; }
            set
            {
                m_CheckValue = value;
                OnCheckValueChanged();
            }
        }

        private void OnCheckValueChanged()
        {
            CheckState cs = GetCheckState(m_CheckValue);
            this.CheckState = cs;
        }

        private bool IsNull(object value)
        {
            return value == null || value is string && (string)value == string.Empty && m_ConsiderEmptyStringAsNull ||
                value == DBNull.Value;
        }

        private object GetCheckValue(CheckState cs)
        {
            if (cs == CheckState.Indeterminate)
                return m_CheckValueIndeterminate;

            if (cs == CheckState.Unchecked)
                return m_CheckValueUnchecked;

            return m_CheckValueChecked;
        }

        private CheckState GetCheckState(object value)
        {
            if (m_CheckValueIndeterminate == null && IsNull(value) || m_CheckValueIndeterminate!=null && m_CheckValueIndeterminate.Equals(value))
            {
                return CheckState.Indeterminate;
            }
            else if (m_CheckValueChecked == null && IsNull(value) || m_CheckValueChecked!=null && m_CheckValueChecked.Equals(value))
            {
                return CheckState.Checked;
            }
            else if (m_CheckValueUnchecked == null && IsNull(value) || m_CheckValueUnchecked!=null && m_CheckValueUnchecked.Equals(value))
            {
                return CheckState.Unchecked;
            }
            else
            {
                if (value is System.Data.SqlTypes.SqlBoolean)
                {
                    System.Data.SqlTypes.SqlBoolean sqlBool = (System.Data.SqlTypes.SqlBoolean)value;
                    if (sqlBool.IsTrue)
                        return CheckState.Checked;
                    else if (sqlBool.IsNull)
                        return CheckState.Indeterminate;
                    else
                        return CheckState.Unchecked;
                }
                else if (value is int? && this.ThreeState)
                {
                    if (value == null) return CheckState.Indeterminate;
                    if (((int?)value).Value == 0)
                        return CheckState.Unchecked;
                    return CheckState.Checked;
                }
                else if (value is int)
                {
                    if (this.ThreeState && (int)value == -1)
                        return CheckState.Indeterminate;
                    return ((int)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is long)
                {
                    if (this.ThreeState && (long)value == -1)
                        return CheckState.Indeterminate;
                    return ((long)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is short)
                {
                    if (this.ThreeState && (short)value == -1)
                        return CheckState.Indeterminate;
                    return ((short)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is float)
                {
                    if (this.ThreeState && (float)value == -1)
                        return CheckState.Indeterminate;
                    return ((float)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is double)
                {
                    if (this.ThreeState && (double)value == -1)
                        return CheckState.Indeterminate;
                    return ((double)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is byte)
                {
                    return ((byte)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is uint)
                {
                    return ((uint)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is ulong)
                {
                    return ((ulong)value) == 0 ? CheckState.Unchecked : CheckState.Checked;
                }
                else if (value is bool)
                {
                    return ((bool)value) == false ? CheckState.Unchecked : CheckState.Checked;
                }
                
                return CheckState.Indeterminate;
            }
        }

        /// <summary>
        /// Gets or sets whether empty string is consider as null value during CheckValue value comparison. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Browsable(true), Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
        public bool ConsiderEmptyStringAsNull
        {
            get { return m_ConsiderEmptyStringAsNull; }
            set { m_ConsiderEmptyStringAsNull = value; }
        }

        /// <summary>
        /// Gets or sets the value that represents the Indeterminate state of check box when CheckValue property is set to that value. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Behavior"), TypeConverter(typeof(StringConverter)), Description("Represents the Indeterminate state value of check box when CheckValue property is set to that value")]
        public object CheckValueIndeterminate
        {
            get { return m_CheckValueIndeterminate; }
            set { m_CheckValueIndeterminate = value; OnCheckValueChanged(); }
        }

        /// <summary>
        /// Gets or sets the value that represents the Checked state of check box when CheckValue property is set to that value. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue("Y"), Category("Behavior"), TypeConverter(typeof(StringConverter)), Description("Represents the Checked state value of check box when CheckValue property is set to that value")]
        public object CheckValueChecked
        {
            get { return m_CheckValueChecked; }
            set { m_CheckValueChecked = value; OnCheckValueChanged(); }
        }

        /// <summary>
        /// Gets or sets the value that represents the Unchecked state of check box when CheckValue property is set to that value. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue("N"), Category("Behavior"), TypeConverter(typeof(StringConverter)), Description("Represents the Unchecked state value of check box when CheckValue property is set to that value")]
        public object CheckValueUnchecked
        {
            get { return m_CheckValueUnchecked; }
            set { m_CheckValueUnchecked = value; OnCheckValueChanged(); }
        }

        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is checked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
        public Image CheckBoxImageChecked
        {
            get { return m_CheckBox.CheckBoxImageChecked; }
            set
            {
                m_CheckBox.CheckBoxImageChecked = value;
            }
        }
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is unchecked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
        public Image CheckBoxImageUnChecked
        {
            get { return m_CheckBox.CheckBoxImageUnChecked; }
            set
            {
                m_CheckBox.CheckBoxImageUnChecked = value;
            }
        }
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is in indeterminate state.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
        public Image CheckBoxImageIndeterminate
        {
            get { return m_CheckBox.CheckBoxImageIndeterminate; }
            set
            {
                m_CheckBox.CheckBoxImageIndeterminate = value;
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
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
            Command command = this.Command;
            if (command != null && command.Checked != this.Checked)
            {
                SetChecked(command.Checked, eEventSource.Code);
            }
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

    /// <summary>
    /// Delegate for OptionGroupChanging event.
    /// </summary>
    public delegate void CheckBoxXChangeEventHandler(object sender, CheckBoxXChangeEventArgs e);

    #region CheckBoxXChangeEventArgs
    /// <summary>
    /// Represents event arguments for OptionGroupChanging event.
    /// </summary>
    public class CheckBoxXChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Set to true to cancel the checking on NewChecked button.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Check-box that will become checked if operation is not cancelled.
        /// </summary>
        public readonly CheckBoxX NewChecked;
        /// <summary>
        /// Check-box that is currently checked and which will be unchecked if operation is not cancelled. This property will have only valid values for eCheckBoxStyle.RadioButton style CheckBoxItems.
        /// </summary>
        public readonly CheckBoxX OldChecked;
        /// <summary>
        /// Indicates the action that has caused the event.
        /// </summary>
        public readonly eEventSource EventSource = eEventSource.Code;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CheckBoxXChangeEventArgs(CheckBoxX oldchecked, CheckBoxX newchecked, eEventSource eventSource)
        {
            NewChecked = newchecked;
            OldChecked = oldchecked;
            EventSource = eventSource;
        }
    }
    #endregion
}
