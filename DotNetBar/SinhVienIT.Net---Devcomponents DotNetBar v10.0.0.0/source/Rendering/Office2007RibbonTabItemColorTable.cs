using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for RibbonTabItem states.
    /// </summary>
    public class Office2007RibbonTabItemColorTable
    {
        /// <summary>
        /// Gets or sets the name of the color table.
        /// </summary>
        public string Name = "";
        /// <summary>
        /// Gets or sets the default tab colors.
        /// </summary>
        public Office2007RibbonTabItemStateColorTable Default = new Office2007RibbonTabItemStateColorTable();
        /// <summary>
        /// Gets or sets the selected tab colors.
        /// </summary>
        public Office2007RibbonTabItemStateColorTable Selected = new Office2007RibbonTabItemStateColorTable();
        /// <summary>
        /// Gets or sets the selected tab colors when mouse is over the tab.
        /// </summary>
        public Office2007RibbonTabItemStateColorTable SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
        /// <summary>
        /// Gets or sets the colors when mouse is over the tab but tab is not selected.
        /// </summary>
        public Office2007RibbonTabItemStateColorTable MouseOver = new Office2007RibbonTabItemStateColorTable();

        /// <summary>
        /// Gets or sets the round corner size for the top part of the ribbon tab item.
        /// </summary>
        public int CornerSize = 3;
    }
}
