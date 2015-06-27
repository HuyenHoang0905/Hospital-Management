#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentCategoryColorCollection
    {
        #region Events

        /// <summary>
        /// Occurs when AppointmentCategoryColorCollection has changed
        /// </summary>
        [Description("Occurs when the AppointmentCategoryColorCollection has changed.")]
        public event EventHandler<EventArgs> AppointmentCategoryColorCollectionChanged;

        #endregion

        #region Private variables

        private Dictionary<string, AppointmentCategoryColor> _List;

        #endregion

        /// <summary>
        /// AppointmentCategoryColorCollection
        /// </summary>
        public AppointmentCategoryColorCollection()
        {
            _List = new Dictionary<string, AppointmentCategoryColor>();
        }

        #region Public properties

        /// <summary>
        /// Gets the Count of items defined
        /// </summary>
        public int Count
        {
            get { return (_List.Count); }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds a AppointmentCategoryColor to the collection
        /// </summary>
        /// <param name="acc"></param>
        public void Add(AppointmentCategoryColor acc)
        {
            _List[acc.ColorName] = acc;

            acc.AppointmentCategoryColorChanged += CategoryColorChanged;

            OnAppointmentCategoryColorCollectionChanged();
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes an entry from the collection, by color name
        /// </summary>
        /// <param name="colorName">Color name</param>
        public void Remove(string colorName)
        {
            if (_List.ContainsKey(colorName))
            {
                _List[colorName].AppointmentCategoryColorChanged -= CategoryColorChanged;

                _List.Remove(colorName);

                OnAppointmentCategoryColorCollectionChanged();
            }
        }

        /// <summary>
        /// Removes an entry from the collection, by AppointmentCategoryColor
        /// </summary>
        /// <param name="categoryColor">AppointmentCategoryColor</param>
        public void Remove(AppointmentCategoryColor categoryColor)
        {
            Remove(categoryColor.ColorName);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the AppointmentCategoryColor collection
        /// </summary>
        public void Clear()
        {
            foreach (AppointmentCategoryColor ac in _List.Values)
                ac.AppointmentCategoryColorChanged -= CategoryColorChanged;

            _List.Clear();

            OnAppointmentCategoryColorCollectionChanged();
        }

        #endregion

        #region Items

        /// <summary>
        /// Gets the entire list of added AppointmentCategoryColor items
        /// </summary>
        public AppointmentCategoryColor[] Items
        {
            get
            {
                AppointmentCategoryColor[] items = new AppointmentCategoryColor[_List.Count];

                _List.Values.CopyTo(items, 0);

                return (items);
            }
        }

        #endregion

        #region this

        /// <summary>
        /// Gets the AppointmentCategoryColor from the given
        /// color name string index
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        public AppointmentCategoryColor this[string colorName]
        {
            get
            {
                AppointmentCategoryColor acc;

                _List.TryGetValue(colorName, out acc);

                return (acc);
            }
        }

        #endregion

        #region CategoryColorChanged

        /// <summary>
        /// CategoryColorChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CategoryColorChanged(object sender, EventArgs e)
        {
            OnAppointmentCategoryColorCollectionChanged();
        }

        #endregion

        #region OnAppointmentCategoryColorCollectionChanged

        /// <summary>
        /// OnAppointmentCategoryColorCollectionChanged
        /// </summary>
        private void OnAppointmentCategoryColorCollectionChanged()
        {
            if (AppointmentCategoryColorCollectionChanged != null)
                AppointmentCategoryColorCollectionChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
#endif