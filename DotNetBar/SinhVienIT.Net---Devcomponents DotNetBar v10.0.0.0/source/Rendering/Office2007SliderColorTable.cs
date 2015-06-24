using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the slider item.
    /// </summary>
    public class Office2007SliderColorTable
    {
        /// <summary>
        /// Gets or sets the default state colors.
        /// </summary>
        public Office2007SliderStateColorTable Default = new Office2007SliderStateColorTable();

        /// <summary>
        /// Gets or sets the mouse over state colors.
        /// </summary>
        public Office2007SliderStateColorTable MouseOver = new Office2007SliderStateColorTable();

        /// <summary>
        /// Gets or sets the mouse pressed colors.
        /// </summary>
        public Office2007SliderStateColorTable Pressed = new Office2007SliderStateColorTable();

        /// <summary>
        /// Gets or sets the disabled colors.
        /// </summary>
        public Office2007SliderStateColorTable Disabled = new Office2007SliderStateColorTable();
    }
}
