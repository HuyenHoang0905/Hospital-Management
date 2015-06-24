using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents Windows Forms designer support for RibbonTabItemGroup object.
	/// </summary>
	public class RibbonTabItemGroupTypeEditor:UITypeEditor
	{
		private IWindowsFormsEditorService m_EdSvc = null;
		private const string CREATE_NEW_GROUP="<Create new group>";

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			if (context != null && context.Instance != null) 
			{
				return UITypeEditorEditStyle.DropDown;
			}
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{

			if (context!=null && context.Instance!=null && provider!=null) 
			{
				m_EdSvc=(IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if(m_EdSvc!=null) 
				{
					RibbonTabItemGroupCollection groups=null;
					RibbonTabItem ribbonTabItem=null;
					RibbonStrip strip=null;
					if(context.Instance is RibbonTabItem)
					{
						ribbonTabItem=(RibbonTabItem)context.Instance;
						strip=ribbonTabItem.ContainerControl as RibbonStrip;
						if(strip!=null)
							groups=strip.TabGroups;
					}
					if(groups==null && context.Instance!=null)
						System.Windows.Forms.MessageBox.Show("Unknow control using RibbonTabGroupEditor. Cannot edit groups. ["+context.Instance.ToString()+"]");
					else if(groups==null)
						System.Windows.Forms.MessageBox.Show("Unknow control using RibbonTabGroupEditor. Cannot edit groups. [context instance null]");

					ListBox listBox=new ListBox();
					foreach(RibbonTabItemGroup g in groups)
					{
						listBox.Items.Add(g);
						if(g==ribbonTabItem.Group)
							listBox.SelectedItem=g;
					}

					listBox.Items.Add(CREATE_NEW_GROUP);

					listBox.SelectedIndexChanged+=new EventHandler(this.SelectedChanged);
					m_EdSvc.DropDownControl(listBox);
					if(listBox.SelectedItem is string && listBox.SelectedItem.ToString()==CREATE_NEW_GROUP)
						value=DesignerSupport.CreateRibbonTabItemGroup(strip,provider);
					else
						value=listBox.SelectedItem;
				}
			}

			return value;
		}

		private void SelectedChanged(object sender, EventArgs e)
		{
			if(m_EdSvc!=null)
				m_EdSvc.CloseDropDown();
		}
	}
}
