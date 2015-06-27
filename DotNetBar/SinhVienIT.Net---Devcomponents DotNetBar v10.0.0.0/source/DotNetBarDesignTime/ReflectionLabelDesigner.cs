using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    public class ReflectionLabelDesigner : ControlDesigner
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
			base.OnSetComponentDefaults();
			SetDesignTimeDefaults();
		}
#endif

        protected virtual void SetDesignTimeDefaults()
        {
            ReflectionLabel l = this.Control as ReflectionLabel;
            l.Text = "<b><font size=\"+6\"><i>Dev</i><font color=\"#B02B2C\">Components</font></font></b>";
        }
    }
}
