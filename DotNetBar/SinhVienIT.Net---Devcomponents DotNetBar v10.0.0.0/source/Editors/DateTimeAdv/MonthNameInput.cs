#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv
{
    public class MonthNameInput : VisualStringListInput, IDateTimePartInput
    {
        #region Private Variables
        private List<string> _Months = null;
        private int _MinValue = 1;
        private int _MaxValue = 12;
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override List<string> GetItems()
        {
            if (this.Items.Count != 12)
            {
                if (_Months == null)
                {
                    _Months = new List<string>(12);
                    if (_UseAbbreviatedNames)
                        _Months.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedMonthNames);
                    else
                        _Months.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames);
                    if (_Months.Count == 13 && _Months[12] == "") _Months.RemoveAt(12);
                }
                return _Months;
            }
            return base.GetItems();
        }

        protected override bool ValidateNewInputStack(string s)
        {
            if (s.Length > 0)
            {
                // Parse also numeric input and map it to the month name
                int month = 0;
                int.TryParse(s, out month);
                if (month > 0 && month >= _MinValue && month <= _MaxValue)
                {
                    List<string> items = GetItems();
                    this.LastMatch = items[month - 1];
                    this.LastValidatedInputStack = s;
                    if (month > 1)
                        this.LastMatchComplete = true;
                    return true;
                }
            }

            bool b = base.ValidateNewInputStack(s);
            if (b && LastValidatedInputStack.Length > 0)
            {
                List<string> items = GetItems();
                int index = items.IndexOf(LastMatch) + 1;
                if (index < _MinValue || index > _MaxValue)
                    return false;
            }

            return b;
        }
        #endregion


        #region IDateTimePartInput Members

        int IDateTimePartInput.Value
        {
            get
            {
                return this.SelectedIndex + 1;
            }
            set
            {
                this.SelectedIndex = value - 1;
            }
        }

        int IDateTimePartInput.MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                    if (!this.IsEmpty && SelectedIndex < _MinValue)
                        this.SelectedIndex = _MinValue;
                }
            }
        }

        int IDateTimePartInput.MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    if (!this.IsEmpty && SelectedIndex > _MaxValue)
                        this.SelectedIndex = _MaxValue;
                }
            }
        }

        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.Month; }
        }

        private bool _UseAbbreviatedNames = false;
        /// <summary>
        /// Gets or sets whether abbreviated month names are used for display instead of full month names. Default value is false.
        /// </summary>
        [DefaultValue(false)]
        public bool UseAbbreviatedNames
        {
            get { return _UseAbbreviatedNames; }
            set
            {
                if (_UseAbbreviatedNames != value)
                {
                    _UseAbbreviatedNames = value;
                    _Months = null;
                    InvalidateArrange();
                }
            }
        }
        #endregion
    }
}
#endif

