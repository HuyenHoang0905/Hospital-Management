#define NO_UNSAFE
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for Hook.
	/// </summary>
	internal class Hook : IDisposable
	{
		private const int WH_MOUSE = 7;
		private const int HC_ACTION = 0;
		private const int WM_MOUSEMOVE = 0x0200;
		private const int WH_KEYBOARD = 2;

		private const int KF_EXTENDED=0x0100;
		private const int KF_DLGMODE=0x0800;
		private const int KF_MENUMODE=0x1000;
		private const int KF_ALTDOWN=0x2000;
		private const int KF_REPEAT=0x4000;
		private const int KF_UP=0x8000;

		private IntPtr m_hMouseHook=IntPtr.Zero;
		private IntPtr m_hKeyboardHook=IntPtr.Zero;
		private MouseProc m_mouseHook;
		private KeyboardProc m_keyboardHook;
		private IMessageHandlerClient m_Client=null;

		public Hook(IMessageHandlerClient client)
		{
			m_Client=client;
			m_mouseHook = new MouseProc(OnMouseHook);
			m_keyboardHook=new KeyboardProc(OnKeyboardHook);
			m_hMouseHook = SetWindowsHookEx(WH_MOUSE, m_mouseHook, IntPtr.Zero, GetCurrentThreadId());
			m_hKeyboardHook=SetWindowsHookExKeyboard(WH_KEYBOARD,m_keyboardHook,IntPtr.Zero,GetCurrentThreadId());
			if (m_hMouseHook == IntPtr.Zero || m_hKeyboardHook==IntPtr.Zero)
			{
				throw new Win32Exception();
			}
		}

		public void Dispose() 
		{
			if (m_hMouseHook != IntPtr.Zero)
			{
				IntPtr h = m_hMouseHook;
				m_hMouseHook = IntPtr.Zero;
				UnhookWindowsHookEx(h);
			}
			if (m_hKeyboardHook != IntPtr.Zero)
			{
				IntPtr h = m_hKeyboardHook;
				m_hKeyboardHook = IntPtr.Zero;
				UnhookWindowsHookEx(h);
			}
		}

		// A note on using unsafe code:
		//
		// When taking a native pointer and converting it into a structure pointer,
		// if you only need to examine the contents of the structure then it is very
		// wasteful to use Marshal.PtrToStructure.  Structures are very efficient
		// because they are stack allocated, but by converting them to an object
		// to be returned, Marshal causes a boxing operation to happen which
		// allocates an object on the heap.  This normally isn't a big deal
		// but for window messages that occur with a high frequency you can
		// see a spike of many hundreds of objects being allocated.  Using
		// unsafe blocks are great when you are writing an EXE that runs on a 
		// user's local machine.  If you are writing a library that can be downloaded
		// over the internet you should not use them, because they require elevated
		// security permissions.  That's not a big deal for this sample because
		// we already need unmanaged code permission to create the hook.

#if NO_UNSAFE
		private IntPtr OnMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode == HC_ACTION)
			{
				int msg=wParam.ToInt32();
				if(msg == NativeFunctions.WM_LBUTTONDOWN || msg==NativeFunctions.WM_NCLBUTTONDOWN ||
							msg==NativeFunctions.WM_RBUTTONDOWN || msg==NativeFunctions.WM_MBUTTONDOWN || 
							msg==NativeFunctions.WM_NCMBUTTONDOWN || msg==NativeFunctions.WM_NCRBUTTONDOWN)
				{
					MOUSEHOOKSTRUCT mhs = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));

					m_Client.OnMouseDown(mhs.hwnd,IntPtr.Zero,lParam);
				}
			}

			return CallNextHookEx(m_hMouseHook, nCode, wParam, lParam);
		}
		private IntPtr OnKeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode == HC_ACTION)
			{
                int ilParam=lParam.ToInt32();
				int iwParam=wParam.ToInt32();
				int iCode=ilParam>>16;
				if((iCode & KF_ALTDOWN)!=0)
				{
					if((iCode & KF_UP)!=0)
					{
						if(m_Client.OnSysKeyUp(IntPtr.Zero,wParam,lParam))
							return (IntPtr)1;
					}
					else //if((ilParam & KF_REPEAT)==0)
					{
						if(m_Client.OnSysKeyDown(IntPtr.Zero,wParam,lParam))
							return (IntPtr)1;
					}
				}
				else
				{
					if((iCode & KF_UP)==0)
					{
						if(m_Client.OnKeyDown(IntPtr.Zero,wParam,lParam))
							return (IntPtr)1;
					}
					else if(iwParam==18 || iwParam==121)
					{
						if(m_Client.OnSysKeyUp(IntPtr.Zero,wParam,lParam))
							return (IntPtr)1;
					}
				}
			}
			return CallNextHookEx(m_hKeyboardHook, nCode, wParam, lParam);
		}
#else
		private unsafe IntPtr OnMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode == HC_ACTION && MouseEvent != null)
			{
				if (wParam == (IntPtr)WM_MOUSEMOVE)
				{
					MOUSEHOOKSTRUCT* pmhs = (MOUSEHOOKSTRUCT*)lParam;
					MouseEventArgs e = new MouseEventArgs(MouseButtons.None, 0, pmhs->pt.x, pmhs->pt.y, 0);
					MouseEvent(this, e);
				}
			}

			return CallNextHookEx(_hHook, nCode, wParam, lParam);
		}
#endif

		private delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);
		private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern IntPtr SetWindowsHookEx(int hookid, MouseProc pfnhook, IntPtr hinst, int threadid);

		[DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true, EntryPoint="SetWindowsHookEx")]
		private static extern IntPtr SetWindowsHookExKeyboard(int hookid, KeyboardProc pfnhook, IntPtr hinst, int threadid);

		[DllImport("user32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhook);

		[DllImport("user32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
		private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

		[DllImport("kernel32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
		private static extern int GetCurrentThreadId();

		private struct POINT
		{
			public POINT(int x, int y)
			{
				this.x=x;
				this.y=y;
			}
			public int x;
			public int y;
		}

		private struct MOUSEHOOKSTRUCT 
		{
			public MOUSEHOOKSTRUCT(POINT pt, IntPtr hwnd, ushort wHitTestCode, IntPtr dwExtraInfo)
			{
				this.pt=pt;
				this.hwnd=hwnd;
				this.wHitTestCode=wHitTestCode;
				this.dwExtraInfo=dwExtraInfo;
			}
			public POINT   pt;
			public IntPtr    hwnd;
			public ushort    wHitTestCode;
			public IntPtr dwExtraInfo;
		}
	}
}
