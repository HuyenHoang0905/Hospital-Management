#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericDayOfYearInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericDayOfYearInput class.
        /// </summary>
        public NumericDayOfYearInput()
        {
            this.MinValue = 1;
            this.MaxValue = 366;
        }
        #endregion

        #region Internal Implementation
        #endregion



        #region IDateTimePartInput Members


        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.DayOfYear; }
        }

        #endregion
    }
}
#endif

