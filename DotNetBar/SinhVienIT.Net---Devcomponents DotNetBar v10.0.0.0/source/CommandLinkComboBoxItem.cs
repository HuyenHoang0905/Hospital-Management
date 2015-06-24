using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the command link for a combo box item
    /// </summary>
    public class CommandLinkComboBoxItem : CommandLink
    {
        /// <summary>
        /// Gets reference to the ComboBoxItem this CommandLink is linked to. Note that this is the first ComboBoxItem object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public ComboBoxItem Item
        {
            get
            {
                return this.GetItem(typeof(ComboBoxItem)) as ComboBoxItem;
            }
        }
    }
}
