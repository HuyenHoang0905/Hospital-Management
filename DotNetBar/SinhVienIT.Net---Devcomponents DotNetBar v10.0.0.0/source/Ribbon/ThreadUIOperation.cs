using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    internal class ThreadUIOperation
    {
        #region Private Variables
        private bool m_IsActive = false;
        private System.Threading.ReaderWriterLock m_FadeImageLock = new System.Threading.ReaderWriterLock();
        private EventHandler m_UpdateUIHandler = null;
        private EventHandler m_RecordStartStateHandler = null;
        private EventHandler m_CleanupHandler = null;
        #endregion

        #region Internal Implementation
        public ThreadUIOperation(EventHandler recordStartState, EventHandler updateUIHandler, EventHandler cleanup)
        {
            m_RecordStartStateHandler = recordStartState;
            m_UpdateUIHandler = updateUIHandler;
            m_CleanupHandler = cleanup;
        }

        public bool IsActive
        {
            get { return m_IsActive; }
        }

        public void Start()
        {
            m_FadeImageLock.AcquireReaderLock(-1);
            try
            {
                if (m_IsActive)
                    return;
            }
            finally
            {
                m_FadeImageLock.ReleaseReaderLock();
            }

            bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
            if (readerLockHeld)
            {
                cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
            }
            else
            {
                m_FadeImageLock.AcquireWriterLock(-1);
            }

            try
            {
                m_IsActive = true;
                m_RecordStartStateHandler.Invoke(this, new EventArgs());
            }
            finally
            {
                if (readerLockHeld)
                {
                    m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    m_FadeImageLock.ReleaseWriterLock();
                }
            }

            FadeAnimator.Fade(this, new EventHandler(this.OnFadeChanged));
            m_IsActive = true;
        }

        public void Stop()
        {
            m_FadeImageLock.AcquireReaderLock(-1);
            try
            {
                if (!m_IsActive)
                    return;
            }
            finally
            {
                m_FadeImageLock.ReleaseReaderLock();
            }
            FadeAnimator.StopFade(this, new EventHandler(OnFadeChanged));

            bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();

            if (readerLockHeld)
            {
                cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
            }
            else
            {
                m_FadeImageLock.AcquireWriterLock(-1);
            }

            try
            {
                m_IsActive = false;
                m_CleanupHandler.Invoke(this, new EventArgs());
            }
            finally
            {
                if (readerLockHeld)
                {
                    m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    m_FadeImageLock.ReleaseWriterLock();
                }
            }
        }

        private void OnFadeChanged(object sender, EventArgs e)
        {
            m_UpdateUIHandler.Invoke(this, new EventArgs());
        }

        public void StartReadOperation()
        {
            m_FadeImageLock.AcquireReaderLock(-1);
        }

        public void EndReadOperation()
        {
            m_FadeImageLock.ReleaseReaderLock();
        }
        #endregion
    }
}
