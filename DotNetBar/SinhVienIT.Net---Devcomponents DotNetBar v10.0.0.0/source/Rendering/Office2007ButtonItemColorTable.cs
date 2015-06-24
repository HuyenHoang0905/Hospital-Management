using System;
using System.Text;
using System.Drawing;


namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for ButtonItem object.
    /// </summary>
    public class Office2007ButtonItemColorTable
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the color table.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Gets or sets the color table applied for button in default state.
        /// </summary>
        public Office2007ButtonItemStateColorTable Default = null;

        /// <summary>
        /// Gets or sets the color table applied when mouse is over the button.
        /// </summary>
        public Office2007ButtonItemStateColorTable MouseOver = null;

        /// <summary>
        /// Gets or sets the color table applied when mouse is over the buttons inactive split part. Applies to split button appearance only.
        /// </summary>
        public Office2007ButtonItemStateColorTable MouseOverSplitInactive = null;

        /// <summary>
        /// Gets or sets the color table applied when mouse is pressed over the button.
        /// </summary>
        public Office2007ButtonItemStateColorTable Pressed = null;

        /// <summary>
        /// Gets or sets the color table applied when mouse is pressed over the button.
        /// </summary>
        public Office2007ButtonItemStateColorTable Checked = null;

        /// <summary>
        /// Gets or sets the color table applied when button is expanded.
        /// </summary>
        public Office2007ButtonItemStateColorTable Expanded = null;

        /// <summary>
        /// Gets or sets the color table applied when cursor is over button on a menu.
        /// </summary>
        public Office2007ButtonItemStateColorTable Disabled = null;
        #endregion
    }
}
