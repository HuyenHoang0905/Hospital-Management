using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms.Design;
using DevComponents.UI;
using System.Windows.Forms;

#if AdvTree
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Represents Color type editor with support for color schemes.
	/// </summary>
	public class ColorTypeEditor:UITypeEditor 
	{
		private IWindowsFormsEditorService m_EditorService=null;

		/// <summary>
		/// Edits the value of the specified object using the editor style indicated by GetEditStyle.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
		/// <param name="provider">An IServiceProvider that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
		/// <returns>The new value of the object.</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null && context.Instance != null && provider != null) 
			{
				m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

				ColorScheme cs=null;
				if(context.Instance!=null)
				{
                    MethodInfo method = null;
                    if (context.Instance is object[])
                    {
                        method = ((object[])context.Instance)[0].GetType().GetMethod("GetColorScheme", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            cs = method.Invoke(((object[])context.Instance)[0], null) as ColorScheme;
                        }
                    }
                    else
                    {
                        method = context.Instance.GetType().GetMethod("GetColorScheme", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            cs = method.Invoke(context.Instance, null) as ColorScheme;
                        }
                    }
                    
				}
				
				Color colorValue=(Color)value;

				if(m_EditorService!=null)
				{
					ColorPicker colorPicker=new ColorPicker();
					colorPicker.EditorService=m_EditorService;
					colorPicker.BackColor=SystemColors.Control;
					colorPicker.ColorScheme=cs;
					colorPicker.SelectedColor=colorValue;

					string propertyName=context.PropertyDescriptor.Name;
                    PropertyInfo property = null;
                    if (context.Instance is object[])
                        property = ((object[])context.Instance)[0].GetType().GetProperty(propertyName + "SchemePart", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    else
    					property=context.Instance.GetType().GetProperty(propertyName+"SchemePart",BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
					
                    if(property!=null)
					{
                        if (context.Instance is object[])
                            colorPicker.SelectedColorSchemeName = property.GetValue(((object[])context.Instance)[0], null).ToString();
                        else
    						colorPicker.SelectedColorSchemeName=property.GetValue(context.Instance,null).ToString();
					}
                    colorPicker.UpdateUIWithSelection();

					m_EditorService.DropDownControl(colorPicker);
					if(!colorPicker.Canceled)
					{
						Color returnColor=colorPicker.SelectedColor;
						eColorSchemePart schemePart=eColorSchemePart.None;
						if(colorPicker.SelectedColorSchemeName!="")
						{
							schemePart=(eColorSchemePart)Enum.Parse(typeof(eColorSchemePart),colorPicker.SelectedColorSchemeName);
						}
						if(property!=null)
						{
                            if (context.Instance is object[])
                            {
                                object[] objects = (object[])context.Instance;
                                foreach (object item in objects)
                                {
                                    property = item.GetType().GetProperty(propertyName + "SchemePart", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                    property.SetValue(item, schemePart, null);
                                }
                            }
                            else
                                property.SetValue(context.Instance, schemePart, null);
						}
						return returnColor;
					}
				}
			}

			return value;
		}

		/// <summary>
		/// Gets the editor style used by the EditValue method.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
		/// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by EditValue. If the UITypeEditor does not support this method, then GetEditStyle will return None.
		/// </returns>
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
