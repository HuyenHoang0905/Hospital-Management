using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    internal class WinApi
    {
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
            WM_MDISETMENU=0x230,
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
        #endregion

        #region Windows API
        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);
        //[DllImport("user32.dll")]
        //public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetTextMetrics(HandleRef hdc, TEXTMETRIC tm);
 
        [DllImport("user32.dll")]
        public static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);
        [DllImport("user32")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

        [DllImport("user32")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT r);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetScrollBarInfo")]
        public static extern int GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int SetScrollInfo(HandleRef hWnd, int fnBar, ref SCROLLINFO si, bool redraw);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle,
           bool bMenu, int dwExStyle);

        // This static method is required because legacy OSes do not support
        // GetWindowLongPtr 
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINT lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("user32")]
        public static extern bool PostMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO pbmi, uint iUsage, int ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32")]
        public static extern bool DeleteObject(int hObject);

        public delegate void TimerDelegate(IntPtr hWnd, IntPtr uMsg, IntPtr idEvent, int dwTime);
        [DllImport("user32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerDelegate lpTimerFunc);
        [DllImport("user32.dll")]
        public static extern bool KillTimer(IntPtr hWnd, UIntPtr uIDEvent);

        //[DllImport("gdi32.dll")]
        //public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, CombineRgnStyles fnCombineMode);
        //public enum CombineRgnStyles : int
        //{
        //    RGN_AND = 1,
        //    RGN_OR = 2,
        //    RGN_XOR = 3,
        //    RGN_DIFF = 4,
        //    RGN_COPY = 5,
        //    RGN_MIN = RGN_AND,
        //    RGN_MAX = RGN_COPY
        //}
        //[DllImport("gdi32.dll")]
        //public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        internal static bool ModifyHwndStyle(IntPtr hwnd, int removeStyle, int addStyle)
        {
            int curWindowStyle = GetWindowLongPtr(hwnd, (int)GWL.GWL_STYLE).ToInt32();
            int newStyle = (curWindowStyle & ~removeStyle) | addStyle;
            if (curWindowStyle == newStyle)
            {
                return false;
            }

            SetWindowLong(hwnd, (int)GWL.GWL_STYLE, newStyle);
            return true;
        }
        #endregion

        #region Structures
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
 
        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLBARINFO
        {
            public int cbSize;
            public RECT rcScrollBar;
            public int dxyLineButton;
            public int xyThumbTop;
            public int xyThumbBottom;
            public int reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public int[] rgstate;
        }

        public enum eScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1
        }

        public enum eScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(System.Drawing.Point p)
            {
                this.x = p.X;
                this.y = p.Y;
            }
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public struct MINMAXINFO
        //{
        //    public POINT ptReserved;
        //    public POINT ptMaxSize;
        //    public POINT ptMaxPosition;
        //    public POINT ptMinTrackSize;
        //    public POINT ptMaxTrackSize;
        //}


        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0, rgrc1, rgrc2;
            public IntPtr lppos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x, y;
            public int cx, cy;
            public int flags;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public RECT(Rectangle r)
            {
                Left = r.Left;
                Top = r.Top;
                Right = r.Right;
                Bottom = r.Bottom;
            }

            public int Height { get { return Bottom - Top; } }
            public int Width { get { return Right - Left; } }
            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                  ^ ((Width << 0x1a) | (Width >> 6))
                  ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads

            public static implicit operator Rectangle(RECT rect)
            {
                return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public override string ToString()
            {
                return "Left=" + this.Left + ", Top=" + this.Top + ", Right=" + this.Right + ", Bottom=" + this.Bottom;
            }
            #endregion
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public WinApi.RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class BITMAPINFO
        {
            public int biSize = 0;
            public int biWidth = 0;
            public int biHeight = 0;
            public short biPlanes = 0;
            public short biBitCount = 0;
            public int biCompression = 0;
            public int biSizeImage = 0;
            public int biXPelsPerMeter = 0;
            public int biYPelsPerMeter = 0;
            public int biClrUsed = 0;
            public int biClrImportant = 0;
            public byte bmiColors_rgbBlue = 0;
            public byte bmiColors_rgbGreen = 0;
            public byte bmiColors_rgbRed = 0;
            public byte bmiColors_rgbReserved = 0;
        }

        public enum eObjectId : uint
        {
            OBJID_CLIENT = 0xFFFFFFFC,
            OBJID_VSCROLL = 0xFFFFFFFB,
            OBJID_HSCROLL = 0xFFFFFFFA
        }

        public enum eStateFlags
        {
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            STATE_SYSTEM_OFFSCREEN = 0x00010000,
            STATE_SYSTEM_PRESSED = 0x00000008,
            STATE_SYSTEM_UNAVAILABLE = 0x00000001
        }
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

        #region DWM
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        } 

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(
           IntPtr hWnd,
           int dwAttribute,
           ref int attributeValue,
            int sizeOfValueRetrived
        );
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            ref int pvAttribute,
            int sizeOfValueRetrived);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();
        
        [DllImport("dwmapi.dll")]
        public static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll")]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

        public static void ExtendGlass(IntPtr handle, int glassHeight)
        {
            MARGINS margins = new MARGINS();
            margins.cxLeftWidth = 0;
            margins.cxRightWidth = 0;
            margins.cyTopHeight = glassHeight;
            margins.cyBottomHeight = 0;

            int result = DwmExtendFrameIntoClientArea(handle, ref margins);

        }

        public static bool IsGlassEnabled
        {
            get
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                    return false;
                return DwmIsCompositionEnabled();
            }
        }

        public enum DWMWINDOWATTRIBUTE : int
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY = 2,
            DWMWA_TRANSITIONS_FORCEDISABLED = 3,
            DWMWA_ALLOW_NCPAINT = 4,
            DWMWA_LAST = 5
        }

        public enum DWMNCRENDERINGPOLICY : int
        {
            DWMNCRP_USEWINDOWSTYLE = 0,
            DWMNCRP_DISABLED = 1,
            DWMNCRP_ENABLED = 2,
            DWMNCRP_LAST = 3
        } 
        #endregion
    }
}
