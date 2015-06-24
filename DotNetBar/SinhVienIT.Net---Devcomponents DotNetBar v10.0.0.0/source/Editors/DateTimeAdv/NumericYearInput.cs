#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericYearInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericYearInput class.
        /// </summary>
        public NumericYearInput()
        {
            this.MinValue = System.DateTime.MinValue.Year;
            this.MaxValue = System.DateTime.MaxValue.Year;
        }
        #endregion

        #region Internal Implementation
        private eYearDisplayFormat _YearDisplayFormat = eYearDisplayFormat.FourDigit;
        /// <summary>
        /// Gets or sets the year display format. Default value is four digit format.
        /// </summary>
        [DefaultValue(eYearDisplayFormat.FourDigit)]
        public eYearDisplayFormat YearDisplayFormat
        {
            get { return _YearDisplayFormat; }
            set
            {
                if (_YearDisplayFormat != value)
                {
                    _YearDisplayFormat = value;
                    InvalidateArrange();
                }
            }
        }

        protected override string GetMeasureString()
        {
            return GetFormattedYear(base.GetMeasureString());
        }

        protected override string GetRenderString()
        {
            return GetFormattedYear(base.GetRenderString());
        }

        private string GetFormattedYear(string s)
        {
            if (this.IsFocused || s.Length < 4 || _YearDisplayFormat == eYearDisplayFormat.FourDigit) return s;
            if (_YearDisplayFormat == eYearDisplayFormat.TwoDigit)
                return s.Substring(2);
            return s.Substring(3);
        }

        protected override void InputComplete(bool sendNotification)
        {
            UpdateYearValue();
            base.InputComplete(sendNotification);
        }

        protected override void OnInputLostFocus()
        {
            UpdateYearValue();
            base.OnInputLostFocus();
        }

        private void UpdateYearValue()
        {
            if (!this.IsEmpty && this.Value < 100)
                this.Value = int.Parse(System.DateTime.Now.Year.ToString().Substring(0, 2) + this.Value.ToString("00"));
        }
        #endregion

        #region IDateTimePartInput Members
        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.Year; }
        }

        #endregion
    }
}
#endif

