#if FRAMEWORK20
using System.Collections.ObjectModel;

namespace DevComponents.DotNetBar.Schedule
{
    public class DisplayedOwnerCollection : Collection<string>
    {
        #region Private variables

        private CalendarView _CalendarView;     // Assoc CalendarView
        private bool _IsRangeSet;               // Range set flag
        private bool _SuspendUpdate;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">CalendarView</param>
        public DisplayedOwnerCollection(CalendarView calendarView)
        {
            _CalendarView = calendarView;
        }

        #region Internal properties

        /// <summary>
        /// Gets and sets the SuspendUpdate state
        /// </summary>
        internal bool SuspendUpdate
        {
            get { return (_SuspendUpdate); }
            set { _SuspendUpdate = value; }
        }

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of Owners to the DisplayedOwner collection
        /// </summary>
        /// <param name="items">Array of Owners to add</param>
        public void AddRange(string[] items)
        {
            int index = Count;

            try
            {
                _IsRangeSet = true;

                for (int i = 0; i < items.Length; i++)
                    Add(items[i]);
            }
            finally
            {
                _IsRangeSet = false;

                _CalendarView.DisplayedOwnersAdded(index);
            }
        }

        #endregion

        #region RemoveItem

        /// <summary>
        /// Processes list RemoveItem calls
        /// </summary>
        /// <param name="index">Index to remove</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            if (_SuspendUpdate == false)
                _CalendarView.DisplayedOwnersRemoved(index, index + 1);
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">Text to add</param>
        protected override void InsertItem(int index, string item)
        {
            if (string.IsNullOrEmpty(item) == false)
            {
                base.InsertItem(index, item);

                if (_SuspendUpdate == false)
                {
                    if (_IsRangeSet == false)
                        _CalendarView.DisplayedOwnersAdded(index);
                }
            }
        }

        #endregion

        #region SetItem

        /// <summary>
        /// Processes list SetItem calls (e.g. replace)
        /// </summary>
        /// <param name="index">Index to replace</param>
        /// <param name="newItem">Text to replace</param>
        protected override void SetItem(int index, string newItem)
        {
            base.SetItem(index, newItem);

            if (_SuspendUpdate == false)
                _CalendarView.DisplayedOwnersSet(index);
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            if (Count > 0)
            {
                int n = Count;

                base.ClearItems();

                _CalendarView.DisplayedOwnersRemoved(0, n);
            }
        }

        #endregion
    }
}
#endif

