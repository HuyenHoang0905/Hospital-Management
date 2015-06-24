#if FRAMEWORK20
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    public class DayInfo
    {
        #region Private data

        private WorkTime _WorkStartTime;
        private WorkTime _WorkEndTime;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets work start time
        /// </summary>
        public WorkTime WorkStartTime
        {
            get { return (_WorkStartTime); }
            set { _WorkStartTime = value; }
        }

        /// <summary>
        /// Gets and sets work end time
        /// </summary>
        public WorkTime WorkEndTime
        {
            get { return (_WorkEndTime); }
            set { _WorkEndTime = value; }
        }

        #endregion
    }
}
#endif