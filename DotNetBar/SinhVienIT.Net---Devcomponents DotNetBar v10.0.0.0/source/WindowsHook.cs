using System;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for WindowsHook.
	/// </summary>
	internal class WindowsHook:IDisposable
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct CWPSTRUCT 
		{ 
			public int lParam; 
			public int wParam; 
			public int message; 
			public int hwnd; 
		}
		[DllImport("user32")]
		private static extern int SetWindowsHookEx(int HookType, CWPCallBack pfnCallBack, uint hInstance, int dwThreadId);
		[DllImport("user32")]
		private static extern int CallNextHookEx(int CurrentHookHandle, int nCode, int wParam, ref CWPSTRUCT lParam);
		[DllImport("user32")]
		private static extern bool UnhookWindowsHookEx(int HookHandle);
		[DllImport("kernel32")]
		private static extern int GetLastError();
		[DllImport("kernel32.dll")]
		private static extern uint GetModuleHandle([MarshalAs(UnmanagedType.LPTStr)]string /*LPCTSTR*/ lpModuleName);

		private delegate int CWPCallBack(int nCode, int wParam, ref CWPSTRUCT lParam);

		private const int WH_CALLWNDPROC=4;
		private const int WH_GETMESSAGE = 3;
		private const int WH_MOUSE = 7;


		private int m_HookHandle=0;
		private DotNetBarManager m_DotNetBar=null;

		public WindowsHook(DotNetBarManager dm)
		{
			m_DotNetBar=dm;
		}

		public void Dispose()
		{
			this.UnHook();
		}

		public void Hook()
		{
			//uint m=GetModuleHandle(null);
			//System.Windows.Forms.MessageBox.Show("Module="+GetLastError().ToString());
			m_HookHandle=SetWindowsHookEx(WH_CALLWNDPROC,new CWPCallBack(this.HookCallBack),0,System.AppDomain.GetCurrentThreadId());
			if(m_HookHandle==0)
				Console.WriteLine("HOOOK DOES NOT WORK="+GetLastError().ToString());
		}
		public void UnHook()
		{
			if(m_HookHandle!=0)
			{
				UnhookWindowsHookEx(m_HookHandle);
				m_HookHandle=0;
			}
		}
		public bool Hooked
		{
			get
			{
				if(m_HookHandle!=0)
					return true;
				return false;
			}
		}
		private int HookCallBack(int nCode, int wParam, ref CWPSTRUCT lParam)
		{
			if(lParam.message == NativeFunctions.WM_SYSKEYDOWN)
			{
				//m_DotNetBar.OnSysKeyDown(lParam.hwnd,lParam.wParam,lParam.lParam);
			}
			else if(lParam.message == NativeFunctions.WM_SYSKEYUP)
			{
				//m_DotNetBar.OnSysKeyUp(lParam.hwnd,lParam.wParam,lParam.lParam);
			}
			else if(lParam.message == NativeFunctions.WM_KEYDOWN)
			{
				//m_DotNetBar.OnKeyDown(lParam.hwnd,lParam.wParam,lParam.lParam);
			}
			else if(lParam.message == NativeFunctions.WM_LBUTTONDOWN || lParam.message==NativeFunctions.WM_NCLBUTTONDOWN ||
				lParam.message==NativeFunctions.WM_RBUTTONDOWN || lParam.message==NativeFunctions.WM_MBUTTONDOWN || 
				lParam.message==NativeFunctions.WM_NCMBUTTONDOWN || lParam.message==NativeFunctions.WM_NCRBUTTONDOWN)
			{
				//m_DotNetBar.OnMouseDown(lParam.hwnd,lParam.wParam,lParam.lParam);
			}
			
			return CallNextHookEx(m_HookHandle,nCode,wParam,ref lParam);
		}
	}
}
