using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Summary description for MessageHandler.
    /// </summary>
    internal class NonClientHook
    {
        private static ArrayList m_Clients = new ArrayList();
        private static Hashtable m_FilterOnThread = new Hashtable();
        private static ReaderWriterLock rwClientsListLock;
        private static MouseProc m_MouseHook = null;

        private delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int hookid, MouseProc pfnhook, IntPtr hinst, int threadid);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "SetWindowsHookEx")]
        //private static extern IntPtr SetWindowsHookExKeyboard(int hookid, KeyboardProc pfnhook, IntPtr hinst, int threadid);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int GetCurrentThreadId();

        private const int WH_MOUSE = 7;
        private const int HC_ACTION = 0;
        private const int WH_CALLWNDPROCRET = 12;
        private const int WH_CALLWNDPROC = 4;

        // Methods
        static NonClientHook()
        {
            NonClientHook.rwClientsListLock = new ReaderWriterLock();
            m_MouseHook = new MouseProc(OnMouseHook);
        }

        public static void RegisterHook(ISkinHook client)
        {
            if (m_Clients.Contains(client))
                return;

            if (!m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
                HookThread();

            LockCookie cookie1 = new LockCookie();
            bool readerLockHeld = NonClientHook.rwClientsListLock.IsReaderLockHeld;

            if (readerLockHeld)
            {
                cookie1 = NonClientHook.rwClientsListLock.UpgradeToWriterLock(-1);
            }
            else
            {
                NonClientHook.rwClientsListLock.AcquireWriterLock(-1);
            }

            try
            {
                m_Clients.Add(client);
            }
            finally
            {
                if (readerLockHeld)
                {
                    NonClientHook.rwClientsListLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    NonClientHook.rwClientsListLock.ReleaseWriterLock();
                }
            }
        }

        public static void UnregisterHook(ISkinHook client)
        {
            if (m_Clients.Contains(client))
            {
                LockCookie cookie1 = new LockCookie();
                bool readerLockHeld = NonClientHook.rwClientsListLock.IsReaderLockHeld;

                if (readerLockHeld)
                {
                    cookie1 = NonClientHook.rwClientsListLock.UpgradeToWriterLock(-1);
                }
                else
                {
                    NonClientHook.rwClientsListLock.AcquireWriterLock(-1);
                }

                try
                {
                    m_Clients.Remove(client);
                    if (m_Clients.Count == 0)
                        UnHookThread();
                }
                finally
                {
                    if (readerLockHeld)
                    {
                        NonClientHook.rwClientsListLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        NonClientHook.rwClientsListLock.ReleaseWriterLock();
                    }
                }
            }
        }

        private static void HookThread()
        {
            if (m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
                return;

            int id = GetCurrentThreadId();
            IntPtr hook = SetWindowsHookEx(WH_MOUSE, m_MouseHook, IntPtr.Zero, id);
            m_FilterOnThread.Add(System.Threading.Thread.CurrentThread.GetHashCode(), hook);
        }

        private static void UnHookThread()
        {
            if (!m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
                return;
            IntPtr hook = (IntPtr)m_FilterOnThread[System.Threading.Thread.CurrentThread.GetHashCode()];
            UnhookWindowsHookEx(hook);
            m_FilterOnThread.Remove(System.Threading.Thread.CurrentThread.GetHashCode());
        }

        private static ISkinHook[] GetMessageClients()
        {
            ISkinHook[] messageClients;
            NonClientHook.rwClientsListLock.AcquireReaderLock(-1);
            try
            {
                messageClients = (ISkinHook[])m_Clients.ToArray(typeof(ISkinHook));
            }
            finally
            {
                NonClientHook.rwClientsListLock.ReleaseReaderLock();
            }

            return messageClients;
        }

        private static unsafe IntPtr OnMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode == HC_ACTION)
                {
                    if (wParam.ToInt32() == (int)WinApi.WindowsMessages.WM_MOUSEMOVE)
                    {
                        MOUSEHOOKSTRUCT* ws = (MOUSEHOOKSTRUCT*)lParam;
                        PostMouseMove(ws->hwnd, ws->pt.ToPoint());
                    }
                    if (wParam.ToInt32() == (int)WinApi.WindowsMessages.WM_LBUTTONUP)
                    {
                        MOUSEHOOKSTRUCT* ws = (MOUSEHOOKSTRUCT*)lParam;
                        PostMouseUp(ws->hwnd, ws->pt.ToPoint());
                    }
                    //CWPRETSTRUCT* ws = (CWPRETSTRUCT*)lParam;
                    //if (ws->message == (uint)WinApi.WindowsMessages.WM_MOUSEMOVE || ws->message == 0x118)
                    //{
                    //    PostMouseMove(ws->hwnd, ws->wParam, ws->lParam);
                    //}

                }



                IntPtr h = (IntPtr)m_FilterOnThread[System.Threading.Thread.CurrentThread.GetHashCode()];
                IntPtr res = CallNextHookEx(h, nCode, wParam, lParam);

                return res;
            }
            catch
            { }
            return IntPtr.Zero;
        }

        public static bool PostMouseMove(IntPtr hWnd, Point mousePos)
        {
            ISkinHook[] messageClients = GetMessageClients();
            foreach (ISkinHook client in messageClients)
            {
                client.PostMouseMove(hWnd, mousePos);
            }
            return false;
        }

        public static bool PostMouseUp(IntPtr hWnd, Point mousePos)
        {
            ISkinHook[] messageClients = GetMessageClients();
            foreach (ISkinHook client in messageClients)
            {
                client.PostMouseUp(hWnd, mousePos);
            }
            return false;
        }

        private struct POINT
        {
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x;
            public int y;

            public Point ToPoint()
            {
                return new Point(x, y);
            }
        }
        private struct MOUSEHOOKSTRUCT
        {
            public MOUSEHOOKSTRUCT(POINT pt, IntPtr hwnd, ushort wHitTestCode, IntPtr dwExtraInfo)
            {
                this.pt = pt;
                this.hwnd = hwnd;
                this.wHitTestCode = wHitTestCode;
                this.dwExtraInfo = dwExtraInfo;
            }
            public POINT pt;
            public IntPtr hwnd;
            public ushort wHitTestCode;
            public IntPtr dwExtraInfo;
        }
    }

    internal interface ISkinHook
    {
        void PostMouseMove(IntPtr hWnd, Point mousePos);
        void PostMouseUp(IntPtr hWnd, Point mousePos);
    }
}
