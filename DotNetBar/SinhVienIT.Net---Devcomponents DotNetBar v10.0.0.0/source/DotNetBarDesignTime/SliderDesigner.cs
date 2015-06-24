using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents WinForms designer for Slider control.
    /// </summary>
    public class SliderDesigner : ControlDesigner
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
            Slider slider = this.Control as Slider;
            if (slider != null)
            {
                slider.Style = eDotNetBarStyle.StyleManagerControlled;
                slider.Width = 150;
            }
        }
    }
}
