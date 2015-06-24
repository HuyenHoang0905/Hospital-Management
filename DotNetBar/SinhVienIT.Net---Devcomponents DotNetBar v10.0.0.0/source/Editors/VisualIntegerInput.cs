#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Editors
{
    public class VisualIntegerInput : VisualNumericInput
    {
        #region Private Variables
        private int _Value = 0;
        private int _MinValue = int.MinValue;
        private int _MaxValue = int.MaxValue;
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void OnInputStackChanged()
        {
            if (this.InputStack.Length > 0)
            {
                this.IsEmpty = false;

                if (this.InputStack == "-")
                {
                    SetValue(0, true);
                    return;
                }

                int newValue = int.Parse(this.InputStack);
                SetValue(newValue, true);
            }
            else
            {
                if (this.AllowEmptyState)
                    this.IsEmpty = true;
                SetValue(0, true);
            }

            base.OnInputStackChanged();
        }

        protected override void OnInputKeyAccepted()
        { 
            CheckInputComplete(true);
            base.OnInputKeyAccepted();
        }

        private void CheckInputComplete(bool sendNotification)
        {
            int predictedValue = GetNextPredictedValue(_Value);

            if (_Value == _MaxValue || (ConvertToString(_Value).Length >= ConvertToString(_MaxValue).Length && ConvertToString(_Value).Length >= ConvertToString(_MinValue).Length) ||
                predictedValue > _MaxValue)
                InputComplete(sendNotification);
            else if (IsLeadingZero && ConvertToString(_Value).Length + 1 >= ConvertToString(_MaxValue).Length)
                InputComplete(sendNotification);
        }

        private int GetNextPredictedValue(int value)
        {
            string s = "";
            if (value == 0)
                s = "1";
            else
                s = ConvertToString(value) + "0";
            int i = value;
            int.TryParse(s, out i);
            return i;
        }

        protected override void OnLostFocus()
        {
            ValidateValue();
            base.OnLostFocus();
        }

        protected virtual void ValidateValue()
        {
            if (_Value < _MinValue && !this.IsEmpty)
                Value = _MinValue;
            else if (_Value > _MaxValue && !this.IsEmpty)
                Value = _MaxValue;
        }

        private void SetValue(int i, bool raiseValueChanged)
        {
            bool changed = _Value != i || raiseValueChanged;
            _Value = i;
            if (changed)
                OnValueChanged();
            InvalidateArrange();
        }

        private void SetValue(int i)
        {
            SetValue(i, false);
        }

        protected override string GetMeasureString()
        {
            string s = ConvertToString(_Value, true);
            if (this.IsEmpty && this.AllowEmptyState)
                s = "T";
            else if (this.InputStack == "-" && _Value == 0)
                s = "-";
            return s;
        }

        protected override string GetRenderString()
        {
            if (this.InputStack == "-" && _Value == 0)
                return "-";

            if (this.IsEmpty && this.AllowEmptyState)
                return "";

            string text = ConvertToString(_Value, true);

            return text;
        }

        protected override void NegateValue()
        {
            if (!this.IsEmpty && _MaxValue >= 0)
            {
                int newValue = -_Value;
                SetValueDirect(FormatNumber(newValue));
            }
        }

        protected override void ResetValue()
        {
            int i = 0;
            if (_MinValue > i) i = _MinValue;
            if (i > _MaxValue) i = _MaxValue;
            SetValue(i);
            InvalidateArrange();
        }

        public int MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;

                    if (_MinValue >= 0)
                        this.AllowsNegativeValue = false;
                    else
                        this.AllowsNegativeValue = true;
                    if (_Value < _MinValue && (!this.IsEmpty || this.IsEmpty && !this.AllowEmptyState))
                    {
                        if (this.IsFocused)
                            SetValueDirect(FormatNumber(_MinValue));
                        else
                            SetValue(_MinValue);
                    }
                }
            }
        }

        private string FormatNumber(int i)
        {
            CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
            IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
            if (formatProvider != null)
                return i.ToString(formatProvider);

            return i.ToString();
        }

        public int MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    if (_Value > _MaxValue && (!this.IsEmpty || this.IsEmpty && !this.AllowEmptyState))
                    {
                        if (this.IsFocused)
                            SetValueDirect(FormatNumber(_MaxValue));
                        else
                            SetValue(_MaxValue);
                    }
                }
            }
        }

        protected override void OnClipboardPaste()
        {
            if (Clipboard.ContainsText())
            {
                string s = Clipboard.GetText();
                int value = 0;
                if (int.TryParse(s, out value))
                {
                    if (value > _MaxValue || value < _MinValue) return;
                }
                else
                    return;
            }
            base.OnClipboardPaste();
        }

        protected override bool ValidateNewInputStack(string s)
        {
            if (s.Length > 0)
            {
                if (s == "-" && this.AllowsNegativeValue)
                    return true;
                int value = 0;
                if (int.TryParse(s, out value))
                {
                    if (value > _MaxValue && _MaxValue >= 0 || value < _MinValue && _MinValue < 0)
                        return false;
                    return true;
                }
                else
                    return false;
            }
            return base.ValidateNewInputStack(s);
        }

        public override void IncreaseValue()
        {
            int newValue = _Value + _Increment;

            if (newValue > _MaxValue)
                newValue = _MaxValue;
            else if (newValue < _MinValue)
                newValue = _MinValue;

            if (_Value < _MaxValue && newValue > _MaxValue) newValue = _MaxValue;

            if (newValue <= _MaxValue)
            {
                if (this.IsEmpty && this.AllowEmptyState) newValue = Math.Max(0, _MinValue);

                SetValueDirect(FormatNumber(newValue));
                CheckInputComplete(false);
            }

            base.IncreaseValue();
        }

        public override void DecreaseValue()
        {
            int newValue = _Value - _Increment;

            if (newValue > _MaxValue)
                newValue = _MaxValue;
            else if (newValue < _MinValue)
                newValue = _MinValue;

            if (_Value > _MinValue && newValue < _MinValue) newValue = _MinValue;
            if (newValue >= _MinValue)
            {
                SetValueDirect(FormatNumber(newValue));
                CheckInputComplete(false);
            }
            base.DecreaseValue();
        }

        private int _Increment = 1;
        /// <summary>
        /// Gets or sets the value to increment or decrement the value of the control when the up or down buttons are clicked. 
        /// </summary>
        [DefaultValue(1)]
        public int Increment
        {
            get { return _Increment; }
            set
            {
                value = Math.Abs(value);
                if (_Increment != value)
                {
                    _Increment = value;
                }
            }
        }

        private void SetValueDirect(string s)
        {
            if (SetInputStack(s))
            {
                SetInputPosition(InputStack.Length);
            }
        }

        private string ConvertToString(int i)
        {
            return ConvertToString(i, false);
        }
        private string ConvertToString(int i, bool useFormat)
        {
            return ConvertToString(i, useFormat, false);
        }

        private string ConvertToString(int i, bool useFormat, bool forceFormat)
        {
            if (!forceFormat && (!useFormat || this.IsFocused) || _DisplayFormat.Length == 0)
                return FormatNumber(i);
            else
            {
                CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
                IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
                if (formatProvider != null)
                    return i.ToString(_DisplayFormat, formatProvider);
                
                return i.ToString(_DisplayFormat);
            }
        }

        [Browsable(false)]
        public virtual string Text
        {
            get
            {
                if (this.IsEmpty && this.AllowEmptyState) return "";
                return ConvertToString(_Value, true, true);
            }
        }

        public virtual int Value
        {
            get
            {
                if (_Value < _MinValue)
                    return _MinValue;
                if (_Value > _MaxValue)
                    return _MaxValue;
                return _Value;
            }
            set
            {
                //if (_Value != value)
                {
                    if (value < _MinValue) value = _MinValue;
                    if (value > _MaxValue) value = _MaxValue;
                    SetValueDirect(ConvertToString(value));
                    if (this.IsFocused)
                        InputComplete(false);
                }
            }
        }

        protected override string GetInputStringValue()
        {
            return ConvertToString(_Value);
        }

        private string _DisplayFormat = "";
        /// <summary>
        /// Gets or sets the Numeric String Format that is used to format the numeric value entered for display purpose.
        /// </summary>
        [Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
        public string DisplayFormat
        {
            get { return _DisplayFormat; }
            set
            {
                if (value != null)
                    _DisplayFormat = value;
            }
        }

        #endregion

    }
}
#endif

