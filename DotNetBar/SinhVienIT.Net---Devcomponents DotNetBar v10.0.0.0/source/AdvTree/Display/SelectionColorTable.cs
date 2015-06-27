using System;
using System.Text;
using DevComponents.WinForms.Drawing;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Defines the color table for tree selection.
    /// </summary>
    public class SelectionColorTable
    {
        /// <summary>
        /// Gets or sets the outer border for the selection.
        /// </summary>
        public Border Border = null;
        /// <summary>
        /// Gets or sets the inner border for the selection.
        /// </summary>
        public Border InnerBorder = null;
        /// <summary>
        /// Gets or sets the selection fill.
        /// </summary>
        public Fill Fill = null;
    }
}
