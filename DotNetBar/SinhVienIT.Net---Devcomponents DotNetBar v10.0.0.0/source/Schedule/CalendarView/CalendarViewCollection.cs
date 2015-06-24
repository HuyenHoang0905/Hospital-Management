#if FRAMEWORK20
using System;
using System.Collections.Generic;

namespace DevComponents.DotNetBar.Schedule
{
    public class CalendarViewCollection<T> where T : BaseView
    {
        #region Private variables

        private CalendarView _CalendarView;         // Assoc CalendarView
        private List<T> _Views = new List<T>();     // BaseView items

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">CalendarView</param>
        public CalendarViewCollection(CalendarView calendarView)
        {
            _CalendarView = calendarView;
        }

        #region Public properties

        /// <summary>
        /// Gets the count of items in the collection
        /// </summary>
        public int Count
        {
            get { return (_Views.Count); }
        }

        /// <summary>
        /// Gets the view at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Requested view</returns>
        public T this[int index]
        {
            get { return (GetView(index)); }
        }

        /// <summary>
        /// Gets the view for the given DisplayedOwner
        /// </summary>
        /// <param name="key">DisplayedOwner</param>
        /// <returns>Requested view</returns>
        public T this[string key]
        {
            get { return (GetView(FindDisplayedOwner(key))); }
        }

        /// <summary>
        /// Locates the view index from the given
        /// DisplayedOwner text
        /// </summary>
        /// <param name="key">DisplayedOwner</param>
        /// <returns>View index, or -1 if not found</returns>
        private int FindDisplayedOwner(string key)
        {
            for (int i = 0; i < _CalendarView.DisplayedOwners.Count; i++)
            {
                if (_CalendarView.DisplayedOwners[i].Equals(key))
                    return (i);
            }

            return (-1);
        }

        /// <summary>
        /// Returns the given view at the specified index.
        /// 
        /// This routine will initiate the creation
        /// of the view if it has not previously been created.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Requested view</returns>
        private T GetView(int index)
        {
            if (index >= 0 && index < _Views.Count)
            {
                if (_Views[index] == null)
                {
                    Type type = typeof(T);

                    _Views[index] = (T)_CalendarView.NewView(type, index);
                }

                return (_Views[index]);
            }

            return (default(T));
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets the collection view list
        /// </summary>
        internal List<T> Views
        {
            get { return (_Views); }
        }

        #endregion
    }
}
#endif
