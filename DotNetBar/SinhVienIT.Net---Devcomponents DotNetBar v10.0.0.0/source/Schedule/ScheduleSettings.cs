#if FRAMEWORK20
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    public static class ScheduleSettings
    {
        public static double TimeSlotSliceHeight = 23d;
        public static string TimeRulerHourFormatString = "00";
        public static string TimeRulerMinuteFormatString = "00";

        private static CultureInfo _currentCulture;

        /// <summary>
        /// Gets or sets the CultureInfo for the culture used by the DateTime and Numeric Input controls.
        /// Default value is null which indicates that controls will use CurrentUICulture.
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get
            {
                return _currentCulture;
            }
            set
            {
                _currentCulture = value;
            }
        }

        /// <summary>
        /// Gets the Culture used by the date time input and month calendar controls
        /// </summary>
        /// <returns>reference to CultureInfo</returns>
        public static CultureInfo GetActiveCulture()
        {
            return (_currentCulture ?? CultureInfo.CurrentCulture);
        }
    }
}
#endif

