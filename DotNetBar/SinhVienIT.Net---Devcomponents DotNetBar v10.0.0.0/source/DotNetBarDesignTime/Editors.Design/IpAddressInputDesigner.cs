#if FRAMEWORK20
using System;
using System.Windows.Forms.Design;
using System.Text;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents designer for IpAddressInput control.
    /// </summary>
    public class IpAddressInputDesigner: VisualControlBaseDesigner
    {
        #region Private Variables
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the IpAddressInputDesigner class.
        /// </summary>
        public IpAddressInputDesigner()
        {
            this.AutoResizeHandles = true;
        }
        #endregion

        #region Internal Implementation
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            IpAddressInput c = this.Control as IpAddressInput;
            if (c != null)
            {
                c.AutoOverwrite = true;
                c.ButtonFreeText.Visible = true;
                c.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
                c.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            }
            base.InitializeNewComponent(defaultValues);
        }

        public override System.Windows.Forms.Design.SelectionRules SelectionRules
        {
            get
            {
                SelectionRules rules = base.SelectionRules;
                if (!this.Control.AutoSize)
                {
                    rules &= ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
                }
                return rules;
            }
        }
        #endregion
    }
}
#endif