using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for ListViewEx control.
    /// </summary>
    public class Office2007ListViewColorTable
    {
        /// <summary>
        /// Gets or sets the background color of the columns.
        /// </summary>
        public LinearGradientColorTable ColumnBackground = null;

        /// <summary>
        /// Gets or sets the color of the column separator.
        /// </summary>
        public Color ColumnSeparator = Color.Empty;

        /// <summary>
        /// Gets or sets the color of the control border.
        /// </summary>
        public Color Border = Color.Empty;

        /// <summary>
        /// Gets or sets the background colors for the selected item.
        /// </summary>
        public LinearGradientColorTable SelectionBackground = null;

        /// <summary>
        /// Gets or sets the color of the selected item border that is draw on top and bottom of the selection.
        /// </summary>
        public Color SelectionBorder = Color.Empty;
    }
}
