#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    public class PropertySettingsCollection : Collection<PropertySettings>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the PropertyGridInfoCollection class.
        /// </summary>
        /// <param name="owner"></param>
        public PropertySettingsCollection(AdvPropertyGrid owner)
        {
            _Owner = owner;
        }
        #endregion

        #region Internal Implementation
        protected override void InsertItem(int index, PropertySettings item)
        {
            base.InsertItem(index, item);
            OnAfterItemAdded(item);
        }

        private void OnAfterItemAdded(PropertySettings item)
        {
            _Owner.PropertySettingsItemAdded(item);
        }

        protected override void ClearItems()
        {
            PropertySettings[] items = new PropertySettings[this.Count];
            this.CopyTo(items, 0);
            
            base.ClearItems();

            foreach (PropertySettings item in items)
            {
                OnAfterItemRemoved(item);
            }
        }

        protected override void RemoveItem(int index)
        {
            PropertySettings info = this[index];
            base.RemoveItem(index);
            OnAfterItemRemoved(info);
        }

        private void OnAfterItemRemoved(PropertySettings info)
        {
            _Owner.PropertySettingsItemRemoved(info);
        }

        protected override void SetItem(int index, PropertySettings item)
        {
            PropertySettings oldItem = this[index];
            base.SetItem(index, item);

            OnAfterItemRemoved(oldItem);
            OnAfterItemAdded(item);
        }

        /// <summary>
        /// Gets property info object based on property name.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>PropertyGridInfo instance or null if it cannot be found.</returns>
        public virtual PropertySettings this[string propertyName]
        {
            get
            {
                foreach (PropertySettings item in this)
                {
                    if (item.PropertyName == propertyName) return item;
                }
                return null;
            }
        }

        private Dictionary<PropertyDescriptor, PropertySettings> _DictionaryByDescriptor = new Dictionary<PropertyDescriptor, PropertySettings>();
        /// <summary>
        /// Gets property info object based on property descriptor.
        /// </summary>
        /// <param name="propertyDescriptor">Property descriptor.</param>
        /// <returns>PropertyGridInfo instance or null if it cannot be found.</returns>
        public virtual PropertySettings this[PropertyDescriptor propertyDescriptor]
        {
            get
            {
                foreach (PropertySettings item in this)
                {
                    if (item.PropertyDescriptor.Equals(propertyDescriptor)) return item;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns property setting based on property descriptor or name if property descriptor is not set on property settings
        /// Property descriptor takes precedence.
        /// </summary>
        /// <param name="propertyDescriptor">Property descriptor to look for.</param>
        /// <param name="propertyName">Property Name to look for.</param>
        /// <returns>Property settings instance or null</returns>
        public virtual PropertySettings this[PropertyDescriptor propertyDescriptor, string propertyName]
        {
            get
            {
                PropertySettings settingByName = null;
                foreach (PropertySettings item in this)
                {
                    if (item.PropertyDescriptor != null && item.PropertyDescriptor.Equals(propertyDescriptor))
                        return item;
                    else if (item.PropertyName == propertyName)
                        settingByName = item;
                }
                return settingByName;
            }
        }

        private AdvPropertyGrid _Owner = null;
        /// <summary>
        /// Gets the owner of the collection.
        /// </summary>
        public AdvPropertyGrid Owner
        {
            get { return _Owner; }
        }
        #endregion
    }
}
#endif