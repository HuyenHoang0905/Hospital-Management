using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for MessageHandler.
	/// </summary>
	internal class MessageHandler
	{
		private static ArrayList m_Clients=new ArrayList();
		//private static bool m_FilterInstalled=false;
		//private static MessageFilterWrapper m_MessageFilter=null;
		private static Hashtable m_FilterOnThread=new Hashtable();

        private static ReaderWriterLock rwClientsListLock;

        // Methods
        static MessageHandler()
        {
            MessageHandler.rwClientsListLock = new ReaderWriterLock();
        }

		public static void RegisterMessageClient(IMessageHandlerClient client)
		{
			if(m_Clients.Contains(client))
				return;

			if(!m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
				InstallMessageFilter();

            LockCookie cookie1 = new LockCookie();
            bool readerLockHeld = MessageHandler.rwClientsListLock.IsReaderLockHeld;

            if (readerLockHeld)
            {
                cookie1 = MessageHandler.rwClientsListLock.UpgradeToWriterLock(-1);
            }
            else
            {
                MessageHandler.rwClientsListLock.AcquireWriterLock(-1);
            }

            try
            {
                m_Clients.Add(client);
            }
            finally
            {
                if (readerLockHeld)
                {
                    MessageHandler.rwClientsListLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    MessageHandler.rwClientsListLock.ReleaseWriterLock();
                }
            }
		}

		public static void UnregisterMessageClient(IMessageHandlerClient client)
		{
            if (m_Clients.Contains(client))
            {
                LockCookie cookie1 = new LockCookie();
                bool readerLockHeld = MessageHandler.rwClientsListLock.IsReaderLockHeld;

                if (readerLockHeld)
                {
                    cookie1 = MessageHandler.rwClientsListLock.UpgradeToWriterLock(-1);
                }
                else
                {
                    MessageHandler.rwClientsListLock.AcquireWriterLock(-1);
                }
                bool unregister = false;
                try
                {
                    m_Clients.Remove(client);
                    unregister = m_Clients.Count == 0;
                }
                finally
                {
                    if (readerLockHeld)
                    {
                        MessageHandler.rwClientsListLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        MessageHandler.rwClientsListLock.ReleaseWriterLock();
                    }
                }

                if (BarUtilities.AutoRemoveMessageFilter && unregister && m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
                    UninstallMessageFilter();
            }
		}

		private static void InstallMessageFilter()
		{
			if(m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
				return;
			MessageFilterWrapper messageFilter=new MessageFilterWrapper();
			Application.AddMessageFilter(messageFilter);
			m_FilterOnThread.Add(System.Threading.Thread.CurrentThread.GetHashCode(),messageFilter);
		}

		private static void UninstallMessageFilter()
		{
			if(!m_FilterOnThread.ContainsKey(System.Threading.Thread.CurrentThread.GetHashCode()))
				return;
			MessageFilterWrapper messageFilter=m_FilterOnThread[System.Threading.Thread.CurrentThread.GetHashCode()] as MessageFilterWrapper;
			Application.RemoveMessageFilter(messageFilter);
			m_FilterOnThread.Remove(System.Threading.Thread.CurrentThread.GetHashCode());
		}

		private static IMessageHandlerClient GetModalClient()
		{
            MessageHandler.rwClientsListLock.AcquireReaderLock(-1);
            try
            {
                foreach (IMessageHandlerClient client in m_Clients)
                {
                    if (client.IsModal)
                    {
                        return client;
                    }
                }
            }
            finally
            {
                MessageHandler.rwClientsListLock.ReleaseReaderLock();
            }
			return null;
		}

        private static IMessageHandlerClient[] GetMessageClients()
        {
            IMessageHandlerClient[] messageClients;
            MessageHandler.rwClientsListLock.AcquireReaderLock(-1);
            try
            {
                messageClients = (IMessageHandlerClient[])m_Clients.ToArray(typeof(IMessageHandlerClient));
            }
            finally
            {
                MessageHandler.rwClientsListLock.ReleaseReaderLock();
            }

            return messageClients;
        }

		public static bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			IMessageHandlerClient modalClient=MessageHandler.GetModalClient();
			if(modalClient!=null)
			{
                if (!IsInvokeRequired(modalClient) && modalClient.OnSysKeyDown(hWnd, wParam, lParam))
					return true;
				return false;
			}

			IMessageHandlerClient[] messageClients = GetMessageClients();

			foreach(IMessageHandlerClient client in messageClients)
			{
                if (!IsInvokeRequired(client) && client.OnSysKeyDown(hWnd, wParam, lParam))
					return true;
			}
			return false;
		}

		public static bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			IMessageHandlerClient modalClient=MessageHandler.GetModalClient();
			if(modalClient!=null)
			{
                if (!IsInvokeRequired(modalClient) && modalClient.OnSysKeyUp(hWnd, wParam, lParam))
					return true;
				return false;
			}

            IMessageHandlerClient[] messageClients = GetMessageClients();
			foreach(IMessageHandlerClient client in messageClients)
			{
                if (!IsInvokeRequired(client) && client.OnSysKeyUp(hWnd, wParam, lParam))
					return true;
			}
			return false;
		}

		public static bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            IMessageHandlerClient[] messageClients = GetMessageClients();

			foreach(IMessageHandlerClient client in messageClients)
			{
                if (!IsInvokeRequired(client) && client.OnKeyDown(hWnd, wParam, lParam))
					return true;
			}
			return false;
		}

		public static bool OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            IMessageHandlerClient[] messageClients = GetMessageClients();
			foreach(IMessageHandlerClient client in messageClients)
			{
				if(!IsInvokeRequired(client) && client.OnMouseDown(hWnd,wParam,lParam))
					return true;
			}
			return false;
		}

        private static bool IsInvokeRequired(IMessageHandlerClient client)
        {
            DotNetBarManager manager = client as DotNetBarManager;
            if (manager != null && manager.ParentForm != null && manager.ParentForm.InvokeRequired)
                return true;
            else if (client is Control && ((Control)client).InvokeRequired)
                return true;

            return false;
        }

		public static bool OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            IMessageHandlerClient[] messageClients = GetMessageClients();
			foreach(IMessageHandlerClient client in messageClients)
			{
                if(!IsInvokeRequired(client) && client.OnMouseMove(hWnd, wParam, lParam))
				    return true;
			}
			return false;
		}

        public static bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            IMessageHandlerClient modalClient = MessageHandler.GetModalClient();
            if (modalClient != null)
            {
                if (!IsInvokeRequired(modalClient) && modalClient.OnMouseWheel(hWnd, wParam, lParam))
                    return true;
                return false;
            }

            IMessageHandlerClient[] messageClients = GetMessageClients();
            foreach (IMessageHandlerClient client in messageClients)
            {
                if (!IsInvokeRequired(client) && client.OnMouseWheel(hWnd, wParam, lParam))
                    return true;
            }
            return false;
        }
	}

	internal class MessageFilterWrapper:IMessageFilter
	{
		public bool PreFilterMessage(ref System.Windows.Forms.Message m)
		{
			switch(m.Msg)
			{
				case NativeFunctions.WM_SYSKEYDOWN:
				{
					return MessageHandler.OnSysKeyDown(m.HWnd,m.WParam,m.LParam);
				}
				case NativeFunctions.WM_SYSKEYUP:
				{
					return MessageHandler.OnSysKeyUp(m.HWnd,m.WParam,m.LParam);
				}
				case (int)WinApi.WindowsMessages.WM_KEYDOWN:
				{
					return MessageHandler.OnKeyDown(m.HWnd,m.WParam,m.LParam);
				}
				case NativeFunctions.WM_LBUTTONDOWN:
				case NativeFunctions.WM_NCLBUTTONDOWN:
				case NativeFunctions.WM_RBUTTONDOWN:
				case NativeFunctions.WM_MBUTTONDOWN:
				case NativeFunctions.WM_NCMBUTTONDOWN:
				case NativeFunctions.WM_NCRBUTTONDOWN:
				{
					return MessageHandler.OnMouseDown(m.HWnd,m.WParam,m.LParam);
				}
				case NativeFunctions.WM_MOUSEMOVE:
				{
					return MessageHandler.OnMouseMove(m.HWnd,m.WParam,m.LParam);
				}
                case NativeFunctions.WM_MOUSEWHEEL:
                {
                    return MessageHandler.OnMouseWheel(m.HWnd, m.WParam, m.LParam);
                }
			}
			return false;
		}

		public void PostFilterMessage(ref System.Windows.Forms.Message m)
		{
		}
	}

	internal interface IMessageHandlerClient
	{
		bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
		bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
		bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
		bool OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
		bool OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
        bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
		bool IsModal{get;}
	}
}
