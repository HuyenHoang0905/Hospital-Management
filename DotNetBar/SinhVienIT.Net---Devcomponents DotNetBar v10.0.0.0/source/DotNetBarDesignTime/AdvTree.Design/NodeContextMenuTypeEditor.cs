using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Design
{
	/// <summary>
	/// Represents context menu type editor for Node.ContextMenu property.
	/// </summary>
	public class NodeContextMenuTypeEditor:System.Drawing.Design.UITypeEditor
	{
		#region Private Variables
		private IWindowsFormsEditorService m_EditorService = null;
		public static string DotNetBarPrefix="DotNetBar.";
		#endregion
		
		#region Internal Implementation
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null
				&& context.Instance != null
				&& provider != null) 
			{								
				m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
								
				if(m_EditorService!=null)
				{
					ListBox lb=new ListBox();
                    DevComponents.DotNetBar.ContextMenuBar contextMenuBar = null;
					lb.SelectedIndexChanged+=new EventHandler(this.SelectedChanged);
					
					IDesignerHost host = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
					foreach(IComponent component in host.Container.Components)
					{
						if(component is ContextMenu || component.GetType().FullName=="System.Windows.Forms.ContextMenuStrip")
						{
							lb.Items.Add(component);
						}
						if (component is DevComponents.DotNetBar.ContextMenuBar)
                        {
                            contextMenuBar = component as DevComponents.DotNetBar.ContextMenuBar;
                            foreach (BaseItem item in contextMenuBar.Items)
                            {
                                lb.Items.Add(DotNetBarPrefix + item.Name);
                            }
                        }
					}

					m_EditorService.DropDownControl(lb);
					
					if(lb.SelectedItem!=null)
					{
						if(lb.SelectedItem.ToString().StartsWith(DotNetBarPrefix))
						{
							Node node=context.Instance as Node;
                            if (node != null && node.TreeControl != null)
                            {
                                TypeDescriptor.GetProperties(node.TreeControl)["ContextMenuBar"].SetValue(node.TreeControl, contextMenuBar);
                            }
                            return contextMenuBar.Items[lb.SelectedItem.ToString().Substring(DotNetBarPrefix.Length)];
						}
						return lb.SelectedItem;
					}
				}
			}

			return value;
		}

		private void SelectedChanged(object sender, EventArgs e)
		{
			if(m_EditorService!=null)
				m_EditorService.CloseDropDown();
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
		#endregion
	}
}
