#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevComponents.Schedule
{
    internal class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string StandardName;
            public NativeMethods.SystemTime StandardDate;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string DaylightName;
            public NativeMethods.SystemTime DaylightDate;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            public TimeZoneInformation(NativeMethods.DynamicTimeZoneInformation dtzi)
            {
                this.Bias = dtzi.Bias;
                this.StandardName = dtzi.StandardName;
                this.StandardDate = dtzi.StandardDate;
                this.StandardBias = dtzi.StandardBias;
                this.DaylightName = dtzi.DaylightName;
                this.DaylightDate = dtzi.DaylightDate;
                this.DaylightBias = dtzi.DaylightBias;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DynamicTimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string StandardName;
            public NativeMethods.SystemTime StandardDate;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string DaylightName;
            public NativeMethods.SystemTime DaylightDate;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
            public string TimeZoneKeyName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SystemTime
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RegistryTimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            public NativeMethods.SystemTime StandardDate;
            public NativeMethods.SystemTime DaylightDate;
            public RegistryTimeZoneInformation(NativeMethods.TimeZoneInformation tzi)
            {
                this.Bias = tzi.Bias;
                this.StandardDate = tzi.StandardDate;
                this.StandardBias = tzi.StandardBias;
                this.DaylightDate = tzi.DaylightDate;
                this.DaylightBias = tzi.DaylightBias;
            }

            public RegistryTimeZoneInformation(byte[] bytes)
            {
                if ((bytes == null) || (bytes.Length != 0x2c))
                {
                    throw new ArgumentException("Argument_InvalidREG_TZI_FORMAT", "bytes");
                }
                this.Bias = BitConverter.ToInt32(bytes, 0);
                this.StandardBias = BitConverter.ToInt32(bytes, 4);
                this.DaylightBias = BitConverter.ToInt32(bytes, 8);
                this.StandardDate.Year = BitConverter.ToInt16(bytes, 12);
                this.StandardDate.Month = BitConverter.ToInt16(bytes, 14);
                this.StandardDate.DayOfWeek = BitConverter.ToInt16(bytes, 0x10);
                this.StandardDate.Day = BitConverter.ToInt16(bytes, 0x12);
                this.StandardDate.Hour = BitConverter.ToInt16(bytes, 20);
                this.StandardDate.Minute = BitConverter.ToInt16(bytes, 0x16);
                this.StandardDate.Second = BitConverter.ToInt16(bytes, 0x18);
                this.StandardDate.Milliseconds = BitConverter.ToInt16(bytes, 0x1a);
                this.DaylightDate.Year = BitConverter.ToInt16(bytes, 0x1c);
                this.DaylightDate.Month = BitConverter.ToInt16(bytes, 30);
                this.DaylightDate.DayOfWeek = BitConverter.ToInt16(bytes, 0x20);
                this.DaylightDate.Day = BitConverter.ToInt16(bytes, 0x22);
                this.DaylightDate.Hour = BitConverter.ToInt16(bytes, 0x24);
                this.DaylightDate.Minute = BitConverter.ToInt16(bytes, 0x26);
                this.DaylightDate.Second = BitConverter.ToInt16(bytes, 40);
                this.DaylightDate.Milliseconds = BitConverter.ToInt16(bytes, 0x2a);
            }
        }
    }
}
#endif

