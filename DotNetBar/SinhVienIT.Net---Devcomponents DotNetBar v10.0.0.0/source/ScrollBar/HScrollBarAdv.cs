using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.ScrollBar
{
    [ToolboxItem(true), ToolboxBitmap(typeof(HScrollBarAdv), "ScrollBar.HScrollBarAdv.ico")]
    public class HScrollBarAdv : ScrollBarAdv
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override bool IsVertical()
        {
            return false;
        }
        #endregion

    }
}
