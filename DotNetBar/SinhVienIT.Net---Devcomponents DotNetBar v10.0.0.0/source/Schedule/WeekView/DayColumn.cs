#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    public class DayColumn
    {
        #region Consts

        public const int NumberOfTimeSlices = 48;

        #endregion

        #region Private variables

        private Rectangle _Bounds;          // Slot bounding rectangle

        private DateTime _Date;             // Column date

        private float _TimeSliceHeight;       // TimeSlice height

        private WorkTime _BusyStartTime;    // Busy time
        private WorkTime _BusyEndTime;

        private WorkTime _WorkStartTime;    // Work time
        private WorkTime _WorkEndTime;

        private List<CalendarItem>          // Column CalendarItems
            _CalendarItems = new List<CalendarItem>();

        private bool _NeedRecalcLayout = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeSliceHeight">Slice height</param>
        public DayColumn(float timeSliceHeight)
        {
            _TimeSliceHeight = timeSliceHeight;
        }

        #region Public properties

        #region BoundingRect

        /// <summary>
        /// Gets and sets the week bounding Rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }

            set
            {
                if (_Bounds.Equals(value) == false)
                {
                    int yOffset = value.Y - _Bounds.Y;

                    _Bounds = value;

                    if (yOffset != 0)
                        OffsetItemRects(yOffset);
                }
            }
        }

        /// <summary>
        /// Offsets the bounding rectangles for the
        /// DayColumn's non-extended appointments
        /// </summary>
        /// <param name="yOffset">Amount to offset</param>
        private void OffsetItemRects(int yOffset)
        {
            for (int i = 0; i < _CalendarItems.Count; i++)
            {
                Rectangle r = _CalendarItems[i].Bounds;

                r.Y += yOffset;

                _CalendarItems[i].Bounds = r;
            }
        }

        #endregion

        #region Date

        /// <summary>
        /// Gets and sets the column date
        /// </summary>
        public DateTime Date
        {
            get { return (_Date); }
            set { _Date = value; }
        }

        #endregion

        #region TimeSliceHeight

        /// <summary>
        /// Gets and sets the TimeSlice height
        /// </summary>
        public float TimeSliceHeight
        {
            get { return (_TimeSliceHeight); }
            set { _TimeSliceHeight = value; }
        }

        #endregion

        #region WorkTime properties

        /// <summary>
        /// Gets and sets the busy time start
        /// </summary>
        public WorkTime BusyStartTime
        {
            get { return (_BusyStartTime); }
            set { _BusyStartTime = value; }
        }

        /// <summary>
        /// Gets and sets the busy time end
        /// </summary>
        public WorkTime BusyEndTime
        {
            get { return (_BusyEndTime); }
            set { _BusyEndTime = value; }
        }

        /// <summary>
        /// Gets and sets the work time start
        /// </summary>
        public WorkTime WorkStartTime
        {
            get { return (_WorkStartTime); }
            set { _WorkStartTime = value; }
        }

        /// <summary>
        /// Gets and sets the work time end
        /// </summary>
        public WorkTime WorkEndTime
        {
            get { return (_WorkEndTime); }
            set { _WorkEndTime = value; }
        }

        #endregion

        #region CalendarItems

        /// <summary>
        /// Gets the column CalendarItems list
        /// </summary>
        public List<CalendarItem> CalendarItems
        {
            get { return (_CalendarItems); }
        }

        #endregion

        #region NeedRecalcLayout

        public bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }
            set { _NeedRecalcLayout = value; }
        }

        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the given time is tagged as a "Busy time"
        /// </summary>
        /// <param name="time">WorkTime to test</param>
        /// <returns>true if specified "time" is a Busy time</returns>
        public bool IsBusyTime(WorkTime time)
        {
            return ((!BusyStartTime.IsEmpty && !BusyEndTime.IsEmpty) &&
                    (time >= BusyStartTime && time < BusyEndTime));
        }

        /// <summary>
        /// Determines if the given time is tagged as a "Work time"
        /// </summary>
        /// <param name="time">WorkTime to test</param>
        /// <returns>true if specified "time" is a Work time</returns>
        public bool IsWorkTime(WorkTime time)
        {
            return (time >= WorkStartTime && time < WorkEndTime);
        }

        #endregion

    }
}
#endif

