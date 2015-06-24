using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(true), ToolboxBitmap(typeof(VScrollBarAdv), "ScrollBar.VScrollBarAdv.ico")]
    public class VScrollBarAdv : ScrollBarAdv
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
            return true;
        }
        #endregion

        #region Property Hiding
        
        #endregion

    }
}
