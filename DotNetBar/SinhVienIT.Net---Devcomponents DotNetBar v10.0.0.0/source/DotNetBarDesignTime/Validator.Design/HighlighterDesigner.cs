#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design
{
    internal class HighlighterDesigner : ComponentDesigner
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
            Highlighter highlighter = this.Component as Highlighter;
            if (highlighter == null)
                return;

#if !TRIAL
            string key = GetLicenseKey();
            highlighter.LicenseKey = key;
#endif
            // Create and assign error provider
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
            {
                highlighter.ContainerControl = dh.RootComponent as Control;
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
            Highlighter highlighter = this.Component as Highlighter;
            if (key != "" && highlighter != null && highlighter.LicenseKey == "" && highlighter.LicenseKey != key)
                TypeDescriptor.GetProperties(highlighter)["LicenseKey"].SetValue(highlighter, key);
        }
#endif
        #endregion
    }
}
#endif