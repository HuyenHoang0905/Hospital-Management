using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides information for ribbon customization events.
    /// </summary>
    public class RibbonCustomizeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether the menu popup will be cancelled. Default value is false.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Gets or sets the reference to the object customize menu will be acting on. This could be an ButtonItem or any
        /// BaseItem derived class as well as RibbonBar object.
        /// </summary>
        public object ContextObject = null;

        /// <summary>
        /// Gets or sets the reference to the popup menu that will be displayed. You can change the members of SubItems collection to add/remove/change
        /// the context menu items that will be displayed.
        /// </summary>
        public BaseItem PopupMenu = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="contextObject">Reference to context object.</param>
        /// <param name="popupMenuItem">Reference to popup menu item if any.</param>
        public RibbonCustomizeEventArgs(object contextObject, BaseItem popupMenuItem)
        {
            this.ContextObject = contextObject;
            this.PopupMenu = popupMenuItem;
        }
    }
}
