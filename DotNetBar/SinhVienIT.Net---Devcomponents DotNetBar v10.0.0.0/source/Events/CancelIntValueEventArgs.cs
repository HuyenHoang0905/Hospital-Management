using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the cancelable event arguments with integer value.
    /// </summary>
    public class CancelIntValueEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets or sets the new value that will be used if event is not cancelled.
        /// </summary>
        public int NewValue = 0;
    }

    /// <summary>
    /// Defines delegate for cancelable events.
    /// </summary>
    public delegate void CancelIntValueEventHandler(object sender, CancelIntValueEventArgs e);
}
