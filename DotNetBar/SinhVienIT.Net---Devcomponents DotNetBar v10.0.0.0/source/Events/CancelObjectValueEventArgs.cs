using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the cancelable event arguments with object value.
    /// </summary>
    public class CancelObjectValueEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CancelObjectValueEventArgs class.
        /// </summary>
        /// <param name="o"></param>
        public CancelObjectValueEventArgs(object o)
        {
            this.Data = o;
        }

        /// <summary>
        /// Gets or sets the data connected to this event.
        /// </summary>
        public object Data = null;
    }

    /// <summary>
    /// Defines delegate for cancelable events.
    /// </summary>
    public delegate void CancelObjectValueEventHandler(object sender, CancelObjectValueEventArgs e);
}
