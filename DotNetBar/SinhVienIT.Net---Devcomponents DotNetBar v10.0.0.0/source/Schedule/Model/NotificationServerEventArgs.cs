#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Schedule.Model
{
    internal class NotificationServerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NotificationServerEventArgs class.
        /// </summary>
        /// <param name="notificationTime"></param>
        public NotificationServerEventArgs(DateTime notificationTime)
        {
            _NotificationTime = notificationTime;
        }

        private DateTime _NotificationTime;
        /// <summary>
        /// Gets the time notification is sent on.
        /// </summary>
        public DateTime NotificationTime
        {
            get { return _NotificationTime; }
        }

        private DateTime _NextRequestedNotificationTime = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the next requested notification time by the handler of the event.
        /// Handler of event must set this to the desired next notification time in order to be notified.
        /// The value recorded will be the lowest value set by all handlers.
        /// </summary>
        public DateTime NextRequestedNotificationTime
        {
            get { return _NextRequestedNotificationTime; }
            set 
            {
                value = CalendarModel.GetCalendarDateTime(value);
                if (value > NotificationTime && (value < _NextRequestedNotificationTime || _NextRequestedNotificationTime == DateTime.MinValue))
                {
                    _NextRequestedNotificationTime = value;
                }
            }
        }
    }
}
#endif

