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
    [ToolboxBitmap(typeof(DotNetBarManager), "DoubleInput.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.NumericInputBaseDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class DoubleInput : NumericInputBase
    {
        #region Private Variables
        private VisualDoubleInput _DoubleInput = null;
        private VisualInputGroup _InputGroup = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when ValueObject property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseDoubleValueEventHandler ParseValue;

        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Copies the current value in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (_DoubleInput != null) _DoubleInput.ProcessClipboardCopy();
        }
        /// <summary>
        /// Pastes the current Clipboard content if possible as the value into the control.
        /// </summary>
        public virtual void Paste()
        {
            if (_DoubleInput != null) _DoubleInput.ProcessClipboardPaste();
        }
        /// <summary>
        /// Moves the current control value to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            if (_DoubleInput != null) _DoubleInput.ProcessClipboardCut();
        }

        internal VisualInputGroup InputGroup
        {
            get { return (_InputGroup); }
        }

        protected override VisualItem CreateRootVisual()
        {
            VisualInputGroup g = new VisualInputGroup();
            VisualDoubleInput i = new VisualDoubleInput();
            i.ValueChanged += new EventHandler(InputItemValueChanged);
            g.Items.Add(i);
            _InputGroup = g;
            _DoubleInput = i;
            return g;
        }

        protected override void UpdateInputFieldAlignment()
        {
            if (this.InputHorizontalAlignment == eHorizontalAlignment.Right)
            {
                if (!ButtonClear.Visible && !ButtonDropDown.Visible && !ButtonFreeText.Visible && !ButtonCustom.Visible && !ButtonCustom2.Visible && !ShowUpDown)
                    _DoubleInput.Alignment = eItemAlignment.Right;
                else
                    _DoubleInput.Alignment = eItemAlignment.Left;
            }
            else
            {
                _DoubleInput.Alignment = eItemAlignment.Left;
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
            _DoubleInput.DisplayFormat = this.DisplayFormat;
        }

        /// <summary>
        /// Gets or sets the value displayed in the control.
        /// </summary>
        [DefaultValue(0d), Description("Indicates value displayed in the control.")]
        public double Value
        {
            get { return _DoubleInput.Value; }
            set
            {
                if (Value != value || _DoubleInput.IsEmpty)
                {
                    _DoubleInput.Value = value;
                    //OnValueChanged(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of the control as an object. This property allows you to bind to the database fields and supports
        /// null values. Expected value is int type or null.
        /// </summary>
        [Bindable(true), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter))]
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
                else if (value is double || value is int || value is float)
                {
                    this.Value = (double)value;
                }
                else if (value is decimal)
                {
                    this.Value = decimal.ToDouble((decimal)value);
                }
                else if (value is long)
                {
                    string t = value.ToString();
                    this.Value = double.Parse(t);
                }
                else if (value is string)
                {
                    double i = 0;
                    if (double.TryParse(value.ToString(), out i))
                        this.Value = i;
                    else
                        throw new ArgumentException("ValueObject property expects either null/nothing value or double type.");
                }
                else
                    throw new ArgumentException("ValueObject property expects either null/nothing value or double type.");
            }
        }

        private bool AcceptCustomValueObject(object value)
        {
            ParseDoubleValueEventArgs e = new ParseDoubleValueEventArgs(value);
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
        protected virtual void OnParseValue(ParseDoubleValueEventArgs e)
        {
            if (ParseValue != null)
                ParseValue(this, e);
        }

        /// <summary>
        /// Gets or sets the maximum value that can be entered.
        /// </summary>
        [DefaultValue(double.MaxValue), Description("Indicates maximum value that can be entered.")]
        public double MaxValue
        {
            get { return _DoubleInput.MaxValue; }
            set
            {
                _DoubleInput.MaxValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value that can be entered.
        /// </summary>
        [DefaultValue(double.MinValue), Description("Indicates minimum value that can be entered.")]
        public double MinValue
        {
            get { return _DoubleInput.MinValue; }
            set
            {
                _DoubleInput.MinValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement the value of the control when the up or down buttons are clicked. 
        /// </summary>
        [DefaultValue(1), Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
        public double Increment
        {
            get { return _DoubleInput.Increment; }
            set { _DoubleInput.Increment = value; }
        }

        protected override bool IsWatermarkRendered
        {
            get
            {
                return !(this.Focused || _FreeTextEntryBox != null && _FreeTextEntryBox.Focused) && _DoubleInput.IsEmpty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return _DoubleInput.Text;
            }
            set
            {
                ValueObject = value;
            }
        }

        /// <summary>
        /// Decreases value of the control.
        /// </summary>
        public override void DecreaseValue()
        {
            if (this.FreeTextEntryMode)
                ApplyFreeTextValue();
            _DoubleInput.DecreaseValue();
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
            _DoubleInput.IncreaseValue();
            if (this.FreeTextEntryMode && _FreeTextEntryBox != null)
                _FreeTextEntryBox.Text = this.Text;
        }

        internal VisualDoubleInput VisualDoubleInput
        {
            get { return (_DoubleInput); }
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
                double value;
                if (double.TryParse(_FreeTextEntryBox.Text, out value) && AutoResolveFreeTextEntries)
                {
                    this.Value = value;
                }
                else
                {
                    FreeTextEntryConversionEventArgs eventArgs = new FreeTextEntryConversionEventArgs(_FreeTextEntryBox.Text);
                    OnConvertFreeTextEntry(eventArgs);
                    if (eventArgs.IsValueConverted)
                    {
                        if (eventArgs.ControlValue is double)
                            this.Value = (double)eventArgs.ControlValue;
                        else if (eventArgs.ControlValue == null)
                            this.ValueObject = null;
                        else
                            throw new ArgumentException("ControlValue assigned is not double type.");
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

