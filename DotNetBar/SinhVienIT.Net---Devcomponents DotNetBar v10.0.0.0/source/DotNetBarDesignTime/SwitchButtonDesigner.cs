using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using System.Collections.Generic;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Designer for SwitchButton control.
    /// </summary>
    public class SwitchButtonDesigner : ControlDesigner
    {
        #if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
        public override void OnSetComponentDefaults()
        {
            SetDesignTimeDefaults();
            base.OnSetComponentDefaults();
        }
#endif

        private void SetDesignTimeDefaults()
        {
            SwitchButton switchButton = this.Control as SwitchButton;
            if (switchButton != null)
            {
                switchButton.Style = eDotNetBarStyle.StyleManagerControlled;
            }
        }
    }
}
