using System;
using System.Windows.Forms;
using System.Text;
using DevComponents.DotNetBar.ScrollBar;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Controls
{
    internal class ScrollBarImplementation: IDisposable
    {
        #region Private Variables
        private System.Windows.Forms.ScrollBar m_ParentScrollBar = null;
        private ScrollBarCore m_ScrollBarCore = null;
        private bool m_FirstPaint = true;
        private int m_MouseDownTrackOffset = 0;
        private Timer m_PaintTimer = null;
        private bool m_IsVScrollBar = false;
        private IScrollBarExtender m_ParentScrollBarWndProc = null;
        private bool m_UseLockWindowUpdate = false;
        #endregion

        #region Constructor
        public ScrollBarImplementation(System.Windows.Forms.ScrollBar sb)
        {
            m_ParentScrollBar = sb;
            m_ParentScrollBarWndProc = (IScrollBarExtender)m_ParentScrollBar;
            m_IsVScrollBar = m_ParentScrollBar is VScrollBar;

            m_PaintTimer = new Timer();
            m_PaintTimer.Interval = 50;
            m_PaintTimer.Tick += new EventHandler(PaintTimerTick);

            m_ScrollBarCore = new ScrollBarCore(m_ParentScrollBar, true);
            //m_ScrollBarCore.IsAppScrollBarStyle = false;
            if (m_ParentScrollBar is HScrollBar)
                m_ScrollBarCore.Orientation = eOrientation.Horizontal;
            else
                m_ScrollBarCore.Orientation = eOrientation.Vertical;
            m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
            m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
            m_ScrollBarCore.Value = m_ParentScrollBar.Value;
        }
        #endregion

        #region Internal Implementation
        internal void OnHandleCreated()
        {
            Themes.SetWindowTheme(m_ParentScrollBar.Handle, "", "");
            UpdateScrollValues();
        }

        internal bool WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.SBM_SETSCROLLINFO || m.Msg == 0x2114 || m.Msg == 0x2115 || m.Msg == (int)WinApi.WindowsMessages.WM_MOUSEMOVE)
            {
                CallBaseWndProc(ref m);
                SendDelayPaintMessage();
                return false;
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_SIZE)
            {
                CallBaseWndProc(ref m);
                m_FirstPaint = true;
                SendDelayPaintMessage();
                return false;
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_ERASEBKGND)
            {
                //PaintUsingDC();
                //m.Result = new IntPtr(1);
                return false;
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_LBUTTONDOWN)
            {
                Point p = new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam));
                InternalMouseDown(new MouseEventArgs(GetMouseButton(m.WParam.ToInt32()), 0, p.X, p.Y, 0));
                return false;
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_LBUTTONUP)
            {
                Point p = new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam));
                InternalMouseUp(new MouseEventArgs(GetMouseButton(m.WParam.ToInt32()), 0, p.X, p.Y, 0));
                return false;
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_LBUTTONDBLCLK)
            {
                Point p = new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam));
                InternalMouseDoubleClick(new MouseEventArgs(GetMouseButton(m.WParam.ToInt32()), 0, p.X, p.Y, 0));
                return false;
            }
            else if (m.Msg == NativeFunctions.WM_USER + 7)
            {
                PaintUsingDC();
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_SYSTIMER)
            {
                if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track && Control.MouseButtons == MouseButtons.Left)
                {
                    Point p = m_ParentScrollBar.PointToClient(Control.MousePosition);
                    OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, p.X, p.Y, 0));
                }
            }
            else if (m.Msg == (int)WinApi.WindowsMessages.WM_CAPTURECHANGED)
            {
                // Recapture the mouse...
                if (m.LParam == IntPtr.Zero && Control.MouseButtons == MouseButtons.Left && m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track)
                {
                    m_ParentScrollBar.Capture = true;
                }
            }

            if (m.Msg == (int)WinApi.WindowsMessages.WM_PAINT)
            {
                if (m_FirstPaint)
                {
                    // Call to base actually updates values returned by GetScrollBarInfo
                    CallBaseWndProc(ref m);
                    // Repaint the control once that is done
                    SendDelayPaintMessage();
                    m_FirstPaint = false;
                }
                WinApi.PAINTSTRUCT ps = new WinApi.PAINTSTRUCT();
                IntPtr hdc = WinApi.BeginPaint(m.HWnd, ref ps);
                try
                {
                    Graphics g = Graphics.FromHdc(hdc);
                    try
                    {
                        PaintInternal(g, hdc);
                    }
                    finally
                    {
                        g.Dispose();
                    }
                }
                finally
                {
                    WinApi.EndPaint(m.HWnd, ref ps);
                }
                return false;
            }
            return true;
        }

        private void CallBaseWndProc(ref Message m)
        {
            m_ParentScrollBarWndProc.CallBaseWndProc(ref m);
        }

        private void SendDelayPaintMessage()
        {
            m_PaintTimer.Start();
        }

        void PaintTimerTick(object sender, EventArgs e)
        {
            m_PaintTimer.Stop();
            PaintUsingDC();
        }

        private void InternalMouseDoubleClick(MouseEventArgs me)
        {
            PerformThumbClick(me.X, me.Y);
        }

        private void PerformThumbClick(int mouseX, int mouseY)
        {
            if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.ThumbDecrease)
                this.SetValue(Math.Max(m_ParentScrollBar.Value - m_ParentScrollBar.SmallChange, m_ParentScrollBar.Minimum), ScrollEventType.SmallDecrement);
            else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.ThumbIncrease)
                this.SetValue(Math.Min(m_ParentScrollBar.Value + m_ParentScrollBar.SmallChange, m_ParentScrollBar.Maximum), ScrollEventType.SmallIncrement);
            else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Control)
            {
                if (this.IsVScroll)
                {
                    if (mouseY < m_ScrollBarCore.TrackRectangle.Y)
                        this.SetValue(Math.Max(m_ParentScrollBar.Value - m_ParentScrollBar.LargeChange, m_ParentScrollBar.Minimum), ScrollEventType.LargeDecrement);
                    else if (mouseY > m_ScrollBarCore.TrackRectangle.Bottom)
                        this.SetValue(Math.Min(m_ParentScrollBar.Value + m_ParentScrollBar.LargeChange, m_ParentScrollBar.Maximum), ScrollEventType.LargeIncrement);
                }
                else
                {
                    if (mouseX < m_ScrollBarCore.TrackRectangle.X)
                        this.SetValue(Math.Max(m_ParentScrollBar.Value - m_ParentScrollBar.LargeChange, m_ParentScrollBar.Minimum), ScrollEventType.LargeDecrement);
                    else if (mouseX > m_ScrollBarCore.TrackRectangle.Right)
                        this.SetValue(Math.Min(m_ParentScrollBar.Value + m_ParentScrollBar.LargeChange, m_ParentScrollBar.Maximum), ScrollEventType.LargeIncrement);
                }
            }
        }

        private void SetValue(int newValue, ScrollEventType type)
        {
            //FieldInfo fi = typeof(System.Windows.Forms.ScrollBar).GetField("value", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (fi != null)
            //{
            //    fi.SetValue(this, newValue);
            //    UpdateSystemScrollInfo();
            //}
            //else

            if (m_ParentScrollBar.Value == newValue) return;

            if (m_UseLockWindowUpdate && m_ParentScrollBar.Parent != null)
                NativeFunctions.LockWindowUpdate(m_ParentScrollBar.Parent.Handle);
            else
                NativeFunctions.SendMessage(m_ParentScrollBar.Handle, NativeFunctions.WM_SETREDRAW, 0, 0);

            m_ParentScrollBarWndProc.SetValue(newValue, type);
            if (m_UseLockWindowUpdate && m_ParentScrollBar.Parent != null)
                NativeFunctions.LockWindowUpdate(IntPtr.Zero);
            else
                NativeFunctions.SendMessage(m_ParentScrollBar.Handle, NativeFunctions.WM_SETREDRAW, 1, 0);

            if (m_ParentScrollBar.Parent is DataGridView && type == ScrollEventType.ThumbTrack)
            {
                if (m_UseLockWindowUpdate)
                    m_ParentScrollBar.Parent.Refresh();
                else
                {
                    Point p = m_ParentScrollBar.PointToClient(Control.MousePosition);
                    if (!m_ParentScrollBar.ClientRectangle.Contains(p)) 
                        m_ParentScrollBar.Parent.Refresh();
                }
            }

            PaintUsingDC();
        }

        //private void UpdateSystemScrollInfo()
        //{
        //    if (this.IsHandleCreated && this.Enabled)
        //    {
        //        WinApi.SCROLLINFO si = new WinApi.SCROLLINFO();
        //        si.cbSize = Marshal.SizeOf(typeof(WinApi.SCROLLINFO));
        //        si.fMask = 0x17;
        //        si.nMin = this.Minimum;
        //        si.nMax = this.Maximum;
        //        si.nPage = this.LargeChange;
        //        if (this.RightToLeft == RightToLeft.Yes)
        //        {
        //            MethodInfo mi = typeof(System.Windows.Forms.ScrollBar).GetMethod("ReflectPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        //            if(mi!=null)
        //                si.nPos = (int)mi.Invoke(this, new object[]{this.Value});
        //            //= this.ReflectPosition(this.value);
        //        }
        //        else
        //        {
        //            si.nPos = this.Value;
        //        }
        //        si.nTrackPos = 0;
        //        WinApi.SetScrollInfo(new HandleRef(this, this.Handle), 2, ref si, false);
        //    }
        //}

        private void InternalMouseUp(MouseEventArgs mouseEventArgs)
        {
            StopAutoScrollTimer();
            m_ScrollBarCore.MouseUp(mouseEventArgs);
            m_ParentScrollBar.Invalidate();
            m_MouseDownTrackOffset = 0;

            if (m_ParentScrollBar.Capture)
                m_ParentScrollBar.Capture = false;
        }

        private void InternalMouseDown(MouseEventArgs me)
        {
            m_MouseDownTrackOffset = 0;
            m_ScrollBarCore.MouseDown(me);
            if (m_ScrollBarCore.MouseOverPart != ScrollBarCore.eScrollPart.Track && m_ScrollBarCore.MouseOverPart != ScrollBarCore.eScrollPart.None)
            {
                PerformThumbClick(me.X, me.Y);
                StartAutoScrollTimer();
            }
            else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track)
            {
                if (IsVScroll)
                    m_MouseDownTrackOffset = me.Y - m_ScrollBarCore.TrackRectangle.Y;
                else
                    m_MouseDownTrackOffset = me.X - m_ScrollBarCore.TrackRectangle.X;
            }
            PaintUsingDC();
        }

        private Timer m_Timer = null;
        private void StartAutoScrollTimer()
        {
            if (m_Timer == null)
            {
                m_Timer = new Timer();
                m_Timer.Interval = 150;
                m_Timer.Tick += new EventHandler(TimerScrollTick);
            }
            m_Timer.Start();
        }

        private void TimerScrollTick(object sender, EventArgs e)
        {
            Point p = m_ParentScrollBar.PointToClient(Control.MousePosition);
            PerformThumbClick(p.X, p.Y);
        }

        private void StopAutoScrollTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer.Dispose();
                m_Timer = null;
            }
        }

        private MouseButtons GetMouseButton(int wParam)
        {
            MouseButtons mb = MouseButtons.None;
            if ((wParam & (int)WinApi.MouseKeyState.MK_LBUTTON) != 0)
                mb |= MouseButtons.Left;
            if ((wParam & (int)WinApi.MouseKeyState.MK_MBUTTON) != 0)
                mb |= MouseButtons.Middle;
            if ((wParam & (int)WinApi.MouseKeyState.MK_RBUTTON) != 0)
                mb |= MouseButtons.Right;
            if ((wParam & (int)WinApi.MouseKeyState.MK_XBUTTON1) != 0)
                mb |= MouseButtons.XButton1;
            if ((wParam & (int)WinApi.MouseKeyState.MK_XBUTTON2) != 0)
                mb |= MouseButtons.XButton2;

            return mb;
        }

        private void PaintUsingDC()
        {
            IntPtr hdc = WinApi.GetWindowDC(m_ParentScrollBar.Handle);
            try
            {
                Graphics g = Graphics.FromHdc(hdc);
                try
                {
                    PaintInternal(g, hdc);
                }
                finally
                {
                    g.Dispose();
                }
            }
            finally
            {
                WinApi.ReleaseDC(m_ParentScrollBar.Handle, hdc);
            }
        }

        private void PaintInternal(Graphics g, IntPtr hdc)
        {
            UpdateScrollValues();
            using (BufferedBitmap bmp = new BufferedBitmap(g, new Rectangle(0, 0, m_ParentScrollBar.Width, m_ParentScrollBar.Height)))
            {
                m_ScrollBarCore.Paint(GetItemPaintArgs(bmp.Graphics));
                bmp.Render(g);
            }
        }

        private ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(m_ParentScrollBar as IOwner, m_ParentScrollBar, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false;
            pa.GlassEnabled = false;
            return pa;
        }

        private ColorScheme m_ColorScheme = null;
        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrived from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            BaseRenderer r = GetRenderer();
            if (r is Office2007Renderer)
                return ((Office2007Renderer)r).ColorTable.LegacyColors;
            if (m_ColorScheme == null)
                m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
            return m_ColorScheme;
        }

        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
                m_DefaultRenderer = new Rendering.Office2007Renderer();

            return m_DefaultRenderer;
        }

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    m_ParentScrollBar.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set { m_Renderer = value; }
        }

        internal void OnMouseEnter(EventArgs e)
        {
            if (m_ParentScrollBar.Capture)
                m_ParentScrollBar.Capture = false;
        }

        internal void OnMouseLeave(EventArgs e)
        {
            StopAutoScrollTimer();
            if (m_ScrollBarCore.MouseOverPart != ScrollBarCore.eScrollPart.None && Control.MouseButtons == MouseButtons.Left)
                m_ParentScrollBar.Capture = true;
            if (m_ScrollBarCore.IsMouseDown && Control.MouseButtons == MouseButtons.None)
            {
                m_ScrollBarCore.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, -10, -10, 0));
                SendDelayPaintMessage();
            }
            else
                m_ScrollBarCore.MouseLeave();
        }

        internal void OnMouseMove(MouseEventArgs e)
        {
            m_ScrollBarCore.MouseMove(e);
            if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track && e.Button == MouseButtons.Left)
            {
                Point p = m_ParentScrollBar.PointToClient(Control.MousePosition);
                SetValue(ValueFromMouseCoordinates(p), ScrollEventType.ThumbTrack);
            }
        }

        private int ValueFromMouseCoordinates(Point p)
        {
            if (IsVScroll)
            {
                int trackY = p.Y - m_MouseDownTrackOffset;
                trackY = Math.Max(trackY, m_ScrollBarCore.ThumbDecreaseRectangle.Bottom);
                trackY = Math.Min(trackY, m_ScrollBarCore.ThumbIncreaseRectangle.Y - GetTrackSize());
                trackY -= m_ScrollBarCore.ThumbDecreaseRectangle.Bottom;

                int totalSize = GetAvailableTrackArea() - GetTrackSize();
                return (int)((m_ParentScrollBar.Maximum - m_ParentScrollBar.Minimum) * ((float)trackY / (float)totalSize));
            }
            else
            {
                int trackX = p.X - m_MouseDownTrackOffset;
                trackX = Math.Max(trackX, m_ScrollBarCore.ThumbDecreaseRectangle.Right);
                trackX = Math.Min(trackX, m_ScrollBarCore.ThumbIncreaseRectangle.X - GetTrackSize());
                trackX -= m_ScrollBarCore.ThumbDecreaseRectangle.Right;

                int totalSize = GetAvailableTrackArea() - GetTrackSize();
                return (int)((m_ParentScrollBar.Maximum - m_ParentScrollBar.Minimum) * ((float)trackX / (float)totalSize));
            }
        }

        private int GetTrackSize()
        {
            if (IsVScroll)
                return m_ScrollBarCore.TrackRectangle.Height;
            else
                return m_ScrollBarCore.TrackRectangle.Width;
        }

        private int GetAvailableTrackArea()
        {
            if (IsVScroll)
                return m_ScrollBarCore.DisplayRectangle.Height - m_ScrollBarCore.ThumbDecreaseRectangle.Height - m_ScrollBarCore.ThumbIncreaseRectangle.Height;
            else
                return m_ScrollBarCore.DisplayRectangle.Width - m_ScrollBarCore.ThumbDecreaseRectangle.Width - m_ScrollBarCore.ThumbIncreaseRectangle.Width;
        }

        private bool IsVScroll
        {
            get
            {
                return m_IsVScrollBar;
            }
        }

        private void UpdateScrollValues()
        {
            WinApi.SCROLLBARINFO psbi = new WinApi.SCROLLBARINFO();
            psbi.cbSize = Marshal.SizeOf(psbi);

            WinApi.GetScrollBarInfo(m_ParentScrollBar.Handle, (uint)WinApi.eObjectId.OBJID_CLIENT, ref psbi);

            Rectangle displayRect = new Rectangle(0, 0, m_ParentScrollBar.Width, m_ParentScrollBar.Height);

            if (IsVScroll)
            {
                if (m_ScrollBarCore.DisplayRectangle != displayRect)
                    m_ScrollBarCore.DisplayRectangle = displayRect;

                Rectangle thumbRect = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, psbi.dxyLineButton);
                if (m_ScrollBarCore.ThumbDecreaseRectangle != thumbRect)
                    m_ScrollBarCore.ThumbDecreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X, displayRect.Bottom - psbi.dxyLineButton, displayRect.Width, psbi.dxyLineButton);
                if (m_ScrollBarCore.ThumbIncreaseRectangle != thumbRect)
                    m_ScrollBarCore.ThumbIncreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X, displayRect.Y + psbi.xyThumbTop, displayRect.Width, psbi.xyThumbBottom - psbi.xyThumbTop);
                if (m_ScrollBarCore.TrackRectangle != thumbRect)
                    m_ScrollBarCore.TrackRectangle = thumbRect;
            }
            else
            {
                if (m_ScrollBarCore.DisplayRectangle != displayRect)
                    m_ScrollBarCore.DisplayRectangle = displayRect;

                Rectangle thumbRect = new Rectangle(displayRect.X, displayRect.Y, psbi.dxyLineButton, displayRect.Height);
                if (m_ScrollBarCore.ThumbDecreaseRectangle != thumbRect)
                    m_ScrollBarCore.ThumbDecreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.Right - psbi.dxyLineButton, displayRect.Y, psbi.dxyLineButton, displayRect.Height);
                if (m_ScrollBarCore.ThumbIncreaseRectangle != thumbRect)
                    m_ScrollBarCore.ThumbIncreaseRectangle = thumbRect;

                thumbRect = new Rectangle(displayRect.X + psbi.xyThumbTop, displayRect.Y, psbi.xyThumbBottom - psbi.xyThumbTop, displayRect.Height);
                if (m_ScrollBarCore.TrackRectangle != thumbRect)
                    m_ScrollBarCore.TrackRectangle = thumbRect;
            }

            if (m_ScrollBarCore.Minimum != m_ParentScrollBar.Minimum)
                m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
            if (m_ScrollBarCore.Maximum != m_ParentScrollBar.Maximum)
                m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
            if (m_ScrollBarCore.SmallChange != m_ParentScrollBar.SmallChange)
                m_ScrollBarCore.SmallChange = m_ParentScrollBar.SmallChange;
            if (m_ScrollBarCore.LargeChange != m_ParentScrollBar.LargeChange)
                m_ScrollBarCore.LargeChange = m_ParentScrollBar.LargeChange;
            if (m_ScrollBarCore.Value != m_ParentScrollBar.Value)
                m_ScrollBarCore.Value = m_ParentScrollBar.Value;
        }

        internal void NotifyInvalidate(Rectangle invalidatedArea)
        {

        }

        //public bool IsAppScrollBarStyle
        //{
        //    get { return m_ScrollBarCore.IsAppScrollBarStyle; }
        //    set
        //    {
        //        m_ScrollBarCore.IsAppScrollBarStyle = value;
        //        m_ParentScrollBar.Invalidate();
        //    }
        //}
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_ScrollBarCore != null)
            {
                m_ScrollBarCore.Dispose();
                m_ScrollBarCore = null;
            }
            if (m_PaintTimer != null)
            {
                m_PaintTimer.Stop();
                m_PaintTimer.Dispose();
                m_PaintTimer = null;
            }
            StopAutoScrollTimer();
        }

        #endregion

        internal interface IScrollBarExtender
        {
            void CallBaseWndProc(ref Message m);
            void SetValue(int newValue, ScrollEventType type);
        }
    }
}
