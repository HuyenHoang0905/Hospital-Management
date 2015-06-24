#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    public class NumericSecondInput : VisualIntegerInput, IDateTimePartInput
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericSecondInput class.
        /// </summary>
        public NumericSecondInput()
        {
            this.MinValue = 0;
            this.MaxValue = 59;
        }
        #endregion

        #region Internal Implementation
        #endregion

        #region IDateTimePartInput Members
        eDateTimePart IDateTimePartInput.Part
        {
            get { return eDateTimePart.Second; }
        }

        #endregion
    }
}
#endif

