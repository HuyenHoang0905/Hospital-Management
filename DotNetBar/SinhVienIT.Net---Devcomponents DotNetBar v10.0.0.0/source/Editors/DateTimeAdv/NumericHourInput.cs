#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericHourInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericHourInput class.
        /// </summary>
        public NumericHourInput()
        {
            UpdateRange();
        }
        #endregion

        #region Internal Implementation
        private bool _Is24HourFormat = true;
        public bool Is24HourFormat
        {
            get { return _Is24HourFormat; }
            set
            {
                if (_Is24HourFormat != value)
                {
                    _Is24HourFormat = value;
                    OnIs24HourFormatChanged();
                }
            }
        }

        private void OnIs24HourFormatChanged()
        {
            if (!_Is24HourFormat && this.Value > 12)
            {
                this.Value = Value;
            }

            UpdateRange();
        }

        private void UpdateRange()
        {
            if (_Is24HourFormat)
            {
                this.MinValue = 0;
                this.MaxValue = 23;
            }
            else
            {
                if (_Period == eHourPeriod.AM)
                {
                    this.MinValue = 1;
                    this.MaxValue = 12;
                }
                else
                {
                    this.MinValue = 1;
                    this.MaxValue = 12;
                }
                //if (this.Value > this.MaxValue) this.Value = this.MaxValue;
                //if (this.Value < this.MinValue) this.Value = this.MinValue;
            }
        }

        protected override void OnValueChanged()
        {
            if (_Is24HourFormat)
            {
                if (Value >= 0 && Value <= 12)
                    this.Period = eHourPeriod.AM;
                else
                    this.Period = eHourPeriod.PM;
            }
            base.OnValueChanged();
        }

        private eHourPeriod _Period;
        public eHourPeriod Period
        {
            get { return _Period; }
            set
            {
                if (_Period != value)
                {
                    _Period = value;
                    if (!_Is24HourFormat)
                    {
                        UpdateRange();
                        this.Value = this.Value;
                    }
                }
            }
        }

        public override int Value
        {
            get { return base.Value; }
            set
            {
                if (!_Is24HourFormat)
                {
                    if (value == 0)
                    {
                        value = 12;
                        _Period = eHourPeriod.AM;
                    }
                    else if (value > 12)
                    {
                        _Period = eHourPeriod.PM;
                        value = value - 12;
                    }
                    //else
                    //    _Period = eHourPeriod.AM;
                    UpdateRange();
                }
                base.Value = value;
            }
        }

        protected override bool ValidateNewInputStack(string s)
        {
            if (!_Is24HourFormat && s.Length > 0)
            {
                int value = 0;
                if (int.TryParse(s, out value))
                {
                    if (value > 12 && value < 24 /*|| value == 12 && _Period == eHourPeriod.AM*/)
                    {
                        SetInputStack("");
                        SetInputPosition(0);
                        this.Value = value;
                        return false;
                    }
                }
            }
            return base.ValidateNewInputStack(s);
        }

        protected override void OnIsEmptyChanged()
        {
            if (this.IsEmpty)
            {
                _Period = eHourPeriod.AM;
                UpdateRange();
            }
            base.OnIsEmptyChanged();
        }
        #endregion


        #region IDateTimePartInput Members
        #region IDateTimePartInput Members
        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.Hour; }
        }

        #endregion

        #endregion
    }
}
#endif

