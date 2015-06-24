#if FRAMEWORK20
using System;
using System.Windows.Forms.Design;
using System.Text;
using DevComponents.DotNetBar;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents the base designer for the NumericInputBase controls.
    /// </summary>
    public class NumericInputBaseDesigner : VisualControlBaseDesigner
    {
        #region Private Variables
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NumericInputBaseDesigner class.
        /// </summary>
        public NumericInputBaseDesigner()
        {
            this.AutoResizeHandles = true;
        }
        #endregion

        #region Internal Implementation
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            NumericInputBase c = this.Control as NumericInputBase;
            if (c != null)
            {
                c.ShowUpDown = true;
                c.AutoOverwrite = false;
                c.ButtonFreeText.Shortcut = eShortcut.F2;
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

