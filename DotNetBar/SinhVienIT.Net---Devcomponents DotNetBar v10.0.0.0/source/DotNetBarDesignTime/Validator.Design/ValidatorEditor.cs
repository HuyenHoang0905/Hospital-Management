#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design
{
    public class ValidatorEditor : UITypeEditor 
    {
        #region Implementation
        private IWindowsFormsEditorService _EditorService = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                ValidatorBase es = value as ValidatorBase;

                _EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (_EditorService != null)
                {
                    ListBox list = new ListBox();
                    list.Items.Add("Required Field Validator");
                    list.Items.Add("Regular Expression Validator");
                    list.Items.Add("Compare Fields Validator");
                    list.Items.Add("Range Validator");
                    list.Items.Add("Custom Code Validator");
                    list.SelectedIndexChanged += new EventHandler(ValidatorListSelectedIndexChanged);

                    _EditorService.DropDownControl(list);

                    if (list.SelectedIndex >= 0)
                    {
                        Type validatorType = null;
                        if (list.SelectedIndex == 1)
                            validatorType = typeof(RegularExpressionValidator);
                        else if (list.SelectedIndex == 2)
                            validatorType = typeof(CompareValidator);
                        else if (list.SelectedIndex == 3)
                            validatorType = typeof(RangeValidator);
                        else if (list.SelectedIndex == 4)
                            validatorType = typeof(CustomValidator);
                        else
                            validatorType = typeof(RequiredFieldValidator);

                        IDesignerHost dh = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                        if (dh != null)
                        {
                            ValidatorBase val = dh.CreateComponent(validatorType) as ValidatorBase;
                            val.HighlightColor = eHighlightColor.Red;
                            val.ErrorMessage = "Your error message here.";
                            return val;
                        }
                    }
                }
                return es;
            }

            return null;
        }

        private void ValidatorListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_EditorService != null)
                _EditorService.CloseDropDown();
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
        #endregion
    }
}
#endif