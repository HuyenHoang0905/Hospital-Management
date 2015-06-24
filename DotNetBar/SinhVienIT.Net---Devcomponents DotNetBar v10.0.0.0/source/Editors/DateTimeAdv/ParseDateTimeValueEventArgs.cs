#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Defines data for the ParseValue event that allows you to provide custom parsing for values set to ValueObject property.
    /// </summary>
    public class ParseDateTimeValueEventArgs : EventArgs
    {
        /// <summary>
        /// Get the value that was set to the ValueObject property and which should be converted to ParsedValue DateTime.
        /// </summary>
        public readonly object ValueObject = null;

        /// <summary>
        /// Gets or sets whether you have provided ParsedValue.
        /// </summary>
        public bool IsParsed = false;

        /// <summary>
        /// Initializes a new instance of the ParseDateTimeValueEventArgs class.
        /// </summary>
        /// <param name="valueObject">Indicates the value object.</param>
        public ParseDateTimeValueEventArgs(object valueObject)
        {
            ValueObject = valueObject;
        }

        private System.DateTime _ParsedValue = DateTimeGroup.MinDateTime;

        /// <summary>
        /// /// <summary>
        /// Gets or sets the parsed value from ValueObject property.
        /// </summary>
        /// </summary>
        public System.DateTime ParsedValue
        {
            get { return _ParsedValue; }
            set
            {
                _ParsedValue = value;
                IsParsed = true;
            }
        }
    }

    /// <summary>
    /// Defines delegate for ParseDateTimeValue event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ParseDateTimeValueEventHandler(object sender, ParseDateTimeValueEventArgs e);
}
#endif

