using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    public class ExpandablePanelDesigner : PanelExDesigner
    {
        #region Internal Implementation
        
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
            ExpandablePanel p = this.Control as ExpandablePanel;
            if (p == null)
                return;
            p.ApplyLabelStyle();
            p.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
            p.Style.Border = eBorderType.SingleLine;
            p.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
            p.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
            p.Text = "";
			p.TitlePanel.ApplyPanelStyle();
            this.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["ColorSchemeStyle"] = TypeDescriptor.CreateProperty(typeof(ExpandablePanelDesigner), (PropertyDescriptor)properties["ColorSchemeStyle"], new Attribute[]
				{
					new DefaultValueAttribute(eDotNetBarStyle.Office2003),
					new BrowsableAttribute(true),
					new CategoryAttribute("Style"),
                    new DescriptionAttribute("Gets or sets color scheme style.")});
        }

        /// <summary>
        ///     Gets or sets color scheme style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets color scheme style."), DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle ColorSchemeStyle
        {
            get
            {
                ExpandablePanel ep = this.Control as ExpandablePanel;
                return ep.ColorSchemeStyle;
            }
            set
            {
                ExpandablePanel ep = this.Control as ExpandablePanel;
                ep.ColorSchemeStyle = value;
                IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (dh != null && !dh.Loading)
                {
                    if (Helpers.IsOffice2007Style(value))
                        ep.TitleStyle.Border = eBorderType.RaisedInner;
                    else
                        ep.TitleStyle.Border = eBorderType.SingleLine;
                }
            }
        }

        protected override bool GetHitTest(System.Drawing.Point pt)
        {
            ExpandablePanel p = this.Control as ExpandablePanel;
            if (p != null)
            {
                Point pt2 = p.PointToClient(pt);
                PanelExTitle titlePanel = p.TitlePanel as PanelExTitle;
                if (titlePanel != null && titlePanel.ExpandChangeButton!=null && titlePanel.ExpandChangeButton.DisplayRectangle.Contains(pt2))
                    return true;
                else if (titlePanel != null && titlePanel.ExpandChangeButton!=null)
                    titlePanel.ExpandChangeButton.InternalMouseLeave();

            }
            return base.GetHitTest(pt);
        }
        #endregion
    }
}
