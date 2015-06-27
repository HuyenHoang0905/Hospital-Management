using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ContextExMenuTypeEditor.
	/// </summary>
	public class ContextExMenuTypeEditor : System.Drawing.Design.UITypeEditor 
	{
		private IWindowsFormsEditorService edSvc = null;
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null
				&& context.Instance != null
				&& provider != null) 
			{				
				edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                ContextMenuBar cb = null;
                GridItem pg = provider as GridItem;
                string propLabel = "";
                if (pg != null)
                    propLabel = pg.Label;
                
                foreach (IComponent c in context.Container.Components)
                {
                    if (c is ContextMenuBar)
                    {
                        if(propLabel.EndsWith(((ContextMenuBar)c).Name) || propLabel=="")
                        {
                            cb = c as ContextMenuBar;
                            break;
                        }
                    }
                }
				
				BaseItem  selectedItem=null;
				if(value!=null && value is BaseItem)
                    selectedItem = (BaseItem)value;
				if(cb!=null && edSvc!=null)
				{
					ListBox lb=new ListBox();
					lb.SelectedIndexChanged+=new EventHandler(this.SelectedChanged);
					foreach(BaseItem item in cb.Items)
					{
						if(item.Name!="")
						{
							int i=lb.Items.Add(item.Name);
                            if (item == selectedItem)
								lb.SelectedIndex=i;
						}
					}
					edSvc.DropDownControl(lb);
					if(lb.SelectedItem!=null)
						return cb.Items[lb.SelectedItem.ToString()];
				}
			}

			return value;
		}

		private void SelectedChanged(object sender, EventArgs e)
		{
			if(edSvc!=null)
				edSvc.CloseDropDown();
		}

		/// <summary>
		/// Gets the editor style used by the EditValue method.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
		/// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by EditValue. If the UITypeEditor does not support this method, then GetEditStyle will return None</returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			if (context != null && context.Instance != null) 
			{
				return UITypeEditorEditStyle.DropDown;
			}
			return base.GetEditStyle(context);
		}

	}
}
