using System;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar.Design
{
    public class ShapeTypeEditor : System.Drawing.Design.UITypeEditor
    {
        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by EditValue. If the UITypeEditor does not support this method, then GetEditStyle will return None</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                IUIService service = (IUIService)provider.GetService(typeof(IUIService));
                IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
                IShapeDescriptor shape = EditShape(service, context.Instance, helpService, value);
                return shape;
            }
            return value;
        }

        internal static IShapeDescriptor EditShape(IUIService uiService, object instance, IHelpService helpService, object currentValue)
        {
#if FRAMEWORK20
            ShapeEditorForm editor = new ShapeEditorForm();
            editor.Value = currentValue as IShapeDescriptor;
            if (uiService != null)
                uiService.ShowDialog(editor);
            else
                editor.ShowDialog();

            if (editor.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                currentValue = editor.Value;
            }

            editor.Dispose();

#endif
			return currentValue as IShapeDescriptor;
        }
    }

    public class ShapeStringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value is RoundRectangleShapeDescriptor)
                {
                    RoundRectangleShapeDescriptor rd = (RoundRectangleShapeDescriptor)value;
                    if (rd.IsEmpty)
                        return "Rectangle";
                    else
                        return "Round Rectangle";
                }
                else if (value is EllipticalShapeDescriptor)
                    return "Ellipse";
                else if (value == null)
                    return "System Default";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
