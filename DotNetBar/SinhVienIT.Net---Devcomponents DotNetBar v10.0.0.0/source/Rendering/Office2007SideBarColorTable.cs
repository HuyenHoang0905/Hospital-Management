using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table for Office 2007 Style Side Bar Control.
    /// </summary>
    public class Office2007SideBarColorTable
    {
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        public LinearGradientColorTable Background = null;
        /// <summary>
        /// Gets or sets the control border color.
        /// </summary>
        public Color Border = Color.Empty;

        /// <summary>
        /// Gets or sets the color of SideBarPanelItem text.
        /// </summary>
        public Color SideBarPanelItemText = Color.Empty;

        /// <summary>
        /// Gets or sets the color table for SideBarPanelItem in default state.
        /// </summary>
        public GradientColorTable SideBarPanelItemDefault = null;

        /// <summary>
        /// Gets or sets the color table for SideBarPanelItem in mouse over state.
        /// </summary>
        public GradientColorTable SideBarPanelItemMouseOver = null;

        /// <summary>
        /// Gets or sets the color table for SideBarPanelItem in expanded state.
        /// </summary>
        public GradientColorTable SideBarPanelItemExpanded = null;

        /// <summary>
        /// Gets or sets the color table for SideBarPanelItem when mouse button is pressed on the item.
        /// </summary>
        public GradientColorTable SideBarPanelItemPressed = null;
    }
}
