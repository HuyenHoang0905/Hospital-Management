using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Reprensets the color table for the CheckBoxItem.
    /// </summary>
    public class Office2007CheckBoxColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the item in default state.
        /// </summary>
        public Office2007CheckBoxStateColorTable Default = new Office2007CheckBoxStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the item when mouse is over the item.
        /// </summary>
        public Office2007CheckBoxStateColorTable MouseOver = new Office2007CheckBoxStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the item when mouse is pressed over the item.
        /// </summary>
        public Office2007CheckBoxStateColorTable Pressed = new Office2007CheckBoxStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the item when item is disabled.
        /// </summary>
        public Office2007CheckBoxStateColorTable Disabled = new Office2007CheckBoxStateColorTable();
    }
}
