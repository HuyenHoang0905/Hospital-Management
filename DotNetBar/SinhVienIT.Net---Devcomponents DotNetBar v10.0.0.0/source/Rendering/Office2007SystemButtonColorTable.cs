using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for Office 2007 style system button displayed in form caption.
    /// </summary>
    public class Office2007SystemButtonColorTable
    {
        /// <summary>
        /// Gets or sets the color table of default button state.
        /// </summary>
        public Office2007SystemButtonStateColorTable Default = null;

        /// <summary>
        /// Gets or sets the color table of button state when mouse is over the button.
        /// </summary>
        public Office2007SystemButtonStateColorTable MouseOver = null;

        /// <summary>
        /// Gets or sets the color table of button state when mouse is pressed over the button.
        /// </summary>
        public Office2007SystemButtonStateColorTable Pressed = null;

    }
}
