#if FRAMEWORK20
using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class DaysOfTheWeek
    {
        #region enums

        public enum eDayType
        {
            Long,       // Long text
            Short,      // Short text
            Single,
            None
        }

        #endregion

        #region Private variables

        private int _DayCount;          // Count of days

        private string[][] _DayText;    // Days of the week text
        private Size[][] _DaySize;      // Days of the week measure

        private bool _NeedsMeasured;    // Text measure flag

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DaysOfTheWeek()
            : this(DateHelper.GetFirstDayOfWeek(), 7)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="day">Day of the week</param>
        /// <param name="count">Count of days</param>
        public DaysOfTheWeek(DayOfWeek day, int count)
        {
            LoadDays(day, count);
        }

        #region Public properties

        /// <summary>
        /// Gets the DayText string arrays
        /// </summary>
        public string[][] DayText
        {
            get { return (_DayText); }
        }

        /// <summary>
        /// Gets the DaySize Size arrays
        /// </summary>
        public Size[][] DaySize
        {
            get {return (_DaySize); }
        }

        /// <summary>
        /// Day text NeedsMeasured flag
        /// </summary>
        public bool NeedsMeasured
        {
            get { return (_NeedsMeasured); }
            set { _NeedsMeasured = value; }
        }

        #endregion

        #region LoadDays

        /// <summary>
        /// Loads the DayText arrays
        /// </summary>
        /// <param name="day">Starting day of week</param>
        /// <param name="count">Count of days</param>
        public void LoadDays(DayOfWeek day, int count)
        {
            _DayCount = count;

            // Allocate our Day text and Size arrays

            _DayText = new string[3][];
            _DaySize = new Size[3][];

            for (int i = 0; i < 3; i++)
            {
                _DayText[i] = new string[count];
                _DaySize[i] = new Size[count];
            }

            // Loop through each day of the week, getting
            // the long and short text strings for the day

            for (int i = 0; i < _DayCount; i++)
            {
                _DayText[0][i] =
                    ScheduleSettings.GetActiveCulture().DateTimeFormat.GetDayName(day);

                _DayText[1][i] =
                    DateHelper.GetThreeLetterDayOfWeek(day);

                _DayText[2][i] = _DayText[1][i].Substring(0, 1);

                day = DateHelper.GetNextDay(day);
            }

            // Flag that the text will need to be measured

            _NeedsMeasured = true;
        }

        #endregion

        #region MeasureText

        /// <summary>
        /// Measures the day text
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="font">Text font</param>
        public void MeasureText(Graphics g, Font font)
        {
            // Calculate our header text threshold, if
            // we haven't done so already

            if (_NeedsMeasured == true)
            {
                for (int i = 0; i < _DayCount; i++)
                {
                    _DaySize[0][i] =
                        TextDrawing.MeasureString(g, _DayText[0][i], font);

                    _DaySize[1][i] =
                        TextDrawing.MeasureString(g, _DayText[1][i], font);

                    _DaySize[2][i] =
                        TextDrawing.MeasureString(g, _DayText[2][i], font);
                }

                _NeedsMeasured = false;
            }
        }

        #endregion
    }
}
#endif

