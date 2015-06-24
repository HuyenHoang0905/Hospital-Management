using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for Themes.
	/// </summary>
	internal abstract class Themes:IDisposable
	{
		protected Control m_Parent=null;
		protected IntPtr m_hTheme=IntPtr.Zero;

        public static int DrawShadowText(Graphics g, Rectangle bounds, string text, ThemeTextFormat format, Color textColor, Color shadowColor, int shadowOffsetX, int shadowOffsetY)
        {
            int result = 0;
            IntPtr hdc = g.GetHdc();
            try
            {
                RECT r = new RECT(bounds);
                result = DrawShadowText(hdc, text, (uint)text.Length, ref r, (int)format, new COLORREF(textColor), new COLORREF(shadowColor), shadowOffsetX, shadowOffsetY);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
            return result;
        }
        [System.Runtime.InteropServices.DllImport("ComCtl32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int DrawShadowText(IntPtr hdc, string text, uint textLength, ref RECT r, int flags, COLORREF textColor, COLORREF shadowColor, int shadowXOffset, int shadowYOffset);

		[StructLayoutAttribute(LayoutKind.Sequential)]
		public struct RECT
		{
			public RECT(Rectangle r)
			{
				this.left=r.Left;
				this.top=r.Top;
				this.right=r.Right;
				this.bottom=r.Bottom;
			}
			int left;
			int top;
			int right;
			int bottom;
            public void Offset(int x, int y)
            {
                left += x;
                top += y;
            }
		};

		[StructLayoutAttribute(LayoutKind.Sequential)]
		public struct MARGINS
		{
			int cxLeftWidth;
			int cxRightWidth;
			int cyTopHeight;
			int cyBottomHeight;
		};

		[StructLayoutAttribute(LayoutKind.Sequential)]
			public struct SIZE
		{
			public int Width;
			public int Height;
		};
        
        [StructLayout(LayoutKind.Sequential)]
        public struct COLORREF
        {
            public uint ColorDWORD;

            public COLORREF(System.Drawing.Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            public System.Drawing.Color GetColor()
            {
                return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
               (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
            }

            public void SetColor(System.Drawing.Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }
        public enum DTT_VALIDBITS : int
        {
            DTT_TEXTCOLOR = (1 << 0),      // crText has been specified
            DTT_BORDERCOLOR = (1 << 1),      // crBorder has been specified
            DTT_SHADOWCOLOR = (1 << 2),      // crShadow has been specified
            DTT_SHADOWTYPE = (1 << 3),      // iTextShadowType has been specified
            DTT_SHADOWOFFSET = (1 << 4),      // ptShadowOffset has been specified
            DTT_BORDERSIZE = (1 << 5),      // iBorderSize has been specified
            DTT_FONTPROP = (1 << 6),      // iFontPropId has been specified
            DTT_COLORPROP = (1 << 7),      // iColorPropId has been specified
            DTT_STATEID = (1 << 8),      // IStateId has been specified
            DTT_CALCRECT = (1 << 9),      // Use pRect as and in/out parameter
            DTT_APPLYOVERLAY = (1 << 10),     // fApplyOverlay has been specified
            DTT_GLOWSIZE = (1 << 11),     // iGlowSize has been specified
            DTT_CALLBACK = (1 << 12),     // pfnDrawTextCallback has been specified
            DTT_COMPOSITED = (1 << 13)     // Draws text with antialiased alpha (needs a DIB section)
        }

        public enum ThemeFontId : int
        {
            TMT_CAPTIONFONT = 801,
            TMT_SMALLCAPTIONFONT = 802,
            TMT_MENUFONT = 803,
            TMT_STATUSFONT = 804,
            TMT_MSGBOXFONT = 805,
            TMT_ICONTITLEFONT = 806
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct DTTOPTS
        {
            public int dwSize;
            public int dwFlags;
            public COLORREF crText;
            public COLORREF crBorder;
            public COLORREF crShadow;
            public int iTextShadowType;
            public WinApi.POINT ptShadowOffset;
            public int iBorderSize;
            public int iFontPropId;
            public int iColorPropId;
            public int iStateId;
            public bool fApplyOverlay;
            public int iGlowSize;
            public int pfnDrawTextCallback;
            public int lParam;
        };
        
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern IntPtr OpenThemeData(IntPtr hWnd, string pszClassList);
        [DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
        protected static extern IntPtr OpenThemeDataEx(IntPtr hWnd, string pszClassList, int flags);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int CloseThemeData(IntPtr hTheme);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pCliprect);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int EnableTheming(bool fEnable);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeEdge(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pDestRect,uint uEdge,uint uFlags,ref RECT pContentRect);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr himl, int iImageIndex);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeParentBackground(IntPtr hwnd, IntPtr hdc, ref RECT prc);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int DrawThemeText(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string pszText, int iCharCount, int dwTextFlags, int dwTextFlags2,ref RECT pRect);
        [DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
        public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string pszText, int iCharCount, int dwTextFlags, ref RECT pRect, ref DTTOPTS options);
        
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int EnableThemeDialogTexture(IntPtr hwnd, long dwFlags);
		[DllImport("uxtheme", ExactSpelling=true, CharSet=CharSet.Unicode)]
		public extern static Int32 GetCurrentThemeName(string stringThemeName, int lengthThemeName, string stringColorName, int lengthColorName, string stringSizeName, int lengthSizeName);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern long GetThemeAppProperties();
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeBackgroundContentRect(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pBoundingRect, ref RECT pContentRect);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeBackgroundExtent(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pContentRect, ref RECT pExtentRect);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeBackgroundRegion(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref IntPtr pRegion);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeBool(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref bool pfVal);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeColor(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref long pColor);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeDocumentationProperty(string pszThemeName, string pszPropertyName, string pszValueBuff, int cchMaxValChars);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeEnumValue(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeFilename(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref string pszThemeFilename, int cchMaxBuffChars);
		//[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		//private static extern int GetThemeFont(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr pFont);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemeInt(IntPtr hTheme,int iPartId,int iStateId,int iPropId,ref int piVal);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		private static extern int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId,int iStateId,int iPropId,ref RECT prc, ref MARGINS pMargins);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern bool IsThemeActive();
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		internal static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int SetWindowTheme(IntPtr hwnd, IntPtr pszSubAppName, string pszSubIdList);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int SetWindowTheme(IntPtr hwnd, IntPtr pszSubAppName, IntPtr pszSubIdList);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern bool IsThemePartDefined(IntPtr hTheme, int iPartId,  int iStateId);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemePartSize(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT prc, int eSize, ref SIZE psz);
		[DllImport("UxTheme.dll",CharSet=CharSet.Auto)]
		protected static extern int GetThemePartSize(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, IntPtr prc, int eSize, ref SIZE psz);


		[DllImport("gdi32.dll",CharSet=CharSet.Auto)]
		protected static extern IntPtr SelectObject(IntPtr hdc,IntPtr hgdiobj);
        
		protected abstract string ThemeClass {get;}

		public static bool ThemesActive=_IsThemeActive();
		
		public Themes(Control parent)
		{
			m_Parent=parent;
            m_hTheme=OpenThemeData(parent.Handle,this.ThemeClass);
		}
		~Themes()
		{
			if(m_hTheme!=IntPtr.Zero)
				this.Dispose();
		}

		private static bool _IsThemeActive()
		{
			if(!BarFunctions.ThemedOS) // System.Windows.Forms.OSFeature.Feature.IsPresent(System.Windows.Forms.OSFeature.Themes))
				return false;
			return IsThemeActive();
		}

		public static void RefreshIsThemeActive()
		{
			ThemesActive=_IsThemeActive();
		}

		public static int SetWindowTheme(Control ctrl, string sTheme)
		{
			return SetWindowTheme(ctrl.Handle,IntPtr.Zero,sTheme);
		}

		protected virtual void InternalDrawBackground(Graphics g, ThemePart part, ThemeState state, Rectangle r)
		{
			RECT rDraw=new RECT(r);
			IntPtr hdc=g.GetHdc();
            try
            {
                int hresult = DrawThemeBackground(m_hTheme, hdc, part.Value, state.Value, ref rDraw, IntPtr.Zero);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
		}

		protected virtual void InternalDrawBackground(Graphics g, ThemePart part, ThemeState state, Rectangle r, Rectangle clip)
		{
			RECT rDraw=new RECT(r);
			RECT rClip=new RECT(clip);
			IntPtr hdc=g.GetHdc();
            try
            {
                int hresult = DrawThemeBackground(m_hTheme, hdc, part.Value, state.Value, ref rDraw, ref rClip);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
		}

		protected virtual IntPtr InternalGetThemeBackgroundRegion(Graphics g, ThemePart part, ThemeState state, Rectangle r)
		{
			RECT rDraw=new RECT(r);
			IntPtr hdc=g.GetHdc();
            IntPtr region = IntPtr.Zero;
            try
            {
                GetThemeBackgroundRegion(m_hTheme, hdc, part.Value, state.Value, ref rDraw, ref region);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
			return region;
		}

		protected virtual void InternalDrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, bool drawdisabled)
		{
			RECT rDraw=new RECT(layoutRect);
			IntPtr hdc=g.GetHdc();
			IntPtr hFont=font.ToHfont();
            IntPtr old=SelectObject(hdc,hFont);
			int hresult=DrawThemeText(m_hTheme,hdc,part.Value,state.Value,text,text.Length,(int)format,(drawdisabled?1:0),ref rDraw);
			SelectObject(hdc,old);
            WinApi.DeleteObject(hFont);
			g.ReleaseHdc(hdc);
		}

        protected virtual void InternalDrawTextEx(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, DTTOPTS options)
        {
            RECT rDraw=new RECT(layoutRect);
			IntPtr hdc=g.GetHdc();
			IntPtr hFont=font.ToHfont();
            IntPtr old=SelectObject(hdc,hFont);
			//int hresult=DrawThemeText(m_hTheme,hdc,part.Value,state.Value,text,text.Length,(int)format,(drawdisabled?1:0),ref rDraw);
            options.dwSize = Marshal.SizeOf(options);
            int hresult = DrawThemeTextEx(m_hTheme, hdc, part.Value, state.Value, text, text.Length, (int)format, ref rDraw, ref options);

			SelectObject(hdc,old);
            WinApi.DeleteObject(hFont);
			g.ReleaseHdc(hdc);
        }

		public virtual bool IsPartDefined(ThemePart part, ThemeState state)
		{
			return IsThemePartDefined(m_hTheme,part.Value,state.Value);
		}

		public void Dispose()
		{
			if(m_hTheme!=IntPtr.Zero)
			{
				CloseThemeData(m_hTheme);
				m_hTheme=IntPtr.Zero;
			}
		}

		public virtual System.Drawing.Size ThemeMinSize(Graphics g, ThemePart part, ThemeState state)
		{
			SIZE m=new SIZE();
			IntPtr hdc=g.GetHdc();
            try
            {
                int hresult = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 0, ref m);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
			return new System.Drawing.Size(m.Width,m.Height);
		}
		public virtual System.Drawing.Size ThemeTrueSize(Graphics g, ThemePart part, ThemeState state)
		{
			SIZE m=new SIZE();
			IntPtr hdc=g.GetHdc();
            try
            {
                int hresult = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 1, ref m);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
			return new System.Drawing.Size(m.Width,m.Height);
		}
		public virtual System.Drawing.Size ThemeDrawSize(Graphics g, ThemePart part, ThemeState state)
		{
			SIZE m=new SIZE();
			IntPtr hdc=g.GetHdc();
            try
            {
                int hresult = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 2, ref m);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
			return new System.Drawing.Size(m.Width,m.Height);
		}
	}

	internal enum ThemeTextFormat:int
	{
		Top=0x00000000,
        Left=0x00000000,
        Center=0x00000001,
        Right=0x00000002,
        VCenter=0x00000004,
        Bottom=0x00000008,
        WordBreak=0x00000010,
        SingleLine=0x00000020,
        ExpandTabs=0x00000040,
        TabStop=0x00000080,
        NoClip=0x00000100,
        ExternalLeading=0x00000200,
        CalcRect=0x00000400,
        NoPrefix=0x00000800,
        Internal=0x00001000,
        EditControl=0x00002000,
        PathElliosis=0x00004000,
        EndEllipsis=0x00008000,
        ModifyString=0x00010000,
        RtlReading=0x00020000,
        WordEllipsis=0x00040000,
        NoFullWidthCharBreak=0x00080000,
        HidePrefix=0x00100000,
        PrefixOnly=0x00200000,
	}

	internal class ThemeButton:Themes
	{
		public ThemeButton(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "Button";
			}
		}
		public void DrawBackground(Graphics g, ThemeButtonParts part, ThemeButtonStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state, ThemeTextFormat format, bool drawdisabled)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,drawdisabled);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state, ThemeTextFormat format)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,false);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,ThemeTextFormat.Left,false);
		}
	}

	internal class ThemeButtonParts:ThemePart
	{
		protected ThemeButtonParts(int themepartvalue):base(themepartvalue)
		{
		}
		public static ThemeButtonParts PushButton=new ThemeButtonParts(1);
		public static ThemeButtonParts RadioButton=new ThemeButtonParts(2);
		public static ThemeButtonParts CheckBox=new ThemeButtonParts(3);
		public static ThemeButtonParts GroupBox=new ThemeButtonParts(4);
		public static ThemeButtonParts UserButton=new ThemeButtonParts(5);
	}

	internal class ThemeButtonStates:ThemeState
	{
		protected ThemeButtonStates(int themestatevalue):base(themestatevalue)
		{
		}
		// Push Buttons
		public static ThemeButtonStates PushButtonNormal=new ThemeButtonStates(1);
		public static ThemeButtonStates PushButtonHot=new ThemeButtonStates(2);
		public static ThemeButtonStates PushButtonPressed=new ThemeButtonStates(3);
		public static ThemeButtonStates PushButtonDisabled=new ThemeButtonStates(4);
		public static ThemeButtonStates PushButtonDefaulted=new ThemeButtonStates(5);

		// Radio Buttons
		public static ThemeButtonStates RadioButtonUncheckedNormal=new ThemeButtonStates(1);
		public static ThemeButtonStates RadioButtonUncheckedHot=new ThemeButtonStates(2);
		public static ThemeButtonStates RadioButtonUncheckedPressed=new ThemeButtonStates(3);
		public static ThemeButtonStates RadioButtonUncheckedDisabled=new ThemeButtonStates(4);
		public static ThemeButtonStates RadioButtonCheckedNormal=new ThemeButtonStates(5);
		public static ThemeButtonStates RadioButtonCheckedHot=new ThemeButtonStates(6);
		public static ThemeButtonStates RadioButtonCheckedPressed=new ThemeButtonStates(7);
		public static ThemeButtonStates RadioButtonCheckedDisabled=new ThemeButtonStates(8);

		// Check Boxes
		public static ThemeButtonStates CheckBoxUncheckedNormal=new ThemeButtonStates(1);
		public static ThemeButtonStates CheckBoxUncheckedHot=new ThemeButtonStates(2);
		public static ThemeButtonStates CheckBoxUncheckedPressed=new ThemeButtonStates(3);
		public static ThemeButtonStates CheckBoxUncheckedDisabled=new ThemeButtonStates(4);
		public static ThemeButtonStates CheckBoxCheckedNormal=new ThemeButtonStates(5);
		public static ThemeButtonStates CheckBoxCheckedHot=new ThemeButtonStates(6);
		public static ThemeButtonStates CheckBoxCheckedPressed=new ThemeButtonStates(7);
		public static ThemeButtonStates CheckBoxCheckedDisabled=new ThemeButtonStates(8);
		public static ThemeButtonStates CheckBoxMixedNormal=new ThemeButtonStates(9);
		public static ThemeButtonStates CheckBoxMixedHot=new ThemeButtonStates(10);
		public static ThemeButtonStates CheckBoxMixedPressed=new ThemeButtonStates(11);
		public static ThemeButtonStates CheckBoxMixedDisabled=new ThemeButtonStates(12);

		// Group Box
		public static ThemeButtonStates GroupBoxNormal=new ThemeButtonStates(1);
		public static ThemeButtonStates GroupBoxDisabled=new ThemeButtonStates(2);
	}

	internal class ThemeToolbar:Themes
	{
		public ThemeToolbar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "Toolbar";
			}
		}
		public void DrawBackground(Graphics g, ThemeToolbarParts part, ThemeToolbarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state, ThemeTextFormat format, bool drawdisabled)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,drawdisabled);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state, ThemeTextFormat format)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,false);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,ThemeTextFormat.Left,false);
		}
	}

	internal class ThemeToolbarParts:ThemePart
	{
		protected ThemeToolbarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeToolbarParts Button=new ThemeToolbarParts(1);
		public static ThemeToolbarParts DropDownButton=new ThemeToolbarParts(2);
		public static ThemeToolbarParts SplitButton=new ThemeToolbarParts(3);
		public static ThemeToolbarParts SplitButtonDropDown=new ThemeToolbarParts(4);
		public static ThemeToolbarParts Separator=new ThemeToolbarParts(5);
		public static ThemeToolbarParts SeparatorVert=new ThemeToolbarParts(6);
	}

	internal class ThemeToolbarStates:ThemeState
	{
		protected ThemeToolbarStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeToolbarStates Normal=new ThemeToolbarStates(1);
		public static ThemeToolbarStates Hot=new ThemeToolbarStates(2);
		public static ThemeToolbarStates Pressed=new ThemeToolbarStates(3);
		public static ThemeToolbarStates Disabled=new ThemeToolbarStates(4);
		public static ThemeToolbarStates Checked=new ThemeToolbarStates(5);
		public static ThemeToolbarStates HotChecked=new ThemeToolbarStates(6);
	}

	internal class ThemeComboBox:Themes
	{
		public ThemeComboBox(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "COMBOBOX";
			}
		}
		public void DrawBackground(Graphics g, ThemeComboBoxParts part, ThemeComboBoxStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeComboBoxParts:ThemePart
	{
		protected ThemeComboBoxParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeComboBoxParts DropDownButton=new ThemeComboBoxParts(1);
	}
	internal class ThemeComboBoxStates:ThemeState
	{
		protected ThemeComboBoxStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeComboBoxStates Normal=new ThemeComboBoxStates(1);
		public static ThemeComboBoxStates Hot=new ThemeComboBoxStates(2);
		public static ThemeComboBoxStates Pressed=new ThemeComboBoxStates(3);
		public static ThemeComboBoxStates Disabled=new ThemeComboBoxStates(4);
	}

	internal class ThemeEdit:Themes
	{
		public ThemeEdit(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "EDIT";
			}
		}
		public void DrawBackground(Graphics g, ThemeEditParts part, ThemeEditStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeEditParts:ThemePart
	{
		protected ThemeEditParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeEditParts EditText=new ThemeEditParts(1);
		public static ThemeEditParts Caret=new ThemeEditParts(2);
	}
	internal class ThemeEditStates:ThemeState
	{
		protected ThemeEditStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeEditStates Normal=new ThemeEditStates(1);
		public static ThemeEditStates Hot=new ThemeEditStates(2);
		public static ThemeEditStates Selected=new ThemeEditStates(3);
		public static ThemeEditStates Disabled=new ThemeEditStates(4);
		public static ThemeEditStates Focused=new ThemeEditStates(5);
		public static ThemeEditStates ReadOnly=new ThemeEditStates(6);
		public static ThemeEditStates Assist=new ThemeEditStates(7);
	}

	internal class ThemeExplorerBar:Themes
	{
		public ThemeExplorerBar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "EXPLORERBAR";
			}
		}
		public void DrawBackground(Graphics g, ThemeExplorerBarParts part, ThemeExplorerBarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state, ThemeTextFormat format, bool drawdisabled)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,drawdisabled);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state, ThemeTextFormat format)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,false);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,ThemeTextFormat.Left,false);
		}
	}
	internal class ThemeExplorerBarParts:ThemePart
	{
		protected ThemeExplorerBarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeExplorerBarParts HeaderBackground=new ThemeExplorerBarParts(1);
		public static ThemeExplorerBarParts HeaderClose=new ThemeExplorerBarParts(2);
		public static ThemeExplorerBarParts HeaderPin=new ThemeExplorerBarParts(3);
		public static ThemeExplorerBarParts IeBarMenu=new ThemeExplorerBarParts(4);
		public static ThemeExplorerBarParts NormalGroupBackground=new ThemeExplorerBarParts(5);
		public static ThemeExplorerBarParts NormalGroupCollapse=new ThemeExplorerBarParts(6);
		public static ThemeExplorerBarParts NormalGroupExpand=new ThemeExplorerBarParts(7);
		public static ThemeExplorerBarParts NormalGroupHead=new ThemeExplorerBarParts(8);
		public static ThemeExplorerBarParts SpecialGroupBackground=new ThemeExplorerBarParts(9);
		public static ThemeExplorerBarParts SpecialGroupCollapse=new ThemeExplorerBarParts(10);
		public static ThemeExplorerBarParts SpecialGroupExpand=new ThemeExplorerBarParts(11);
		public static ThemeExplorerBarParts SpecialGroupHead=new ThemeExplorerBarParts(12);
	}
	internal class ThemeExplorerBarStates:ThemeState
	{
		protected ThemeExplorerBarStates(int themestatevalue):base(themestatevalue) {}
		// Header Background
		public static ThemeExplorerBarStates HeaderBackgroundNormal=new ThemeExplorerBarStates(1);
		// Header Close
		public static ThemeExplorerBarStates HeaderCloseNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates HeaderCloseHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates HeaderClosePressed=new ThemeExplorerBarStates(3);
		// Header Pin
		public static ThemeExplorerBarStates HeaderPinNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates HeaderPinHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates HeaderPinPressed=new ThemeExplorerBarStates(3);
		public static ThemeExplorerBarStates HeaderPinSelectedNormal=new ThemeExplorerBarStates(4);
		public static ThemeExplorerBarStates HeaderPinSelectedHot=new ThemeExplorerBarStates(5);
		public static ThemeExplorerBarStates HeaderPinSelectedPressed=new ThemeExplorerBarStates(6);
		// IE Bar Menu
		public static ThemeExplorerBarStates IeBarMenuNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates IeBarMenuHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates IeBarMenuPressed=new ThemeExplorerBarStates(3);
		// Normal Group Background
		public static ThemeExplorerBarStates NormalGroupBackgroundNormal=new ThemeExplorerBarStates(1);
		// Normal Group Collapse
		public static ThemeExplorerBarStates NormalGroupCollapseNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates NormalGroupCollapseHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates NormalGroupCollapsePressed=new ThemeExplorerBarStates(3);
		// Normal Group Expand
		public static ThemeExplorerBarStates NormalGroupExpandNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates NormalGroupExpandHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates NormalGroupExpandPressed=new ThemeExplorerBarStates(3);
		// Normal Group Head
		public static ThemeExplorerBarStates NormalGroupHeadNormal=new ThemeExplorerBarStates(1);
		// Special Group Background
		public static ThemeExplorerBarStates SpecialGroupBackgroundNormal=new ThemeExplorerBarStates(1);
        // Special Group Collapse
		public static ThemeExplorerBarStates SpecialGroupCollapseNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates SpecialGroupCollapseHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates SpecialGroupCollapsePressed=new ThemeExplorerBarStates(3);
		// Special Group Expand
		public static ThemeExplorerBarStates SpecialGroupExpandNormal=new ThemeExplorerBarStates(1);
		public static ThemeExplorerBarStates SpecialGroupExpandHot=new ThemeExplorerBarStates(2);
		public static ThemeExplorerBarStates SpecialGroupExpandPressed=new ThemeExplorerBarStates(3);
		// Special Group Head
		public static ThemeExplorerBarStates SpecialGroupHeadNormal=new ThemeExplorerBarStates(1);
	}


	internal class ThemeHeader:Themes
	{
		public ThemeHeader(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "HEADER";
			}
		}
		public void DrawBackground(Graphics g, ThemeHeaderParts part, ThemeHeaderStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state, ThemeTextFormat format, bool drawdisabled)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,drawdisabled);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state, ThemeTextFormat format)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,false);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,ThemeTextFormat.Left,false);
		}
	}
	internal class ThemeHeaderParts:ThemePart
	{
		protected ThemeHeaderParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeHeaderParts HeaderItem=new ThemeHeaderParts(1);
		public static ThemeHeaderParts HeaderItemLeft=new ThemeHeaderParts(2);
		public static ThemeHeaderParts HeaderItemRight=new ThemeHeaderParts(3);
		public static ThemeHeaderParts HeaderSortArrow=new ThemeHeaderParts(4);
	}
	internal class ThemeHeaderStates:ThemeState
	{
		protected ThemeHeaderStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeHeaderStates ItemNormal=new ThemeHeaderStates(1);
		public static ThemeHeaderStates ItemHot=new ThemeHeaderStates(2);
		public static ThemeHeaderStates ItemPressed=new ThemeHeaderStates(3);
		// Header Sort Arrow
		public static ThemeHeaderStates SortArrowSortedUp=new ThemeHeaderStates(1);
		public static ThemeHeaderStates SortArrowSortedDown=new ThemeHeaderStates(2);
	}

	internal class ThemeListView:Themes
	{
		public ThemeListView(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "LISTVIEW";
			}
		}
		public void DrawBackground(Graphics g, ThemeListViewParts part, ThemeListViewStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeListViewParts:ThemePart
	{
		protected ThemeListViewParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeListViewParts ListItem=new ThemeListViewParts(1);
		public static ThemeListViewParts ListGroup=new ThemeListViewParts(2);
		public static ThemeListViewParts ListDetail=new ThemeListViewParts(3);
		public static ThemeListViewParts ListSortedDetail=new ThemeListViewParts(4);
		public static ThemeListViewParts EmptyText=new ThemeListViewParts(5);
	}
	internal class ThemeListViewStates:ThemeState
	{
		protected ThemeListViewStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeListViewStates ListGroupNormal=new ThemeListViewStates(1);
		public static ThemeListViewStates ListDetailNormal=new ThemeListViewStates(1);
		public static ThemeListViewStates ListSortedDetailNormal=new ThemeListViewStates(1);
		public static ThemeListViewStates EmptyTextNormal=new ThemeListViewStates(1);

		public static ThemeListViewStates ListItemNormal=new ThemeListViewStates(1);
		public static ThemeListViewStates ListItemHot=new ThemeListViewStates(2);
		public static ThemeListViewStates ListItemSelected=new ThemeListViewStates(3);
		public static ThemeListViewStates ListItemDisabled=new ThemeListViewStates(4);
		public static ThemeListViewStates ListItemSelectedNotFocus=new ThemeListViewStates(5);	
	}

	internal class ThemeMenu:Themes
	{
		public ThemeMenu(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "MENU";
			}
		}
		public void DrawBackground(Graphics g, ThemeMenuParts part, ThemeMenuStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeMenuParts:ThemePart
	{
		protected ThemeMenuParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeMenuParts MenuItem=new ThemeMenuParts(1);
		public static ThemeMenuParts MenuDropDown=new ThemeMenuParts(2);
		public static ThemeMenuParts MenuBarItem=new ThemeMenuParts(3);
		public static ThemeMenuParts MenuBarDropDown=new ThemeMenuParts(4);
		public static ThemeMenuParts Chevron=new ThemeMenuParts(5);
		public static ThemeMenuParts Separator=new ThemeMenuParts(6);
	}
	internal class ThemeMenuStates:ThemeState
	{
		protected ThemeMenuStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeMenuStates Normal=new ThemeMenuStates(1);
		public static ThemeMenuStates Selected=new ThemeMenuStates(2);
		public static ThemeMenuStates Demoted=new ThemeMenuStates(3);
	}

	internal class ThemeMenuBand:Themes
	{
		public ThemeMenuBand(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "MENUBAND";
			}
		}
		public void DrawBackground(Graphics g, ThemeMenuBandParts part, ThemeMenuBandStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeMenuBandParts:ThemePart
	{
		protected ThemeMenuBandParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeMenuBandParts NewAppButton=new ThemeMenuBandParts(1);
		public static ThemeMenuBandParts Separator=new ThemeMenuBandParts(2);
	}
	internal class ThemeMenuBandStates:ThemeState
	{
		protected ThemeMenuBandStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeMenuBandStates Normal=new ThemeMenuBandStates(1);
		public static ThemeMenuBandStates Hot=new ThemeMenuBandStates(2);
		public static ThemeMenuBandStates Pressed=new ThemeMenuBandStates(3);
		public static ThemeMenuBandStates Disabled=new ThemeMenuBandStates(4);
		public static ThemeMenuBandStates Checked=new ThemeMenuBandStates(5);
		public static ThemeMenuBandStates HotChecked=new ThemeMenuBandStates(6);
	}

	internal class ThemePage:Themes
	{
		public ThemePage(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "PAGE";
			}
		}
		public void DrawBackground(Graphics g, ThemePageParts part, ThemePageStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemePageParts:ThemePart
	{
		protected ThemePageParts(int themepartvalue):base(themepartvalue) {}
		public static ThemePageParts Up=new ThemePageParts(1);
		public static ThemePageParts Down=new ThemePageParts(2);
		public static ThemePageParts UpHorz=new ThemePageParts(3);
		public static ThemePageParts DownHorz=new ThemePageParts(4);
	}
	internal class ThemePageStates:ThemeState
	{
		protected ThemePageStates(int themestatevalue):base(themestatevalue) {}
		public static ThemePageStates Normal=new ThemePageStates(1);
		public static ThemePageStates Hot=new ThemePageStates(2);
		public static ThemePageStates Pressed=new ThemePageStates(3);
		public static ThemePageStates Disabled=new ThemePageStates(4);
	}

	internal class ThemeProgress:Themes
	{
		public ThemeProgress(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "PROGRESS";
			}
		}
		public void DrawBackground(Graphics g, ThemeProgressParts part, ThemeProgressStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeProgressParts:ThemePart
	{
		protected ThemeProgressParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeProgressParts Bar=new ThemeProgressParts(1);
		public static ThemeProgressParts BarVert=new ThemeProgressParts(2);
		public static ThemeProgressParts Chunk=new ThemeProgressParts(3);
		public static ThemeProgressParts ChunkVert=new ThemeProgressParts(4);
	}
	internal class ThemeProgressStates:ThemeState
	{
		protected ThemeProgressStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeProgressStates Normal=new ThemeProgressStates(1);
	}

	internal class ThemeRebar:Themes
	{
		public ThemeRebar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "REBAR";
			}
		}
		public void DrawBackground(Graphics g, ThemeRebarParts part, ThemeRebarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeRebarParts:ThemePart
	{
		protected ThemeRebarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeRebarParts Background=new ThemeRebarParts(0);
		public static ThemeRebarParts Gripper=new ThemeRebarParts(1);
		public static ThemeRebarParts GripperVert=new ThemeRebarParts(2);
		public static ThemeRebarParts Band=new ThemeRebarParts(3);
		public static ThemeRebarParts Chevron=new ThemeRebarParts(4);
		public static ThemeRebarParts ChevronVert=new ThemeRebarParts(5);
	}
	internal class ThemeRebarStates:ThemeState
	{
		protected ThemeRebarStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeRebarStates Normal=new ThemeRebarStates(0);
		public static ThemeRebarStates ChevronNormal=new ThemeRebarStates(1);
		public static ThemeRebarStates ChevronHot=new ThemeRebarStates(2);
		public static ThemeRebarStates ChevronPressed=new ThemeRebarStates(3);
	}

	internal class ThemeScrollBar:Themes
	{
		public ThemeScrollBar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "SCROLLBAR";
			}
		}
		public void DrawBackground(Graphics g, ThemeScrollBarParts part, ThemeScrollBarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeScrollBarParts:ThemePart
	{
		protected ThemeScrollBarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeScrollBarParts ArrowBtn=new ThemeScrollBarParts(1);
		public static ThemeScrollBarParts ThumbBtnHorz=new ThemeScrollBarParts(2);
		public static ThemeScrollBarParts ThumbBtnVert=new ThemeScrollBarParts(3);
		public static ThemeScrollBarParts LowerTrackHorz=new ThemeScrollBarParts(4);
		public static ThemeScrollBarParts UpperTrackHorz=new ThemeScrollBarParts(5);
		public static ThemeScrollBarParts LowerTrackVert=new ThemeScrollBarParts(6);
		public static ThemeScrollBarParts UpperTrackVert=new ThemeScrollBarParts(7);
		public static ThemeScrollBarParts GripperHorz=new ThemeScrollBarParts(8);
		public static ThemeScrollBarParts GripperVert=new ThemeScrollBarParts(9);
		public static ThemeScrollBarParts SizeBox=new ThemeScrollBarParts(10);
	}
	internal class ThemeScrollBarStates:ThemeState
	{
		protected ThemeScrollBarStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeScrollBarStates ArrowBtnUpNormal=new ThemeScrollBarStates(1);
		public static ThemeScrollBarStates ArrowBtnUpHot=new ThemeScrollBarStates(2);
		public static ThemeScrollBarStates ArrowBtnUpPressed=new ThemeScrollBarStates(3);
		public static ThemeScrollBarStates ArrowBtnUpDisabled=new ThemeScrollBarStates(4);
		public static ThemeScrollBarStates ArrowBtnDownNormal=new ThemeScrollBarStates(5);
		public static ThemeScrollBarStates ArrowBtnDownHot=new ThemeScrollBarStates(6);
		public static ThemeScrollBarStates ArrowBtnDownPressed=new ThemeScrollBarStates(7);
		public static ThemeScrollBarStates ArrowBtnDownDisabled=new ThemeScrollBarStates(8);
		public static ThemeScrollBarStates ArrowBtnLeftNormal=new ThemeScrollBarStates(9);
		public static ThemeScrollBarStates ArrowBtnLeftHot=new ThemeScrollBarStates(10);
		public static ThemeScrollBarStates ArrowBtnLeftPressed=new ThemeScrollBarStates(11);
		public static ThemeScrollBarStates ArrowBtnLeftDisabled=new ThemeScrollBarStates(12);
		public static ThemeScrollBarStates ArrowBtnRightNormal=new ThemeScrollBarStates(13);
		public static ThemeScrollBarStates ArrowBtnRightHot=new ThemeScrollBarStates(14);
		public static ThemeScrollBarStates ArrowBtnRightPressed=new ThemeScrollBarStates(15);
		public static ThemeScrollBarStates ArrowBtnRightDisabled=new ThemeScrollBarStates(16);
		public static ThemeScrollBarStates GripperHorzNormal=new ThemeScrollBarStates(0);
		public static ThemeScrollBarStates GripperVertNormal=new ThemeScrollBarStates(0);
		public static ThemeScrollBarStates TrackNormal=new ThemeScrollBarStates(1);
		public static ThemeScrollBarStates TrackHot=new ThemeScrollBarStates(2);
		public static ThemeScrollBarStates TrackPressed=new ThemeScrollBarStates(3);
		public static ThemeScrollBarStates TrackDisabled=new ThemeScrollBarStates(4);
		public static ThemeScrollBarStates ThumbNormal=new ThemeScrollBarStates(1);
		public static ThemeScrollBarStates ThumbHot=new ThemeScrollBarStates(2);
		public static ThemeScrollBarStates ThumbPressed=new ThemeScrollBarStates(3);
		public static ThemeScrollBarStates ThumbDisabled=new ThemeScrollBarStates(4);
		public static ThemeScrollBarStates SizeBoxRightAlign=new ThemeScrollBarStates(1);
		public static ThemeScrollBarStates SizeBoxLeftAlign=new ThemeScrollBarStates(2);
	}

	internal class ThemeSpin:Themes
	{
		public ThemeSpin(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "SPIN";
			}
		}
		public void DrawBackground(Graphics g, ThemeSpinParts part, ThemeSpinStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeSpinParts:ThemePart
	{
		protected ThemeSpinParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeSpinParts Up=new ThemeSpinParts(1);
		public static ThemeSpinParts Down=new ThemeSpinParts(2);
		public static ThemeSpinParts UpHorz=new ThemeSpinParts(3);
		public static ThemeSpinParts DownHorz=new ThemeSpinParts(4);
	}
	internal class ThemeSpinStates:ThemeState
	{
		protected ThemeSpinStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeSpinStates Normal=new ThemeSpinStates(1);
		public static ThemeSpinStates Hot=new ThemeSpinStates(2);
		public static ThemeSpinStates Pressed=new ThemeSpinStates(3);
		public static ThemeSpinStates Disabled=new ThemeSpinStates(4);
	}

	internal class ThemeStartPanel:Themes
	{
		public ThemeStartPanel(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "STARTPANEL";
			}
		}
		public void DrawBackground(Graphics g, ThemeStartPanelParts part, ThemeStartPanelStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeStartPanelParts:ThemePart
	{
		protected ThemeStartPanelParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeStartPanelParts UserPane=new ThemeStartPanelParts(1);
		public static ThemeStartPanelParts MorePrograms=new ThemeStartPanelParts(2);
		public static ThemeStartPanelParts MoreProgramsArrow=new ThemeStartPanelParts(3);
		public static ThemeStartPanelParts ProgList=new ThemeStartPanelParts(4);
		public static ThemeStartPanelParts ProgListSeparator=new ThemeStartPanelParts(5);
		public static ThemeStartPanelParts PlacesList=new ThemeStartPanelParts(6);
		public static ThemeStartPanelParts PlacesListSeparator=new ThemeStartPanelParts(7);
		public static ThemeStartPanelParts LogOff=new ThemeStartPanelParts(8);
		public static ThemeStartPanelParts LogOffButtons=new ThemeStartPanelParts(9);
		public static ThemeStartPanelParts UserPicture=new ThemeStartPanelParts(10);
		public static ThemeStartPanelParts Preview=new ThemeStartPanelParts(11);
	}
	internal class ThemeStartPanelStates:ThemeState
	{
		protected ThemeStartPanelStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeStartPanelStates UserPaneNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates MoreProgramsNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates MoreProgramsArrowNormal=new ThemeStartPanelStates(1);
		public static ThemeStartPanelStates MoreProgramsArrowHot=new ThemeStartPanelStates(2);
		public static ThemeStartPanelStates MoreProgramsArrowPressed=new ThemeStartPanelStates(3);
		public static ThemeStartPanelStates ProgListNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates ProgListSeparatorNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates PlacesListNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates PlacesListSeparatorNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates LogOffNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates LogOffButtonsNormal=new ThemeStartPanelStates(1);
		public static ThemeStartPanelStates LogOffButtonsHot=new ThemeStartPanelStates(2);
		public static ThemeStartPanelStates LogOffButtonsPressed=new ThemeStartPanelStates(3);
		public static ThemeStartPanelStates UserPictureNormal=new ThemeStartPanelStates(0);
		public static ThemeStartPanelStates PreviewNormal=new ThemeStartPanelStates(0);

	}

	internal class ThemeStatus:Themes
	{
		public ThemeStatus(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "STATUS";
			}
		}
		public void DrawBackground(Graphics g, ThemeStatusParts part, ThemeStatusStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeStatusParts:ThemePart
	{
		protected ThemeStatusParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeStatusParts Pane=new ThemeStatusParts(1);
		public static ThemeStatusParts GripperPane=new ThemeStatusParts(2);
		public static ThemeStatusParts Gripper=new ThemeStatusParts(3);
	}
	internal class ThemeStatusStates:ThemeState
	{
		protected ThemeStatusStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeStatusStates Normal=new ThemeStatusStates(0);
	}

	internal class ThemeTab:Themes
	{
		public ThemeTab(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TAB";
			}
		}
		public void DrawBackground(Graphics g, ThemeTabParts part, ThemeTabStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTabParts:ThemePart
	{
		protected ThemeTabParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTabParts TabItem=new ThemeTabParts(1);
		public static ThemeTabParts TabItemLeftEdge=new ThemeTabParts(2);
		public static ThemeTabParts TabItemRightEdge=new ThemeTabParts(3);
		public static ThemeTabParts TabItemBothEdge=new ThemeTabParts(4);
		public static ThemeTabParts TopTabItem=new ThemeTabParts(5);
		public static ThemeTabParts TopTabItemLeftEdge=new ThemeTabParts(6);
		public static ThemeTabParts TopTabItemRightEdge=new ThemeTabParts(7);
		public static ThemeTabParts TopTabItemBothEdge=new ThemeTabParts(8);
		public static ThemeTabParts Pane=new ThemeTabParts(9);
		public static ThemeTabParts Body=new ThemeTabParts(10);
	}
	internal class ThemeTabStates:ThemeState
	{
		protected ThemeTabStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTabStates BodyNormal=new ThemeTabStates(0);
		public static ThemeTabStates PaneNormal=new ThemeTabStates(0);
		public static ThemeTabStates Normal=new ThemeTabStates(1);
		public static ThemeTabStates Hot=new ThemeTabStates(2);
		public static ThemeTabStates Selected=new ThemeTabStates(3);
		public static ThemeTabStates Disabled=new ThemeTabStates(4);
		public static ThemeTabStates Focused=new ThemeTabStates(4);

	}

	internal class ThemeTaskBand:Themes
	{
		public ThemeTaskBand(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TASKBAND";
			}
		}
		public void DrawBackground(Graphics g, ThemeTaskBandParts part, ThemeTaskBandStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTaskBandParts:ThemePart
	{
		protected ThemeTaskBandParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTaskBandParts GroupCount=new ThemeTaskBandParts(1);
		public static ThemeTaskBandParts FlashButton=new ThemeTaskBandParts(2);
		public static ThemeTaskBandParts FlashButtonGroupMenu=new ThemeTaskBandParts(3);
	}
	internal class ThemeTaskBandStates:ThemeState
	{
		protected ThemeTaskBandStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTaskBandStates Normal=new ThemeTaskBandStates(0);
	}

	internal class ThemeTaskBar:Themes
	{
		public ThemeTaskBar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TASKBAR";
			}
		}
		public void DrawBackground(Graphics g, ThemeTaskBarParts part, ThemeTaskBarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTaskBarParts:ThemePart
	{
		protected ThemeTaskBarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTaskBarParts BackgroundBottom=new ThemeTaskBarParts(1);
		public static ThemeTaskBarParts BackgroundRight=new ThemeTaskBarParts(2);
		public static ThemeTaskBarParts BackgroundTop=new ThemeTaskBarParts(3);
		public static ThemeTaskBarParts BackgroundLeft=new ThemeTaskBarParts(4);
		public static ThemeTaskBarParts SizingBarBottom=new ThemeTaskBarParts(5);
		public static ThemeTaskBarParts SizingBarRight=new ThemeTaskBarParts(6);
		public static ThemeTaskBarParts SizingBarTop=new ThemeTaskBarParts(7);
		public static ThemeTaskBarParts SizingBarLeft=new ThemeTaskBarParts(8);
	}
	internal class ThemeTaskBarStates:ThemeState
	{
		protected ThemeTaskBarStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTaskBarStates Normal=new ThemeTaskBarStates(0);
	}

	internal class ThemeTooltip:Themes
	{
		public ThemeTooltip(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TOOLTIP";
			}
		}
		public void DrawBackground(Graphics g, ThemeTooltipParts part, ThemeTooltipStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTooltipParts:ThemePart
	{
		protected ThemeTooltipParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTooltipParts Standard=new ThemeTooltipParts(1);
		public static ThemeTooltipParts StandardTitle=new ThemeTooltipParts(2);
		public static ThemeTooltipParts Balloon=new ThemeTooltipParts(3);
		public static ThemeTooltipParts BalloonTitle=new ThemeTooltipParts(4);
		public static ThemeTooltipParts Close=new ThemeTooltipParts(5);
	}
	internal class ThemeTooltipStates:ThemeState
	{
		protected ThemeTooltipStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTooltipStates StandardNormal=new ThemeTooltipStates(1);
		public static ThemeTooltipStates StandardLink=new ThemeTooltipStates(2);
		public static ThemeTooltipStates BalloonNormal=new ThemeTooltipStates(1);
		public static ThemeTooltipStates BalloonLink=new ThemeTooltipStates(2);
		public static ThemeTooltipStates CloseNormal=new ThemeTooltipStates(1);
		public static ThemeTooltipStates CloseHot=new ThemeTooltipStates(2);
		public static ThemeTooltipStates ClosePressed=new ThemeTooltipStates(3);
	}

	internal class ThemeTrackbar:Themes
	{
		public ThemeTrackbar(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TRACKBAR";
			}
		}
		public void DrawBackground(Graphics g, ThemeTrackbarParts part, ThemeTrackbarStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTrackbarParts:ThemePart
	{
		protected ThemeTrackbarParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTrackbarParts Track=new ThemeTrackbarParts(1);
		public static ThemeTrackbarParts TrackVert=new ThemeTrackbarParts(2);
		public static ThemeTrackbarParts Thumb=new ThemeTrackbarParts(3);
		public static ThemeTrackbarParts ThumbBottom=new ThemeTrackbarParts(4);
		public static ThemeTrackbarParts ThumbTop=new ThemeTrackbarParts(5);
		public static ThemeTrackbarParts ThumbVert=new ThemeTrackbarParts(6);
		public static ThemeTrackbarParts ThumbLeft=new ThemeTrackbarParts(7);
		public static ThemeTrackbarParts ThumbRight=new ThemeTrackbarParts(8);
		public static ThemeTrackbarParts Tics=new ThemeTrackbarParts(9);
		public static ThemeTrackbarParts TicsVert=new ThemeTrackbarParts(10);
	}
	internal class ThemeTrackbarStates:ThemeState
	{
		protected ThemeTrackbarStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTrackbarStates TrackNormal=new ThemeTrackbarStates(1);
		public static ThemeTrackbarStates TicsNormal=new ThemeTrackbarStates(1);
        public static ThemeTrackbarStates ThumbNormal=new ThemeTrackbarStates(1);
		public static ThemeTrackbarStates ThumbHot=new ThemeTrackbarStates(2);
		public static ThemeTrackbarStates ThumbPressed=new ThemeTrackbarStates(3);
		public static ThemeTrackbarStates ThumbFocused=new ThemeTrackbarStates(4);
		public static ThemeTrackbarStates ThumbDisabled=new ThemeTrackbarStates(5);
	}

	internal class ThemeTrayNotify:Themes
	{
		public ThemeTrayNotify(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TRAYNOTIFY";
			}
		}
		public void DrawBackground(Graphics g, ThemeTrayNotifyParts part, ThemeTrayNotifyStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTrayNotifyParts:ThemePart
	{
		protected ThemeTrayNotifyParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTrayNotifyParts Background=new ThemeTrayNotifyParts(1);
		public static ThemeTrayNotifyParts AnimBackground=new ThemeTrayNotifyParts(2);
	}
	internal class ThemeTrayNotifyStates:ThemeState
	{
		protected ThemeTrayNotifyStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTrayNotifyStates Normal=new ThemeTrayNotifyStates(0);
	}

	internal class ThemeTreeView:Themes
	{
		public ThemeTreeView(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "TREEVIEW";
			}
		}
		public void DrawBackground(Graphics g, ThemeTreeViewParts part, ThemeTreeViewStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
	}
	internal class ThemeTreeViewParts:ThemePart
	{
		protected ThemeTreeViewParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeTreeViewParts TreeItem=new ThemeTreeViewParts(1);
		public static ThemeTreeViewParts Glyph=new ThemeTreeViewParts(1);
		public static ThemeTreeViewParts Branch=new ThemeTreeViewParts(1);
	}
	internal class ThemeTreeViewStates:ThemeState
	{
		protected ThemeTreeViewStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeTreeViewStates BranchNormal=new ThemeTreeViewStates(0);
		public static ThemeTreeViewStates GlyphClosed=new ThemeTreeViewStates(1);
		public static ThemeTreeViewStates GlyphOpen=new ThemeTreeViewStates(2);
		public static ThemeTreeViewStates TreeItemNormal=new ThemeTreeViewStates(1);
		public static ThemeTreeViewStates TreeItemHot=new ThemeTreeViewStates(2);
		public static ThemeTreeViewStates TreeItemSelected=new ThemeTreeViewStates(3);
		public static ThemeTreeViewStates TreeItemDisabled=new ThemeTreeViewStates(4);
		public static ThemeTreeViewStates TreeItemSelectedNotFocus=new ThemeTreeViewStates(5);
	}

	internal class ThemeWindow:Themes
	{
		public ThemeWindow(Control parent):base(parent)
		{
		}
		protected override string ThemeClass
		{
			get
			{
				return "WINDOW";
			}
		}
		public void DrawBackground(Graphics g, ThemeWindowParts part, ThemeWindowStates state, Rectangle r)
		{
			InternalDrawBackground(g,part,state,r);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state, ThemeTextFormat format, bool drawdisabled)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,drawdisabled);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state, ThemeTextFormat format)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,format,false);
		}
		public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state)
		{
			InternalDrawText(g,text,font,layoutRect,part,state,ThemeTextFormat.Left,false);
		}
        public void DrawTextEx(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, DTTOPTS options)
        {
            InternalDrawTextEx(g, text, font, layoutRect, part, state, format, options);
        }

		public IntPtr GetThemeBackgroundRegion(Graphics g, ThemeWindowParts part, ThemeWindowStates state, Rectangle r)
		{
            return this.InternalGetThemeBackgroundRegion(g,part,state,r);
		}
	}
	internal class ThemeWindowParts:ThemePart
	{
		protected ThemeWindowParts(int themepartvalue):base(themepartvalue) {}
		public static ThemeWindowParts Root=new ThemeWindowParts(0);
		public static ThemeWindowParts Caption=new ThemeWindowParts(1);
		public static ThemeWindowParts SmallCaption=new ThemeWindowParts(2);
		public static ThemeWindowParts MinCaption=new ThemeWindowParts(3);
		public static ThemeWindowParts SmallMinCaption=new ThemeWindowParts(4);
		public static ThemeWindowParts MaxCaption=new ThemeWindowParts(5);
		public static ThemeWindowParts SmallMaxCaption=new ThemeWindowParts(6);
		public static ThemeWindowParts FrameLeft=new ThemeWindowParts(7);
		public static ThemeWindowParts FrameRight=new ThemeWindowParts(8);
		public static ThemeWindowParts FrameBottom=new ThemeWindowParts(9);
		public static ThemeWindowParts SmallFrameLeft=new ThemeWindowParts(10);
		public static ThemeWindowParts SmallFrameRight=new ThemeWindowParts(11);
		public static ThemeWindowParts SmallFrameBottom=new ThemeWindowParts(12);
		public static ThemeWindowParts SysButton=new ThemeWindowParts(13);
		public static ThemeWindowParts MdiSysButton=new ThemeWindowParts(14);
		public static ThemeWindowParts MdiMinButton=new ThemeWindowParts(16);
		public static ThemeWindowParts MaxButton=new ThemeWindowParts(17);
		public static ThemeWindowParts CloseButton=new ThemeWindowParts(18);
		public static ThemeWindowParts SmallCloseButton=new ThemeWindowParts(19);
		public static ThemeWindowParts MdiCloseButton=new ThemeWindowParts(20);
		public static ThemeWindowParts RestoreButton=new ThemeWindowParts(21);
		public static ThemeWindowParts MdiRestoreButton=new ThemeWindowParts(22);
		public static ThemeWindowParts HelpButton=new ThemeWindowParts(23);
		public static ThemeWindowParts MdiHelpButton=new ThemeWindowParts(24);
		public static ThemeWindowParts HorzScroll=new ThemeWindowParts(25);
		public static ThemeWindowParts HorzThumb=new ThemeWindowParts(26);
		public static ThemeWindowParts VertScroll=new ThemeWindowParts(27);
		public static ThemeWindowParts VertThumb=new ThemeWindowParts(28);
		public static ThemeWindowParts Dialog=new ThemeWindowParts(29);
		//---- hit-test templates ---
		public static ThemeWindowParts HtCaptionSizingTemplate=new ThemeWindowParts(30);
		public static ThemeWindowParts HtSmallCaptionSizingTemplate=new ThemeWindowParts(31);
		public static ThemeWindowParts HtFrameLeftSizingTemplate=new ThemeWindowParts(32);
		public static ThemeWindowParts HtSmallFrameLeftSizingTemplate=new ThemeWindowParts(33);
		public static ThemeWindowParts HtFrameRightSizingTemplate=new ThemeWindowParts(34);
		public static ThemeWindowParts HtSmallFrameRightSizingTemplate=new ThemeWindowParts(35);
		public static ThemeWindowParts HtFrameBottomSizingTemplate=new ThemeWindowParts(36);
		public static ThemeWindowParts HtSmallFrameBottomSizingTemplate=new ThemeWindowParts(37);
	}
	internal class ThemeWindowStates:ThemeState
	{
		protected ThemeWindowStates(int themestatevalue):base(themestatevalue) {}
		public static ThemeWindowStates Normal=new ThemeWindowStates(0);
		public static ThemeWindowStates CaptionActive=new ThemeWindowStates(1);
		public static ThemeWindowStates CaptionInactive=new ThemeWindowStates(2);
		public static ThemeWindowStates CaptionDisabled=new ThemeWindowStates(3);
		public static ThemeWindowStates FrameActive=new ThemeWindowStates(1);
		public static ThemeWindowStates FrameInactive=new ThemeWindowStates(2);
		public static ThemeWindowStates ButtonNormal=new ThemeWindowStates(1);
		public static ThemeWindowStates ButtonHot=new ThemeWindowStates(2);
		public static ThemeWindowStates ButtonPushed=new ThemeWindowStates(3);
		public static ThemeWindowStates ButtonDisabled=new ThemeWindowStates(4);
		public static ThemeWindowStates HorzScrollNormal=new ThemeWindowStates(1);
		public static ThemeWindowStates HorzScrollHot=new ThemeWindowStates(2);
		public static ThemeWindowStates HorzScrollPushed=new ThemeWindowStates(3);
		public static ThemeWindowStates HorzScrollDisabled=new ThemeWindowStates(4);
		public static ThemeWindowStates HorzThumbNormal=new ThemeWindowStates(1);
		public static ThemeWindowStates HorzThumbHot=new ThemeWindowStates(2);
		public static ThemeWindowStates HorzThumbPushed=new ThemeWindowStates(3);
		public static ThemeWindowStates HorzThumbDisabled=new ThemeWindowStates(4);
		public static ThemeWindowStates VertScrollNormal=new ThemeWindowStates(1);
		public static ThemeWindowStates VertScrollHot=new ThemeWindowStates(2);
		public static ThemeWindowStates VertScrollPushed=new ThemeWindowStates(3);
		public static ThemeWindowStates VertScrollDisabled=new ThemeWindowStates(4);
		public static ThemeWindowStates VertThumbNormal=new ThemeWindowStates(1);
		public static ThemeWindowStates VertThumbHot=new ThemeWindowStates(2);
		public static ThemeWindowStates VertThumbPushed=new ThemeWindowStates(3);
		public static ThemeWindowStates VertThumbDisabled=new ThemeWindowStates(4);
	}

	internal class ThemePart
	{
		public readonly int Value;
		public ThemePart(int themepartvalue)
		{
			this.Value=themepartvalue;
		}
	}

	internal abstract class ThemeState
	{
		public readonly int Value;
		public ThemeState(int themestatevalue)
		{
			this.Value=themestatevalue;
		}
	}
}
