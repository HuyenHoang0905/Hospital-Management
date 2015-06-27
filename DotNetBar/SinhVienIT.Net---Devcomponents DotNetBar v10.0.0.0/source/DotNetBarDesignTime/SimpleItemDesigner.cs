using System;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
    public class SimpleItemDesigner : System.ComponentModel.Design.ComponentDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["Visible"] = TypeDescriptor.CreateProperty(typeof(SimpleItemDesigner), (PropertyDescriptor)properties["Visible"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Layout")});
        }

        /// <summary>
        /// Gets or sets whether item is visible.
        /// </summary>
        [DefaultValue(true), Browsable(true), DevCoBrowsable(true), Category("Layout"), Description("Gets or sets whether item is visible.")]
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
