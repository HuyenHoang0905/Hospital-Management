using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DevComponents.Instrumentation.Design
{
    public class PivotPointEditor : UITypeEditor
    {
        #region GetEditStyle

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return (UITypeEditorEditStyle.DropDown);
        }

        #endregion

        #region GetPaintValueSupported

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return (false);
        }

        #endregion

        #region EditValue

        public override object EditValue(
            ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService =
                    provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (editorService != null)
                {
                    GaugeCircularScale scale = context.Instance as GaugeCircularScale;

                    if (scale != null)
                    {
                        PivotPointDropDown pv = new PivotPointDropDown((PointF) value, scale.GaugeControl.Frame.Style,
                            scale.Radius, scale.StartAngle, scale.SweepAngle, editorService, context);

                        pv.EscapePressed = false;

                        editorService.DropDownControl(pv);

                        if (pv.EscapePressed == true)
                            context.PropertyDescriptor.SetValue(context.Instance, value);
                        else
                            return (pv.PivotPoint);
                    }
                }
            }

            return (base.EditValue(context, provider, value));
        }

        #endregion
    }
}
