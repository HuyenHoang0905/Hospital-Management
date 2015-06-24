using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    public class CustomTypeEditorProvider : ITypeDescriptorContext, IWindowsFormsEditorService
    {
        #region Private Variables, Constructor
        private IContainer m_Container = null;
        private object m_Instance = null;
        private IServiceProvider m_Provider = null;
        private PropertyDescriptor m_PropertyDescriptor = null;

        public CustomTypeEditorProvider(IContainer container, IServiceProvider provider)
        {
            m_Container = container;
            m_Provider = provider;
        }

        public void SetInstance(object instance, PropertyDescriptor desc)
        {
            m_Instance = instance;
            m_PropertyDescriptor = desc;
        }
        #endregion

        #region ITypeDescriptorContext Members

        public IContainer Container
        {
            get { return m_Container; }
        }

        public object Instance
        {
            get { return m_Instance; }
        }

        public void OnComponentChanged()
        {
            IComponentChangeService cc = m_Provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (cc != null)
                cc.OnComponentChanged(m_Instance, m_PropertyDescriptor, null, null);
        }

        public bool OnComponentChanging()
        {
            return true;
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get { return m_PropertyDescriptor; }
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return m_Provider.GetService(serviceType);
        }

        #endregion

        #region IWindowsFormsEditorService Members

        public void CloseDropDown()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DropDownControl(System.Windows.Forms.Control control)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form dialog)
        {
            DialogResult result;

            IntPtr focushWnd = WinApi.GetFocus();
            IUIService uiService = (IUIService)this.GetService(typeof(IUIService));
            if (uiService != null)
            {
                result = uiService.ShowDialog(dialog);
            }
            else
            {
                result = dialog.ShowDialog();
            }
            if (focushWnd != IntPtr.Zero)
            {
                WinApi.SetFocus(focushWnd);
            }
            return result;
        }

        #endregion
    }
}
