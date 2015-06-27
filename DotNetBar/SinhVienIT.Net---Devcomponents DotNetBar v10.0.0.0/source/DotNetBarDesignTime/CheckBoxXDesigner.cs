using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
#if FRAMEWORK20
using System.Collections.Generic;
using System.Collections;
#endif

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents WinForms designer for Slider control.
    /// </summary>
    public class CheckBoxXDesigner : ControlDesigner
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
            CheckBoxX checkBox = this.Control as CheckBoxX;
            if (checkBox != null)
            {
                checkBox.Style = eDotNetBarStyle.StyleManagerControlled;
                checkBox.Width = 100;
            }
        }
    }
}
