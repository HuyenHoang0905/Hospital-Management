namespace DevComponents.DotNetBar
{
    using System;
	using System.Runtime.InteropServices;
    using System.Drawing;
using System.Security;

	[Flags()]
	internal enum eDrawCaption:uint		/* flags for DrawCaption */
	{
		DC_ACTIVE=0x0001,
		DC_SMALLCAP=0x0002,
		DC_ICON=0x0004,
		DC_TEXT=0x0008,
		DC_INBUTTON=0x0010,
		DC_GRADIENT=0x0020
    }

    /// <summary>
    ///    Summary description for NativeFunctions.
    /// </summary>
    [SuppressUnmanagedCodeSecurity()]
    internal class NativeFunctions
    {
        #region Licensing
#if !TRIAL
		internal static bool keyValidated=false;
		internal static int keyValidated2=0;
		internal static bool ValidateLicenseKey(string key)
		{
			bool ret = false;
			string[] parts = key.Split('-');
			int i = 10;
			foreach(string s in parts)
			{
				if(s=="88405280")
					i++;
				else if(s=="D06E")
					i += 10;
				else if(s=="4617")
					i += 8;
				else if(s=="8810")
					i += 12;
				else if(s=="64462F60FA93")
					i += 3;
			}
			if(i==29)
				return true;
			keyValidated = true;
			return ret;
		}
        internal static bool CheckLicenseKey(string key)
        {
            // F962CEC7-CD8F-4911-A9E9-CAB39962FC1F, 189, 266
            string[] parts = key.Split('-');
            int test = 0;
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                if (parts[i] == "A9E9")
                    test += 11;
                else if (parts[i] == "F962CEC7")
                    test += 12;
                else if (parts[i] == "CAB39962FC1F")
                    test += 2;
                else if (parts[i] == "4911")
                    test += 99;
                else if (parts[i] == "CD8F")
                    test += 65;
            }

            keyValidated2 = test + 77;

            if (test == 23)
                return false;

            return true;
        }
#endif
        #endregion

#if TRIAL
		private static Color m_ColorExpFlag=Color.Empty;
        internal static bool CheckedThrough = false;
		internal static bool ColorExpAlt()
		{
#if NOTIMELIMIT
				return false;
#else
			Color clr=SystemColors.Control;
			Color clr2;
			Color clr3;
			clr2=clr;
			if(clr2.ToArgb()==clr.ToArgb())
			{
				clr3=clr2;
			}
			else
			{
				clr3=clr;
			}

			if(!m_ColorExpFlag.IsEmpty)
			{
				return (m_ColorExpFlag==Color.Black?false:true);
			}
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.ClassesRoot;
                try
                {
                    key = key.CreateSubKey("CLSID\\{542FD3B2-2F65-4290-AB4F-EBFF0444C54C}\\InprocServer32");
                }
                catch (System.UnauthorizedAccessException)
                {
                    key = key.OpenSubKey("CLSID\\{542FD3B2-2F65-4290-AB4F-EBFF0444C54C}\\InprocServer32");
                }
				try
				{
					if(key.GetValue("")==null || key.GetValue("").ToString()=="")
					{
						key.SetValue("",DateTime.Today.ToOADate().ToString());
					}
					else
					{
						if(key.GetValue("").ToString()=="windows3.dll")
						{
							m_ColorExpFlag=Color.White;
							key.Close();
							key=null;
							return true;
						}
						DateTime date=DateTime.FromOADate(double.Parse(key.GetValue("").ToString()));
						if(((TimeSpan)DateTime.Today.Subtract(date)).TotalDays>28)
						{
							m_ColorExpFlag=Color.White;
							key.SetValue("","windows4.dll");
							key.Close();
							key=null;
							return true;
						}
						if(((TimeSpan)DateTime.Today.Subtract(date)).TotalDays<0)
						{
							m_ColorExpFlag=Color.White;
							key.SetValue("","windows3.dll");
							key.Close();
							key=null;
							return true;
						}
					}
				}
				finally
				{
					if(key!=null)
						key.Close();
                    CheckedThrough = true;
				}
			}
			catch{}
			m_ColorExpFlag=Color.Black;
			return false;
#endif
		}
