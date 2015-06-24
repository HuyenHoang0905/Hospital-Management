using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    public class RibbonPopupCloseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether the closing of the ribbon menu is canceled.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Gets or sets the source object that was cause of the menu closing. For example this could be reference to an item that was clicked.
        /// Default value is null which indicates that there is no information about the object that caused closing available.
        /// </summary>
        /// <remarks>
        /// 	<para>Following is the possible list of types that this property could be set to
        ///     and causes for closing:</para>
        /// 	<list type="bullet">
        /// 		<item>BaseItem - when an item is clicked the Source will be set to the instance
        ///         of the item that was clicked.</item>
        /// 		<item>RibbonControl - when parent form RibbonControl is on loses the input
        ///         focus the Source will be set to the RibbonControl</item>
        /// 		<item>RibbonTabItem - when tab menu is displayed and user clicks the same tab
        ///         to close the menu. The RibbonTabItem with EventSource=Code will be also set as
        ///         source when user double-clicks the tab to maximize the ribbon.</item>
        /// 		<item>Any other type if RibbonControl.PopupRibbon method is called by your
        ///         code.</item>
        /// 	</list>
        /// </remarks>
        public object Source = null;

        /// <summary>
        /// Gets or sets the source of the event.
        /// </summary>
        public eEventSource EventSource = eEventSource.Code;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public RibbonPopupCloseEventArgs(object source, eEventSource eventSource)
        {
            this.Source = source;
            this.EventSource = eventSource;
        }
    }
}
