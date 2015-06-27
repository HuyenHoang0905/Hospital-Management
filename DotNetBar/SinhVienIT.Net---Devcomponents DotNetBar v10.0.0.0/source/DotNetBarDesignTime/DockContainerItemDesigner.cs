using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for DockContainerItemDesigner.
	/// </summary>
	public class DockContainerItemDesigner:ComponentDesigner
	{
		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);

			if(!component.Site.DesignMode)
				return;

			DockContainerItem c=component as DockContainerItem;
			if(c!=null)
			{
				this.Visible=c.Visible;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties["Visible"] = TypeDescriptor.CreateProperty(typeof(DockContainerItemDesigner),(PropertyDescriptor)properties["Visible"], new Attribute[]
				{
						new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Layout")});

		}

		/// <summary>
		/// Gets or sets whether item is visible.
		/// </summary>
		[DefaultValue(true),Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Gets or sets whether item is visible.")]
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
