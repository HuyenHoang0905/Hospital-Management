using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the typed command link for ButtonItem type.
    /// </summary>
    public class CommandLinkButtonItem : CommandLink
    {
        /// <summary>
        /// Gets reference to the ButtonItem this CommandLink is linked to. Note that this is the first ButtonItem object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public ButtonItem Item
        {
            get
            {
                return this.GetItem(typeof(ButtonItem)) as ButtonItem;
            }
        }
    }
}
