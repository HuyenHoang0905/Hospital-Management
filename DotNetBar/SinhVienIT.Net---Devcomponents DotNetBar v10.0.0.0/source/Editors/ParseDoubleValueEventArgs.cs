using System;
using System.Text;

namespace DevComponents.Editors
{
    /// <summary>
    /// Defines data for the ParseValue event that allows you to provide custom parsing for values set to ValueObject property.
    /// </summary>
    public class ParseDoubleValueEventArgs : EventArgs
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
        public ParseDoubleValueEventArgs(object valueObject)
        {
            ValueObject = valueObject;
        }

        private double _ParsedValue = 0;

        /// <summary>
        /// /// <summary>
        /// Gets or sets the parsed value from ValueObject property.
        /// </summary>
        /// </summary>
        public double ParsedValue
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
    /// Defines delegate for ParseValue event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ParseDoubleValueEventHandler(object sender, ParseDoubleValueEventArgs e);
}

