using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Tree.Helpers
{
    /// <summary>
    /// Defines various system information based helper functions.
    /// </summary>
    internal static class SystemHelpers
    {
        #region Private Variables
        private static bool _ThemedOS = false;
        private static bool _SupportsAnimation = false;
        private static bool _IsWindowsXP = false;
        private static bool _IsVista = false;
        private static bool _IsWindows7 = false;
        #endregion

        #region Internal Implementation
        static SystemHelpers()
        {

            WinApi.OSVERSIONINFO os = new WinApi.OSVERSIONINFO();
            os.dwOSVersionInfoSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(WinApi.OSVERSIONINFO));
            WinApi.GetVersionEx(ref os);
            if (os.dwPlatformId == 2 && os.dwMajorVersion == 4)
                _SupportsAnimation = false;
            if (os.dwMajorVersion == 5 && os.dwMinorVersion >= 1 && os.dwPlatformId == 2 ||
                os.dwMajorVersion > 5 && os.dwPlatformId == 2)
                _ThemedOS = System.Windows.Forms.OSFeature.Feature.IsPresent(System.Windows.Forms.OSFeature.Themes);
            Version osVersion = System.Environment.OSVersion.Version;
            _IsWindowsXP = osVersion.Major <= 5;
            _IsVista = osVersion.Major >= 6;
            _IsWindows7 = osVersion.Major >= 6 && osVersion.Minor >= 1 && osVersion.Build >= 7000;
        }

        public static bool ThemedOS
        {
            get { return _ThemedOS; }
            set
            {
                _ThemedOS = value;
            }
        }

        public static bool SupportsAnimation
        {
            get { return _SupportsAnimation; }
            set
            {
                _SupportsAnimation = value;
            }
        }

        public static bool IsWindowsXP
        {
            get { return _IsWindowsXP; }
            set
            {
                _IsWindowsXP = value;
            }
        }

        public static bool IsVista
        {
            get { return _IsVista; }
            set
            {
                _IsVista = value;
            }
        }

        public static bool IsWindows7
        {
            get { return _IsWindows7; }
            set
            {
                _IsWindows7 = value;
            }
        }
        #endregion
        
    }
}
