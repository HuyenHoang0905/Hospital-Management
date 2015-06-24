#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator
{
    /// <summary>
    /// Defines an interface that can be implemented by custom error providers to be used with SuperValidator.
    /// </summary>
    public interface IErrorProvider
    {
        /// <summary>
        /// Sets the error state on the control.
        /// </summary>
        /// <param name="control">Control for which error state is being set.</param>
        /// <param name="value">The error message from validator.</param>
        void SetError(Control control, string value);
        /// <summary>
        /// Clears the error state for the control.
        /// </summary>
        /// <param name="control">Control to clear error state for.</param>
        void ClearError(Control control);
    }
}
#endif