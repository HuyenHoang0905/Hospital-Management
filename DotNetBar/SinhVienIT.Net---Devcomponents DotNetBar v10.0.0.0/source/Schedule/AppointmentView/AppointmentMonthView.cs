#if FRAMEWORK20
using System;
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentMonthView : AppointmentHView
    {
        #region Constants
        private const int DaysInWeek = 7;
        #endregion

        #region Private variables
        private MonthWeek _MonthWeek;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseView"></param>
        /// <param name="appointment"></param>
        public AppointmentMonthView(BaseView baseView, Appointment appointment)
            : base(baseView, appointment)
        {
        }

        #region Public properties

        /// <summary>
        /// Gets and sets View MonthWeek
        /// </summary>
        public MonthWeek MonthWeek
        {
            get { return (_MonthWeek); }

            set
            {
                if (_MonthWeek != value)
                {
                    _MonthWeek = value;

                    SetViewEnds();
                }
            }
        }

        #endregion

        #region SetViewEnds

        /// <summary>
        /// Sets the view display end types
        /// </summary>
        protected override void SetViewEnds()
        {
            if (_MonthWeek != null)
            {
                DateTime start = _MonthWeek.FirstDayOfWeek;
                DateTime end = start.AddDays(DaysInWeek);

                ViewEnds = eViewEnds.Complete;

                // Check to see if we can only display
                // a partial representation for the view

                if (EndTime >= start && StartTime < end)
                {
                    if (StartTime < start)
                        ViewEnds |= eViewEnds.PartialLeft;

                    if (EndTime > end)
                        ViewEnds |= eViewEnds.PartialRight;
                }
            }
        }

        #endregion
    }
}
#endif

