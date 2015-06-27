using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for RibbonBar in various states.
    /// </summary>
    public class Office2007RibbonBarColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the default RibbonBar state.
        /// </summary>
        public Office2007RibbonBarStateColorTable Default = new Office2007RibbonBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for RibbonBar when mouse is over the control.
        /// </summary>
        public Office2007RibbonBarStateColorTable MouseOver = new Office2007RibbonBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for RibbonBar when ribbon bar is in overflow state and expanded to show all the items.
        /// </summary>
        public Office2007RibbonBarStateColorTable Expanded = new Office2007RibbonBarStateColorTable();
    }
}
