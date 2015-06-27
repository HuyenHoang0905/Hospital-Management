#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Editors
{
    public class VisualDoubleInput : VisualNumericInput
    {
        #region Private Variables
        private double _Value = 0;
        private double _MinValue = double.MinValue;
        private double _MaxValue = double.MaxValue;
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void OnLostFocus()
        {
            ValidateValue();
            base.OnLostFocus();
        }

        protected virtual void ValidateValue()
        {
            if (_Value < _MinValue)
                Value = _MinValue;
            else if (_Value > _MaxValue)
                Value = _MaxValue;
        }

        protected override bool AcceptKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            string decimalSeparator = NumberDecimalSeparator;
            if (e.KeyChar.ToString() == decimalSeparator)
            {
                if (!this.InputStack.Contains(decimalSeparator))
                    return true;
                else
                    return false;
            }

            return base.AcceptKeyPress(e);
        }

        protected override string ProcessNewInputStack(string s)
        {
            if (this.InputStack == "0" && s != "0" && s.StartsWith("0") && s.EndsWith(NumberDecimalSeparator))
                return s;
            return base.ProcessNewInputStack(s);
        }

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
                else if (this.InputStack == NumberDecimalSeparator || this.InputStack == "-" + NumberDecimalSeparator)
                {
                    SetValue(0, true);
                    return;
                }

                SetValue(Parse(this.InputStack), true);
            }
            else
            {
                if (this.AllowEmptyState)
                    this.IsEmpty = true;
                SetValue(0);
            }

            base.OnInputStackChanged();
        }

        private double Parse(string s)
        {
            CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
            IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
            if (formatProvider != null)
                return double.Parse(s, formatProvider);
            return double.Parse(s);
        }

        protected override void OnInputKeyAccepted()
        {
            CheckInputComplete(true);
            base.OnInputKeyAccepted();
        }

        private void CheckInputComplete(bool sendNotification)
        {
            string predictedStack1 = "", predictedStack2 = "";
            if (this.InputStack.Length > 0)
            {
                if (this.InputStack.Contains(NumberDecimalSeparator))
                {
                    predictedStack1 = this.InputStack + "1";
                    predictedStack2 = predictedStack1;
                }
                else
                {
                    predictedStack1 = this.InputStack + NumberDecimalSeparator;
                    predictedStack2 = this.InputStack + "0";
                }
            }

            if (this.InputStack == "-" || this.InputStack.StartsWith(NumberDecimalSeparator) || this.InputStack == "-" + NumberDecimalSeparator)
                return;

            if (_Value == _MaxValue ||
                !ValidateNewInputStack(predictedStack1) && !ValidateNewInputStack(predictedStack2))
                InputComplete(sendNotification);
        }

        private void SetValue(double i, bool raiseValueChanged)
        {
            bool changed = _Value != i || raiseValueChanged;
            _Value = i;
            
            if (changed)
                OnValueChanged();

            InvalidateArrange();
        }

        private void SetValue(double i)
        {
            SetValue(i, false);
        }

        protected override string GetMeasureString()
        {
            string s = GetRenderString();
            if (this.IsEmpty && this.AllowEmptyState)
                s = "T";
            else if (this.InputStack == "-" && _Value == 0)
                s = "-";
            else if ((this.InputStack == NumberDecimalSeparator || this.InputStack == "-" + NumberDecimalSeparator) && _Value == 0)
                return this.InputStack;

            return s;
        }

        protected override string GetRenderString()
        {
            if (this.InputStack == "-" && _Value == 0)
                return "-";
            else if ((this.InputStack == NumberDecimalSeparator || this.InputStack == "-" + NumberDecimalSeparator) && _Value == 0)
                return this.InputStack;
            else if (this.IsFocused && this.InputStack.Length > 0)
                return this.InputStack;

            string text = "";

            text = ConvertToString(_Value, true);

            return text;
        }

        protected override void NegateValue()
        {
            if (_MaxValue < 0) return;
            double newValue = -_Value;
            SetValueDirect(FormatNumber(newValue));
        }

        protected override void ResetValue()
        {
            double v = 0;
            if (_MinValue > v) v = _MinValue;
            if (v > _MaxValue) v = _MaxValue;
            SetValue(v);
            InvalidateArrange();
        }

        public double MinValue
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
                    ValidateValue();
                }
            }
        }

        public double MaxValue
        {
            get { return _MaxValue; }
            set 
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    ValidateValue();
                }
            }
        }

        private bool TryParse(string s, out double value)
        {
            CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
            IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
            if (formatProvider != null)
                return double.TryParse(s, NumberStyles.Number, formatProvider, out value);
            return double.TryParse(s, out value);
        }

        protected override void OnClipboardPaste()
        {
            if (Clipboard.ContainsText())
            {
                string s = Clipboard.GetText();
                double value = 0;
                if (TryParse(s, out value))
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
                else if (s == NumberDecimalSeparator || s == "-" + NumberDecimalSeparator)
                    return true;
                
                double value = 0;
                if (TryParse(s, out value))
                {
                    if (value > _MaxValue && _MaxValue >= 0 || value < _MinValue && _MinValue < 0)
                        return false;
                    // Check number of decimal places, do not allow entry of more decimals than needed
                    string formattedValue = ConvertToString(value, true, true);
                    if (s.Contains(NumberDecimalSeparator) && formattedValue.Contains(NumberDecimalSeparator))
                    {
                        int decimalPlaces = GetNumberOfDecimalPlaces(); // formattedValue.Substring(formattedValue.IndexOf(NumberDecimalSeparator) + 1).Length;
                        int inputDecimalPlaces = s.Substring(s.IndexOf(NumberDecimalSeparator) + 1).Length;
                        if (decimalPlaces > 0 && inputDecimalPlaces > decimalPlaces /*&& !s.EndsWith("0")*/)
                            return false;
                    }

                    return true;
                }
                else
                    return false;
            }
            return base.ValidateNewInputStack(s);
        }

        private int GetNumberOfDecimalPlaces()
        {
            if (this.DisplayFormat == null || this.DisplayFormat.Length == 0)
            {
                CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
                IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
                if (formatProvider != null && formatProvider is NumberFormatInfo)
                {
                    NumberFormatInfo nfi = (NumberFormatInfo)formatProvider;
                    return nfi.NumberDecimalDigits;
                }
            }
            else
            {
                string s = ConvertToString(11.111111111111d, true, true);
                return s.Substring(s.IndexOf(NumberDecimalSeparator) + 1).Length;
            }

            return 2;
        }

        public override void IncreaseValue()
        {
            double newValue = _Value + _Increment;

            if (newValue > _MaxValue)
                newValue = _MaxValue;
            else if (newValue < _MinValue)
                newValue = _MinValue;

            if (_Value < _MaxValue && newValue > _MaxValue) newValue = _MaxValue;
            if (newValue <= _MaxValue)
            {
                if (this.IsEmpty && this.AllowEmptyState) newValue = Math.Max(0, _MinValue);

                SetValueDirect(StripNonNumeric(FormatNumber(newValue)));
                CheckInputComplete(false);
            }

            base.IncreaseValue();
        }

        public override void DecreaseValue()
        {
            double newValue = _Value - _Increment;

            if (newValue > _MaxValue)
                newValue = _MaxValue;
            else if (newValue < _MinValue)
                newValue = _MinValue;

            if (_Value > _MinValue && newValue < _MinValue) newValue = _MinValue;
            if (newValue >= _MinValue)
            {
                SetValueDirect(StripNonNumeric(FormatNumber(newValue)));
                CheckInputComplete(false);
            }

            base.DecreaseValue();
        }

        private double _Increment = 1;
        /// <summary>
        /// Gets or sets the value to increment or decrement the value of the control when the up or down buttons are clicked. 
        /// </summary>
        [DefaultValue(1)]
        public double Increment
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
            string formattedValue = s;
            if (s.Contains(NumberDecimalSeparator) && formattedValue.Contains(NumberDecimalSeparator))
            {
                int decimalPlaces = GetNumberOfDecimalPlaces(); // formattedValue.Substring(formattedValue.IndexOf(NumberDecimalSeparator) + 1).Length;
                int inputDecimalPlaces = s.Substring(s.IndexOf(NumberDecimalSeparator) + 1).Length;
                if (decimalPlaces > 0 && inputDecimalPlaces > decimalPlaces)
                    s = s.Substring(0, s.IndexOf(NumberDecimalSeparator) + 1 + decimalPlaces);
            }

            if (SetInputStack(s))
            {
                SetInputPosition(InputStack.Length);
            }
        }

        private string ConvertToString(double d)
        {
            return ConvertToString(d, false, false);
        }

        private string ConvertToString(double d, bool useFormat)
        {
            return ConvertToString(d, useFormat, false);
        }

        private string ConvertToString(double d, bool useFormat, bool forceDisplayFormat)
        {
            string s = FormatNumber(d);
            if (!forceDisplayFormat && (_DisplayFormat.Length == 0 || this.IsFocused || !useFormat))
            {
                if (this.InputStack.Contains(NumberDecimalSeparator))
                {
                    if (!s.Contains(NumberDecimalSeparator))
                        s += this.InputStack.Substring(this.InputStack.IndexOf(NumberDecimalSeparator));
                    else if (s.Substring(s.IndexOf(NumberDecimalSeparator)) != this.InputStack.Substring(this.InputStack.IndexOf(NumberDecimalSeparator)))
                        s = s.Substring(0, s.IndexOf(NumberDecimalSeparator)) + this.InputStack.Substring(this.InputStack.IndexOf(NumberDecimalSeparator));
                }
            }
            else if (_DisplayFormat.Length > 0)
            {
                try
                {
                    CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
                    IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
                    if (formatProvider != null)
                        s = d.ToString(_DisplayFormat, formatProvider);
                    else
                        s = d.ToString(_DisplayFormat);
                }
                catch
                {
                    s = d.ToString();
                }
            }

            return s;
        }

        private string FormatNumber(double d)
        {
            string format = null;
            CultureInfo ci = DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture();
            
            if (!string.IsNullOrEmpty(DisplayFormat) && DisplayFormat.ToLower()!="p" && this.IsFocused)
                format = DisplayFormat;
            else
            {
                format = "n";
                int places = ci.NumberFormat.NumberDecimalDigits; // GetDecimalPlaces(d);
                if (places > 0)
                    format += places.ToString();
            }

            
            IFormatProvider formatProvider = ci.GetFormat(typeof(NumberFormatInfo)) as IFormatProvider;
            if (formatProvider != null)
            {
                if (format != null) return d.ToString(format, formatProvider);
                return d.ToString(formatProvider);
            }

            if (format != null) return d.ToString(format);
            return d.ToString();
        }

        private int GetDecimalPlaces(double d)
        {
            double dec = d - Math.Truncate(d);
            if (dec == 0) return 0;
            int places = 0;
            while (dec < 1)
            {
                dec *= 10;
                places++;
            }
            return places;
        }

        public double Value
        {
            get
            {
                if (_Value < _MinValue)
                    return _MinValue;
                else if (_Value > _MaxValue)
                    return _MaxValue;

                return _Value;
            }
            set
            {
                //if (_Value != value)
                {
                    if (value < _MinValue) value = _MinValue;
                    if (value > _MaxValue) value = _MaxValue;
                    if (this.IsFocused)
                    {
                        SetValueDirect(StripNonNumeric(FormatNumber(value)));
                        if (this.IsFocused)
                            InputComplete(false);
                    }
                    else
                    {
                        SetValue(value, true);
                        IsEmpty = false;
                    }
                }
            }
        }

        private string StripNonNumeric(string p)
        {
            string s = "";
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].ToString() == NumberDecimalSeparator || p[i] >= '0' && p[i] <= '9' || i == 0 && p[i] == '-')
                    s += p[i];
                else if (s.Length > 0 && p[i].ToString() != NumberThousandsSeparator) break;
            }
            return s;
        }

        private string _DisplayFormat = "";
        /// <summary>
        /// Gets or sets the Numeric String Format that is used to format the numeric value entered for display purpose.
        /// </summary>
        [DefaultValue("")]
        public string DisplayFormat
        {
            get { return _DisplayFormat; }
            set
            {
                if (value != null)
                    _DisplayFormat = value;
            }
        }

        private string NumberDecimalSeparator
        {
            get { return DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator; }
        }

        private string NumberThousandsSeparator
        {
            get { return DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator; }
        }

        protected override string GetInputStringValue()
        {
            return ConvertToString(_Value);
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
        #endregion

    }
}
#endif

