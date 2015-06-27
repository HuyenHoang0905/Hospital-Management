#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv
{
    public class DayLabelItem : VisualLabel, IDateTimePartInput
    {
        #region Private Variables

        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        private int _Day = -1;
        /// <summary>
        /// Gets or sets the day in numeric format to display. Allowed values are from -1 to 6. -1 represents an empty state.
        /// 0 represents Sunday and 6 Represents Saturday.
        /// </summary>
        [DefaultValue(-1)]
        public int Day
        {
            get { return _Day; }
            set
            {
                if (_Day != value)
                {
                    if (_Day < -1 || _Day > 6)
                        throw new ArgumentException("Day must be value between -1 and 6");
                    _Day = value;
                    OnDayChanged();
                }
            }
        }

        private void OnDayChanged()
        {
            UpdateLabelText();
        }

        private void UpdateLabelText()
        {
            if (_Day == -1)
                Text = "";
            else if (_DayNames != null)
                Text = _DayNames[_Day];
            else if (_UseAbbreviatedNames)
                Text = DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedDayNames[_Day];
            else
                Text = DateTimeInput.GetActiveCulture().DateTimeFormat.DayNames[_Day];
        }

        private List<string> _DayNames = null;

        /// <summary>
        /// Gets or sets the array of custom names for days. The array must have exactly 7 elements representing day names from 0 to 6.
        /// </summary>
        public List<string> DayNames
        {
            get { return _DayNames; }
            set
            {
                if (value != null && value.Count != 7)
                    throw new ArgumentException("DayNames must have exactly 7 items in collection.");
                _DayNames = value;
            }
        }

        private bool _UseAbbreviatedNames = false;
        /// <summary>
        /// Gets or sets whether abbreviated day names are used for display instead of full day names. Default value is false.
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
                    UpdateLabelText();
                }
            }
        }
        #endregion


        #region IDateTimePartInput Members

        int IDateTimePartInput.Value
        {
            get
            {
                return _Day;
            }
            set
            {
                Day = value;
            }
        }

        int IDateTimePartInput.MinValue
        {
            get
            {
                return 0;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        int IDateTimePartInput.MaxValue
        {
            get
            {
                return 6;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.DayName; }
        }

        bool IDateTimePartInput.IsEmpty
        {
            get
            {
                return _Day == -1;
            }
            set
            {
                this.Day = -1;
            }
        }

        void IDateTimePartInput.UndoInput()
        {

        }
        #endregion
    }
}
#endif

