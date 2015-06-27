using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    internal static class WinApi
    {
        #region API
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetTextMetrics(HandleRef hdc, TEXTMETRIC tm);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_LBUTTONDBLCLK = 0x0203;

        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public static int MAKELPARAM(int low, int high)
        {
            return ((high << 0x10) | (low & 0xffff));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }
        }

        [DllImport("user32")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT r);
        #endregion

        #region Windows Messages
        public enum WindowsMessages
        {
            WM_NCPAINT = 0x0085,
            WM_NCCALCSIZE = 0x0083,
            WM_NCACTIVATE = 0x0086,
            WM_SETTEXT = 0x000C,
            WM_INITMENUPOPUP = 0x0117,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_NCHITTEST = 0x0084,
            WM_ERASEBKGND = 0x0014,
            WM_NCMOUSEMOVE = 0xA0,
            WM_NCLBUTTONDOWN = 0xA1,
            WM_NCMOUSELEAVE = 0x2A2,
            WM_NCLBUTTONUP = 0xA2,
            WM_NCMOUSEHOVER = 0x2A0,
            WM_SETCURSOR = 0x20,
            WM_SETICON = 0x0080,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x115,
            WM_MOUSEWHEEL = 0x020A,
            WM_STYLECHANGED = 0x7D,
            WM_NCLBUTTONDBLCLK = 0xA3,
            WM_MOUSEACTIVATE = 0x21,
            WM_MOUSEMOVE = 0x0200,
            WM_MDISETMENU = 0x230,
            WM_MDIREFRESHMENU = 0x234,
            WM_KEYDOWN = 0x0100,
            WM_SIZE = 0x5,
            WM_DWMCOMPOSITIONCHANGED = 0x031E,
            WM_DRAWITEM = 0x002B,
            SBM_SETPOS = 0x00E0,
            SBM_SETSCROLLINFO = 0x00E9,
            EM_GETMODIFY = 0x00B8,
            EM_SETMODIFY = 0x00B9,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_SYSTIMER = 0x0118,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_PAINT = 0x000F,
            WM_COMMAND = 0x0111,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_MDIACTIVATE = 0x222,
            WM_CAPTURECHANGED = 0x0215,
            CB_GETDROPPEDSTATE = 0x0157,
            WVR_VALIDRECTS = 0x400
        }

        public enum ComboNotificationCodes : int
        {
            CBN_DROPDOWN = 7,
            CBN_CLOSEUP = 8
        }

        public enum MouseKeyState : int
        {
            MK_LBUTTON = 0x0001,
            MK_RBUTTON = 0x0002,
            MK_SHIFT = 0x0004,
            MK_CONTROL = 0x0008,
            MK_MBUTTON = 0x0010,
            MK_XBUTTON1 = 0x0020,
            MK_XBUTTON2 = 0x0040

        }

        /// <summary>Options available when a form is tested for mose positions.</summary>
        public enum WindowHitTestRegions
        {
            /// <summary>HTERROR: On the screen background or on a dividing line between windows 
            /// (same as HTNOWHERE, except that the DefWindowProc function produces a system 
            /// beep to indicate an error).</summary>
            Error = -2,
            /// <summary>HTTRANSPARENT: In a window currently covered by another window in the 
            /// same thread (the message will be sent to underlying windows in the same thread 
            /// until one of them returns a code that is not HTTRANSPARENT).</summary>
            TransparentOrCovered = -1,
            /// <summary>HTNOWHERE: On the screen background or on a dividing line between 
            /// windows.</summary>
            NoWhere = 0,
            /// <summary>HTCLIENT: In a client area.</summary>
            ClientArea = 1,
            /// <summary>HTCAPTION: In a title bar.</summary>
            TitleBar = 2,
            /// <summary>HTSYSMENU: In a window menu or in a Close button in a child window.</summary>
            SystemMenu = 3,
            /// <summary>HTGROWBOX: In a size box (same as HTSIZE).</summary>
            GrowBox = 4,
            /// <summary>HTMENU: In a menu.</summary>
            Menu = 5,
            /// <summary>HTHSCROLL: In a horizontal scroll bar.</summary>
            HorizontalScrollBar = 6,
            /// <summary>HTVSCROLL: In the vertical scroll bar.</summary>
            VerticalScrollBar = 7,
            /// <summary>HTMINBUTTON: In a Minimize button. </summary>
            MinimizeButton = 8,
            /// <summary>HTMAXBUTTON: In a Maximize button.</summary>
            MaximizeButton = 9,
            /// <summary>HTLEFT: In the left border of a resizable window (the user can click 
            /// the mouse to resize the window horizontally).</summary>
            LeftSizeableBorder = 10,
            /// <summary>HTRIGHT: In the right border of a resizable window (the user can click 
            /// the mouse to resize the window horizontally).</summary>
            RightSizeableBorder = 11,
            /// <summary>HTTOP: In the upper-horizontal border of a window.</summary>
            TopSizeableBorder = 12,
            /// <summary>HTTOPLEFT: In the upper-left corner of a window border.</summary>
            TopLeftSizeableCorner = 13,
            /// <summary>HTTOPRIGHT: In the upper-right corner of a window border.</summary>
            TopRightSizeableCorner = 14,
            /// <summary>HTBOTTOM: In the lower-horizontal border of a resizable window (the 
            /// user can click the mouse to resize the window vertically).</summary>
            BottomSizeableBorder = 15,
            /// <summary>HTBOTTOMLEFT: In the lower-left corner of a border of a resizable 
            /// window (the user can click the mouse to resize the window diagonally).</summary>
            BottomLeftSizeableCorner = 16,
            /// <summary>HTBOTTOMRIGHT: In the lower-right corner of a border of a resizable 
            /// window (the user can click the mouse to resize the window diagonally).</summary>
            BottomRightSizeableCorner = 17,
            /// <summary>HTBORDER: In the border of a window that does not have a sizing 
            /// border.</summary>
            NonSizableBorder = 18,
            /// <summary>HTOBJECT: Unknown...No Documentation Found</summary>
            Object = 19,
            /// <summary>HTCLOSE: In a Close button.</summary>
            CloseButton = 20,
            /// <summary>HTHELP: In a Help button.</summary>
            HelpButton = 21,
            /// <summary>HTSIZE: In a size box (same as HTGROWBOX). (Same as GrowBox).</summary>
            SizeBox = GrowBox,
            /// <summary>HTREDUCE: In a Minimize button. (Same as MinimizeButton).</summary>
            ReduceButton = MaximizeButton,
            /// <summary>HTZOOM: In a Maximize button. (Same as MaximizeButton).</summary>
            ZoomButton = MaximizeButton,
        }

        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        public enum RedrawWindowFlags : uint
        {
            RDW_INVALIDATE = 0x0001,
            RDW_INTERNALPAINT = 0x0002,
            RDW_ERASE = 0x0004,
            RDW_VALIDATE = 0x0008,
            RDW_NOINTERNALPAINT = 0x0010,
            RDW_NOERASE = 0x0020,
            RDW_NOCHILDREN = 0x0040,
            RDW_ALLCHILDREN = 0x0080,
            RDW_UPDATENOW = 0x0100,
            RDW_ERASENOW = 0x0200,
            RDW_FRAME = 0x0400,
            RDW_NOFRAME = 0x0800
        }
        [Flags()]
        public enum WindowStyles
        {
            WS_VISIBLE = 0x10000000,
            WS_CAPTION = 0x00C00000
        }

        public const int HWND_TOP = 0;
        public const int HWND_BOTTOM = 1;
        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;
        public const int WM_USER = 0x0400;
        #endregion

        #region Functions
        public static int LOWORD(int n)
        {
            return (short)(n & 0xffff);
        }
        public static int HIWORD(int n)
        {
            return (short)((n >> 0x10) & 0xffff);
        }
        public static int LOWORD(IntPtr n)
        {
            return LOWORD((int)((long)n));
        }
        public static int HIWORD(IntPtr n)
        {
            return HIWORD((int)((long)n));
        }

        public static IntPtr CreateLParam(int loWord, int hiWord)
        {
            byte[] bx = BitConverter.GetBytes(loWord);
            byte[] by = BitConverter.GetBytes(hiWord);
            byte[] blp = new byte[] { bx[0], bx[1], by[0], by[1] };
            return new IntPtr(BitConverter.ToInt32(blp, 0));
        }
        #endregion
    }
}
