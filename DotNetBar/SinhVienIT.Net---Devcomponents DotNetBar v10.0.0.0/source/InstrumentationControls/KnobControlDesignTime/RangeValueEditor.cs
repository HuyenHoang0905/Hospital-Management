using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DevComponents.Instrumentation.Design
{
    public class RangeValueEditor : UITypeEditor
    {
        #region Private variables

        private const float MinValue = 0;
        private const float MaxValue = 1;

        #endregion

        #region Public properties

        public virtual float Minimum
        {
            get { return (MinValue); }
        }

        public virtual float Maximum
        {
            get { return (MaxValue); }
        }

        #endregion

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
                    RangeValueDropDown rv = new
                        RangeValueDropDown((float)value, Minimum, Maximum, editorService, context);

                    rv.EscapePressed = false;

                    editorService.DropDownControl(rv);

                    if (rv.EscapePressed == true)
                        context.PropertyDescriptor.SetValue(context.Instance, value);
                    else
                        return (rv.Value);
                }
            }

            return (base.EditValue(context, provider, value));
        }

        #endregion
    }

    #region AngleRangeValueEditor

    public class AngleRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (360); }
        }

        #endregion
    }

    #endregion

    #region RadiusRangeValueEditor

    public class RadiusRangeValueEditor : RangeValueEditor
    {
        #region Public properties
        
        public override float Maximum
        {
            get { return (.5f); }
        }

        #endregion
    }

    #endregion

    #region WidthRangeValueEditor

    public class WidthRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (.3f); }
        }

        #endregion
    }

    #endregion

    #region CapWidthRangeValueEditor

    public class CapWidthRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (1f); }
        }

        #endregion
    }

    #endregion

    #region OffsetRangeValueEditor

    public class OffsetRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (.3f); }
        }

        public override float Minimum
        {
            get { return (-.3f); }
        }

        #endregion
    }

    #endregion

    #region OffsetRangeValueEditor

    public class OffsetPosRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (.9f); }
        }

        public override float Minimum
        {
            get { return (0f); }
        }

        #endregion
    }

    #endregion

    #region WidthMaxRangeValueEditor

    public class WidthMaxRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (1f); }
        }

        #endregion
    }

    #endregion

    #region HalfRadiusRangeValueEditor

    public class HalfRadiusRangeValueEditor : RangeValueEditor
    {
        #region Public properties

        public override float Maximum
        {
            get { return (.5f); }
        }

        #endregion
    }

    #endregion
}
