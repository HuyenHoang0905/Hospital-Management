#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors
{
    public class IpAddressGroup : VisualInputGroup
    {
        #region Events
        public event EventHandler ValueChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the IPAddressGroup class.
        /// </summary>
        public IpAddressGroup()
        {
            // Create IP4 default items
            for (int i = 0; i < 4; i++)
            {
                VisualIntegerInput input = new VisualIntegerInput();
                input.MaxValue = 255;
                input.MinValue = 0;
                this.Items.Add(input);
                if (i < 3)
                {
                    VisualLabel label = new VisualLabel();
                    label.Text = ".";
                    this.Items.Add(label);
                }
            }
        }
        #endregion

        #region Internal Implementation
        private string _Value;
        public string Value
        {
            get 
            {
                if (this.IsFocused)
                {
                    if (IsInputValid())
                        return GetCurrentInputValue();
                }
                return _Value; 
            }
            set
            {
                bool valueChanged = _Value != value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (!IsValueValid(value))
                        throw new ArgumentException("Value is not valid IP value");
                }
                _Value = value;
                UpdateInputItems();
                if(valueChanged)
                    OnValueChanged(EventArgs.Empty);
            }
        }

        bool _UpdatingInputItems = false;
        private void UpdateInputItems()
        {
            if (_UpdatingInputItems) return;

            _UpdatingInputItems = true;
            try
            {
                if (string.IsNullOrEmpty(_Value))
                {
                    // Clear all input
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (this.Items[i] is VisualIntegerInput)
                        {
                            VisualIntegerInput input = this.Items[i] as VisualIntegerInput;
                            input.IsEmpty = true;
                        }
                    }
                }
                else
                {
                    int[] value = ParseIPv4Value(_Value);
                    int index = 0;
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (this.Items[i] is VisualIntegerInput)
                        {
                            VisualIntegerInput input = this.Items[i] as VisualIntegerInput;
                            input.Value = value[index];
                            index++;
                        }
                    }
                }
            }
            finally
            {
                _UpdatingInputItems = false;
            }
        }

        private void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null) ValueChanged(this, e);
        }

        internal bool IsValueValid(string ipValue)
        {
            int[] value = ParseIPv4Value(ipValue);
            return value != null;
        }

        private int[] ParseIPv4Value(string ipValue)
        {
            if (string.IsNullOrEmpty(ipValue)) return null;
            string[] parts = ipValue.Split('.');
            if (parts.Length != 4) return null;

            int[] value = new int[4];

            for (int i = 0; i < parts.Length; i++)
            {
                int partValue = 0;
                if (int.TryParse(parts[i], out partValue))
                {
                    if (partValue < 0 || partValue > 255) return null;
                    value[i] = partValue;
                }
                else return null;
            }

            return value;
        }

        private bool IsInputValid()
        {
            bool allEmpty = true;
            for (int i = 0; i < this.Items.Count; i++)
            {
                VisualIntegerInput input = this.Items[i] as VisualIntegerInput;
                if (input != null)
                {
                    if (input.IsEmpty)
                    {
                        if (!allEmpty && AllowEmptyState) return false;
                    }
                    else
                    {
                        if (allEmpty && i > 0) return false;
                        allEmpty = false;
                    }
                }
            }

            return true;
        }

        protected override void OnLostFocus()
        {
            UpdateValue();
            base.OnLostFocus();
        }

        protected override void OnInputChanged(VisualInputBase input)
        {
            if (!_UpdatingInputItems && GetCurrentInputValue() != null)
            {
                _UpdatingInputItems = true; // Stop update of UI items
                try
                {
                    UpdateValue();
                }
                finally
                {
                    _UpdatingInputItems = false;
                }
            }
            base.OnInputChanged(input);
        }

        private string GetCurrentInputValue()
        {
            int[] value = new int[4];
            int index = 0;
            bool isInvalid = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                VisualIntegerInput input = this.Items[i] as VisualIntegerInput;
                if (input != null)
                {
                    if (input.IsEmpty)
                    {
                        if (!this.AllowEmptyState)
                            value[index] = 0;
                        else
                        {
                            isInvalid = true;
                            break;
                        }
                    }
                    value[index] = input.Value;
                    index++;
                }
            }

            if (isInvalid)
                return null;
            else
            {
                string s = "";
                for (int i = 0; i < value.Length; i++)
                {
                    s += value[i].ToString();
                    if (i < value.Length - 1)
                        s += ".";

                }
                return s;
            }
        }

        private void UpdateValue()
        {
            Value = GetCurrentInputValue();
        }

        private bool _ResettingValue = false;
        protected override void ResetValue()
        {
            _ResettingValue = true;
            try
            {
                this.Value = null;
            }
            finally
            {
                _ResettingValue = false;
            }
        }
        #endregion


    }
}
#endif