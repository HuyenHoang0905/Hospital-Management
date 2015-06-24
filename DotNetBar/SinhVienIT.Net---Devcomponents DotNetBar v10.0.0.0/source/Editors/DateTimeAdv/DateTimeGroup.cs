#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.Editors.DateTimeAdv
{
    public class DateTimeGroup : VisualInputGroup
    {
        #region Private Variables
        private IDateTimePartInput _DayInput = null;
        private IDateTimePartInput _MonthInput = null;
        private IDateTimePartInput _YearInput = null;
        private IDateTimePartInput _HourInput = null;
        private IDateTimePartInput _MinuteInput = null;
        private IDateTimePartInput _SecondInput = null;
        private IDateTimePartInput _DayOfYearInput = null;
        /// <summary>
        /// Gets the minimum date value of the DateTimePicker control. 
        /// </summary>
        public static readonly System.DateTime MinDateTime = new System.DateTime(1753, 1, 1);
        /// <summary>
        /// Specifies the maximum date value of the DateTimePicker control. This field is read-only.
        /// </summary>
        public static readonly System.DateTime MaxDateTime = new System.DateTime(9998, 12, 31);
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Value or IsEmpty property has changed.
        /// </summary>
        public event EventHandler ValueChanged;
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
        {
            if (collectionChangedInfo.ChangeType == eCollectionChangeType.Cleared)
            {
                _DayInput = null;
                _MonthInput = null;
                _YearInput = null;
                _HourInput = null;
                _MinuteInput = null;
                _SecondInput = null;
                _DayOfYearInput = null;
            }

            if (collectionChangedInfo.Removed != null)
            {
                foreach (VisualItem item in collectionChangedInfo.Removed)
                {
                    if (item == _DayInput)
                        _DayInput = null;
                    else if (item == _MonthInput)
                        _MonthInput = null;
                    else if (item == _YearInput)
                        _YearInput = null;
                    else if (item == _HourInput)
                        _HourInput = null;
                    else if (item == _MinuteInput)
                        _MinuteInput = null;
                    else if (item == _SecondInput)
                        _SecondInput = null;
                    else if (item == _DayOfYearInput)
                        _DayOfYearInput = null;
                }
            }

            if (collectionChangedInfo.Added != null)
            {
                List<VisualItem> hourLabels = new List<VisualItem>();
                foreach (VisualItem item in collectionChangedInfo.Added)
                {
                    if (item is HourPeriodLabel)
                    {
                        if (_HourInput != null && _HourInput is NumericHourInput)
                            ((HourPeriodLabel)item).HourInput = (NumericHourInput)_HourInput;
                    }
                    else if (item is HourPeriodInput)
                    {
                        if (_HourInput != null && _HourInput is NumericHourInput)
                            ((HourPeriodInput)item).HourInput = (NumericHourInput)_HourInput;
                    }

                    IDateTimePartInput idp = item as IDateTimePartInput;
                    if (idp == null)
                        continue;
                    if (idp.Part == eDateTimePart.Day)
                        _DayInput = idp;
                    else if (idp.Part == eDateTimePart.Hour)
                    {
                        _HourInput = idp;
                        if (_HourInput is NumericHourInput)
                        {
                            foreach (VisualItem vi in this.Items)
                            {
                                if (vi is HourPeriodLabel)
                                    ((HourPeriodLabel)vi).HourInput = (NumericHourInput)_HourInput;
                                else if (vi is HourPeriodInput)
                                    ((HourPeriodInput)vi).HourInput = (NumericHourInput)_HourInput;
                            }
                        }
                    }
                    else if (idp.Part == eDateTimePart.Minute)
                        _MinuteInput = idp;
                    else if (idp.Part == eDateTimePart.Month)
                        _MonthInput = idp;
                    else if (idp.Part == eDateTimePart.Second)
                        _SecondInput = idp;
                    else if (idp.Part == eDateTimePart.Year)
                        _YearInput = idp;
                    else if (idp.Part == eDateTimePart.DayOfYear)
                        _DayOfYearInput = idp;

                }
            }

            UpdateInnerDateTimeGroupMinMax();
            UpdateInputItems();

            base.OnItemsCollectionChanged(collectionChangedInfo);
        }

        private bool _ProcessingInputChanged = false;

        protected override void OnInputChanged(VisualInputBase input)
        {
            if (!_ProcessingInputChanged && !_ResettingDateValue)
            {
                try
                {
                    _ProcessingInputChanged = true;

                    if (input == _MonthInput || input == _YearInput)
                    {
                        if (_DayInput != null)
                        {
                            // Assign day min max values
                            UpdateDayMaxValue();
                        }
                    }
                    if (input == _YearInput && _DayOfYearInput != null)
                        UpdateDayOfYearMaxValue();

                    if (input != _YearInput && _YearInput != null && _YearInput.IsEmpty && _DefaultInputValues)
                        _YearInput.Value = Math.Min(System.DateTime.Now.Year, _MaxDate.Year);
                    if (input != _MonthInput && _MonthInput != null && _MonthInput.IsEmpty)
                    {
                        if (_DefaultInputValues)
                        {
                            if (_YearInput != null && _YearInput.Value == _MaxDate.Year)
                                _MonthInput.Value = _MaxDate.Month;
                            else
                                _MonthInput.Value = System.DateTime.Now.Month;
                        }
                        // Assign day min max values
                        UpdateDayMaxValue();
                    }
                    if (input != _DayInput && _DayInput != null && _DayInput.IsEmpty && _DefaultInputValues)
                    {
                        if (_YearInput != null && _YearInput.Value == _MaxDate.Year && _MonthInput != null && _MonthInput.Value == _MaxDate.Month)
                            _DayInput.Value = _MaxDate.Day;
                        else
                            _DayInput.Value = System.DateTime.Now.Day;
                    }
                    if (input != _HourInput && _HourInput != null && _HourInput.IsEmpty && _DefaultInputValues)
                    {
                        _HourInput.Value = 0; // System.DateTime.Now.Hour;
                    }
                    if (input != _MinuteInput && _MinuteInput != null && _MinuteInput.IsEmpty && _DefaultInputValues)
                    {
                        _MinuteInput.Value = 0; // System.DateTime.Now.Minute;
                    }
                    if (input != _SecondInput && _SecondInput != null && _SecondInput.IsEmpty && _DefaultInputValues)
                    {
                        _SecondInput.Value = 0;
                    }

                    // Reset empty flag
                    if (_YearInput != null && !_YearInput.IsEmpty || _MonthInput != null && !_MonthInput.IsEmpty || _DayInput != null && !_DayInput.IsEmpty
                        || _HourInput != null && !_HourInput.IsEmpty || _MinuteInput != null && !_MinuteInput.IsEmpty || _SecondInput != null && !_SecondInput.IsEmpty)
                        this.IsEmpty = false;
                    SyncValues(input);
                }
                finally
                {
                    _ProcessingInputChanged = false;
                }
                if (IsUserInput && IsCurrentInputValid()) OnValueChanged();
            }

            base.OnInputChanged(input);
        }

        private void UpdateDayOfYearMaxValue()
        {
            if (_YearInput != null && !_YearInput.IsEmpty && _DayOfYearInput != null)
            {
                _DayOfYearInput.MaxValue = DateTimeInput.GetActiveCulture().Calendar.GetDaysInYear(_YearInput.Value);
            }
        }

        protected override void OnFocusedItemChanged(VisualItem previousFocus)
        {
            if (this.FocusedItem == _DayInput && _DayInput is NumericDayInput)
            {
                UpdateDayMaxValue();
            }
            else if (previousFocus == _YearInput || previousFocus == _DayInput)
            {
                UpdateDayMaxValue();
            }
            else
            {
                SyncValues(previousFocus);
            }

            base.OnFocusedItemChanged(previousFocus);
        }

        private void UpdateDayMaxValue()
        {
            if (_DayInput != null && (this.FocusedItem == _DayInput || this.FocusedItem == _MonthInput) && _DayInput is NumericDayInput)
                _DayInput.MaxValue = 31;
            else if (_YearInput != null && _MonthInput != null && !_MonthInput.IsEmpty && _DayInput != null)
            {
                // Assign day min max values
                _DayInput.MaxValue = System.DateTime.DaysInMonth(_YearInput.IsEmpty ? System.DateTime.Now.Year : _YearInput.Value, _MonthInput.Value);
            }
        }

        protected override void OnInputComplete()
        {
            SyncValues(this.FocusedItem);
            base.OnInputComplete();
        }

        //private bool _Validating = false;
        //protected override bool ValidateInput(VisualItem inputItem)
        //{
        //    if (!_Validating && inputItem is IDateTimePartInput && !this.IsEmpty)
        //    {
        //        IDateTimePartInput input = (IDateTimePartInput)inputItem;
        //        System.DateTime v = GetCurrentInputValue();
        //        if (v < _MinDate || v > _MaxDate)
        //        {
        //            try
        //            {
        //                _Validating = true;
        //                input.UndoInput();
        //            }
        //            finally
        //            {
        //                _Validating = false;
        //            }
        //            return false;
        //        }
        //    }
        //    return base.ValidateInput(inputItem);
        //}

        private void SyncValues(VisualItem visualItem)
        {
            IDateTimePartInput inputPart = visualItem as IDateTimePartInput;
            if (inputPart == null)
                return;
            for (int i = 0; i < this.Items.Count; i++)
            {
                IDateTimePartInput part = this.Items[i] as IDateTimePartInput;
                if (part == null) continue;

                if (part.Part == inputPart.Part && part != inputPart)
                    part.Value = inputPart.Value;
                else if (part != inputPart && inputPart.Part == eDateTimePart.DayOfYear && (part.Part == eDateTimePart.Day || part.Part == eDateTimePart.Month))
                {
                    if (_YearInput != null && !_YearInput.IsEmpty)
                    {
                        System.DateTime d = CreateDateTime(_YearInput.Value, inputPart.Value);
                        if (part.Part == eDateTimePart.Day)
                            part.Value = d.Day;
                        else if (part.Part == eDateTimePart.Month)
                            part.Value = d.Month;
                    }
                }
                else if (part.Part == eDateTimePart.DayName)
                {
                    if (_YearInput != null && !_YearInput.IsEmpty &&
                        (_MonthInput != null && !_MonthInput.IsEmpty && _DayInput != null && !_DayInput.IsEmpty ||
                        _DayOfYearInput != null && !_DayOfYearInput.IsEmpty))
                    {
                        try
                        {
                            System.DateTime date;
                            if (_DayOfYearInput != null)
                                date = CreateDateTime(_YearInput.Value, _DayOfYearInput.Value);
                            else
                                date = new System.DateTime(_YearInput.Value, _MonthInput.Value, _DayInput.Value);
                            part.Value = (int)date.DayOfWeek;
                        }
                        catch
                        {
                            part.IsEmpty = true;
                        }
                    }
                    else
                        part.IsEmpty = true;
                }
            }


        }

        internal static System.DateTime CreateDateTime(int year, int dayOfYear)
        {
            System.DateTime d = new System.DateTime(year, 1, 1);
            if (dayOfYear > 0)
            {
                d.AddDays(dayOfYear - 1);
            }

            return d;
        }

        protected override void OnLostFocus()
        {
            UpdateValue(false);

            base.OnLostFocus();
        }

        protected override void OnGroupInputComplete()
        {
            UpdateValue(false);
            base.OnGroupInputComplete();
        }

        protected internal void UpdateValue(bool updateDirect)
        {
            // Validate Complete Input
            System.DateTime v = System.DateTime.Now;

            if (_YearInput != null && _YearInput.IsEmpty || _MonthInput != null && _MonthInput.IsEmpty ||
                _DayInput != null && _DayInput.IsEmpty || _HourInput != null && _HourInput.IsEmpty ||
                _MinuteInput != null && _MinuteInput.IsEmpty || _SecondInput != null && _SecondInput.IsEmpty)
            {
                if (HasNestedGroups)
                    UpdateIsEmpty();
                else
                {
                    if (!this.IsFocused || _DefaultInputValues)
                        this.IsEmpty = true;
                }
            }
            else
            {
                v = GetCurrentInputValue();

                if (v > _MaxDate)
                    v = _MaxDate;
                else if (v < _MinDate)
                    v = _MinDate;

                if (updateDirect)
                    _Value = v;
                else
                    Value = v;
            }
        }

        private bool IsCurrentInputValid()
        {
            DateTime d = GetCurrentInputValue();
            if (d < _MinDate || d > _MaxDate) return false;
            return true;
        }

        private bool HasNestedGroups
        {
            get
            {
                foreach (VisualItem item in this.Items)
                {
                    if (item is VisualGroup)
                        return true;
                }
                return false;
            }
        }

        private System.DateTime GetCurrentInputValue()
        {
            System.DateTime v = System.DateTime.Now;
            if (_Value != DateTime.MinValue) v = _Value;
            if (v < _MinDate)
                v = _MinDate;
            else if (v > _MaxDate)
                v = _MaxDate;

            int year = v.Year, month = v.Month, day = v.Day, hour = v.Hour, minute = v.Minute, second = v.Second, dayOfYear = v.DayOfYear;

            if (_YearInput != null)
            {
                year = _YearInput.Value;
                if (year < 30)
                    year += 2000;
                else if (year < 100)
                    year += 1900;
            }
            else if (_Value != DateTime.MinValue)
                year = _Value.Year;

            if (_MonthInput != null)
                month = _MonthInput.Value;
            else if (_Value != DateTime.MinValue)
                month = _Value.Month;

            if (_DayInput != null)
            {
                day = _DayInput.Value;
                if (_YearInput != null && _MonthInput != null)
                    day = Math.Min(day, System.DateTime.DaysInMonth(_YearInput.IsEmpty ? System.DateTime.Now.Year : _YearInput.Value, _MonthInput.Value));
            }
            else if (_Value != DateTime.MinValue)
                day = _Value.Day;

            if (_HourInput != null)
            {
                hour = _HourInput.Value;
                if (hour == 12 && _HourInput is NumericHourInput && ((NumericHourInput)_HourInput).Period == eHourPeriod.AM && !((NumericHourInput)_HourInput).Is24HourFormat)
                    hour = 0;
                else if (hour < 12 && _HourInput is NumericHourInput && ((NumericHourInput)_HourInput).Period == eHourPeriod.PM && !((NumericHourInput)_HourInput).Is24HourFormat)
                {
                    hour += 12;
                }
            }
            else if (_Value != DateTime.MinValue)
                hour = _Value.Hour;

            if (_MinuteInput != null)
                minute = _MinuteInput.Value;
            else if (_Value != DateTime.MinValue)
                minute = _Value.Minute;

            if (_SecondInput != null)
                second = _SecondInput.Value;
            else if (_Value != DateTime.MinValue)
                second = _Value.Second;
            else
                second = 0;

            if (_DayOfYearInput != null)
            {
                dayOfYear = _DayOfYearInput.Value;
                if (dayOfYear > 0)
                {
                    System.DateTime d = CreateDateTime(year, dayOfYear);
                    month = d.Month;
                    day = d.Day;
                }
            }

            // Correct day for leap year etc. case
            if (System.DateTime.DaysInMonth(year, month) < day)
                day = System.DateTime.DaysInMonth(year, month);

            if (_HourInput != null || _MinuteInput != null || _SecondInput != null || _Value != DateTime.MinValue)
                v = new System.DateTime(year, month, day, hour, minute, second);
            else
                v = new System.DateTime(year, month, day);

            return v;
        }

        private System.DateTime _Value;
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public System.DateTime Value
        {
            get
            {
                if (this.IsFocused) this.UpdateValue(true);
                return _Value;
            }
            set
            {
                bool changed = _Value != value;
                _Value = value;
                this.IsEmpty = false;
                UpdateInputItems();
                if(changed)
                    OnValueChanged();
            }
        }

        private bool _InValueChanged = false;
        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (_InValueChanged) return;
            try
            {
                _InValueChanged = true;
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
                if (this.Parent is DateTimeGroup)
                    ((DateTimeGroup)this.Parent).OnValueChanged();
                this.InvalidateArrange();
            }
            finally
            {
                _InValueChanged = false;
            }
        }

        /// <summary>
        /// Gets or sets the values of the nested DateTimeGroup items.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.DateTime[] Values
        {
            get
            {
                if (HasNestedGroups)
                {
                    List<System.DateTime> dates = new List<System.DateTime>();
                    foreach (VisualItem item in this.Items)
                    {
                        DateTimeGroup group = item as DateTimeGroup;
                        if (group != null)
                        {
                            dates.Add(group.Value);
                        }
                    }

                    return dates.ToArray();
                }

                return new System.DateTime[] { this.Value };
            }
            set
            {
                if (value == null || value.Length == 0) return;

                if (HasNestedGroups)
                {
                    int i = 0;
                    foreach (VisualItem item in this.Items)
                    {
                        DateTimeGroup group = item as DateTimeGroup;
                        if (group != null)
                        {
                            group.Value = value[i];
                            i++;
                            if (i >= value.Length) break;
                        }
                    }
                }
                else
                    this.Value = value[0];
            }
        }

        private void UpdateInputItems()
        {
            System.DateTime d = _Value;
            if (_YearInput != null)
            {
                if (this.IsEmpty)
                    _YearInput.IsEmpty = true;
                else
                    _YearInput.Value = d.Year;
            }
            if (_MonthInput != null)
            {
                if (this.IsEmpty)
                    _MonthInput.IsEmpty = true;
                else
                    _MonthInput.Value = d.Month;
            }
            if (_DayInput != null)
            {
                if (this.IsEmpty)
                    _DayInput.IsEmpty = true;
                else if (!(this.FocusedItem == _DayInput && _DayInput is NumericDayInput && _ProcessingInputChanged))
                    _DayInput.Value = d.Day;
            }
            if (_HourInput != null)
            {
                if (this.IsEmpty)
                    _HourInput.IsEmpty = true;
                else
                {
                    if (_HourInput is NumericHourInput)
                        ((NumericHourInput)_HourInput).Period = eHourPeriod.AM;
                    _HourInput.Value = d.Hour;
                    if (_HourInput is NumericHourInput && !((NumericHourInput)_HourInput).Is24HourFormat && d.Hour >= 12)
                        ((NumericHourInput)_HourInput).Period = eHourPeriod.PM;
                }
            }
            if (_MinuteInput != null)
            {
                if (this.IsEmpty)
                    _MinuteInput.IsEmpty = true;
                else
                    _MinuteInput.Value = d.Minute;
            }
            if (_SecondInput != null)
            {
                if (this.IsEmpty)
                    _SecondInput.IsEmpty = true;
                else
                    _SecondInput.Value = d.Second;
            }

            for (int i = 0; i < this.Items.Count; i++)
            {
                IDateTimePartInput part = this.Items[i] as IDateTimePartInput;
                if (part == null) continue;
                if (part.Part == eDateTimePart.DayName)
                    part.Value = this.IsEmpty ? -1 : (int)_Value.DayOfWeek;
                else if (part.Part == eDateTimePart.DayOfYear)
                {
                    if (this.IsEmpty)
                        part.IsEmpty = true;
                    else
                        part.Value = _Value.DayOfYear;
                }
            }
            
            InvalidateArrange();
        }


        private bool _ResettingDateValue = false;
        protected override void ResetValue()
        {
            _ResettingDateValue = true;
            try
            {
                if (this.AllowEmptyState)
                {
                    if (_DayInput != null) _DayInput.IsEmpty = true;
                    if (_MonthInput != null) _MonthInput.IsEmpty = true;
                    if (_YearInput != null) _YearInput.IsEmpty = true;
                    if (_HourInput != null) _HourInput.IsEmpty = true;
                    if (_MinuteInput != null) _MinuteInput.IsEmpty = true;
                    if (_SecondInput != null) _SecondInput.IsEmpty = true;

                    foreach (VisualItem item in this.Items)
                    {
                        if (item is DateTimeGroup)
                            ((DateTimeGroup)item).IsEmpty = true;
                        else if (item is IDateTimePartInput && !((IDateTimePartInput)item).IsEmpty)
                            ((IDateTimePartInput)item).IsEmpty = true;
                    }
                    _Value = DateTime.MinValue;
                }
                else
                {
                    SetNonEmptyValue();
                }
            }
            finally
            {
                _ResettingDateValue = false;
            }

            OnValueChanged();
        }

        private System.DateTime _MinDate = MinDateTime;
        /// <summary>
        /// Gets or sets the minimum value represented by the group.
        /// </summary>
        public System.DateTime MinDate
        {
            get { return _MinDate; }
            set { _MinDate = value; OnMinDateChanged(); }
        }

        private void OnMinDateChanged()
        {
            UpdateInnerDateTimeGroupMinMax();
        }

        private System.DateTime _MaxDate = MaxDateTime;
        /// <summary>
        /// Gets or sets maximum value represented by the group.
        /// </summary>
        public System.DateTime MaxDate
        {
            get { return _MaxDate; }
            set { _MaxDate = value; OnMaxDateChanged(); }
        }

        private void OnMaxDateChanged()
        {
            UpdateInnerDateTimeGroupMinMax();
        }

        private void UpdateInnerDateTimeGroupMinMax()
        {
            foreach (VisualItem  item in this.Items)
            {
                DateTimeGroup dg = item as DateTimeGroup;
                if (dg != null)
                {
                    dg.MinDate = _MinDate;
                    dg.MaxDate = _MaxDate;
                }
            }
        }

        protected override void OnAllowEmptyStateChanged()
        {
            if (!this.AllowEmptyState && this.IsEmpty)
            {
                SetNonEmptyValue();
            }
            base.OnAllowEmptyStateChanged();
        }

        private void SetNonEmptyValue()
        {
            if (this.MinDate > MinDateTime)
                this.Value = this.MinDate;
            else
                this.Value = DateTime.Now;
        }

        internal override void ProcessKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode != System.Windows.Forms.Keys.Back && e.KeyCode != System.Windows.Forms.Keys.Delete)
                IsUserInput = true;
            base.ProcessKeyDown(e);
            IsUserInput = false;
        }

        internal override void ProcessKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            IsUserInput = true;
            base.ProcessKeyPress(e);
            IsUserInput = false;
        }

        internal override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            IsUserInput = true;
            bool b = base.ProcessCmdKey(ref msg, keyData);
            IsUserInput = false;
            return b;
        }

        internal override void ProcessMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            IsUserInput = true;
            base.ProcessMouseWheel(e);
            IsUserInput = false;
        }

        private bool _DefaultInputValues = true;
        /// <summary>
        /// Gets or sets whether input values are set to defaults while user is entering data. Default value is true.
        /// </summary>
        public bool DefaultInputValues
        {
            get { return _DefaultInputValues; }
            set
            {
                _DefaultInputValues = value;
            }
        }
        
        #endregion

    }
}
#endif

