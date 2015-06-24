using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for ColorItem.
    /// </summary>
    public class Office2007ColorItemColorTable
    {
        #region ColorItem Colors
        /// <summary>
        /// Gets or sets the border that is drawn around each individual color item or color item group.
        /// </summary>
        public Color Border = ColorScheme.GetColor("ECE9D8");
        /// <summary>
        /// Gets or sets the inner mouse over color.
        /// </summary>
        public Color MouseOverInnerBorder = ColorScheme.GetColor("FFE294");
        /// <summary>
        /// Gets or sets the outer mouse over color.
        /// </summary>
        public Color MouseOverOuterBorder = ColorScheme.GetColor("F29436");
        #endregion
    }
}
