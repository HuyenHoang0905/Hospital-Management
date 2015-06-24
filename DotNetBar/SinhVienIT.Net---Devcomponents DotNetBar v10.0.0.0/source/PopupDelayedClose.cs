using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace DevComponents.DotNetBar
{
    internal class PopupDelayedClose
    {
        #region Private Variables
        private Timer m_Timer = null;
        private ArrayList m_Popups = new ArrayList();
        private System.Threading.ReaderWriterLock m_PopupsLock = new System.Threading.ReaderWriterLock();
        #endregion

        public PopupDelayedClose()
        {
            m_Timer = new Timer();
            m_Timer.Enabled = false;
            m_Timer.Interval = 800;
            m_Timer.Tick += new EventHandler(TimerDelayedClose);
        }

        void TimerDelayedClose(object sender, EventArgs e)
        {
            m_Timer.Stop();
            CloseAllPopups();
        }

        public void Dispose()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                CloseAllPopups();

                m_Timer.Dispose();
                m_Timer = null;
            }
        }

        private void CloseAllPopups()
        {
            if (m_Popups.Count > 0)
            {
                bool readerLockHeld = m_PopupsLock.IsReaderLockHeld;
                System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
                if (readerLockHeld)
                {
                    cookie1 = m_PopupsLock.UpgradeToWriterLock(-1);
                }
                else
                {
                    m_PopupsLock.AcquireWriterLock(-1);
                }

                try
                {
                    foreach (PopupItem item in m_Popups)
                        item.ClosePopup();
                    m_Popups.Clear();
                }
                finally
                {
                    if (readerLockHeld)
                    {
                        m_PopupsLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        m_PopupsLock.ReleaseWriterLock();
                    }
                }
            }
        }

        public void DelayClose(PopupItem item)
        {
            if (m_Timer == null)
                return;

            bool readerLockHeld = m_PopupsLock.IsReaderLockHeld;
            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
            if (readerLockHeld)
            {
                cookie1 = m_PopupsLock.UpgradeToWriterLock(-1);
            }
            else
            {
                m_PopupsLock.AcquireWriterLock(-1);
            }

            try
            {
                m_Popups.Add(item);
                m_Timer.Start();
            }
            finally
            {
                if (readerLockHeld)
                {
                    m_PopupsLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    m_PopupsLock.ReleaseWriterLock();
                }
            }
        }

        public void EraseDelayClose()
        {
            bool readerLockHeld = m_PopupsLock.IsReaderLockHeld;
            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
            if (readerLockHeld)
            {
                cookie1 = m_PopupsLock.UpgradeToWriterLock(-1);
            }
            else
            {
                m_PopupsLock.AcquireWriterLock(-1);
            }

            try
            {
                m_Popups.Clear();
            }
            finally
            {
                if (readerLockHeld)
                {
                    m_PopupsLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    m_PopupsLock.ReleaseWriterLock();
                }
            }
        }
    }
}
