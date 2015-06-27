using System;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    public class SuperTooltipInfoEditor : UITypeEditor 
    {

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
                IWindowsFormsEditorService m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                SuperTooltipInfo info = value as SuperTooltipInfo;

                SuperTooltip superTooltipInstance = null;
                if (context.PropertyDescriptor != null)
                {
                    string name = context.PropertyDescriptor.DisplayName;
                    IDesignerHost dh = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (dh != null && dh.Container!=null)
                    {
                        foreach (IComponent comp in dh.Container.Components)
                        {
                            SuperTooltip st = comp as SuperTooltip;
                            if (st != null && st.Site != null)
                            {
                                if (name.EndsWith(st.Site.Name))
                                {
                                    superTooltipInstance = st;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (info == null)
                {
                    info = new SuperTooltipInfo();
                    if (!(context.Instance is SuperTooltip))
                    {
                        if (superTooltipInstance != null && superTooltipInstance.DefaultTooltipSettings!=null)
                        {
                            info.BodyImage = superTooltipInstance.DefaultTooltipSettings.BodyImage;
                            info.BodyText = superTooltipInstance.DefaultTooltipSettings.BodyText;
                            info.Color = superTooltipInstance.DefaultTooltipSettings.Color;
                            info.CustomSize = superTooltipInstance.DefaultTooltipSettings.CustomSize;
                            info.FooterImage = superTooltipInstance.DefaultTooltipSettings.FooterImage;
                            info.FooterText = superTooltipInstance.DefaultTooltipSettings.FooterText;
                            info.FooterVisible = superTooltipInstance.DefaultTooltipSettings.FooterVisible;
                            info.HeaderText = superTooltipInstance.DefaultTooltipSettings.HeaderText;
                            info.HeaderVisible = superTooltipInstance.DefaultTooltipSettings.HeaderVisible;
                        }
                        else
                            info.Color = eTooltipColor.System;
                    }
                }
                
                if (m_EditorService != null)
                {
                    SuperTooltipVisualEditor tipDesigner = new SuperTooltipVisualEditor();
                    tipDesigner.EditorProvider = new CustomTypeEditorProvider(context.Container, provider);
                    tipDesigner.EditorService = m_EditorService;
                    tipDesigner.SuperTooltipInfo = info;
                    tipDesigner.ParentSuperTooltip = superTooltipInstance;
                    
                    Form f = new Form();
                    f.Controls.Add(tipDesigner);
                    f.AcceptButton = tipDesigner.buttonOK;
                    f.CancelButton = tipDesigner.buttonCancel;
                    f.Size = new Size(tipDesigner.Size.Width+SystemInformation.Border3DSize.Width*4,
                        tipDesigner.Size.Height+SystemInformation.Border3DSize.Height*4+SystemInformation.CaptionHeight);
                    tipDesigner.Dock = DockStyle.Fill;
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.MinimizeBox = false;
                    f.MaximizeBox = false;
                    f.Text = "SuperTooltip Editor";
                    m_EditorService.ShowDialog(f);

                    //m_EditorService.DropDownControl(tipDesigner);

                    if (!tipDesigner.Canceled)
                    {
                        SuperTooltipInfo returnInfo = tipDesigner.SuperTooltipInfo;
                        f.Dispose();
                        return returnInfo;
                    }
                    f.Dispose();
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
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }
    }
}
