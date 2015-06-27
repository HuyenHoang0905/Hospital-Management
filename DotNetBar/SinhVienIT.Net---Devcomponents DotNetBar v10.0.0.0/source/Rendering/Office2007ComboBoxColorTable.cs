using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the combo box.
    /// </summary>
    public class Office2007ComboBoxColorTable
    {
        /// <summary>
        /// Gets or sets the color for combo box in default state.
        /// </summary>
        public Office2007ComboBoxStateColorTable Default = new Office2007ComboBoxStateColorTable();

        /// <summary>
        /// Gets or sets the color for standalone combo box in default state. Standalone combo box is a control not used by ComboBoxItem.
        /// </summary>
        public Office2007ComboBoxStateColorTable DefaultStandalone = new Office2007ComboBoxStateColorTable();

        /// <summary>
        /// Gets or sets the colors when mouse is over the control.
        /// </summary>
        public Office2007ComboBoxStateColorTable MouseOver = new Office2007ComboBoxStateColorTable();

        /// <summary>
        /// Gets or sets the colors when control is dropped down.
        /// </summary>
        public Office2007ComboBoxStateColorTable DroppedDown = new Office2007ComboBoxStateColorTable();
    }
}
