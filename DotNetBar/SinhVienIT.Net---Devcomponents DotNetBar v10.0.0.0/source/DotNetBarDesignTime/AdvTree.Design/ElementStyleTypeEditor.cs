using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Design
{
	/// <summary>
	/// Represents type editor for ElementStyle used for Windows Forms design-time support.
	/// </summary>
	public class ElementStyleTypeEditor:System.Drawing.Design.UITypeEditor 
	{
		#region Private Variables
		private IWindowsFormsEditorService m_EditorService = null;
		private const string OPTION_CREATE="Create style";
		private const string OPTION_REMOVE="Delete selected style";
		private const string OPTION_STYLES="Styles.";
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
					AdvTree tree = null;
					if(context.Instance is AdvTree)
						tree=context.Instance as AdvTree;
					else if(context.Instance is Node)
						tree=((Node)context.Instance).TreeControl;
					else if(context.Instance is Cell)
						tree=((Cell)context.Instance).TreeControl;
					
					ListBox lb=new ListBox();
					
					if(es==null)
						lb.Items.Add(OPTION_CREATE);
					else
						lb.Items.Add(OPTION_REMOVE);
					
					if(tree!=null)
					{
						foreach(ElementStyle style in tree.Styles)
							lb.Items.Add(style);
					}
					
					string[] styles = Enum.GetNames(typeof (ePredefinedElementStyle));
					foreach(string s in styles)
						lb.Items.Add(OPTION_STYLES + s);

					lb.SelectedIndexChanged+=new EventHandler(this.SelectedChanged);
					m_EditorService.DropDownControl(lb);
					
					IDesignerHost dh=(IDesignerHost)provider.GetService(typeof(IDesignerHost));
					if(lb.SelectedItem!=null && dh!=null)
					{
						if(lb.SelectedItem is ElementStyle)
						{
							value=lb.SelectedItem as ElementStyle;
						}
						else if(lb.SelectedItem!=null && lb.SelectedItem.ToString().StartsWith(OPTION_STYLES))
						{
							string styleName = lb.SelectedItem.ToString().Substring(OPTION_STYLES.Length);
							Type t = typeof (NodeStyles);
							ElementStyle predefinedStyle=t.GetProperty(styleName).GetValue(null, null) as ElementStyle;
							ElementStyle newStyle=dh.CreateComponent(typeof(ElementStyle)) as ElementStyle;
							newStyle.ApplyStyle(predefinedStyle);
							newStyle.Description = styleName;
							value = newStyle;
							if(tree!=null)
							{
								IComponentChangeService cc = provider.GetService(typeof (IComponentChangeService)) as IComponentChangeService;
								if(cc!=null)
									cc.OnComponentChanging(tree, TypeDescriptor.GetProperties(tree)["Style"]);
								tree.Styles.Add(value as ElementStyle);
								if(cc!=null)
									cc.OnComponentChanged(tree, TypeDescriptor.GetProperties(tree)["Style"],null,null);
							}
						}
						else if(lb.SelectedItem!=null && lb.SelectedItem.ToString()==OPTION_CREATE)
						{
							value=dh.CreateComponent(typeof(ElementStyle)) as ElementStyle;
							if(tree!=null)
							{
								IComponentChangeService cc = provider.GetService(typeof (IComponentChangeService)) as IComponentChangeService;
								if(cc!=null)
									cc.OnComponentChanging(tree, TypeDescriptor.GetProperties(tree)["Style"]);
								tree.Styles.Add(value as ElementStyle);
								if(cc!=null)
									cc.OnComponentChanged(tree, TypeDescriptor.GetProperties(tree)["Style"],null,null);
							}
						}
						else if(lb.SelectedItem!=null && lb.SelectedItem.ToString()==OPTION_REMOVE)
						{
							if(tree!=null)
							{
								IComponentChangeService cc = provider.GetService(typeof (IComponentChangeService)) as IComponentChangeService;
								if(cc!=null)
									cc.OnComponentChanging(tree, TypeDescriptor.GetProperties(tree)["Style"]);
								if(tree!=null)
									tree.Styles.Remove(value as ElementStyle);
								if(cc!=null)
									cc.OnComponentChanged(tree, TypeDescriptor.GetProperties(tree)["Style"],null,null);
								if(tree.Styles.Count>0)
									value = tree.Styles[0];
								else
									value=null;
							}
							else
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
