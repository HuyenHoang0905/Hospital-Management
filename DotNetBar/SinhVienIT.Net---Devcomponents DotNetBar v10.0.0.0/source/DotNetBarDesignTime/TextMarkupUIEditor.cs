using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    public class TextMarkupUIEditor : System.Drawing.Design.UITypeEditor
    {
        #region Private Variables
        private IWindowsFormsEditorService m_EditorService = null;
        #endregion

        #region Internal Implementation
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (m_EditorService != null)
                {
                    TextMarkupEditor editor = new TextMarkupEditor();
                    editor.buttonOK.Click += new EventHandler(MarkupEditorButtonClick);
                    editor.buttonCancel.Click += new EventHandler(MarkupEditorButtonClick);
                    
                    if(value!=null)
                        editor.inputText.Text = value.ToString();

                    m_EditorService.DropDownControl(editor);

                    if (editor.DialogResult == DialogResult.OK)
                    {
                        string text = editor.inputText.Text;
                        editor.Dispose();
                        return text;
                    }
                    editor.Dispose();
                }
            }

            return value;
        }

        void MarkupEditorButtonClick(object sender, EventArgs e)
        {
            if (m_EditorService != null)
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
