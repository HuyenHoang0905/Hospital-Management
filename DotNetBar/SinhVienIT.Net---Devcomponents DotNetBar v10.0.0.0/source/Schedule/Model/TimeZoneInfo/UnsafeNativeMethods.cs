#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace DevComponents.Schedule
{
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
        internal static extern int GetDynamicTimeZoneInformation(out NativeMethods.DynamicTimeZoneInformation lpDynamicTimeZoneInformation);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern int GetTimeZoneInformation(out NativeMethods.TimeZoneInformation lpTimeZoneInformation);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool GetFileMUIPath(int flags, [MarshalAs(UnmanagedType.LPWStr)] string filePath, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder language, ref int languageLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder fileMuiPath, ref int fileMuiPathLength, ref long enumerator);

        [SecurityCritical, DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern SafeLibraryHandle LoadLibraryEx(string libFilename, IntPtr reserved, int flags);

        [SecurityCritical, DllImport("user32.dll", EntryPoint = "LoadStringW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern int LoadString(SafeLibraryHandle handle, int id, StringBuilder buffer, int bufferLength);
 

 


    }
}
#endif

