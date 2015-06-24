using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Globalization;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for ShortcutsDesigner.
	/// </summary>
	public class ShortcutsDesigner:UITypeEditor
	{
		private IWindowsFormsEditorService m_EdSvc = null;

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
					ShortcutsListBox lst=null;
					if(context.Instance is BaseItem)
						lst=new ShortcutsListBox(((BaseItem)context.Instance).Shortcuts);
                    else if (context.Instance is ButtonX)
                        lst = new ShortcutsListBox(((ButtonX)context.Instance).Shortcuts);
					else if(context.Instance is DotNetBarManager)
						lst=new ShortcutsListBox(((DotNetBarManager)context.Instance).AutoDispatchShortcuts);
					else if(context.Instance!=null)
						System.Windows.Forms.MessageBox.Show("Unknow control using shortcuts. Cannot edit shortcuts. ["+context.Instance.ToString()+"]");
					else
						System.Windows.Forms.MessageBox.Show("Unknow control using shortcuts. Cannot edit shortcuts. [context instance null]");
					if(lst!=null)
					{
						m_EdSvc.DropDownControl(lst);
						value=lst.Shortcuts();
						((ShortcutsCollection)value).Parent=context.Instance as BaseItem;
					}
				}
			}

			return value;
		}

		private class ShortcutsListBox:CheckedListBox
		{
			public ShortcutsListBox(ShortcutsCollection editingInstance):base()
			{
				// Load all shortcuts
				Array a=eShortcut.GetValues(typeof(eShortcut));
				for(int i=1;i<a.Length;i++)
				{
					NameValue nv=new NameValue(eShortcut.GetName(typeof(eShortcut),a.GetValue(i)),(eShortcut)a.GetValue(i));
					if(editingInstance.Contains((eShortcut)a.GetValue(i)))
					{
						this.Items.Add(nv,System.Windows.Forms.CheckState.Checked);
					}
					else
						this.Items.Add(nv,System.Windows.Forms.CheckState.Unchecked);
				}
			}

			public ShortcutsCollection Shortcuts()
			{
				ShortcutsCollection sl=new ShortcutsCollection(null);
				foreach(NameValue nv in this.CheckedItems)
					sl.Add(nv.Key);
				return sl;
			}

			private struct NameValue
			{
				public NameValue(string name,eShortcut k)
				{
					Name=name;
					Key=k;
				}
				public override string ToString()
				{
					return Name;
				}
				public string Name;
				public eShortcut Key;
			}
		}
	}
	public class ShortcutsConverter:TypeConverter 
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
		{
			if (sourceType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}
		// Overrides the ConvertFrom method of TypeConverter.
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
		{
			if(value is string)
			{
				return this.FromString((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}
		// Overrides the ConvertTo method of TypeConverter.
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{  
			if (destinationType == typeof(string)) 
			{
                if (value == null) return "";
				ShortcutsCollection sl=(ShortcutsCollection)value;
				System.Text.StringBuilder sb=new System.Text.StringBuilder();

				for(int i=0;i<sl.Count;i++)
				{
					sb.Append(eShortcut.GetName(typeof(eShortcut),sl[i]));
					if(i<sl.Count-1)
						sb.Append(",");
				}

				return sb.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
            
		private ShortcutsCollection FromString(string str)
		{
			ShortcutsCollection sl=new ShortcutsCollection(null);
			if(str=="" || str==null)
				return sl;
			string[] v=str.Split(',');
			for(int i=0;i<v.Length;i++)
			{
				sl.Add((eShortcut)eShortcut.Parse(typeof(eShortcut),v[i],true));
			}
			return sl;
		}
	}
}
