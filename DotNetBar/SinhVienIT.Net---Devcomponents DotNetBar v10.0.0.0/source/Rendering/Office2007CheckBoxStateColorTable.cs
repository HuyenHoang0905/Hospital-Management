using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the single CheckBoxItem state.
    /// </summary>
    public class Office2007CheckBoxStateColorTable
    {
        /// <summary>
        /// Gets or sets the background colors of the check box.
        /// </summary>
        public LinearGradientColorTable CheckBackground = null;

        /// <summary>
        /// Gets or sets the color of the check border.
        /// </summary>
        public Color CheckBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the inner color of check box border.
        /// </summary>
        public Color CheckInnerBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the inner background color of check box.
        /// </summary>
        public LinearGradientColorTable CheckInnerBackground = null;

        /// <summary>
        /// Gets or sets the color of the check sign that is drawn when item is checked.
        /// </summary>
        public LinearGradientColorTable CheckSign = null;

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color Text = Color.Empty;
    }
}
