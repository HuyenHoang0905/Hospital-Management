#if FRAMEWORK20
using System.Drawing;

namespace DevComponents.DotNetBar
{
    internal class Office2007SuperTabStrip : SuperTabStripBaseDisplay
    {
        /// <summary>
        /// Office2007 TabStrip base display constructor
        /// </summary>
        /// <param name="tabStripItem">Associated TabStrip</param>
        public Office2007SuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
        }

        #region Internal properties

        /// <summary>
        /// Tab layout offsets
        /// </summary>
        internal override Size TabLayoutOffset
        {
            get { return (new Size(3, 2)); }
        }

        #endregion

    }
}
#endif