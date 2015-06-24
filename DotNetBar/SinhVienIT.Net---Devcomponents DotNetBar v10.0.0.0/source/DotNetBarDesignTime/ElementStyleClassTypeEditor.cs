using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for ContextExMenuTypeEditor.
    /// </summary>
    public class ElementStyleClassTypeEditor : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (edSvc != null)
                {
                    ListBox lb = new ListBox();
                    lb.SelectedIndexChanged += new EventHandler(this.SelectedChanged);
                    Type t = typeof(ElementStyleClassKeys);
                    FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
                    foreach (FieldInfo pi in fields)
                    {
                        string s = pi.GetValue(null).ToString();
                        lb.Items.Add(s);
                        if (s == value.ToString())
                            lb.SelectedItem = s;
                    }

                    edSvc.DropDownControl(lb);
                    if (lb.SelectedItem != null)
                        return lb.SelectedItem.ToString();
                }
            }

            return value;
        }

        private void SelectedChanged(object sender, EventArgs e)
        {
            if (edSvc != null)
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
