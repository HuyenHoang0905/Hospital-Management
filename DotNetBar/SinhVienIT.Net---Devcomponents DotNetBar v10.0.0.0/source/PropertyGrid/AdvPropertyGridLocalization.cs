#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the class that stores text used by property grid control for localization purposes.
    /// </summary>
    [ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AdvPropertyGridLocalization : INotifyPropertyChanged
    {
        #region Internal Implementation
        private string _CategorizeToolbarTooltip = "Categorized";
        /// <summary>
        /// Gets or sets tooltip used by Categorized toolbar button.
        /// </summary>
        [DefaultValue("Categorized"), Description("Tooltip used by Categorize toolbar button"), Localizable(true)]
        public string CategorizeToolbarTooltip
        {
            get { return _CategorizeToolbarTooltip; }
            set
            {
                if (value != _CategorizeToolbarTooltip)
                {
                    string oldValue = _CategorizeToolbarTooltip;
                    _CategorizeToolbarTooltip = value;
                    OnCategorizeToolbarTooltipChanged(oldValue, value);
                }
            }
        }
        private void OnCategorizeToolbarTooltipChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("CategorizeToolbarTooltip"));
        }

        private string _AlphabeticalToolbarTooltip = "Alphabetical";
        /// <summary>
        /// Gets or sets tooltip used by Alphabetical toolbar button.
        /// </summary>
        [DefaultValue("Alphabetical"), Description("Tooltip used by Alphabetical toolbar button"), Localizable(true)]
        public string AlphabeticalToolbarTooltip
        {
            get { return _AlphabeticalToolbarTooltip; }
            set
            {
                if (value != _AlphabeticalToolbarTooltip)
                {
                    string oldValue = _AlphabeticalToolbarTooltip;
                    _AlphabeticalToolbarTooltip = value;
                    OnAplhabeticalToolbarTooltipChanged(oldValue, value);
                }
            }
        }
        private void OnAplhabeticalToolbarTooltipChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("AlphabeticalToolbarTooltip"));
        }

        private string _ErrorSettingPropertyValueTooltip = "Error setting the value. ";
        /// <summary>
        /// Gets or sets the tooltip text used in tooltip when error occurred during property value setting.
        /// </summary>
        [DefaultValue("Error setting the value. "), Description(""), Localizable(true)]
        public string ErrorSettingPropertyValueTooltip
        {
            get { return _ErrorSettingPropertyValueTooltip; }
            set
            {
                if (value != _ErrorSettingPropertyValueTooltip)
                {
                    string oldValue = _ErrorSettingPropertyValueTooltip;
                    _ErrorSettingPropertyValueTooltip = value;
                    OnErrorSettingPropertyValueTooltipChanged(oldValue, value);
                }
            }
        }
        private void OnErrorSettingPropertyValueTooltipChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ErrorSettingPropertyValueTooltip"));
            
        }

        private string _SearchBoxWatermarkText = "Quick Search";
        /// <summary>
        /// Gets or sets the watermark text displayed in search text-box.
        /// </summary>
        [DefaultValue("Quick Search"), Localizable(true), Description("Indicates watermark text displayed in search text-box.")]
        public string SearchBoxWatermarkText
        {
            get { return _SearchBoxWatermarkText; }
            set
            {
                if (value != _SearchBoxWatermarkText)
                {
                    string oldValue = _SearchBoxWatermarkText;
                    _SearchBoxWatermarkText = value;
                    OnSearchBoxWatermarkTextChanged(oldValue, value);
                }
            }
        }

        private void OnSearchBoxWatermarkTextChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("SearchBoxWatermarkText"));
            
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Occurs when property defined by AdvPropertyGridLocalization class has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
#endif