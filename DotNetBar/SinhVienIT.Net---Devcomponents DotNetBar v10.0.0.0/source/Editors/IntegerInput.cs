#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    /// <summary>
    /// Control for input of the integer value.
    /// </summary>
    [ToolboxBitmap(typeof(DotNetBarManager), "IntegerInput.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.NumericInputBaseDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class IntegerInput : NumericInputBase
    {
        #region Private Variables
        private VisualIntegerInput _IntegerInput = null;
        private VisualInputGroup _InputGroup = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when ValueObject property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseIntegerValueEventHandler ParseValue;

        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Decreases value of the control.
        /// </summary>
        public override void DecreaseValue()
        {
            if (this.FreeTextEntryMode)
                ApplyFreeTextValue();
            _IntegerInput.DecreaseValue();
            if (this.FreeTextEntryMode && _FreeTextEntryBox != null)
                _FreeTextEntryBox.Text = this.Text;
        }

        /// <summary>
        /// Increases the value of the control.
        /// </summary>
        public override void IncreaseValue()
        {
            if (this.FreeTextEntryMode)
                ApplyFreeTextValue();
            _IntegerInput.IncreaseValue();
            if (this.FreeTextEntryMode && _FreeTextEntryBox != null)
                _FreeTextEntryBox.Text = this.Text;
        }

        /// <summary>
        /// Copies the current value in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (_IntegerInput != null) _IntegerInput.ProcessClipboardCopy();
        }
        /// <summary>
        /// Pastes the current Clipboard content if possible as the value into the control.
        /// </summary>
        public virtual void Paste()
        {
            if (_IntegerInput != null) _IntegerInput.ProcessClipboardPaste();
        }
        /// <summary>
        /// Moves the current control value to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            if (_IntegerInput != null) _IntegerInput.ProcessClipboardCut();
        }

        protected override VisualItem CreateRootVisual()
        {
            VisualInputGroup g = new VisualInputGroup();
            VisualIntegerInput i = new VisualIntegerInput();
            i.ValueChanged += new EventHandler(InputItemValueChanged);
            g.Items.Add(i);
            _InputGroup = g;
            _IntegerInput = i;
            return g;
        }

        protected override void UpdateInputFieldAlignment()
        {
            if (this.InputHorizontalAlignment == eHorizontalAlignment.Right)
            {
                if (!ButtonClear.Visible && !ButtonDropDown.Visible && !ButtonFreeText.Visible && !ButtonCustom.Visible && !ButtonCustom2.Visible && !ShowUpDown)
                    _IntegerInput.Alignment = eItemAlignment.Right;
                else
                    _IntegerInput.Alignment = eItemAlignment.Left;
            }
            else
            {
                _IntegerInput.Alignment = eItemAlignment.Left;
            }
            base.UpdateInputFieldAlignment();
        }

        private void InputItemValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
            if (this.FreeTextEntryMode && _FreeTextEntryBox != null && _FreeTextEntryBox.Text != this.Text)
                _FreeTextEntryBox.Text = this.Text;
        }

        protected override void OnDisplayFormatChanged()
        {
            _IntegerInput.DisplayFormat = this.DisplayFormat;
        }

        /// <summary>
        /// Gets or sets the value displayed in the control.
        /// </summary>
        [DefaultValue(0), Description("Indicates value displayed in the control.")]
        public int Value
        {
            get { return _IntegerInput.Value; }
            set
            {
                if (Value != value || _IntegerInput.IsEmpty)
                {
                    _IntegerInput.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of the control as an object. This property allows you to bind to the database fields and supports
        /// null values. Expected value is int type or null.
        /// </summary>
        [Bindable(true), RefreshProperties(RefreshProperties.All), DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter))]
        public object ValueObject
        {
            get
            {
                if (this.IsEmpty)
                    return null;
                return Value;
            }
            set
            {
                if (AcceptCustomValueObject(value))
                    return;
                else if (IsNull(value))
                    this.IsEmpty = true;
                else if (value is int)
                {
                    this.Value = (int)value;
                }
                else if (value is byte || value is Int16)
                {
                    this.Value = (int)value;
                }
                else if (value is string)
                {
                    int i = 0;
                    if (int.TryParse(value.ToString(), out i))
                        this.Value = i;
                    else
                        throw new ArgumentException("ValueObject property expects either null/nothing value or int type.");
                }
                else
                    throw new ArgumentException("ValueObject property expects either null/nothing value or int type.");
            }
        }

        private bool AcceptCustomValueObject(object value)
        {
            ParseIntegerValueEventArgs e = new ParseIntegerValueEventArgs(value);
            OnParseValue(e);
            if (e.IsParsed)
            {
                this.Value = e.ParsedValue;
            }

            return e.IsParsed;
        }

        /// <summary>
        /// Raises the ParseValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseValue(ParseIntegerValueEventArgs e)
        {
            if (ParseValue != null)
                ParseValue(this, e);
        }

        /// <summary>
        /// Gets or sets the maximum value that can be entered.
        /// </summary>
        [DefaultValue(int.MaxValue), Description("Indicates maximum value that can be entered.")]
        public int MaxValue
        {
            get { return _IntegerInput.MaxValue; }
            set
            {
                _IntegerInput.MaxValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value that can be entered.
        /// </summary>
        [DefaultValue(int.MinValue), Description("Indicates minimum value that can be entered.")]
        public int MinValue
        {
            get { return _IntegerInput.MinValue; }
            set
            {
                _IntegerInput.MinValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement the value of the control when the up or down buttons are clicked. 
        /// </summary>
        [DefaultValue(1), Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
        public int Increment
        {
            get { return _IntegerInput.Increment; }
            set { _IntegerInput.Increment = value; }
        }

        protected override bool IsWatermarkRendered
        {
            get
            {
                return !(this.Focused || _FreeTextEntryBox != null && _FreeTextEntryBox.Focused) && _IntegerInput.IsEmpty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return _IntegerInput.Text;
            }
            set
            {
                ValueObject = value;
            }
        }
        #endregion

        #region Free Text Entry Support

        protected override void ApplyFreeTextValue()
        {
            if (_FreeTextEntryBox == null) return;
            if (string.IsNullOrEmpty(_FreeTextEntryBox.Text))
                this.ValueObject = null;
            else
            {
                int value;
                if (int.TryParse(_FreeTextEntryBox.Text, out value) && AutoResolveFreeTextEntries)
                {
                    this.Value = value;
                }
                else
                {
                    FreeTextEntryConversionEventArgs eventArgs = new FreeTextEntryConversionEventArgs(_FreeTextEntryBox.Text);
                    OnConvertFreeTextEntry(eventArgs);
                    if (eventArgs.IsValueConverted)
                    {
                        if (eventArgs.ControlValue is int)
                            this.Value = (int)eventArgs.ControlValue;
                        else if (eventArgs.ControlValue == null)
                            this.ValueObject = null;
                        else
                            throw new ArgumentException("ControlValue assigned is not int type.");
                    }
                    else
                    {
                        //if (_AutoResolveFreeTextEntries)
                        //{
                        //    value = DateTime.MinValue;
                        //    string text = _FreeTextEntryBox.Text.ToLower();
                        //    if (text == "now")
                        //        value = DateTime.Now;
                        //    else if (text == "today")
                        //        value = DateTime.Today;
                        //    else if (text == "tomorrow")
                        //        value = DateTime.Today.AddDays(1);
                        //    else if (text == "yesterday")
                        //        value = DateTime.Today.AddDays(-1);
                        //    if (value == DateTime.MinValue)
                        //        this.ValueObject = null;
                        //    else
                        //        this.Value = value;
                        //}
                        this.Value = 0;
                    }
                }
            }
        }
        #endregion

    }
}
#endif

