using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for SuperTooltipDesigner.
	/// </summary>
	public class SuperTooltipDesigner:ComponentDesigner
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
			SuperTooltip sp=this.Component as SuperTooltip;
			if(sp==null)
				return;
            //try
            //{
            //    sp.DefaultFont = new System.Drawing.Font("Tahoma", System.Windows.Forms.Control.DefaultFont.Size);
            //}
            //catch { }

#if !TRIAL
            string key = GetLicenseKey();
            sp.LicenseKey = key;
#endif
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
            SuperTooltip sp = this.Component as SuperTooltip;
            if (key != "" && sp != null && sp.LicenseKey == "" && sp.LicenseKey != key)
                TypeDescriptor.GetProperties(sp)["LicenseKey"].SetValue(sp, key);
        }
#endif
        #endregion
    }
}
