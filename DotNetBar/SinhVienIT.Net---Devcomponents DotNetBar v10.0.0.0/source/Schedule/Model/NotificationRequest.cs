#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Schedule.Model
{
    internal class NotificationRequest
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the NotificationRequest class.
        /// </summary>
        /// <param name="notificationTime"></param>
        /// <param name="notificationHandler"></param>
        public NotificationRequest(DateTime notificationTime, NotificationServerEventHandler notificationHandler)
        {
            _NotificationTime = notificationTime;
            _NotificationHandler = notificationHandler;
            _ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        private int _ThreadId;
        public int ThreadId
        {
            get { return _ThreadId; }
        }
        


        private DateTime _NotificationTime = DateTime.MinValue;
        /// <summary>
        /// Gets or sets requested notification time.
        /// </summary>
        public DateTime NotificationTime
        {
            get { return _NotificationTime; }
            set
            {
            	_NotificationTime = value;
            }
        }

        private NotificationServerEventHandler _NotificationHandler = null;
        /// <summary>
        /// Gets the callback handler for notification when notification time is reached.
        /// </summary>
        public NotificationServerEventHandler NotificationHandler
        {
            get { return _NotificationHandler; }
        }
        

        #endregion
    }
}
#endif

