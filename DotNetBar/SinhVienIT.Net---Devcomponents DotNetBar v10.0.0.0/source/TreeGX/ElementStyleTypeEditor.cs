using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents type editor for ElementStyle used for Windows Forms design-time support.
	/// </summary>
	public class ElementStyleTypeEditor:System.Drawing.Design.UITypeEditor 
	{
		#region Private Variables
		private IWindowsFormsEditorService m_EditorService = null;
		private const string OPTION_CREATE="Create new style";
		private const string OPTION_REMOVE="Remove style";
		#endregion

		#region Internal Implementation
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null
				&& context.Instance != null
				&& provider != null) 
			{				
				ElementStyle es=value as ElementStyle;
				
				m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
								
				if(m_EditorService!=null)
				{
					ListBox lb=new ListBox();
					lb.SelectedIndexChanged+=new EventHandler(this.SelectedChanged);
					if(es==null)
						lb.Items.Add(OPTION_CREATE);
					else
						lb.Items.Add(OPTION_REMOVE);

					m_EditorService.DropDownControl(lb);
					
					IDesignerHost dh=(IDesignerHost)provider.GetService(typeof(IDesignerHost));
					if(lb.SelectedItem!=null && dh!=null)
					{
						if(lb.SelectedItem.ToString()==OPTION_CREATE)
						{
							value=dh.CreateComponent(typeof(ElementStyle)) as ElementStyle;
						}
						else if(lb.SelectedItem.ToString()==OPTION_REMOVE)
						{
							value=null;
							dh.DestroyComponent(es);
						}
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
