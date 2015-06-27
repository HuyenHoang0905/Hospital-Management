#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design
{
    internal class SuperValidatorDesigner : ComponentDesigner
    {
        #region Internal Implementation
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

#if !TRIAL
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete += new EventHandler(dh_LoadComplete);
#endif
        }

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            SuperValidator sv = this.Component as SuperValidator;
            if (sv == null)
                return;
            
#if !TRIAL
            string key = GetLicenseKey();
            sv.LicenseKey = key;
#endif
            // Create and assign error provider
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
            {
                ErrorProvider provider = dh.CreateComponent(typeof(ErrorProvider)) as ErrorProvider;
                if (provider == null) return;
                Highlighter highlighter = dh.CreateComponent(typeof(Highlighter)) as Highlighter;
                if (highlighter == null) return;
                if (highlighter.ContainerControl == null)
                    highlighter.ContainerControl = dh.RootComponent as Control;

                TypeDescriptor.GetProperties(sv)["Highlighter"].SetValue(sv, highlighter);
                TypeDescriptor.GetProperties(sv)["ErrorProvider"].SetValue(sv, provider);
                provider.Icon = Helpers.LoadIcon("Validator.SuperValidator.ico");
                sv.ContainerControl = dh.RootComponent as Control;
            }
        }
        #endregion

        #region Licensing
#if !TRIAL
        private string GetLicenseKey()
        {
            string key = "";
            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine;
            regkey = regkey.OpenSubKey("Software\\DevComponents\\Licenses", false);
            if (regkey != null)
            {
                object keyValue = regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2");
                if (keyValue != null)
                    key = keyValue.ToString();
            }
            return key;
        }
        private void dh_LoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(dh_LoadComplete);

            string key = GetLicenseKey();
            SuperValidator sv = this.Component as SuperValidator;
            if (key != "" && sv != null && sv.LicenseKey == "" && sv.LicenseKey != key)
                TypeDescriptor.GetProperties(sv)["LicenseKey"].SetValue(sv, key);
        }
#endif
        #endregion
    }
}
#endif