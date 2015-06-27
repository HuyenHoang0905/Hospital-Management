#if FRAMEWORK20
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    public class CustomCalendarItemCollection : Collection<CustomCalendarItem>
    {
        #region Events

        /// <summary>
        /// Occurs when the collection has changed
        /// </summary>
        [Description("Occurs when the collection has changed.")]
        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region Private variables

        private int _UpdateCount;

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of CustomCalendarItems to the collection
        /// </summary>
        /// <param name="items">Array of items to add</param>
        public void AddRange(CustomCalendarItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
                Add(items[i]);

            OnCollectionChanged();
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes a CustomCalendarItem from
        /// the collection.
        /// </summary>
        /// <param name="item">Item to remove</param>
        public new void Remove(CustomCalendarItem item)
        {
            if (item.BaseCalendarItem != null)
                item = item.BaseCalendarItem;

            base.Remove(item);
        }

        #endregion

        #region RemoveItem

        /// <summary>
        /// Processes list RemoveItem calls
        /// </summary>
        /// <param name="index">Index to remove</param>
        protected override void RemoveItem(int index)
        {
            HookItem(Items[index], false);

            base.RemoveItem(index);

            OnCollectionChanged();
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">CustomCalendarItem to add</param>
        protected override void InsertItem(int index, CustomCalendarItem item)
        {
            if (item != null)
            {
                HookItem(item, true);

                base.InsertItem(index, item);

                OnCollectionChanged();
            }
        }

        #endregion

        #region SetItem

        /// <summary>
        /// Processes list SetItem calls (e.g. replace)
        /// </summary>
        /// <param name="index">Index to replace</param>
        /// <param name="newItem">CustomCalendarItem to replace</param>
        protected override void SetItem(int index, CustomCalendarItem newItem)
        {
            if (newItem != null)
            {
                HookItem(Items[index], false);
                HookItem(newItem, true);

                base.SetItem(index, newItem);

                OnCollectionChanged();
            }
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
                HookItem(Items[i], false);

            base.ClearItems();

            OnCollectionChanged();
        }

        #endregion

        #region HookItem

        /// <summary>
        /// Hooks needed system events
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hook"></param>
        private void HookItem(CustomCalendarItem item, bool hook)
        {
            if (hook == true)
            {
                item.StartTimeChanged += ItemStartTimeChanged;
                item.EndTimeChanged += ItemEndTimeChanged;
                item.OwnerKeyChanged += ItemOwnerKeyChanged;
                item.CategoryColorChanged += ItemCategoryColorChanged;
                item.VisibleChanged += ItemVisibleChanged;
                item.CollateIdChanged += ItemCollateIdChanged;
            }
            else
            {
                item.StartTimeChanged -= ItemStartTimeChanged;
                item.EndTimeChanged -= ItemEndTimeChanged;
                item.OwnerKeyChanged -= ItemOwnerKeyChanged;
                item.CategoryColorChanged -= ItemCategoryColorChanged;
                item.VisibleChanged -= ItemVisibleChanged;
                item.CollateIdChanged -= ItemCollateIdChanged;
            }
        }

        #endregion

        #region Event processing

        /// <summary>
        /// Processes OwnerKeyChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemOwnerKeyChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        /// <summary>
        /// Processes StartTimeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemStartTimeChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        /// <summary>
        /// Processes EndTimeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemEndTimeChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        /// <summary>
        /// Processes ItemCategoryColorChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemCategoryColorChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        /// <summary>
        /// Processes ItemVisibleChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemVisibleChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        /// <summary>
        /// Processes ItemCollateIdChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemCollateIdChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        #endregion

        #region OnCollectionChanged

        /// <summary>
        /// Propagates CollectionChanged events
        /// </summary>
        protected virtual void OnCollectionChanged()
        {
            if (_UpdateCount == 0)
            {
                if (CollectionChanged != null)
                    CollectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Begin/EndUpdate

        /// <summary>
        /// Begins Update block
        /// </summary>
        public void BeginUpdate()
        {
            _UpdateCount++;
        }

        /// <summary>
        /// Ends update block
        /// </summary>
        public void EndUpdate()
        {
            if (_UpdateCount == 0)
            {
                throw new InvalidOperationException(
                    "EndUpdate must be called After BeginUpdate");
            }

            _UpdateCount--;

            if (_UpdateCount == 0)
                OnCollectionChanged();
        }

        #endregion
    }
}
#endif

