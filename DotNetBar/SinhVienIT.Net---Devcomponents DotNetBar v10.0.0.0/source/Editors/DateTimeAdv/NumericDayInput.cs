#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericDayInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericDayInput class.
        /// </summary>
        public NumericDayInput()
        {
            this.MinValue = 1;
            this.MaxValue = 31;
        }
        #endregion

        #region Internal Implementation
        #endregion


        #region IDateTimePartInput Members


        public eDateTimePart Part
        {
            get { return eDateTimePart.Day; }
        }

        #endregion
    }
}
#endif

