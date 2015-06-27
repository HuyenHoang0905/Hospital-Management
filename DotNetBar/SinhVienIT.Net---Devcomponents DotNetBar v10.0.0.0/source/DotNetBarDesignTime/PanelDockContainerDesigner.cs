using System;
using System.Windows.Forms.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
	#region PanelDockContainerDesigner
	/// <summary>
	/// Designer for Tab Control Panel.
	/// </summary>
	public class PanelDockContainerDesigner:PanelExDesigner
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
			PanelDockContainer p = this.Control as PanelDockContainer;
			if (p == null)
				return;
			p.ApplyLabelStyle();
			p.Text = "";
		}
	}
	#endregion
}
