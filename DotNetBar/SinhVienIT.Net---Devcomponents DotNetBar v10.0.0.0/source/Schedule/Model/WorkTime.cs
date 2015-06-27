#if FRAMEWORK20
using System;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents a work time.
    /// </summary>
    public struct WorkTime
    {
        #region Private variables

        private int _Hour;      // Hour
        private int _Minute;    // Minute

        #endregion

        /// <summary>
        /// Initializes a new instance of the WorkTime structure.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public WorkTime(int hour, int minute)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentException("Hour value must be from 0 to 23");

            if (minute < 0 || minute > 59)
                throw new ArgumentException("Minute value must be from 0 to 59");

            _Hour = hour;
            _Minute = minute;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the hour from 0 to 23 this time instance represents.
        /// </summary>
        public int Hour
        {
            get { return _Hour; }

            set
            {
                if (value < 0 || value > 23)
                    throw new ArgumentException("Hour value must be from 0 to 23");

                _Hour = value;
            }
        }

        /// <summary>
        /// Gets or sets the minute from 0 to 59 this time instance represents.
        /// </summary>
        public int Minute
        {
            get { return _Minute; }

            set
            {
                if (value < 0 || value > 59)
                    throw new ArgumentException("Minute value must be from 0 to 59");

                _Minute = value;
            }
        }

        /// <summary>
        /// Determines if the WorkTime is Empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_Hour == 0 && _Minute == 0);
            }
        }

        #endregion

        #region Operator overloades

        public static bool operator >=(WorkTime op1, WorkTime op2)
        {
            if ((op1.Hour > op2.Hour) ||
                (op1.Hour == op2.Hour && op1.Minute >= op2.Minute))
            {
                return (true);
            }

            return (false);
        }

        public static bool operator <=(WorkTime op1, WorkTime op2)
        {
            if ((op1.Hour < op2.Hour) ||
                (op1.Hour == op2.Hour && op1.Minute <= op2.Minute))
            {
                return (true);
            }

            return (false);
        }

        public static bool operator >(WorkTime op1, WorkTime op2)
        {
            if ((op1.Hour > op2.Hour) ||
                (op1.Hour == op2.Hour && op1.Minute > op2.Minute))
            {
                return (true);
            }

            return (false);
        }

        public static bool operator <(WorkTime op1, WorkTime op2)
        {
            if ((op1.Hour < op2.Hour) ||
                (op1.Hour == op2.Hour && op1.Minute < op2.Minute))
            {
                return (true);
            }

            return (false);
        }

        #endregion
    }
}
#endif

