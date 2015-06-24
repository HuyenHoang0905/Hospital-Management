#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design
{
    public class ValidationExpressionEditor : UITypeEditor
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
                    list.Items.Add("At least 5 characters");
                    list.Items.Add("Phone Number");
                    list.Items.Add("E-mail Address");
                    list.Items.Add("ZIP Code");
                    list.Items.Add("Social Security Number");
                    list.Items.Add("URL");
                    list.Items.Add("First, Last Name");
                    list.SelectedIndexChanged += new EventHandler(ValidatorListSelectedIndexChanged);

                    _EditorService.DropDownControl(list);

                    if (list.SelectedIndex < 0) return es;

                    string regExp = "";
                    if (list.SelectedIndex == 1) // Phone Number
                        regExp = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
                    else if (list.SelectedIndex == 2) // Email Address
                        regExp = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                    else if (list.SelectedIndex == 3) // ZIP Code
                        regExp = @"^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$";
                    else if (list.SelectedIndex == 4) // SSN
                        regExp = @"^\d{3}-\d{2}-\d{4}$";
                    else if (list.SelectedIndex == 5) // URL
                        regExp = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
                    else if (list.SelectedIndex == 6) // First Name, Last Name
                        regExp = @"^[a-zA-Z''-'\s]{1,40}$";
                    else // At least 5 characters
                        regExp = @"\S{5,5}";

                    return regExp;
                    
                }
                return "";
            }

            return "";
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