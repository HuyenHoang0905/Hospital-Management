#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.Editors.DateTimeAdv
{
    public class HourPeriodInput : VisualStringListInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        private NumericHourInput _HourInput;
        public NumericHourInput HourInput
        {
            get { return _HourInput; }
            set
            {
                if (_HourInput != null)
                {
                    _HourInput.ValueChanged -= new EventHandler(HourChanged);
                    _HourInput.IsEmptyChanged -= new EventHandler(HourIsEmptyChanged);
                }
                _HourInput = value;
                if (_HourInput != null)
                {
                    _HourInput.ValueChanged += new EventHandler(HourChanged);
                    _HourInput.IsEmptyChanged += new EventHandler(HourIsEmptyChanged);
                }
            }
        }

        private void HourIsEmptyChanged(object sender, EventArgs e)
        {
            if (_HourInput != null && _HourInput.IsEmpty)
                this.IsEmpty = true;
        }
        private void HourChanged(object sender, EventArgs e)
        {
            if (_HourInput == null) return;
            if (!_HourInput.IsEmpty)
            {
                if (_HourInput.Period == eHourPeriod.AM)
                {
                    this.SelectedIndex = 0;
                }
                else
                {
                    this.SelectedIndex = 1;
                }
            }
            else
                this.SelectedIndex = -1;
        }

        protected override List<string> GetItems()
        {
            List<string> items = new List<string>(2);
            items.Add(GetAMLabel());
            items.Add(GetPMLabel());
            return items;
        }

        private string GetPMLabel()
        {
            string s = "";
            if (_PMText != null && _PMText.Length > 0)
                s = _PMText;
            else
                s = DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator;
            if (_UseSingleLetterLabel && s.Length > 0)
                s = s[0].ToString();
            return s;
        }

        private string GetAMLabel()
        {
            string s = "";
            if (_AMText != null && _AMText.Length > 0)
                s = _AMText;
            else
                s = DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator;
            if (_UseSingleLetterLabel && s.Length > 0)
                s = s[0].ToString();
            return s;
        }

        private bool _UseSingleLetterLabel = false;
        public bool UseSingleLetterLabel
        {
            get { return _UseSingleLetterLabel; }
            set
            {
                if (_UseSingleLetterLabel != value)
                {
                    _UseSingleLetterLabel = value;
                    this.InvalidateArrange();
                }
            }
        }

        private string _AMText = "";
        /// <summary>
        /// Gets or sets custom AM text used.
        /// </summary>
        [DefaultValue("")]
        public string AMText
        {
            get { return _AMText; }
            set { _AMText = value; InvalidateArrange(); }
        }

        private string _PMText = "";
        /// <summary>
        /// Gets or sets custom PM text used.
        /// </summary>
        [DefaultValue("")]
        public string PMText
        {
            get { return _PMText; }
            set { _PMText = value; InvalidateArrange(); }
        }

        protected override void OnSelectedIndexChanged(EventArgs eventArgs)
        {
            if (_HourInput != null && !_HourInput.IsEmpty)
            {
                eHourPeriod period = this.SelectedIndex == 1 ? eHourPeriod.PM : eHourPeriod.AM;
                if (_HourInput.Period != period)
                    _HourInput.Period = period;
            }
            base.OnSelectedIndexChanged(eventArgs);
        }
        #endregion

    }
}
#endif

