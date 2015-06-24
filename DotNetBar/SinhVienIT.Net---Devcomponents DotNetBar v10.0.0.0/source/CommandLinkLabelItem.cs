using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the command link for a label item
    /// </summary>
    public class CommandLinkLabelItem : CommandLink
    {
        /// <summary>
        /// Gets reference to the LabelItem this CommandLink is linked to. Note that this is the first LabelItem object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public LabelItem Item
        {
            get
            {
                return this.GetItem(typeof(LabelItem)) as LabelItem;
            }
        }
    }
}
