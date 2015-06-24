using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the typed command link for ControlContainerItem type.
    /// </summary>
    public class CommandLinkControlContainerItem : CommandLink
    {
        /// <summary>
        /// Gets reference to the ControlContainerItem this CommandLink is linked to. Note that this is the first ControlContainerItem object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public ControlContainerItem Item
        {
            get
            {
                return this.GetItem(typeof(ControlContainerItem)) as ControlContainerItem;
            }
        }
    }
}
