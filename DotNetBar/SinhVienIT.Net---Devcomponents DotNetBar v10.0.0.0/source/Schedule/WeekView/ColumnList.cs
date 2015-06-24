#if FRAMEWORK20
using System;
using System.Collections.Generic;

namespace DevComponents.DotNetBar.Schedule
{
    public class ColumnList
    {
        #region Private variables

        private List<List<SlotItem>> _SList = new List<List<SlotItem>>();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the column slot list
        /// </summary>
        public List<List<SlotItem>> SList
        {
            get { return (_SList); }
        }

        #endregion

        #region Public methods

        #region AddColumnSlot

        /// <summary>
        /// Adds a CalendarItem to the running slot list
        /// </summary>
        /// <param name="item">CalendarItem to add</param>
        /// <param name="n">Slot level to add the item to</param>
        /// <returns>The added slot item</returns>
        public SlotItem AddColumnSlot(CalendarItem item, int n)
        {
            // Add a new SlotItem list if we have exceeded
            // the current list count

            if (n >= _SList.Count)
                _SList.Add(new List<SlotItem>());

            // Determine whether this item can fit in the
            // the slot list at the current level

            SlotItem si = GetColumnSlot(item, n);

            if (si != null)
            {
                // The item will fit, so add it to the list

                si.AddPeerSlot(
                    AddColumnSlot(item, n + 1), n + 1);
            }
            else
            {
                // The item won't fit, so allocate a new slot
                // item and add it to the list

                si = new SlotItem(item);

                _SList[n].Add(si);

                // Look ahead to see it we have a peer slot
                // in a future slot list

                while (n + 1 < _SList.Count)
                {
                    n++;

                    SlotItem ni = GetColumnSlot(item, n);

                    if (ni != null)
                    {
                        si.AddPeerSlot(ni, n);
                        break;
                    }
                }
            }

            // Return the added slot item

            return (si);
        }

        /// <summary>
        /// Returns the SlotItem (if present) in the given list for
        /// the CalendarItem in question
        /// </summary>
        /// <param name="item">CalendarItem</param>
        /// <param name="n">Slot level to scan</param>
        /// <returns>SlotItem, if found</returns>
        private SlotItem GetColumnSlot(CalendarItem item, int n)
        {
            if (n < _SList.Count)
            {
                // Loop through each SlotItem at the given
                // level, looking for an intersection with the
                // given CalendarItem

                List<SlotItem> list = _SList[n];

                for (int i = 0; i < list.Count; i++)
                {
                    SlotItem si = list[i];

                    DateTime start = item.StartTime > si.CItem.StartTime ? item.StartTime : si.CItem.StartTime;
                    DateTime end = item.EndTime < si.CItem.EndTime ? item.EndTime : si.CItem.EndTime;

                    // If we found an item, return it

                    if (start < end)
                        return (si);
                }
            }

            // Nothing currently at that slot

            return (null);
        }

        #endregion

        #region CountColumns

        /// <summary>
        /// Counts the number of columns for
        /// each column zero entry slot lists
        /// </summary>
        public void CountColumns()
        {
            if (_SList.Count > 0)
            {
                for (int i = 0; i < _SList[0].Count; i++)
                {
                    SlotItem si = _SList[0][i];

                    SetColumnCount(si, GetColumnCount(si, 1));
                }
            }
        }

        /// <summary>
        /// Gets the max column count from all
        /// zero level slot paths
        /// </summary>
        /// <param name="si">Initial SlotItem</param>
        /// <param name="count">Running level count</param>
        /// <returns></returns>
        private int GetColumnCount(SlotItem si, int count)
        {
            if (si.Count > 0)
                return (si.Count);

            int maxCount = count;

            if (si.SList != null)
            {
                for (int i = 0; i < si.SList.Count; i++)
                {
                    int c = GetColumnCount(si.SList[i], count + 1);

                    if (c > maxCount)
                        maxCount = c;
                }
            }

            return (maxCount);
        }

        /// <summary>
        /// Sets all column entry counts to the given
        /// count
        /// </summary>
        /// <param name="si">Initial SlotItem</param>
        /// <param name="count">Count</param>
        private void SetColumnCount(SlotItem si, int count)
        {
            if (si.SList != null)
            {
                for (int i = 0; i < si.SList.Count; i++)
                    SetColumnCount(si.SList[i], count);
            }

            if (si.Count == 0)
                si.Count = count;
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the Column slot list
        /// </summary>
        public void Clear()
        {
            _SList.Clear();
        }

        #endregion

        #endregion
    }

    #region SlotItem class definition

    public class SlotItem
    {
        #region Private variables

        private CalendarItem _CItem;        // CalendarItem
        private List<SlotItem> _SList;      // List of peer SlotItems
        private int _Count;                 // Count of peer items
        private int _Column;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cItem">CalendarItem</param>
        public SlotItem(CalendarItem cItem)
        {
            _CItem = cItem;
        }

        #region Public properties

        /// <summary>
        /// Gets and sets the slots CalendarItem
        /// </summary>
        public CalendarItem CItem
        {
            get { return (_CItem); }
            set { _CItem = value; }
        }

        /// <summary>
        /// Gets the peer SlotItem list
        /// </summary>
        public List<SlotItem> SList
        {
            get { return (_SList); }
        }

        /// <summary>
        /// Gets and sets the peer level count
        /// </summary>
        public int Count
        {
            get { return (_Count); }
            set { _Count = value; }
        }

        /// <summary>
        /// Gets and sets the peer column
        /// </summary>
        public int Column
        {
            get { return (_Column); }
            set { _Column = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a slot to the peer SlotItem list
        /// </summary>
        /// <param name="si">SlotItem to add</param>
        /// <param name="column">Slot column</param>
        public void AddPeerSlot(SlotItem si, int column)
        {
            if (si != null)
            {
                if (_SList == null)
                    _SList = new List<SlotItem>();

                if (_SList.Contains(si) == false)
                {
                    si.Column = column;

                    _SList.Add(si);
                }
            }
        }

        #endregion
    }

    #endregion
}
#endif

