using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    public class BufferedBitmap : IDisposable
    {
        #region Private Variables
        private Rectangle m_TargetRect = Rectangle.Empty;
        private IntPtr m_MemDC = IntPtr.Zero;
        private Graphics m_BitmapGraphics = null;
        private IntPtr m_MemDib = IntPtr.Zero;
        #endregion

        #region Internal Implementation
        public BufferedBitmap(Graphics source, Rectangle rect)
        {
            IntPtr hdc = source.GetHdc();
            try
            {
                Initialize(hdc, rect);
            }
            finally
            {
                source.ReleaseHdc(hdc);
            }
        }

        public Graphics Graphics
        {
            get
            {
                return m_BitmapGraphics;
            }
        }

        public BufferedBitmap(IntPtr hdcSource, Rectangle rect)
        {
            Initialize(hdcSource, rect);
        }
        private void Initialize(IntPtr hdcSource, Rectangle r)
        {
            m_TargetRect = r;

            m_MemDC = WinApi.CreateCompatibleDC(hdcSource);
            WinApi.BITMAPINFO info = new WinApi.BITMAPINFO();
            info.biWidth = r.Width;
            info.biHeight = r.Height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biSize = Marshal.SizeOf(info);
            m_MemDib = WinApi.CreateDIBSection(hdcSource, info, 0, 0, IntPtr.Zero, 0);
            WinApi.SelectObject(m_MemDC, m_MemDib);
            m_BitmapGraphics = Graphics.FromHdc(m_MemDC);
        }

        public void Render(Graphics targetGraphics)
        {
            Render(targetGraphics, new Rectangle[] { });
        }

        public void Render(Graphics targetGraphics, Rectangle exclude)
        {
            Render(targetGraphics, new Rectangle[] { exclude });
        }

        public void Render(Graphics targetGraphics, Rectangle[] excludeArr)
        {
            const int SRCCOPY = 0x00CC0020;
            IntPtr hdc = targetGraphics.GetHdc();
            try
            {
                if (excludeArr!=null && excludeArr.Length > 0)
                {
                    foreach (Rectangle r in excludeArr)
                    {
                        if (!r.IsEmpty)
                            WinApi.ExcludeClipRect(hdc, r.X, r.Y, r.Right, r.Bottom);
                    }
                }
                WinApi.BitBlt(hdc, m_TargetRect.Left, m_TargetRect.Top, m_TargetRect.Width, m_TargetRect.Height, m_MemDC, 0, 0, SRCCOPY);
            }
            finally
            {
                targetGraphics.ReleaseHdc(hdc);
            }
        }

        public Rectangle TargetRect
        {
            get { return m_TargetRect; }
            set { m_TargetRect = value; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_BitmapGraphics != null)
            {
                m_BitmapGraphics.Dispose();
                m_BitmapGraphics = null;
            }

            if (m_MemDib != IntPtr.Zero)
            {
                WinApi.DeleteObject(m_MemDib);
                m_MemDib = IntPtr.Zero;
            }

            if (m_MemDC != IntPtr.Zero)
            {
                WinApi.DeleteDC(m_MemDC);
                m_MemDC = IntPtr.Zero;
            }
        }

        #endregion
    }
}
