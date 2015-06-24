#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Timers;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Provides notification support for appointments.
    /// </summary>
    internal static class NotificationServer
    {
        #region Private Variables
        private static Timer _Timer = null;
        internal static event NotificationServerEventHandler NotifySubscribers;
        private static DateTime _NextEventTime;
        private static List<NotificationRequest> _RequestList = new List<NotificationRequest>();
        #endregion

        #region Constructor
        static NotificationServer()
        {
            _Timer = new Timer();
            _Timer.Elapsed += new ElapsedEventHandler(NotificationEventTimerTick);
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
            _NextEventTime = DateTime.MinValue;
        }

        static void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            if (_NextEventTime > DateTime.MinValue && UtcNow.Subtract(_NextEventTime).TotalSeconds <= 0)
                NotifySubscribersOnTimeEvent();
        }

        static void NotificationEventTimerTick(object sender, ElapsedEventArgs e)
        {
            NotifySubscribersOnTimeEvent();
        }

        private static bool _Notifying = false;
        private static void NotifySubscribersOnTimeEvent()
        {
            try
            {
                _Notifying = true;
                _Timer.Stop();
#if DEBUG
                Console.WriteLine("{0} NotificationServer notifying subscribers count: {1}", DateTime.Now, _RequestList.Count);
#endif
                // Reset next notification time...
                _NextEventTime = DateTime.MinValue;

                if (_RequestList.Count == 0)
                {
                    return;
                }
                DateTime now = CalendarModel.GetCalendarDateTime(UtcNow);
                NotificationRequest[] requestList = new NotificationRequest[_RequestList.Count];
                _RequestList.CopyTo(requestList);
                DateTime nextNotification = DateTime.MinValue;
                foreach (NotificationRequest item in requestList)
                {
                    if (item.NotificationTime <= now)
                    {
                        if (item.NotificationHandler != null)
                        {
                            //if (System.Threading.Thread.CurrentThread.ManagedThreadId != item.ThreadId)
                            //{
                                
                            //    //System.Threading.SendOrPostCallback callback = new System.Threading.SendOrPostCallback(SendNotification);
                            //    //System.Windows.Threading.DispatcherSynchronizationContext.Current.Post(callback, item);
                            //}
                            //else
                                item.NotificationHandler(item, new NotificationServerEventArgs(now));
                        }
                        _RequestList.Remove(item);
                    }
                    else if (item.NotificationTime < nextNotification || nextNotification == DateTime.MinValue)
                        nextNotification = item.NotificationTime;
                }

                if (nextNotification > DateTime.MinValue)
                {
                    SetNextEventTime(nextNotification);
                }
                //else
                //    Console.WriteLine("Not setting next notification time...");
            }
            finally
            {
                _Notifying = false;
            }
        }

        private static void SendNotification(object e)
        {
            NotificationRequest item = (NotificationRequest)e;
            if (item.NotificationHandler != null)
            {
                if (System.Threading.Thread.CurrentThread.ManagedThreadId != item.ThreadId)
                {
                    Console.WriteLine("Different THREAD");
                }
                else
                {
                    DateTime now = CalendarModel.GetCalendarDateTime(UtcNow);
                    item.NotificationHandler(item, new NotificationServerEventArgs(now));
                }
            }
        }


        private static void SetNextEventTime(DateTime dateTime)
        {
            DateTime now = CalendarModel.GetCalendarDateTime(UtcNow);
            if (dateTime.Subtract(now).TotalSeconds <= 0)
            {
                if (!_Notifying)
                    NotifySubscribersOnTimeEvent();
                else
                    _NextEventTime = DateTime.MinValue;
            }
            else if (dateTime.Subtract(now).TotalSeconds > 0 && (_NextEventTime == DateTime.MinValue || dateTime < _NextEventTime))
            {
                double nextEvent = dateTime.Subtract(UtcNow).TotalSeconds + 1;
                if (nextEvent > int.MaxValue / 1000)
                    nextEvent = (int.MaxValue - 1) / 1000;
                _Timer.Interval = (int)Math.Max(5, nextEvent * 1000);
#if DEBUG
                Console.WriteLine("{0} NotificationServer scheduling next event on: {1}, previous scheduled event on: {2}", DateTime.Now, dateTime, _NextEventTime);
#endif
                _Timer.Start();
                _NextEventTime = dateTime;
            }
            //else
            //    _NextEventTime = DateTime.MinValue;
        }

        private static DateTime UtcNow
        {
            get
            {
                return TimeZoneInfo.ConvertTimeToUtc(CalendarModel.CurrentDateTime);
            }
        }

        public static void UpdateNotification(NotificationRequest request)
        {
            SetNextEventTime(request.NotificationTime);
        }

        #endregion

        #region Internal Implementation
        public static void Register(NotificationRequest request)
        {
            _RequestList.Add(request);
            SetNextEventTime(request.NotificationTime);
        }

        public static void Unregister(NotificationRequest request)
        {
            _RequestList.Remove(request);
        }
        #endregion
    }

    internal delegate void NotificationServerEventHandler(NotificationRequest sender, NotificationServerEventArgs e);
}
#endif

