using System;
using System.Windows.Forms.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Designer for Navigation Pane Panel.
	/// </summary>
	public class NavigationPanePanelDesigner:PanelExDesigner
	{
		public override SelectionRules SelectionRules
		{
			get{return (SelectionRules.Locked | SelectionRules.Visible);}
		}

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
		private void SetDesignTimeDefaults()
		{
			PanelEx p = this.Control as PanelEx;
			if (p == null)
				return;
			p.ApplyLabelStyle();
			p.Text = "";
		}
	}
}
