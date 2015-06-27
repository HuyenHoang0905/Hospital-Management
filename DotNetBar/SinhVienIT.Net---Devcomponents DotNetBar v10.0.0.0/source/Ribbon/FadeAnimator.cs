using System;
using System.Text;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar
{
    internal class FadeAnimator
    {
        // Fields
        private static Thread fadeThread;
        private static ArrayList fadeInfoList;
        private static ReaderWriterLock rwImgListLock;
        [ThreadStatic]
        private static int threadWriterLockWaitCount;

        // Methods
        static FadeAnimator()
        {
            FadeAnimator.rwImgListLock = new ReaderWriterLock();
        }

        private FadeAnimator() {}

        public static void Fade(object caller, EventHandler onFadeChangedHandler)
        {
            if (caller != null)
            {
                FadeAnimator.FadeInfo fadeInfo = null;
                lock (caller)
                {
                    fadeInfo = new FadeAnimator.FadeInfo(caller);
                }
                FadeAnimator.StopFade(caller, onFadeChangedHandler);
                bool flag1 = FadeAnimator.rwImgListLock.IsReaderLockHeld;
                LockCookie cookie1 = new LockCookie();
                FadeAnimator.threadWriterLockWaitCount++;
                try
                {
                    if (flag1)
                    {
                        cookie1 = FadeAnimator.rwImgListLock.UpgradeToWriterLock(-1);
                    }
                    else
                    {
                        FadeAnimator.rwImgListLock.AcquireWriterLock(-1);
                    }
                }
                finally
                {
                    FadeAnimator.threadWriterLockWaitCount--;
                }
                try
                {
                    if (FadeAnimator.fadeInfoList == null)
                    {
                        FadeAnimator.fadeInfoList = new ArrayList();
                    }
                    fadeInfo.FadeChangedHandler = onFadeChangedHandler;
                    FadeAnimator.fadeInfoList.Add((FadeAnimator.FadeInfo)fadeInfo);
                    if (FadeAnimator.fadeThread == null)
                    {
                        FadeAnimator.fadeThread = new Thread(new ThreadStart(FadeAnimator.AnimateFade50ms));
                        FadeAnimator.fadeThread.Name = typeof(FadeAnimator).Name;
                        FadeAnimator.fadeThread.IsBackground = true;
                        FadeAnimator.fadeThread.Start();
                    }
                }
                finally
                {
                    if (flag1)
                    {
                        FadeAnimator.rwImgListLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        FadeAnimator.rwImgListLock.ReleaseWriterLock();
                    }
                }
            }
        }

        private static void AnimateFade50ms()
        {
            while (true)
            {
                FadeAnimator.rwImgListLock.AcquireReaderLock(-1);
                try
                {
                    for (int num1 = 0; num1 < FadeAnimator.fadeInfoList.Count; num1++)
                    {
                        FadeAnimator.FadeInfo info1 = FadeAnimator.fadeInfoList[num1] as FadeAnimator.FadeInfo;
                        info1.Fade();
                    }
                }
                finally
                {
                    FadeAnimator.rwImgListLock.ReleaseReaderLock();
                }
                Thread.Sleep(40);
            }
        }

        public static void StopFade(object caller, EventHandler onFrameChangedHandler)
        {
            if ((caller != null) && (FadeAnimator.fadeInfoList != null))
            {
                bool readerLockHeld = FadeAnimator.rwImgListLock.IsReaderLockHeld;
                LockCookie cookie1 = new LockCookie();
                FadeAnimator.threadWriterLockWaitCount++;
                try
                {
                    if (readerLockHeld)
                    {
                        cookie1 = FadeAnimator.rwImgListLock.UpgradeToWriterLock(-1);
                    }
                    else
                    {
                        FadeAnimator.rwImgListLock.AcquireWriterLock(-1);
                    }
                }
                finally
                {
                    FadeAnimator.threadWriterLockWaitCount--;
                }
                try
                {
                    for (int num1 = 0; num1 < FadeAnimator.fadeInfoList.Count; num1++)
                    {
                        FadeAnimator.FadeInfo info1 = FadeAnimator.fadeInfoList[num1] as FadeAnimator.FadeInfo;
                        if (caller == info1.Caller)
                        {
                            if ((onFrameChangedHandler == info1.FadeChangedHandler) || ((onFrameChangedHandler != null) && onFrameChangedHandler.Equals(info1.FadeChangedHandler)))
                            {
                                FadeAnimator.fadeInfoList.Remove((FadeAnimator.FadeInfo)info1);
                            }
                            return;
                        }
                    }
                }
                finally
                {
                    if (readerLockHeld)
                    {
                        FadeAnimator.rwImgListLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        FadeAnimator.rwImgListLock.ReleaseWriterLock();
                    }
                }
            }
        }

        // Nested Types
        private class FadeInfo
        {
            private object m_Caller;
            private EventHandler m_OnFadeChangedHandler;

            // Methods
            public FadeInfo(object caller)
            {
                this.m_Caller = caller;
            }

            protected void OnFadeChanged(EventArgs e)
            {
                if (m_OnFadeChangedHandler != null)
                {
                    m_OnFadeChangedHandler(m_Caller, e);
                }
            }

            public void Fade()
            {
                this.OnFadeChanged(EventArgs.Empty);
            }

            public EventHandler FadeChangedHandler
            {
                get
                {
                    return this.m_OnFadeChangedHandler;
                }
                set
                {
                    this.m_OnFadeChangedHandler = value;
                }
            }

            internal object Caller
            {
                get
                {
                    return m_Caller;
                }
            }

        }
    }

}
