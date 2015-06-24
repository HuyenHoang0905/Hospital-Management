#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms designer for the VisualControlBase control.
    /// </summary>
    public class VisualControlBaseDesigner : ControlDesigner
    {
        #region Private Variables
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            VisualControlBase c = this.Control as VisualControlBase;
            if (c != null)
            {
                c.Height = c.PreferredHeight;
            }
            base.InitializeNewComponent(defaultValues);
        }

        #endregion

    }
}
#endif

