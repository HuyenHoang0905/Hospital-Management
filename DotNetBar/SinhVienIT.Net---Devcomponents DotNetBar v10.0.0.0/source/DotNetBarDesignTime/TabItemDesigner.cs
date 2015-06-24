using System;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Defines Windows Forms designer for the TabItem object.
    /// </summary>
    public class TabItemDesigner : ComponentDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["Visible"] = TypeDescriptor.CreateProperty(typeof(TabItemDesigner), (PropertyDescriptor)properties["Visible"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Layout")});
        }

        /// <summary>
        /// Gets or sets whether item is visible.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Layout"), Description("Gets or sets whether item is visible.")]
        public bool Visible
        {
            get
            {
                return (bool)ShadowProperties["Visible"];
            }
            set
            {
                // this value is not passed to the actual control
                this.ShadowProperties["Visible"] = value;
            }
        }
    }
}
