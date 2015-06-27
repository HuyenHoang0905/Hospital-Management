#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Collections;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents designer for the CalendarView control.
    /// </summary>
    public class CalendarViewDesigner : ControlDesigner
    {
        #region Internal Implementation
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (component == null || component.Site == null || !component.Site.DesignMode)
                return;

#if !TRIAL
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete += new EventHandler(DesignerLoadComplete);
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
            SetDesignTimeDefaults();
			base.OnSetComponentDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            CalendarView calendar = this.Control as CalendarView;
#if !TRIAL
            string key = GetLicenseKey();
            calendar.LicenseKey = key;
#endif
        }
        #endregion

        #region Licensing Stuff
#if !TRIAL
        internal static string GetLicenseKey()
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
        private void DesignerLoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(DesignerLoadComplete);

            string key = GetLicenseKey();
            CalendarView calendar = this.Control as CalendarView;
            if (key != "" && calendar != null && calendar.LicenseKey == "" && calendar.LicenseKey != key)
                TypeDescriptor.GetProperties(calendar)["LicenseKey"].SetValue(calendar, key);
        }
#endif
        #endregion
    }
}
#endif

