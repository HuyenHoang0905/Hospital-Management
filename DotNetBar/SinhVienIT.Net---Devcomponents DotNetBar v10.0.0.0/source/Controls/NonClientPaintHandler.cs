using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Controls
{
    internal class NonClientPaintHandler : ISkinHook
    {
        #region Private Variables
        private INonClientControl m_Control = null;
        private Rectangle m_ClientRectangle = Rectangle.Empty;
        private ScrollBarCore m_VScrollBar = null;
        private ScrollBarCore m_HScrollBar = null;
        private const int HC_ACTION = 0;
        private eScrollBarSkin m_SkinScrollbars = eScrollBarSkin.Optimized;
        private bool m_SuspendPaint = false;
        private bool m_IsVista = false;
        private bool m_IsListView = false;
        private bool m_IsAppScrollBarStyle = false;
        #endregion

        #region Events
        public event CustomNCPaintEventHandler BeforeBorderPaint;
        public event CustomNCPaintEventHandler AfterBorderPaint;
        #endregion

        #region Internal Implementation
        public bool IsAppScrollBarStyle
        {
            get { return m_IsAppScrollBarStyle; }
            set { m_IsAppScrollBarStyle = value; }
        }

        public NonClientPaintHandler(INonClientControl c, eScrollBarSkin skinScrollbars)
        {
            m_IsVista = System.Environment.OSVersion.Version.Major >= 6;
//#if DEBUG
//            m_IsVista = false;
//#endif
            m_SkinScrollbars = skinScrollbars;
            m_Control = c;
            if (m_Control is Control)
            {
                ((Control)m_Control).HandleCreated += new EventHandler(Control_HandleCreated);
                ((Control)m_Control).HandleDestroyed += new EventHandler(Control_HandleDestroyed);
            }
            m_IsListView = m_Control is ListView;
            if (ShouldSkinScrollbars)
            {
                CreateScrollbars();
            }
            if (m_Control.IsHandleCreated && ShouldSkinScrollbars)
                RegisterHook();
        }

        private bool ShouldSkinScrollbars
        {
            get { return m_SkinScrollbars != eScrollBarSkin.None && !m_IsVista; }
        }

        public void Dispose()
        {
            UnRegisterHook();
        }

        private void CreateScrollbars()
        {
            m_VScrollBar = new ScrollBarCore(m_Control as Control, true);
            m_VScrollBar.IsAppScrollBarStyle = m_IsAppScrollBarStyle;
            m_HScrollBar = new ScrollBarCore(m_Control as Control, true);
            m_HScrollBar.IsAppScrollBarStyle = m_IsAppScrollBarStyle;
            m_HScrollBar.Orientation = eOrientation.Horizontal;
            m_HScrollBar.Enabled = false;
        }

        public bool SuspendPaint
        {
            get { return m_SuspendPaint; }
            set { m_SuspendPaint = value; }
        }

        public eScrollBarSkin SkinScrollbars
        {
            get { return m_SkinScrollbars; }
            set
            {
                if (m_SkinScrollbars != value)
                {
                    if (m_SkinScrollbars != eScrollBarSkin.None)
                    {
                        if (m_VScrollBar != null)
                        {
                            m_VScrollBar.Dispose();
                            m_VScrollBar = null;
                        }
                        if (m_HScrollBar != null)
                        {
                            m_HScrollBar.Dispose();
                            m_HScrollBar = null;
                        }
                        UnRegisterHook();
                    }

                    m_SkinScrollbars = value;
                    if (ShouldSkinScrollbars)
                    {
                        CreateScrollbars();
                        if(m_Control.IsHandleCreated)
                            RegisterHook();
                    }
                }
            }
        }

        private void Control_HandleDestroyed(object sender, EventArgs e)
        {
            UnRegisterHook();
        }

        private void Control_HandleCreated(object sender, EventArgs e)
        {
            if (ShouldSkinScrollbars)
                RegisterHook();
        }

        private void RegisterHook()
        {
            UpdateScrollValues();
            NonClientHook.RegisterHook(this);
        }

        private void UnRegisterHook()
        {
            if (ShouldSkinScrollbars)
                NonClientHook.UnregisterHook(this);
        }

        public Rectangle ClientRectangle
        {
            get { return m_ClientRectangle; }
        }

        public virtual bool WndProc(ref Message m)
        {
            bool callBase = true;
            
            switch (m.Msg)
            {
                case (int)WinApi.WindowsMessages.WM_NCPAINT:
                    {
                        callBase = WindowsMessageNCPaint(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_CTLCOLORSCROLLBAR:
                    {
                        PaintNonClientAreaBuffered();
                        callBase = false;
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_NCCALCSIZE:
                    {
                        callBase = WindowsMessageNCCalcSize(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_VSCROLL:
                    {
                        callBase = WindowsMessageVScroll(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_HSCROLL:
                    {
                        callBase = WindowsMessageHScroll(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.SBM_SETPOS:
                    {
                        callBase = WindowsMessageSbmSetPos(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_ERASEBKGND:
                    {
                        callBase = WindowsMessageEraseBkgnd(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.EM_GETMODIFY:
                    {
                        callBase = WindowsMessageEmGetModify(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_NCLBUTTONDOWN:
                    {
                        callBase = WindowsMessageNcLButtonDown(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_NCLBUTTONUP:
                    {
                        callBase = WindowsMessageNcLButtonUp(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_NCMOUSELEAVE:
                    {
                        callBase = WindowsMessageNcMouseLeave(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_NCMOUSEMOVE:
                    {
                        callBase = WindowsMessageNcMouseMove(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_MOUSEWHEEL:
                    {
                        callBase = WindowsMessageMouseWheel(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_SIZE:
                    {
                        callBase = WindowsMessageSize(ref m);
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_COMMAND:
                    {
                        if (WinApi.HIWORD(m.WParam) == 0x100)
                        {
                            CallBaseWndProcAndPaint(ref m);
                            callBase = false;
                        }
                        break;
                    }
                case (int)WinApi.WindowsMessages.WM_CTLCOLORBTN:
                    {
                        CallBaseWndProcAndPaint(ref m);
                        callBase = false;
                        break;
                    }
                case 0x120:
                    {
                        WindowsMessageCustomNotify(ref m);
                        callBase = false;
                        break;
                    }
            }

            return callBase;
        }

        private bool WindowsMessageSize(ref Message m)
        {
            if (m_IsListView)
            {
                CallBaseWndProcAndPaint(ref m);
                return false;
            }

            return true;
        }

        private void WindowsMessageCustomNotify(ref Message m)
        {
            if (m.WParam.ToInt32() == 0)
            {
                PaintNonClientAreaBuffered();
            }
            else if (m.WParam.ToInt32() == 1)
            {
                Point p = ScreenToNonClientPoint(Control.MousePosition);
                if (m_VScrollBar.Visible)
                {
                    if (m_VScrollBar.IsMouseDown)
                        m_VScrollBar.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
                    else if (!m_VScrollBar.DisplayRectangle.Contains(p))
                        m_VScrollBar.MouseLeave();
                }

                if (m_HScrollBar.Visible)
                {
                    if (m_HScrollBar.IsMouseDown)
                        m_HScrollBar.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
                    else if (!m_HScrollBar.DisplayRectangle.Contains(p))
                        m_HScrollBar.MouseLeave();
                }

                PaintNonClientAreaBuffered();
            }
        }

        private bool WindowsMessageMouseWheel(ref Message m)
        {
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageNcMouseLeave(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            if(m_VScrollBar.Visible)
                m_VScrollBar.MouseLeave();
            if(m_HScrollBar.Visible)
                m_HScrollBar.MouseLeave();
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageNcMouseMove(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            Point p = ScreenToNonClientPoint(new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam)));
            if (m_VScrollBar.Visible)
                m_VScrollBar.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, p.X, p.Y, 0));
            if(m_HScrollBar.Visible)
                m_HScrollBar.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, p.X, p.Y, 0));
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageNcLButtonUp(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            Point p = ScreenToNonClientPoint(new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam)));
            if(m_VScrollBar.Visible)
                m_VScrollBar.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
            if (m_HScrollBar.Visible)
                m_HScrollBar.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageNcLButtonDown(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            Point p = ScreenToNonClientPoint(new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam)));
            if(m_VScrollBar.Visible)
                m_VScrollBar.MouseDown(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
            if (m_HScrollBar.Visible)
                m_HScrollBar.MouseDown(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageEmGetModify(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private bool WindowsMessageEraseBkgnd(ref Message m)
        {
            if (!m_IsListView || !ShouldSkinScrollbars) return true;
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private void WindowsMessageMouseMove(IntPtr hWnd, Point mousePosition)
        {
            if (Control.MouseButtons == MouseButtons.Left && hWnd == m_Control.Handle && ShouldSkinScrollbars)
            {
                WinApi.PostMessage(hWnd.ToInt32(), 0x120, 0, 0);
            }
        }

        private bool WindowsMessageSbmSetPos(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            CallBaseWndProcAndPaint(ref m);
            return false;
        }

        private void CallBaseWndProcAndPaintWndStyle(ref Message m)
        {
            const int WS_VISIBLE = 0x10000000;
            IntPtr style = WinApi.GetWindowLongPtr(m_Control.Handle, (int)WinApi.GWL.GWL_STYLE);
            int newStyle = style.ToInt32();
            newStyle = newStyle & ~(newStyle & WS_VISIBLE);
            WinApi.SetWindowLong(m_Control.Handle, (int)WinApi.GWL.GWL_STYLE, newStyle);
            //PaintUnBuffered();
            using (BufferedBitmap bmp = GetNonClientAreaBitmap())
            {
                ((INonClientControl)m_Control).BaseWndProc(ref m);
                WinApi.SetWindowLong(m_Control.Handle, (int)WinApi.GWL.GWL_STYLE, style.ToInt32());
                RenderNonClientAreaBitmap(bmp);
            }
            if (m_Control is Control)
                ((Control)m_Control).Invalidate(true);
        }

        private bool WindowsMessageVScroll(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;

            if (m_SkinScrollbars == eScrollBarSkin.Optimized)
                CallBaseWndProcAndPaintWndStyle(ref m);
            else
                CallBaseWndProcAndPaint(ref m);

            return false;
        }

        private bool WindowsMessageHScroll(ref Message m)
        {
            if (!ShouldSkinScrollbars) return true;
            if (m_SkinScrollbars == eScrollBarSkin.Optimized)
                CallBaseWndProcAndPaintWndStyle(ref m);
            else
                CallBaseWndProcAndPaint(ref m);
            return false;
        }
        
        private void CallBaseWndProcAndPaint(ref Message m)
        {
            //PaintUnBuffered();
            using (BufferedBitmap bmp = GetNonClientAreaBitmap())
            {
                ((INonClientControl)m_Control).BaseWndProc(ref m);
                RenderNonClientAreaBitmap(bmp);
            }
        }

        /// <summary>
        /// Calculates the size of non-client area of the control.
        /// </summary>
        protected virtual bool WindowsMessageNCCalcSize(ref Message m)
        {
            ElementStyle style = ((INonClientControl)m_Control).BorderStyle;
            if (style != null && style.PaintBorder)
            {
                ((INonClientControl)m_Control).BaseWndProc(ref m);
                if (m.WParam == IntPtr.Zero)
                {
                    WinApi.RECT r = (WinApi.RECT)Marshal.PtrToStructure(m.LParam, typeof(WinApi.RECT));
                    Rectangle rc = GetClientRectangleForBorderStyle(r.ToRectangle(), style);
                    WinApi.RECT newClientRect = WinApi.RECT.FromRectangle(rc);
                    Marshal.StructureToPtr(newClientRect, m.LParam, false);

                    // Store the client rectangle in Window coordinates non-client coordinates
                    m_ClientRectangle = rc;
                    m_ClientRectangle.X = m_ClientRectangle.X - r.Left;
                    m_ClientRectangle.Y = m_ClientRectangle.Y - r.Top;
                }
                else
                {
                    WinApi.NCCALCSIZE_PARAMS csp;
                    csp = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(WinApi.NCCALCSIZE_PARAMS));

                    WinApi.WINDOWPOS pos = (WinApi.WINDOWPOS)Marshal.PtrToStructure(csp.lppos, typeof(WinApi.WINDOWPOS));
                    WinApi.RECT rgrc0 = csp.rgrc0;
                    WinApi.RECT newClientRect = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(rgrc0.Left, rgrc0.Top, rgrc0.Width, rgrc0.Height), style));
                    csp.rgrc0 = newClientRect;
                    csp.rgrc1 = newClientRect;
                    Marshal.StructureToPtr(csp, m.LParam, false);

                    // Store the client rectangle in Window coordinates non-client coordinates
                    m_ClientRectangle = csp.rgrc0.ToRectangle();
                    m_ClientRectangle.X = m_ClientRectangle.X - pos.x;
                    m_ClientRectangle.Y = m_ClientRectangle.Y - pos.y;
                }
            }
            else
            {
                // Take client rectangle directly
                if (m.WParam == IntPtr.Zero)
                {
                    WinApi.RECT rWindow = (WinApi.RECT)Marshal.PtrToStructure(m.LParam, typeof(WinApi.RECT));
                    ((INonClientControl)m_Control).BaseWndProc(ref m);
                    WinApi.RECT r = (WinApi.RECT)Marshal.PtrToStructure(m.LParam, typeof(WinApi.RECT));
                    WinApi.RECT newClientRect = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(r.Left, r.Top, r.Width, r.Height), style));
                    Marshal.StructureToPtr(newClientRect, m.LParam, false);

                    // Store the client rectangle in Window coordinates non-client coordinates
                    m_ClientRectangle = newClientRect.ToRectangle();
                    m_ClientRectangle.X = m_ClientRectangle.X - rWindow.Left;
                    m_ClientRectangle.Y = m_ClientRectangle.Y - rWindow.Top;
                }
                else
                {
                    ((INonClientControl)m_Control).BaseWndProc(ref m);

                    WinApi.NCCALCSIZE_PARAMS csp;
                    csp = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(WinApi.NCCALCSIZE_PARAMS));
                    WinApi.WINDOWPOS pos = (WinApi.WINDOWPOS)Marshal.PtrToStructure(csp.lppos, typeof(WinApi.WINDOWPOS));
                    WinApi.RECT rgrc0 = csp.rgrc0;
                    WinApi.RECT newClientRect = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(rgrc0.Left, rgrc0.Top, rgrc0.Width, rgrc0.Height), style));
                    csp.rgrc0 = newClientRect;
                    Marshal.StructureToPtr(csp, m.LParam, false);

                    // Store the client rectangle in Window coordinates non-client coordinates
                    m_ClientRectangle = csp.rgrc0.ToRectangle();
                    m_ClientRectangle.X = m_ClientRectangle.X - pos.x;
                    m_ClientRectangle.Y = m_ClientRectangle.Y - pos.y;
                }
            }
            return false;
        }

        internal int GetLeftBorderWidth(ElementStyle style)
        {
            if (style.PaintLeftBorder)
            {
                int w = style.BorderLeftWidth;
                if (style.CornerTypeTopLeft == eCornerType.Rounded || style.CornerTypeTopLeft == eCornerType.Diagonal
                    || style.CornerTypeBottomLeft == eCornerType.Rounded || style.CornerTypeBottomLeft == eCornerType.Diagonal ||
                    (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) &&
                    (style.CornerTypeTopLeft == eCornerType.Inherit || style.CornerTypeBottomLeft == eCornerType.Inherit))
                    w = Math.Max(w, style.CornerDiameter / 2 + 1);
                return w;
            }
            return 0;
        }

        internal int GetTopBorderWidth(ElementStyle style)
        {
            if (style.PaintTopBorder)
            {
                int w = style.BorderTopWidth;
                if (style.CornerTypeTopLeft == eCornerType.Rounded || style.CornerTypeTopLeft == eCornerType.Diagonal
                    || style.CornerTypeTopRight == eCornerType.Rounded || style.CornerTypeTopRight == eCornerType.Diagonal ||
                    (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) &&
                    (style.CornerTypeTopLeft == eCornerType.Inherit || style.CornerTypeTopRight == eCornerType.Inherit))
                    w = Math.Max(w, style.CornerDiameter / 2 + 1);
                return w;
            }
            return 0;
        }

        internal int GetRightBorderWidth(ElementStyle style)
        {
            if (style.PaintRightBorder)
            {
                int w = style.BorderRightWidth;
                if (style.CornerTypeTopRight == eCornerType.Rounded || style.CornerTypeBottomRight == eCornerType.Rounded
                    || style.CornerTypeTopRight == eCornerType.Diagonal || style.CornerTypeBottomRight == eCornerType.Diagonal ||
                    (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) &&
                    (style.CornerTypeTopRight == eCornerType.Inherit || style.CornerTypeBottomRight == eCornerType.Inherit))
                    w = Math.Max(w, style.CornerDiameter / 2 + 1);
                return w;
            }
            return 0;
        }

        internal int GetBottomBorderWidth(ElementStyle style)
        {
            if (style.PaintBottomBorder)
            {
                int w = style.BorderBottomWidth;
                if (style.CornerTypeBottomLeft == eCornerType.Rounded || style.CornerTypeBottomRight == eCornerType.Rounded ||
                    style.CornerTypeBottomLeft == eCornerType.Diagonal || style.CornerTypeBottomRight == eCornerType.Diagonal ||
                    (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) &&
                    (style.CornerTypeBottomLeft == eCornerType.Inherit || style.CornerTypeBottomRight == eCornerType.Inherit))
                    w = Math.Max(w, style.CornerDiameter / 2 + 1);
                return w;
            }
            return 0;
        }

        private Rectangle GetClientRectangleForBorderStyle(Rectangle rect, ElementStyle style)
        {
            if (style == null)
                return rect;

            if (style.PaintLeftBorder)
            {
                int w = GetLeftBorderWidth(style);
                rect.X += w;
                rect.Width -= w;
            }
            if (style.PaintTopBorder)
            {
                int w = GetTopBorderWidth(style);
                rect.Y += w;
                rect.Height -= w;
            }
            if (style.PaintRightBorder)
            {
                int w = GetRightBorderWidth(style);
                rect.Width -= w;
            }
            if (style.PaintBottomBorder)
            {
                int w = GetBottomBorderWidth(style);
                rect.Height -= w;
            }

            TextBox tb= m_Control as TextBox;
            if (tb!=null && tb.Multiline && tb.ScrollBars != ScrollBars.None)
            {
                if (tb.ScrollBars == ScrollBars.Vertical || tb.ScrollBars == ScrollBars.Both)
                {
                    if (tb.RightToLeft == RightToLeft.Yes)
                        rect.Width -= style.PaddingRight;
                    else
                    {
                        rect.X += style.PaddingLeft;
                        rect.Width -= style.PaddingLeft;
                    }
                }
                
                return rect;
            }

            rect.X += style.PaddingLeft;
            rect.Width -= style.PaddingLeft + style.PaddingRight;
            rect.Y += style.PaddingTop;
            rect.Height -= style.PaddingTop + ((m_Control is TextBox)? 0 : style.PaddingBottom);

            m_Control.AdjustClientRectangle(ref rect);

            return rect;
        }

		private bool m_Painting = false;
        /// <summary>
        /// Paints the non-client area of the control.
        /// </summary>
        protected virtual bool WindowsMessageNCPaint(ref Message m)
        {
			if (!ShouldSkinScrollbars)
			{
				((INonClientControl)m_Control).BaseWndProc(ref m);
				if (m_Control.IsHandleCreated && m_Control.Handle != IntPtr.Zero)
					PaintNonClientAreaBuffered();
				return false;
			}
            //PaintUnBuffered();
            using (BufferedBitmap bmp = GetNonClientAreaBitmap())
            {
                ((INonClientControl)m_Control).BaseWndProc(ref m);
                RenderNonClientAreaBitmap(bmp);
            }
			m.Result = IntPtr.Zero;

			//if (m_Control.IsHandleCreated && m_Control.Handle != IntPtr.Zero)
			//{
			//    PaintNonClientAreaBuffered();
			//}

			return false;
        }

        /// <summary>
        /// Draws the non-client area buffered.
        /// </summary>
        public void PaintNonClientAreaBuffered()
        {
            //PaintUnBuffered();

            if (m_Control.Width <= 0 || m_Control.Height <= 0 || m_SuspendPaint) return;

            using (BufferedBitmap bmp = GetNonClientAreaBitmap())
            {
                RenderNonClientAreaBitmap(bmp);
            }
        }

        //private Bitmap GetNonClientAreaBitmap()
        //{
        //    if (m_Control.Width <= 0 || m_Control.Height <= 0 || m_SuspendPaint || !m_Control.IsHandleCreated) return null;
        //    Bitmap bmp = new Bitmap(m_Control.Width, m_Control.Height);
        //    using (Graphics g = Graphics.FromImage(bmp))
        //    {
        //        PaintNonClientArea(g);
        //    }
        //    return bmp;
        //}

        private BufferedBitmap GetNonClientAreaBitmap()
        {
            if (m_Control.Width <= 0 || m_Control.Height <= 0 || m_SuspendPaint || !m_Control.IsHandleCreated) return null;
            BufferedBitmap bmp = null;
            IntPtr hdc = WinApi.GetWindowDC(m_Control.Handle);
            Rectangle r = GetControlRectangle();
            try
            {
                bmp = new BufferedBitmap(hdc, new Rectangle(0, 0, r.Width, r.Height));
                PaintNonClientArea(bmp.Graphics);
            }
            finally
            {
                WinApi.ReleaseDC(m_Control.Handle, hdc);
            }
            return bmp;
        }

        //private void PaintUnBuffered()
        //{
        //    IntPtr dc = WinApi.GetWindowDC(m_Control.Handle);
        //    try
        //    {
        //        using (Graphics g = Graphics.FromHdc(dc))
        //        {
        //            g.SetClip(GetClientRectangle(), CombineMode.Exclude);
        //            if (!ShouldSkinScrollbars)
        //            {
        //                WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
        //                psbi.cbSize = Marshal.SizeOf(psbi);
        //                WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_VSCROLL, ref psbi);
        //                if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //                {
        //                    Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                    g.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //                }
        //                psbi = new WinApi.SCROLLBARINFO();
        //                psbi.cbSize = Marshal.SizeOf(psbi);
        //                WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_HSCROLL, ref psbi);
        //                if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //                {
        //                    Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                    g.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //                }
        //            }

        //            PaintNonClientArea(g);
                    
        //            g.ResetClip();
        //        }
        //    }
        //    finally
        //    {
        //        WinApi.ReleaseDC(m_Control.Handle, dc);
        //    }
        //}
        private void RenderNonClientAreaBitmap(BufferedBitmap bmp)
        {
            if (bmp == null) return;
            IntPtr dc = WinApi.GetWindowDC(m_Control.Handle);
            try
            {
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    Rectangle[] clips = new Rectangle[3];
                    clips[0] = GetClientRectangle();
                    
                    if (!ShouldSkinScrollbars)
                    {
                        WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
                        psbi.cbSize = Marshal.SizeOf(psbi);
                        WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_VSCROLL, ref psbi);
                        if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
                        {
                            Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
                            clips[1] = rvs;
                        }
                        psbi = new WinApi.SCROLLBARINFO();
                        psbi.cbSize = Marshal.SizeOf(psbi);
                        WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_HSCROLL, ref psbi);
                        if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
                        {
                            Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
                            clips[2] = rvs;
                        }
                    }

                    bmp.Render(g, clips);
                }
            }
            finally
            {
                WinApi.ReleaseDC(m_Control.Handle, dc);
            }
        }
        //private void RenderNonClientAreaBitmap(Bitmap bmp)
        //{
        //    if (bmp == null) return;
        //    IntPtr dc = WinApi.GetWindowDC(m_Control.Handle);
        //    try
        //    {
        //        using (Graphics g = Graphics.FromHdc(dc))
        //        {
        //            g.SetClip(GetClientRectangle(), CombineMode.Exclude);
        //            if (!ShouldSkinScrollbars)
        //            {
        //                WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
        //                psbi.cbSize = Marshal.SizeOf(psbi);
        //                WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_VSCROLL, ref psbi);
        //                if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //                {
        //                    Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                    g.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //                }
        //                psbi = new WinApi.SCROLLBARINFO();
        //                psbi.cbSize = Marshal.SizeOf(psbi);
        //                WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_HSCROLL, ref psbi);
        //                if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //                {
        //                    Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                    g.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //                }
        //            }
	                
        //            g.DrawImageUnscaled(bmp, 0, 0);
        //            g.ResetClip();
        //        }
        //    }
        //    finally
        //    {
        //        WinApi.ReleaseDC(m_Control.Handle, dc);
        //    }
        //}

        //protected virtual void PaintNonClientAreaBuffered(Graphics targetGraphics)
        //{
        //    Bitmap bmp = new Bitmap(m_Control.Width, m_Control.Height);
            
        //    try
        //    {
        //        using (Graphics g = Graphics.FromImage(bmp))
        //        {
        //            PaintNonClientArea(g);
        //        }

        //        targetGraphics.SetClip(GetClientRectangle(), CombineMode.Exclude);
        //        if (!m_SkinScrollbars)
        //        {
        //            WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
        //            psbi.cbSize = Marshal.SizeOf(psbi);
        //            WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_VSCROLL, ref psbi);
        //            if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //            {
        //                Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                targetGraphics.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //            }
        //            psbi = new WinApi.SCROLLBARINFO();
        //            psbi.cbSize = Marshal.SizeOf(psbi);
        //            WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_HSCROLL, ref psbi);
        //            if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
        //            {
        //                Rectangle rvs = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
        //                targetGraphics.SetClip(rvs, System.Drawing.Drawing2D.CombineMode.Exclude);
        //            }
        //        }

        //        targetGraphics.DrawImageUnscaled(bmp, 0, 0);
        //        targetGraphics.ResetClip();
        //    }
        //    finally
        //    {
        //        bmp.Dispose();
        //    }
        //}

        private Rectangle GetClientRectangle()
        {
            return m_ClientRectangle;
        }

        public void UpdateScrollValues()
        {
            if (!ShouldSkinScrollbars) return;

            WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
            psbi.cbSize = Marshal.SizeOf(psbi);
            WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_VSCROLL, ref psbi);
            if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
            {
                Rectangle displayRect = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
                if (m_VScrollBar.DisplayRectangle != displayRect)
                    m_VScrollBar.DisplayRectangle = displayRect;

                Rectangle thumbRect = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, psbi.dxyLineButton);
                if (m_VScrollBar.ThumbDecreaseRectangle != thumbRect)
                    m_VScrollBar.ThumbDecreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X, displayRect.Bottom - psbi.dxyLineButton, displayRect.Width, psbi.dxyLineButton);
                if (m_VScrollBar.ThumbIncreaseRectangle != thumbRect)
                    m_VScrollBar.ThumbIncreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X, displayRect.Y + psbi.xyThumbTop, displayRect.Width, psbi.xyThumbBottom - psbi.xyThumbTop);
                if (m_VScrollBar.TrackRectangle != thumbRect)
                    m_VScrollBar.TrackRectangle = thumbRect;
                if (psbi.rgstate[0] == (int)WinApi.eStateFlags.STATE_SYSTEM_UNAVAILABLE)
                {
                    if (m_VScrollBar.Enabled) m_VScrollBar.Enabled = false;
                }
                else if (!m_VScrollBar.Enabled)
                    m_VScrollBar.Enabled = true;
                m_VScrollBar.Visible = true;
            }
            else
                m_VScrollBar.Visible = false;


            // Horizontal scroll bar
            psbi = new WinApi.SCROLLBARINFO();
            psbi.cbSize = Marshal.SizeOf(psbi);
            WinApi.GetScrollBarInfo(m_Control.Handle, (uint)WinApi.eObjectId.OBJID_HSCROLL, ref psbi);
            if (psbi.rgstate[0] != (int)WinApi.eStateFlags.STATE_SYSTEM_INVISIBLE)
            {
                Rectangle displayRect = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
                if (m_HScrollBar.DisplayRectangle != displayRect)
                    m_HScrollBar.DisplayRectangle = displayRect;

                Rectangle thumbRect = new Rectangle(displayRect.X, displayRect.Y, psbi.dxyLineButton, displayRect.Height);
                if (m_HScrollBar.ThumbDecreaseRectangle != thumbRect)
                    m_HScrollBar.ThumbDecreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.Right - psbi.dxyLineButton, displayRect.Y, psbi.dxyLineButton, displayRect.Height);
                if (m_HScrollBar.ThumbIncreaseRectangle != thumbRect)
                    m_HScrollBar.ThumbIncreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X + psbi.xyThumbTop, displayRect.Y, psbi.xyThumbBottom - psbi.xyThumbTop, displayRect.Height);
                if (m_HScrollBar.TrackRectangle != thumbRect)
                    m_HScrollBar.TrackRectangle = thumbRect;
                if (psbi.rgstate[0] == (int)WinApi.eStateFlags.STATE_SYSTEM_UNAVAILABLE)
                {
                    if (m_HScrollBar.Enabled) m_VScrollBar.Enabled = false;
                }
                else if (!m_HScrollBar.Enabled)
                    m_HScrollBar.Enabled = true;
                m_HScrollBar.Visible = true;
            }
            else
                m_HScrollBar.Visible = false;
        }

        protected virtual void PaintNonClientArea(Graphics g)
        {
			if(m_Painting) return;
			m_Painting=true;
			try
			{
				// Paint Background and border
				PaintBorder(g);

				if (ShouldSkinScrollbars)
				{
					UpdateScrollValues();

					if (m_VScrollBar.Visible)
						m_VScrollBar.Paint(((INonClientControl)m_Control).GetItemPaintArgs(g));
					if (m_HScrollBar.Visible)
						m_HScrollBar.Paint(((INonClientControl)m_Control).GetItemPaintArgs(g));
				}
			}
			finally
			{
				m_Painting=false;
			}
        }

        private Rectangle GetControlRectangle()
        {
            if (m_Control.IsHandleCreated)
            {
                WinApi.RECT r = new WinApi.RECT();
                WinApi.GetWindowRect(m_Control.Handle, ref r);
                return new Rectangle(0, 0, r.Width, r.Height);
            }
            else
                return new Rectangle(0, 0, m_Control.Width, m_Control.Height);
        }

        private void PaintBorder(Graphics g)
        {
            ElementStyle style = ((INonClientControl)m_Control).BorderStyle;
            Rectangle r = GetControlRectangle();
            if (style == null || r.Width <= 0 || r.Height <= 0) return;
            
            ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, g, r);

            if (style.BackColor == Color.Transparent && (style.BackColor2.IsEmpty || style.BackColor2 == Color.Transparent))
            {
                ((INonClientControl)m_Control).PaintBackground(new PaintEventArgs(g, r));
            }
            else
            {
                if (style.BackColor.IsEmpty && m_Control.BackColor != Color.Transparent)
                {
                    using (SolidBrush brush = new SolidBrush((m_Control.Enabled ? m_Control.BackColor : SystemColors.Control)))
                        g.FillRectangle(brush, r);
                }
                else
                {
                    if (m_Control.BackColor == Color.Transparent || style.PaintBorder && (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal ||
                        style.CornerTypeBottomLeft == eCornerType.Rounded || style.CornerTypeBottomLeft == eCornerType.Diagonal ||
                        style.CornerTypeBottomRight == eCornerType.Rounded || style.CornerTypeBottomRight == eCornerType.Diagonal ||
                        style.CornerTypeTopLeft == eCornerType.Rounded || style.CornerTypeTopLeft == eCornerType.Diagonal ||
                        style.CornerTypeTopRight == eCornerType.Rounded || style.CornerTypeTopRight == eCornerType.Diagonal))
                    {
                        if (m_Control is TextBox || m_Control.BackColor == Color.Transparent)
                            ((INonClientControl)m_Control).PaintBackground(new PaintEventArgs(g, r));
                        else
                            using (SolidBrush brush = new SolidBrush(m_Control.BackColor))
                                g.FillRectangle(brush, r);
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(m_Control.BackColor))
                            g.FillRectangle(brush, r);
                    }
                }

                Rectangle rback = r;
                if (style.PaintBorder)
                {
                    if (style.PaintRightBorder && style.BorderRightWidth > 1)
                        rback.Width--;
                    if (style.PaintBottomBorder && style.BorderBottomWidth > 1)
                        rback.Height--;
                }
                m_Control.AdjustBorderRectangle(ref rback);
                displayInfo.Bounds = rback;
                ElementStyleDisplay.PaintBackground(displayInfo);
                m_Control.RenderNonClient(g);
            }

            SmoothingMode sm = g.SmoothingMode;
            if (style.PaintBorder && (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal))
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            m_Control.AdjustBorderRectangle(ref r);
            displayInfo.Bounds = r;
            this.BeforePaintBorder(g, r);
            ElementStyleDisplay.PaintBorder(displayInfo);
            g.SmoothingMode = sm;
            this.AfterPaintBorder(g, r);
        }

        private void AfterPaintBorder(Graphics g, Rectangle r)
        {
            if (AfterBorderPaint != null)
                AfterBorderPaint(this, new CustomNCPaintEventArgs(g, r));
        }

        private void BeforePaintBorder(Graphics g, Rectangle r)
        {
            if(BeforeBorderPaint!=null)
                BeforeBorderPaint(this, new CustomNCPaintEventArgs(g, r));
        }

        private Point ScreenToNonClientPoint(Point p)
        {
            Point pScreen = m_Control.PointToScreen(new Point(-m_ClientRectangle.X, -m_ClientRectangle.Y));
            p.X = p.X - pScreen.X;
            p.Y = p.Y - pScreen.Y;
            return p;
        }

        private Rectangle ScreenToNonClientRectangle(Rectangle r)
        {
            Point p = m_Control.PointToScreen(new Point(-m_ClientRectangle.X, -m_ClientRectangle.Y));
            r.X = r.X - p.X;
            r.Y = r.Y - p.Y;

            return r;
        }
        #endregion



        #region ISkinHook Members
        public delegate void InvokeMouseMethodDelegate(IntPtr handle, Point mousePos);

        public void PostMouseMove(IntPtr hWnd, Point mousePos)
        {
            if (m_Control is Control)
            {
                Control c = m_Control as Control;
                if (c == null || c.IsDisposed || !c.IsHandleCreated)
                    return;

                if (c.InvokeRequired)
                {
#if FRAMEWORK20
                    c.BeginInvoke(new InvokeMouseMethodDelegate(this.PostMouseMoveSafe), hWnd, mousePos);
#else
				c.BeginInvoke(new InvokeMouseMethodDelegate(this.PostMouseMoveSafe), new object[]{hWnd, mousePos});
#endif
                }
                else
                    this.PostMouseMoveSafe(hWnd, mousePos);
            }
            else
                PostMouseMoveSafe(hWnd, mousePos);
        }
        private void PostMouseMoveSafe(IntPtr hWnd, Point mousePos)
        {
            if (hWnd == m_Control.Handle)
                WindowsMessageMouseMove(hWnd, mousePos);
        }

        public void PostMouseUp(IntPtr hWnd, Point mousePos)
        {
            if (m_Control is Control)
            {
                Control c = m_Control as Control;
                if (c == null || c.IsDisposed || !c.IsHandleCreated)
                    return;

                if (c.InvokeRequired)
                {
#if FRAMEWORK20
                    c.BeginInvoke(new InvokeMouseMethodDelegate(this.PostMouseUpSafe), hWnd, mousePos);
#else
				c.BeginInvoke(new InvokeMouseMethodDelegate(this.PostMouseUpSafe), new object[] {hWnd, mousePos});
#endif
                }
                else
                {
                    this.PostMouseUpSafe(hWnd, mousePos);
                }
            }
            else
                PostMouseUpSafe(hWnd, mousePos);
        }

        public void PostMouseUpSafe(IntPtr hWnd, Point mousePos)
        {
            if (hWnd == m_Control.Handle && ShouldSkinScrollbars)
            {
                if (m_VScrollBar.IsMouseDown)
                    WinApi.PostMessage(m_Control.Handle, 0x120, (IntPtr)1, (IntPtr)0);
                else if (m_HScrollBar.IsMouseDown)
                    WinApi.PostMessage(m_Control.Handle, 0x120, (IntPtr)1, (IntPtr)0);
            }
        }
        #endregion
    }

    internal delegate void CustomNCPaintEventHandler(object sender, CustomNCPaintEventArgs e);
    internal class CustomNCPaintEventArgs : EventArgs
    {
        public Graphics Graphics = null;
        public Rectangle Bounds = Rectangle.Empty;

        public CustomNCPaintEventArgs(Graphics g, Rectangle r)
        {
            this.Graphics = g;
            this.Bounds = r;
        }
    }

    public interface INonClientControl
    {
        /// <summary>
        /// Calls base WndProc implementation
        /// </summary>
        /// <param name="m">Describes Windows Message</param>
        void BaseWndProc(ref Message m);

        /// <summary>
        /// Gets the ItemPaintArgs which provide information for rendering.
        /// </summary>
        /// <param name="g">Reference to Canvas</param>
        /// <returns>Returns the new ItemPaintArgs instance</returns>
        ItemPaintArgs GetItemPaintArgs(Graphics g);

        /// <summary>
        /// Gets the reference to the BorderStyle used by the control if any.
        /// </summary>
        ElementStyle BorderStyle { get;}

        /// <summary>
        /// Paints the background of the control
        /// </summary>
        /// <param name="e">PaintEventArgs arguments</param>
        void PaintBackground(PaintEventArgs e);

        /// <summary>
        /// Returns the Windows handle associated with the control.
        /// </summary>
        IntPtr Handle { get;}

        /// <summary>
        /// Returns width of the control.
        /// </summary>
        int Width { get;}

        /// <summary>
        /// Returns the height of the control.
        /// </summary>
        int Height { get;}

        /// <summary>
        /// Returns whether handled for the control has been created.
        /// </summary>
        bool IsHandleCreated { get;}

        /// <summary>
        /// Converts the client point into the screen point.
        /// </summary>
        /// <param name="client">Client point</param>
        /// <returns>Client point converted into screen coordinates.</returns>
        Point PointToScreen(Point client);

        /// <summary>
        /// Returns background color of the control.
        /// </summary>
        Color BackColor { get;}

        /// <summary>
        /// Returns whether control is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Adjusts the client rectangle for the control.
        /// </summary>
        /// <param name="r">Reference to new client rectangle.</param>
        void AdjustClientRectangle(ref Rectangle r);

        /// <summary>
        /// Adjusts the border rectangle before the non-client border is rendered.
        /// </summary>
        /// <param name="r">Border rectangle</param>
        void AdjustBorderRectangle(ref Rectangle r);

        /// <summary>
        /// Occurs after non-client area is rendered and provides opportunity to render on top of it.
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        void RenderNonClient(Graphics g);
    }
}
