using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the scroll bar control.
    /// </summary>
    public class Office2007ScrollBarColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the control default state.
        /// </summary>
        public Office2007ScrollBarStateColorTable Default = new Office2007ScrollBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the control when mouse is pressed over the control.
        /// </summary>
        public Office2007ScrollBarStateColorTable Pressed = new Office2007ScrollBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the control when mouse is over the control but not over an active part of the control like scroll buttons.
        /// </summary>
        public Office2007ScrollBarStateColorTable MouseOverControl = new Office2007ScrollBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the control when mouse is over the active part of the control like scroll buttons.
        /// </summary>
        public Office2007ScrollBarStateColorTable MouseOver = new Office2007ScrollBarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the control when control is disabled.
        /// </summary>
        public Office2007ScrollBarStateColorTable Disabled = new Office2007ScrollBarStateColorTable();
    }
}
