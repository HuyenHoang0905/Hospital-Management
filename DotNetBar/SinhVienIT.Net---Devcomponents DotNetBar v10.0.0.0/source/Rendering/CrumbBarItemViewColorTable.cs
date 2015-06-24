using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for CrumbBarItemView.
    /// </summary>
    public class CrumbBarItemViewColorTable
    {
        /// <summary>
        /// Gets or sets the default state color table.
        /// </summary>
        public CrumbBarItemViewStateColorTable Default = null;
        /// <summary>
        /// Gets or sets active mouse over color table.
        /// </summary>
        public CrumbBarItemViewStateColorTable MouseOver = null;
        /// <summary>
        /// Gets or sets inactive part mouse over color table.
        /// </summary>
        public CrumbBarItemViewStateColorTable MouseOverInactive = null;
        /// <summary>
        /// Gets or sets the pressed color table.
        /// </summary>
        public CrumbBarItemViewStateColorTable Pressed = null;
    }
}
