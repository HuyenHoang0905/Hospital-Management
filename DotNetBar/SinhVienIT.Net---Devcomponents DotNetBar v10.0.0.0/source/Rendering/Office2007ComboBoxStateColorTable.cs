using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the combo box colors for a particular state.
    /// </summary>
    public class Office2007ComboBoxStateColorTable
    {
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public Color Border = Color.Empty;
        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color Background = Color.Empty;
        /// <summary>
        /// Gets or sets the outer expand button border.
        /// </summary>
        public LinearGradientColorTable ExpandBorderOuter = null;
        /// <summary>
        /// Gets or sets the inner expand button border.
        /// </summary>
        public LinearGradientColorTable ExpandBorderInner = null;
        /// <summary>
        /// Gets or sets the background color of the expand button.
        /// </summary>
        public LinearGradientColorTable ExpandBackground = null;
        /// <summary>
        /// Gets or sets the foreground color of the expand button.
        /// </summary>
        public Color ExpandText = Color.Empty;
    }
}
