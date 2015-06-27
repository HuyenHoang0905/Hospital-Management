#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Represents a label for the NumericHourInput control that shows whether time is AM or PM.
    /// </summary>
    public class HourPeriodLabel : VisualLabel
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
                    _HourInput.ValueChanged -= new EventHandler(HourChanged);
                _HourInput = value;
                if (_HourInput != null)
                    _HourInput.ValueChanged += new EventHandler(HourChanged);
            }
        }

        private void HourChanged(object sender, EventArgs e)
        {
            if (_HourInput == null) return;
            string s = "";
            if (!_HourInput.IsEmpty)
            {
                if (_HourInput.Period == eHourPeriod.AM)
                {
                    if (_AMText != null && _AMText.Length > 0)
                        s = _AMText;
                    else
                        s = DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator;
                }
                else
                {
                    if (_PMText != null && _PMText.Length > 0)
                        s = _PMText;
                    else
                        s = DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator;
                }
                if (_UseSingleLetterLabel && s.Length > 0)
                    s = s[0].ToString();
            }
            this.Text = s;
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
        #endregion

    }
}
#endif

