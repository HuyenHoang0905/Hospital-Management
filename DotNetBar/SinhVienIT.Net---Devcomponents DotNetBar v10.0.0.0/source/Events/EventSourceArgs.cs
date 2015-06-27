using System;
using System.Text;

namespace DevComponents.DotNetBar.Events
{
    /// <summary>
    /// Represents event arguments that provide information on source of action.
    /// </summary>
    public class EventSourceArgs : EventArgs
    {
        /// <summary>
        /// Gets the source of the event.
        /// </summary>
        public readonly eEventSource Source = eEventSource.Code;

        /// <summary>
        /// Initializes a new instance of the EventSourceArgs class.
        /// </summary>
        /// <param name="source"></param>
        public EventSourceArgs(eEventSource source)
        {
            Source = source;
        }
    }

    /// <summary>
    /// Represents event arguments that provide information on source of action and allow canceling of action.
    /// </summary>
    public class CancelableEventSourceArgs : EventSourceArgs
    {
        /// <summary>
        /// Gets or sets whether event action will be canceled.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Initializes a new instance of the EventSourceArgs class.
        /// </summary>
        /// <param name="source"></param>
        public CancelableEventSourceArgs(eEventSource source)
            : base(source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CancelableEventSourceArgs class.
        /// </summary>
        /// <param name="cancel"></param>
        public CancelableEventSourceArgs(eEventSource source, bool cancel)
            : base(source)
        {
            Cancel = cancel;
        }
    }
}
