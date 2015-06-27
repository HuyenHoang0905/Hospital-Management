#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericMonthInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericMonthInput class.
        /// </summary>
        public NumericMonthInput()
        {
            this.MinValue = 1;
            this.MaxValue = 12;
        }
        #endregion

        #region Internal Implementation
        #endregion


        #region IDateTimePartInput Members
        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.Month; }
        }

        #endregion
    }
}
#endif

