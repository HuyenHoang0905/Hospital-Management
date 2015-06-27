#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors
{
    public class InputValidationEventArgs : EventArgs
    {
        public readonly string Input = "";
        public bool AcceptInput = true;

        /// <summary>
        /// Initializes a new instance of the InputValidationEventArgs class.
        /// </summary>
        /// <param name="input">Indicates current input.</param>
        public InputValidationEventArgs(string input)
        {
            Input = input;
        }
    }

    /// <summary>
    /// Defines delegate for input validation event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void InputValidationEventHandler(object sender, InputValidationEventArgs e);
}
#endif

