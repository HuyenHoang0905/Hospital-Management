using System;
using System.Drawing.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ColorSchemeVSEditor.
	/// </summary>
	public sealed class ColorSchemeVSEditor:UITypeEditor
	{
		private System.Windows.Forms.Design.IWindowsFormsEditorService m_EditorService=null;
		public ColorSchemeVSEditor()
		{
		}
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			if (context != null && context.Instance != null) 
			{
				return System.Drawing.Design.UITypeEditorEditStyle.Modal;
			}
			return base.GetEditStyle(context);
		}
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context!=null && context.Instance!=null && provider!=null) 
			{
				m_EditorService=(System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
				
				if(m_EditorService!=null) 
				{
					if(context.Instance is Bar)
					{
						Bar bar=context.Instance as Bar;
						if(bar.Owner is DotNetBarManager && ((DotNetBarManager)bar.Owner).UseGlobalColorScheme)
							System.Windows.Forms.MessageBox.Show("Please note that your DotNetBarManager has its UseGlobalColorScheme set to true and any changes you make to ColorScheme object on the bar will not be used.");
					}
					else if(context.Instance is DotNetBarManager && !((DotNetBarManager)context.Instance).UseGlobalColorScheme)
					{
						System.Windows.Forms.MessageBox.Show("Please note that you need to set UseGlobalColorScheme=true in order for all bars to use ColorScheme you change on this dialog.");
					}

					if(value==null)
						value=new ColorScheme();
					ColorSchemeEditor editor=new ColorSchemeEditor();
					editor.CreateControl();
					editor.ColorScheme=(ColorScheme)value;
					m_EditorService.ShowDialog(editor);
					if(editor.ColorSchemeChanged)
					{
						value=editor.ColorScheme;
						context.OnComponentChanged();
						((ColorScheme)value)._DesignTimeSchemeChanged=true;
						if(context.Instance is Bar)
						{
							((Bar)context.Instance).Refresh();
						}
					}
					editor.Close();
				}
			}
			
			return value;
		}
	}
}