#endif

        [StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public RECT(int left,int top,int right,int bottom)
			{
				this.Left=left;
				this.Top=top;
				this.Right=right;
				this.Bottom=bottom;
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

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
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
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SIZE
		{
			public int cx;
			public int cy;
		}

		[StructLayout(LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT {
            public int cbSize;
            public uint dwFlags;
            public int dwHoverTime;
            public IntPtr hwndTrack;
        }

		[StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS {
			public int hwnd;
			public int hwndInsertAfter;
			public int  x;
			public int  y;
			public int  cx;
			public int  cy;
			public int flags;
        }

		[DllImport("user32.dll")] 
		public static extern int MapWindowPoints( 
			IntPtr hWndFrom, 
			IntPtr hWndTo, 
			ref POINT lpPoints, 
			int cPoints);

        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, [Out] System.Text.StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

		[DllImport("user32.dll")] 
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32")]
		public static extern IntPtr WindowFromPoint(POINT p);

		[DllImport("user32")]
		public static extern IntPtr ChildWindowFromPoint(IntPtr parent, POINT p);
        [DllImport("user32.dll")]
        public static extern IntPtr RealChildWindowFromPoint(IntPtr hwndParent, POINT ptParentClientCoords);

		[DllImport("user32")]
		public static extern bool DrawIconEx(IntPtr hDC, int X, int Y, IntPtr hIcon, int width, int height, int frameIndex, IntPtr flickerFreeBrush, int flags);

		[DllImport("user32")]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32")]
		public static extern bool GetKeyboardState(byte[] state);

		[DllImport("user32.dll")]
		public static extern bool DrawCaption(IntPtr hwnd, IntPtr hdc, ref RECT lprc,eDrawCaption uFlags);

		[DllImport("user32")]
		public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState,  byte[] lpChar, uint uFlags);

		[DllImport("user32")]
		public static extern int SetFocus(int hWnd);

		[DllImport("user32")]
		public static extern bool IsWindow(int hWnd);

		[DllImport("user32")]
		public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT tme);

		[DllImport("user32")]
        public static extern bool DrawEdge(int hdc,ref RECT pRect, uint edge, uint grfFlags);

		[DllImport("user32")]
		public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
		
//		[DllImport("user32")]
//		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32")]
		public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

		//[DllImport("user32")]
		//public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);

        [DllImport("user32")]
        public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);
        //[DllImport("user32")]
        //public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

		[DllImport("user32")]
		public static extern bool SystemParametersInfo(
			uint uiAction,  // system parameter to retrieve or set
			uint uiParam,   // depends on action to be taken
			ref bool pvParam,  // depends on action to be taken
			uint fWinIni    // user profile update option
			);
        [DllImport("user32")]
        public static extern bool SystemParametersInfo(
            uint uiAction,  // system parameter to retrieve or set
            uint uiParam,   // depends on action to be taken
            ref int pvParam,  // depends on action to be taken
            uint fWinIni    // user profile update option
            );
		public const uint SPI_GETMENUANIMATION=0x1002;
		public const uint SPI_GETMENUFADE=0x1012;
		public const uint SPI_GETMENUUNDERLINES=0x100A;
		public const uint SPI_GETDROPSHADOW=0x1024;
		public const uint SPI_GETCURSORSHADOW=0x101A;
		public const uint SPI_GETMENUDROPALIGNMENT=27;
        public const uint SM_CXBORDER = 5;

        public static int BorderMultiplierFactor
        {
            get
            {
                int i = 0;
                SystemParametersInfo(SM_CXBORDER, 0, ref i, 0);
                return i;
            }
        }

		[DllImport("gdi32")]
        public static extern int SetROP2(int hDC, int DrawMode);

		[DllImport("gdi32")]
		public static extern int SelectClipRgn(IntPtr hDC, int hRgn);
		[DllImport("gdi32")]
		public static extern int  CreateRectRgn(
			int nLeftRect,   // x-coordinate of upper-left corner
			int nTopRect,    // y-coordinate of upper-left corner
			int nRightRect,  // x-coordinate of lower-right corner
			int nBottomRect  // y-coordinate of lower-right corner
			);

		[DllImport("gdi32")]
        public static extern int CreateDC(string lpszDriver, int lpszDevice, int lpszOutput, int lpInitData);

        [DllImport("gdi32")]
        public static extern bool DeleteDC(int hDC);

		[DllImport("user32")]
		public static extern bool DrawFocusRect(int hDC, ref RECT r);
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32")]
		public static extern bool PostMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern bool PostMessage(int hWnd, int Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y,
           int nReserved, IntPtr hWnd, IntPtr prcRect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32")]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32")]
		public static extern IntPtr GetActiveWindow();

		[DllImport("user32")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
		public const int LWA_COLORKEY=0x00000001;
		public const int LWA_ALPHA=0x00000002;
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BLENDFUNCTION
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}
		public enum Win32UpdateLayeredWindowsFlags
		{
			ULW_COLORKEY = 0x00000001,
			ULW_ALPHA    = 0x00000002,
			ULW_OPAQUE   = 0x00000004
		}

		public enum Win23AlphaFlags : byte
		{
			AC_SRC_OVER  = 0x00,
			AC_SRC_ALPHA = 0x01
		}

		const int R2_NOTXORPEN=10; // DPx

		[DllImport("winmm")]
		public static extern int sndPlaySound(string lpszSoundName, int uFlags);
		public const int SND_ASYNC=0x0001;  /* play asynchronously */
		public const int SND_NODEFAULT=0x0002;  /* silence (!default) if sound not found */

		[DllImport("user32")]
		public static extern bool ValidateRect(IntPtr hWnd,ref NativeFunctions.RECT pRect);
		[DllImport("user32")]
		public static extern bool LockWindowUpdate(IntPtr hWndLock);

		// 3D border styles
		public const uint 
			BDR_RAISEDOUTER=0x0001,
            BDR_SUNKENOUTER=0x0002,
            BDR_RAISEDINNER=0x0004,
            BDR_SUNKENINNER=0x0008,
            BDR_OUTER=(BDR_RAISEDOUTER | BDR_SUNKENOUTER),
            BDR_INNER=(BDR_RAISEDINNER | BDR_SUNKENINNER),
            BDR_RAISED=(BDR_RAISEDOUTER | BDR_RAISEDINNER),
            BDR_SUNKEN=(BDR_SUNKENOUTER | BDR_SUNKENINNER),
            EDGE_RAISED=(BDR_RAISEDOUTER | BDR_RAISEDINNER),
            EDGE_SUNKEN=(BDR_SUNKENOUTER | BDR_SUNKENINNER),
            EDGE_ETCHED=(BDR_SUNKENOUTER | BDR_RAISEDINNER),
            EDGE_BUMP=(BDR_RAISEDOUTER | BDR_SUNKENINNER);
        // Border flags
        public const uint
			BF_LEFT=0x0001,
            BF_TOP=0x0002,
            BF_RIGHT=0x0004,
            BF_BOTTOM=0x0008,
            BF_TOPLEFT=(BF_TOP | BF_LEFT),
            BF_TOPRIGHT=(BF_TOP | BF_RIGHT),
            BF_BOTTOMLEFT=(BF_BOTTOM | BF_LEFT),
            BF_BOTTOMRIGHT=(BF_BOTTOM | BF_RIGHT),
            BF_RECT=(BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM),
            BF_DIAGONAL=0x0010,
			// For diagonal lines, the BF_RECT flags specify the end point of the
			// vector bounded by the rectangle parameter.
			BF_DIAGONAL_ENDTOPRIGHT=(BF_DIAGONAL | BF_TOP | BF_RIGHT),
			BF_DIAGONAL_ENDTOPLEFT=(BF_DIAGONAL | BF_TOP | BF_LEFT),
			BF_DIAGONAL_ENDBOTTOMLEFT=(BF_DIAGONAL | BF_BOTTOM | BF_LEFT),
			BF_DIAGONAL_ENDBOTTOMRIGHT=(BF_DIAGONAL | BF_BOTTOM | BF_RIGHT),
			BF_MIDDLE=0x0800,  /* Fill in the middle */
			BF_SOFT=0x1000,  /* For softer buttons */
			BF_ADJUST=0x2000,  /* Calculate the space left over */
			BF_FLAT=0x4000,  /* For flat rather than 3D borders */
			BF_MONO=0x8000;  /* For monochrome borders */

		// Track Mouse Event Flags
		public const uint
			TME_HOVER=0x00000001,
			TME_LEAVE=0x00000002,
			TME_NONCLIENT=0x00000010,
			TME_QUERY=0x40000000,
			TME_CANCEL=0x80000000,
			HOVER_DEFAULT=0xFFFFFFFF;

		public const long WM_ACTIVATEAPP=0x001C;
		public const int WM_LBUTTONDOWN=0x0201;
		public const int WM_LBUTTONUP=0x0202;
		public const int WM_LBUTTONDBLCLK=0x0203;
		public const int WM_NCLBUTTONDOWN=0x00A1;
		public const int WM_NCMBUTTONDBLCLK=0x00A9;
		public const int WM_NCACTIVATE=0x0086;
		public const int WM_RBUTTONDOWN=0x0204;
		public const int WM_RBUTTONUP=0x0205;
		public const int WM_MBUTTONDOWN=0x0207;
		public const int WM_NCRBUTTONDOWN=0x00A4;
		public const int WM_NCMBUTTONDOWN=0x00A7;
		public const int WM_CONTEXTMENU=0x007B;
		public const int WM_USER=0x0400;
		public const int WM_MOUSEMOVE=0x0200;
		public const int WM_MOUSELEAVE=0x02A3;
		public const int HWND_TOP=0;
		public const int HWND_BOTTOM=1;
		public const int HWND_TOPMOST=-1;
		public const int HWND_NOTOPMOST=-2;
		public const int SWP_NOMOVE=0x0002;
		public const int SWP_NOSIZE=0x0001;
		public const int SWP_NOACTIVATE=0x0010;
		public const int SWP_NOZORDER=0x0004;
		public const int SWP_SHOWWINDOW=0x0040;
		public const int SWP_HIDEWINDOW=0x0080;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;

		public const int WM_SETFOCUS=0x0007;
		public const int WM_KILLFOCUS=0x0008;
		public const int WM_SHOWWINDOW = 0x18;
		public const int WM_SETREDRAW=0x000B;
		public const int WM_PAINT=0x000F;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;



		//public const int WM_KEYFIRST					=0x0100;
		//public const int WM_KEYUP                       =0x0101;
		//public const int WM_CHAR                        =0x0102;
		//public const int WM_DEADCHAR                    =0x0103;
		public const int WM_SYSKEYDOWN                  =0x0104;
		public const int WM_SYSKEYUP                    =0x0105;
		//public const int WM_SYSCHAR                     =0x0106;

		public const int WM_ACTIVATE=0x0006;
		public const int WA_INACTIVE=0;
		public const int WA_ACTIVE=1;
		public const int WA_CLICKACTIVE=2;

		// Animate Window Constants
		public const int AW_HOR_POSITIVE             =0x00000001;
		public const int AW_HOR_NEGATIVE             =0x00000002;
		public const int AW_VER_POSITIVE             =0x00000004;
		public const int AW_VER_NEGATIVE             =0x00000008;
		public const int AW_CENTER                   =0x00000010;
		public const int AW_HIDE                     =0x00010000;
		public const int AW_ACTIVATE                 =0x00020000;
		public const int AW_SLIDE                    =0x00040000;
		public const int AW_BLEND                    =0x00080000;

		public const int WM_SYSCOMMAND=0x0112;
		public const int SC_CLOSE=0xF060;
        public const int SC_CONTEXTHELP = 0xF180;
		public const int SC_NEXTWINDOW=0xF040;
		public const int SC_PREVWINDOW=0xF050;
		public const int SC_KEYMENU=0xF100;
		public const int SC_RESTORE=0xF120;
		public const int SC_MINIMIZE=0xF020;
		public const int SC_MAXIMIZE=0xF030;
        public const int SC_MOVE = 0xF010;

		public const int WM_MOUSEACTIVATE = 0x21;
		public const int MA_NOACTIVATE = 3;
		public const int MA_NOACTIVATEANDEAT = 4;
		public const uint WS_POPUP=0x80000000;
		public const uint WS_CLIPSIBLINGS=0x04000000;
		public const uint WS_CLIPCHILDREN=0x02000000;
		public const uint WS_EX_TOPMOST=0x00000008;
		public const uint WS_EX_TOOLWINDOW=0x00000080;

		public const int WM_THEMECHANGED=0x031A;
		public const int WM_PRINTCLIENT=0x0318;

		[DllImport("user32",SetLastError=true, CharSet=CharSet.Auto)]
		public static extern int AnimateWindow(int hWnd, int dwTime, int dwFlags);

		[DllImport("gdi32",SetLastError=true, CharSet=CharSet.Auto)]
		public static extern int GetDeviceCaps(int hdc, int nIndex);
		public const int WM_DISPLAYCHANGE = 0x7E;
		public const int BITSPIXEL = 12; // Number of bits per pixel
		//[DllImport("gdi32",SetLastError=true, CharSet=CharSet.Auto, EntryPoint="CreateDC")]
		//public static extern int CreateDCAsNull(string lpDriverName, int lpDeviceName, int lpOutput, int lpInitData);
//		[DllImport("gdi32",SetLastError=true, CharSet=CharSet.Auto)]
//        public static extern int DeleteDC(int hdc);

		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFO 
		{
			public int dwOSVersionInfoSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
			public string szCSDVersion;
		}
		[DllImport("kernel32.Dll")] public static extern short GetVersionEx(ref OSVERSIONINFO o);

		public static void DrawReversibleDesktopRect(System.Drawing.Rectangle rect, int iWidth)
		{
//			System.Windows.Forms.ControlPaint.DrawReversibleFrame(rect,System.Drawing.SystemColors.ControlText,System.Windows.Forms.FrameStyle.Thick);
//			return;
			RECT r;
			r.Left=rect.Left;
			r.Top=rect.Top;
			r.Right=rect.Right;
			r.Bottom=rect.Bottom;
			int dc=CreateDC("DISPLAY",0,0,0);
			int oldRop=SetROP2(dc,R2_NOTXORPEN);
			for(int i=0;i<iWidth;i++)
			{
				DrawFocusRect(dc,ref r);
				r.Left++;
				r.Top++;
				r.Right--;
				r.Bottom--;
			}
			SetROP2(dc,oldRop);
			DeleteDC(dc);
		}

		public static char GetAccessKey(string s)
		{
			int i=s.IndexOf('&',0);
			while(i>=0 && i<s.Length-1)
			{
				i++;
				if(s[i]!='&')
				{
					return (s.Substring(i,1).ToLower())[0];
				}
				i=s.IndexOf('&',i);
			}
			return '\0';
		}

		public static void DrawRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen,  int x, int y, int Width, int Height)
		{
			// TODO: BETA 2 fix for drawing the rectangle
			Width--;
			Height--;
			g.DrawRectangle(pen,x,y,Width,Height);
		}
		public static void DrawRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen, System.Drawing.Rectangle r)
		{
			DrawRectangle(g,pen,r.X,r.Y,r.Width,r.Height);
		}

		public static ePopupAnimation SystemMenuAnimation
		{
			get
			{
				bool bRet=false;
				SystemParametersInfo(SPI_GETMENUANIMATION,0,ref bRet,0);
				if(bRet)
				{
					bRet=false;
					SystemParametersInfo(SPI_GETMENUFADE,0,ref bRet,0);
					if(bRet)
						return ePopupAnimation.Fade;
					else
						return ePopupAnimation.Slide;
				}
				return ePopupAnimation.None;
			}
		}

		private static bool m_ShowKeyboardCues=true;
		public static bool ShowKeyboardCues
		{
			get
			{
				return m_ShowKeyboardCues;
			}
		}
		private static bool m_ShowDropShadow=true;
		public static bool ShowDropShadow
		{
			get
			{
				return m_ShowDropShadow;
			}
		}
		internal static void RefreshSettings()
		{
#if FRAMEWORK20
            m_ShowKeyboardCues = System.Windows.Forms.SystemInformation.MenuAccessKeysUnderlined;
            m_ShowDropShadow = System.Windows.Forms.SystemInformation.IsDropShadowEnabled;
            m_CursorShadow = m_ShowDropShadow;
            m_RightHandedMenus = System.Windows.Forms.SystemInformation.RightAlignedMenus;
#else
            bool bRet=false;
            SystemParametersInfo(SPI_GETMENUUNDERLINES,0,ref bRet,0);
			m_ShowKeyboardCues=bRet;
            bRet=false;
            SystemParametersInfo(SPI_GETDROPSHADOW,0,ref bRet,0);
			m_ShowDropShadow=bRet;
			bRet=false;
            SystemParametersInfo(SPI_GETCURSORSHADOW,0,ref bRet,0);
			m_CursorShadow=bRet;
			bRet=false;
            SystemParametersInfo(SPI_GETMENUDROPALIGNMENT,0,ref bRet,0);
			m_RightHandedMenus=bRet;
#endif
        }

		public static int ColorDepth=0;
		internal static void OnDisplayChange()
		{
			int hdc = CreateDC("DISPLAY", 0, 0, 0);
            ColorDepth = GetColorDepth(hdc);
			DeleteDC(hdc);
		}

        internal static int GetColorDepth(int hdc)
        {
            return GetDeviceCaps(hdc, BITSPIXEL);
        }

		public static bool AlphaBlendingSupported
		{
			get
			{
				if(Environment.OSVersion.Version.Major>=5)  // Windows 2000 and up
					return true;
				else
					return false;
			}
		}

		private static bool m_CursorShadow=true;
		public static bool CursorShadow
		{
			get
			{
				return m_CursorShadow;
			}
		}

		private static bool m_RightHandedMenus=false;
		public static bool RightHandedMenus
		{
			get { return m_RightHandedMenus; }
		}

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(int nIndex);
		public const int SM_REMOTESESSION = 0x1000;
		public static bool IsTerminalSession() 
		{
			return ( 0 != GetSystemMetrics(SM_REMOTESESSION) );
		}

        public static string GetClassName(IntPtr handle)
        {
            System.Text.StringBuilder className = new System.Text.StringBuilder(150);
            //Get the window class name
            int res = GetClassName(handle, className, className.Capacity);
            if (res != 0)
            {
                return className.ToString();
            }

            return "";
        }

        public static int MAKELPARAM(int low, int high)
        {
            return ((high << 0x10) | (low & 0xffff));
        }
    }
}
