#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class MonthWeek
    {
        #region Private constants

        private const int DaysInWeek = 7;

        #endregion

        #region Private variables

        private DateTime _FirstDayOfWeek;   // First day of the week
        private DateTime _LastDayOfWeek;    // Last day of the week

        private Rectangle _Bounds;          // Week bounding rectangle
        private ItemRects _DayRects;        // Day bounding Rectangles
        private ItemRects _MoreItems;

        private List<CalendarItem> 
            _CalendarItems = new List<CalendarItem>();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseItem"></param>
        public MonthWeek(BaseItem baseItem)
        {
            // Allocate our DayRects array

            _DayRects = new ItemRects(baseItem, DaysInWeek);
            _MoreItems = new ItemRects(baseItem, DaysInWeek);
        }

        #region Public properties

        #region CalendarItems

        /// <summary>
        /// Gets array of CalendarItems
        /// </summary>
        public List<CalendarItem> CalendarItems
        {
            get { return (_CalendarItems); }
        }

        #endregion

        #region FirstDayOfWeek

        /// <summary>
        /// Gets the first day of the week
        /// </summary>
        public DateTime FirstDayOfWeek
        {
            get { return (_FirstDayOfWeek); }

            internal set
            {
                _FirstDayOfWeek = value;

                _LastDayOfWeek = _FirstDayOfWeek.AddDays(DaysInWeek - 1);
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region Bounds

        /// <summary>
        /// Gets and sets the week bounding Rectangle
        /// </summary>
        internal Rectangle Bounds
        {
            get { return (_Bounds); }

            set
            {
                if (_Bounds.Equals(value) == false)
                {
                    _Bounds = value;

                    CalcDayRects();
                }
            }
        }

        #endregion

        #region DayRects

        /// <summary>
        /// Gets the day Rectangles
        /// </summary>
        internal ItemRects DayRects
        {
            get { return (_DayRects); }
        }

        #endregion

        #region MoreItems

        /// <summary>
        /// Gets the MoreItems
        /// </summary>
        internal ItemRects MoreItems
        {
            get { return (_MoreItems); }
        }

        #endregion

        #region WeekRange

        /// <summary>
        /// Gets the week day range text
        /// </summary>
        internal string WeekRange
        {
            get
            {
                string s1 = String.Format("{0:MMM} {1:d} - ",
                    _FirstDayOfWeek, _FirstDayOfWeek.Day);

                if (_FirstDayOfWeek.Month.Equals(_LastDayOfWeek.Month) == false)
                    s1 = s1 + String.Format("{0:MMM} ", _LastDayOfWeek);

                s1 = s1 + String.Format("{0:d}", _LastDayOfWeek.Day);

                return (s1);
            }
        }

        #endregion

        #endregion

        #region Private properties

        /// <summary>
        /// Gets day height
        /// </summary>
        private int DayHeight
        {
            get { return (_Bounds.Height); }
        }

        /// <summary>
        /// Gets day width
        /// </summary>
        private float DayWidth
        {
            get { return ((float)_Bounds.Width / DaysInWeek); }
        }

        #endregion

        #region DayRect calculation

        /// <summary>
        /// Calculates the day rectangles for the
        /// current bounding rectangle
        /// </summary>
        private void CalcDayRects()
        {
            float dx = 0;

            int sx = _Bounds.X;
            int sy = _Bounds.Y;

            int x2 = sx;

            // Loop through each day in the week

            for (int i = 0; i < DaysInWeek; i++)
            {
                int x1 = x2;

                x2 = sx + (int)(dx + DayWidth);

                dx += DayWidth;

                if (i + 1 == DaysInWeek)
                    x2 = _Bounds.X + _Bounds.Width;

                _DayRects[i].Bounds =
                    new Rectangle(x1, sy, x2 - x1, DayHeight);
            }
        }

        #endregion
    }
}
#endif

