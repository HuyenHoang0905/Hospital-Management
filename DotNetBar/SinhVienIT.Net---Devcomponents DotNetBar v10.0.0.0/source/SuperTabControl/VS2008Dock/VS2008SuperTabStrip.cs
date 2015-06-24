#if FRAMEWORK20
using System.Drawing;

namespace DevComponents.DotNetBar
{
    internal class VS2008DockSuperTabStrip : SuperTabStripBaseDisplay
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public VS2008DockSuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
        }

        #region Internal properties

        /// <summary>
        /// Gets the TabLayoutOffset
        /// </summary>
        internal override Size TabLayoutOffset
        {
            get { return (new Size(3, 2)); } 
        }

        #endregion
    }
}
#endif