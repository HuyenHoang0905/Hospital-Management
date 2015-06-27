using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevComponents.Tree
{
    /// <summary>
    /// Defines Windows API function access.
    /// </summary>
    internal static class WinApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFO
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }
        [DllImport("kernel32.Dll")]
        public static extern short GetVersionEx(ref OSVERSIONINFO o);
    }
}
