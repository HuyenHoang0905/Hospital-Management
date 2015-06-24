using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar.ScrollBar
{
    public class ScrollBarCore : IDisposable
    {
        #region Private Variables
        private Rectangle m_DisplayRectangle = Rectangle.Empty;
        private eOrientation m_Orientation = eOrientation.Vertical;

        // Default thumb sizes...
        private Size m_VThumbSize = new Size(15, 17);
        private Size m_HThumbSize = new Size(17, 15);
        private Rectangle m_ThumbDecrease = Rectangle.Empty;
        private Rectangle m_ThumbIncrease = Rectangle.Empty;
        private Rectangle m_Track = Rectangle.Empty;
        private eScrollPart m_MouseOverPart = eScrollPart.None;

        private Control m_ParentControl = null;
        private bool m_MouseDown = false;
        private Timer m_ClickTimer = null;
        private int m_MouseDownTrackOffset = 0;
        private Bitmap m_CashedView = null;
        private bool m_ViewExpired = true;
        private bool m_PassiveScrollBar = false;
        private bool m_IsAppScrollBarStyle = false;
        public event EventHandler ValueChanged;
        private bool m_IsScrollBarParent = false;
        private bool m_IsAdvScrollBar = false;
        private int m_AutoClickCount = 0;
        public event ScrollEventHandler Scroll;
        #endregion

        #region Internal Implementation
        public ScrollBarCore() : this(null) { }

        public ScrollBarCore(Control parentControl) : this(parentControl, false) { }

        public ScrollBarCore(Control parentControl, bool isPassive)
        {
            m_ParentControl = parentControl;
            m_IsScrollBarParent = m_ParentControl is System.Windows.Forms.ScrollBar || m_ParentControl is ScrollBarAdv;
            m_IsAdvScrollBar = m_ParentControl is ScrollBarAdv;
            m_PassiveScrollBar = isPassive;
            DisposeCashedView();
        }

        internal void DisposeCashedView()
        {
            if (m_CashedView != null)
            {
                m_CashedView.Dispose();
                m_CashedView = null;
            }
        }

        private bool _IsPainting = false;
        public void Paint(ItemPaintArgs p)
        {
            if (_IsPainting) return;

            _IsPainting = true;
            try
            {
                if (m_DisplayRectangle.IsEmpty || m_DisplayRectangle.Width <= 0 || m_DisplayRectangle.Height <= 0 || p.Graphics == null) return;

                Office2007ScrollBarPainter painter = new Office2007ScrollBarPainter();
                painter.ColorTable = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable;
                painter.AppStyleScrollBar = m_IsAppScrollBarStyle;

                if (m_CashedView == null)
                {
                    try
                    {
                        m_CashedView = new Bitmap(m_DisplayRectangle.Width, m_DisplayRectangle.Height, p.Graphics);
                    }
                    catch
                    {
                        return;
                    }
                    m_CashedView.MakeTransparent();
                    m_ViewExpired = true;
                }

                if (!m_ViewExpired)
                {
                    Bitmap cachedView = m_CashedView;
                    if (cachedView != null)
                        p.Graphics.DrawImageUnscaled(cachedView, m_DisplayRectangle.Location);
                    return;
                }

                using (Graphics g = Graphics.FromImage(m_CashedView))
                {
                    if (!m_DisplayRectangle.Location.IsEmpty)
                        g.TranslateTransform(-m_DisplayRectangle.X, -m_DisplayRectangle.Y);
                    eScrollBarState state = m_Enabled ? eScrollBarState.Normal : eScrollBarState.Disabled;
                    if (m_Enabled && (m_MouseOverPart != eScrollPart.None || IsFocused)) state = eScrollBarState.ControlMouseOver;
                    bool rtl = false;
                    if (m_ParentControl != null) rtl = m_ParentControl.RightToLeft == RightToLeft.Yes;
                    painter.PaintBackground(g, m_DisplayRectangle, state, m_Orientation == eOrientation.Horizontal, m_SideBorderOnly, rtl);

                    eScrollThumbPosition tp = eScrollThumbPosition.Top;
                    if (m_Orientation == eOrientation.Horizontal) tp = eScrollThumbPosition.Left;
                    if (m_MouseOverPart == eScrollPart.ThumbDecrease && m_Enabled)
                    {
                        if (m_MouseDown)
                            state = eScrollBarState.Pressed;
                        else
                            state = eScrollBarState.PartMouseOver;
                    }
                    painter.PaintThumb(g, m_ThumbDecrease, tp, state);

                    tp = eScrollThumbPosition.Bottom;
                    if (m_MouseOverPart == eScrollPart.ThumbIncrease && m_Enabled)
                    {
                        if (m_MouseDown)
                            state = eScrollBarState.Pressed;
                        else
                            state = eScrollBarState.PartMouseOver;
                    }
                    else if (m_MouseOverPart != eScrollPart.None && m_Enabled)
                        state = eScrollBarState.ControlMouseOver;

                    if (m_Orientation == eOrientation.Horizontal) tp = eScrollThumbPosition.Right;
                    painter.PaintThumb(g, m_ThumbIncrease, tp, state);

                    if (m_MouseOverPart == eScrollPart.Track && m_Enabled)
                    {
                        if (m_MouseDown)
                            state = eScrollBarState.Pressed;
                        else
                            state = eScrollBarState.PartMouseOver;
                    }
                    else if ((m_MouseOverPart != eScrollPart.None || this.IsFocused) && m_Enabled)
                        state = eScrollBarState.ControlMouseOver;
                    if (m_Orientation == eOrientation.Horizontal)
                        painter.PaintTrackHorizontal(g, m_Track, state);
                    else
                        painter.PaintTrackVertical(g, m_Track, state);
                }

                p.Graphics.DrawImageUnscaled(m_CashedView, m_DisplayRectangle.Location);
                m_ViewExpired = false;
            }
            finally
            {
                _IsPainting = false;
            }
        }

        private bool IsFocused
        {
            get
            {
                if (m_IsAdvScrollBar && m_ParentControl.Focused)
                    return true;

                return false;
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (!m_Enabled)
                return;

            Point p = new Point(e.X, e.Y);
            if (m_MouseDown && m_MouseOverPart == eScrollPart.Track)
            {
                if (!m_PassiveScrollBar)
                {
                    // Update Track position based on mouse position...
                    int i = ValueFromMouseCoordinates(p);
                    SetValue(i, ScrollEventType.ThumbTrack);
                    //this.Value = i;
                }
                return;
            }

            if (e.Button != MouseButtons.None && m_MouseOverPart!=eScrollPart.None) return;

            if (m_DisplayRectangle.Contains(p))
            {
                eScrollPart part = eScrollPart.Control;
                if (m_ThumbDecrease.Contains(p))
                    part = eScrollPart.ThumbDecrease;
                else if (m_ThumbIncrease.Contains(p))
                    part = eScrollPart.ThumbIncrease;
                else if (m_Track.Contains(p))
                    part = eScrollPart.Track;

                if (m_MouseOverPart != part)
                {
                    m_MouseOverPart = part;
                    this.Invalidate();
                }
            }
            else if (m_MouseOverPart != eScrollPart.None)
            {
                m_MouseOverPart = eScrollPart.None;
                this.Invalidate();
            }
        }

        private int ValueFromMouseCoordinates(Point p)
        {
            if (m_Orientation == eOrientation.Vertical)
            {
                int trackY = p.Y - m_MouseDownTrackOffset;
                trackY = Math.Max(trackY, m_ThumbDecrease.Bottom);
                trackY = Math.Min(trackY, m_ThumbIncrease.Y - GetTrackSize());
                trackY -= m_ThumbDecrease.Bottom;

                int totalSize = GetAvailableTrackArea() - GetTrackSize();
                return (int)((this.GetMaximumValue() - this.Minimum) * ((float)trackY / (float)totalSize));
            }
            else
            {
                int trackX = p.X - m_MouseDownTrackOffset;
                trackX = Math.Max(trackX, m_ThumbDecrease.Right);
                trackX = Math.Min(trackX, m_ThumbIncrease.X - GetTrackSize());
                trackX -= m_ThumbDecrease.Right;

                int totalSize = GetAvailableTrackArea() - GetTrackSize();
                return (int)((this.GetMaximumValue() - this.Minimum) * ((float)trackX / (float)totalSize));
            }
        }

        public void MouseLeave()
        {
            if (m_MouseOverPart != eScrollPart.None)
            {
                if (m_MouseDown && (m_MouseOverPart == eScrollPart.Track || m_MouseOverPart != eScrollPart.None && m_PassiveScrollBar))
                    return;
                m_MouseOverPart = eScrollPart.None;
                this.Invalidate();
            }
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (m_MouseOverPart != eScrollPart.None)
            {
                m_MouseDown = true;
                if (m_PassiveScrollBar)
                {
                    m_ViewExpired = true;
                    return;
                }

                if(m_Orientation == eOrientation.Vertical)
                    m_MouseDownTrackOffset = e.Y - m_Track.Y;
                else
                    m_MouseDownTrackOffset = e.X - m_Track.X;

                if (m_MouseOverPart == eScrollPart.ThumbDecrease)
                    SetValue(this.Value - m_SmallChange, ScrollEventType.SmallDecrement);
                else if (m_MouseOverPart == eScrollPart.ThumbIncrease)
                    SetValue(this.Value + m_SmallChange, ScrollEventType.SmallIncrement);
                else if (m_MouseOverPart == eScrollPart.Track)
                {
                    this.Invalidate();
                    if (m_ParentControl != null)
                        m_ParentControl.Capture = true;
                }
                else if (m_MouseOverPart == eScrollPart.Control)
                {
                    MousePageDown(new Point(e.X, e.Y));
                }

                SetupClickTimer();
            }
        }

        private void MousePageDown(Point p)
        {
            if (m_Orientation == eOrientation.Vertical && m_Track.Y > p.Y || m_Orientation == eOrientation.Horizontal && m_Track.X > p.X)
                SetValue(this.Value - this.LargeChange, ScrollEventType.LargeDecrement);
            else
            {
                if (m_IsScrollBarParent)
                {
                    if (this.Value + this.LargeChange > this.GetMaximumValue())
                    {
                        SetValue(this.Value + (this.Maximum - this.LargeChange - 1), ScrollEventType.LargeIncrement);
                    }
                    else
                        SetValue(this.Value + this.LargeChange, ScrollEventType.LargeIncrement);
                }
                else
                    SetValue(this.Value + this.LargeChange, ScrollEventType.LargeIncrement);
            }
        }

        public bool IsMouseDown
        {
            get { return m_MouseDown; }
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (m_ParentControl != null && m_ParentControl.Capture)
                m_ParentControl.Capture = false;

            if(m_MouseDown)
            {
                if (m_MouseOverPart != eScrollPart.None && _RaiseEndScroll)
                {
#if FRAMEWORK20
                    if(m_MouseOverPart == eScrollPart.Track)
                        OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, this.Value, this.Value, this.ScrollOrientation));
                    OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.Value, this.Value, this.ScrollOrientation));
#else
					if(m_MouseOverPart == eScrollPart.Track)
						OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, this.Value));
					OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.Value));
#endif
                    _RaiseEndScroll = false;
                }
                DisposeTimer();
                m_MouseDown = false;
                this.Invalidate();
            }

            if (!this.DisplayRectangle.Contains(e.X, e.Y))
                MouseLeave();
        }

        private ScrollOrientation ScrollOrientation
        {
            get
            {
                return this.Orientation == eOrientation.Horizontal ? ScrollOrientation.HorizontalScroll : ScrollOrientation.VerticalScroll;
            }
        }

        private void DisposeTimer()
        {
            Timer clickTimer = m_ClickTimer;
            m_ClickTimer = null;
            if (clickTimer != null)
            {
                clickTimer.Enabled = false;
                clickTimer.Tick -= new EventHandler(ClickTimer_Tick);
                clickTimer.Dispose();
            }
        }

        private void SetupClickTimer()
        {
            if (m_ClickTimer != null) return;
            m_AutoClickCount = 0;
            m_ClickTimer = new Timer();
            m_ClickTimer.Interval = 200;
            m_ClickTimer.Tick += new EventHandler(ClickTimer_Tick);
            m_ClickTimer.Enabled = true;
        }

        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            Control parentControl = m_ParentControl;
            if (m_MouseDown && m_MouseOverPart == eScrollPart.ThumbDecrease)
            {
                SetValue(this.Value - m_SmallChange, ScrollEventType.SmallDecrement);
            }
            else if (m_MouseDown && m_MouseOverPart == eScrollPart.ThumbIncrease)
            {
                SetValue(this.Value + m_SmallChange, ScrollEventType.SmallIncrement);
            }
            else if (m_MouseDown && m_MouseOverPart == eScrollPart.Control && parentControl != null)
            {
                Point p = parentControl.PointToClient(Control.MousePosition);
                if (m_Track.Contains(p))
                {
                    DisposeTimer();
                    m_MouseDown = false;
                }
                else
                    MousePageDown(p);
            }
            m_AutoClickCount++;
            if (m_AutoClickCount > 4 && m_ClickTimer != null && m_ClickTimer.Interval > 20)
            {
                m_ClickTimer.Interval -= 10;
            }
        }

        private void Invalidate()
        {
            m_ViewExpired = true;
            Control parentControl = m_ParentControl;
            if (parentControl != null && !m_PassiveScrollBar)
            {
                parentControl.Invalidate(m_DisplayRectangle);
                parentControl.Update();
            }
        }

        private bool m_HasBorder = true;
        internal bool HasBorder
        {
            get { return m_HasBorder; }
            set { m_HasBorder = value; }
        }

        private bool m_SideBorderOnly = false;
        internal bool SideBorderOnly
        {
            get { return m_SideBorderOnly; }
            set { m_SideBorderOnly = value; }
        }

        private void UpdateLayout()
        {
            if (m_PassiveScrollBar)
                return;

            Rectangle r = m_DisplayRectangle;
            if(m_HasBorder)
                r.Inflate(-1, -1);

            if (m_Orientation == eOrientation.Vertical)
            {
                m_ThumbDecrease = new Rectangle(r.X, r.Y, r.Width, m_VThumbSize.Height);
                m_ThumbIncrease = new Rectangle(r.X, r.Bottom - m_VThumbSize.Height, r.Width, m_VThumbSize.Height);
                int i = GetTrackSize();
                m_Track = new Rectangle(r.X, r.Y + GetTrackPosition(), r.Width, i);
            }
            else
            {
                m_ThumbDecrease = new Rectangle(r.X, r.Y, m_HThumbSize.Width, r.Height);
                m_ThumbIncrease = new Rectangle(r.Right - m_HThumbSize.Width, r.Y, m_HThumbSize.Width, r.Height);
                int i = GetTrackSize();
                m_Track = new Rectangle(r.X + GetTrackPosition(), r.Y, i, r.Height);

            }
        }

        private int GetTrackSize()
        {
            int i = 0;
            int size = GetAvailableTrackArea();
            //Rectangle r = m_DisplayRectangle;
            //r.Inflate(-1, -1);

            i = (int)(size * ((float)m_LargeChange / (float)(m_Maximum - m_Minimum + 1)));
            //i = (int)(size / ((float)m_Maximum / (float)m_LargeChange));
            //if (m_Orientation == eOrientation.Horizontal)
            //{
            //    i = (int)(r.Width * ((float)m_LargeChange / (float)m_Maximum));
            //}
            //else
            //{
            //    i = (int)(r.Height * ((float)m_LargeChange / (float)m_Maximum));
            //}
            i = Math.Max(14, i);
            i = Math.Min(i, size);

            return i;
        }

        private int GetAvailableTrackArea()
        {
            Rectangle r = m_DisplayRectangle;
            if(m_HasBorder)
                r.Inflate(-1, -1);

            int size = r.Height;

            if (m_Orientation == eOrientation.Horizontal)
            {
                size = r.Width - m_HThumbSize.Width * 2;
            }
            else
            {
                size -= m_VThumbSize.Height * 2;
            }

            return Math.Max(size, 8);
        }

        private int GetTrackPosition()
        {
            int trackSize = GetTrackSize();
            int totalSize = GetAvailableTrackArea() - trackSize;
            int i = Math.Min(totalSize, Math.Max((int)(totalSize * ((float)m_Value / (float)GetMaximumValue())), 0));
            if (m_Orientation == eOrientation.Vertical)
                i += m_VThumbSize.Height;
            else
                i += m_HThumbSize.Width;
            return i;
        }

        public Rectangle ThumbDecreaseRectangle
        {
            get { return m_ThumbDecrease; }
            set
            {
                m_ThumbDecrease = value;
                DisposeCashedView();
            }
        }

        public Rectangle ThumbIncreaseRectangle
        {
            get { return m_ThumbIncrease; }
            set
            {
                m_ThumbIncrease = value;
                DisposeCashedView();
            }
        }

        public Rectangle TrackRectangle
        {
            get { return m_Track; }
            set
            {
                m_Track = value;
                DisposeCashedView();
            }
        }

        public Rectangle DisplayRectangle
        {
            get { return m_DisplayRectangle; }
            set
            {
                m_DisplayRectangle = value;
                DisposeCashedView();
                UpdateLayout();
            }
        }

        public eOrientation Orientation
        {
            get { return m_Orientation; }
            set
            { 
                m_Orientation = value;
                DisposeCashedView();
            }
        }

        private int m_Minimum = 0;
        public int Minimum
        {
            get { return m_Minimum; }
            set 
            {
                m_Minimum = value;
                DisposeCashedView();
                UpdateLayout();
            }
        }

        private int m_Maximum = 0;
        public int Maximum
        {
            get { return m_Maximum; }
            set
            {
                m_Maximum = value;
                DisposeCashedView();
                UpdateLayout();
            }
        }

        private int m_SmallChange = 1;
        public int SmallChange
        {
            get { return m_SmallChange; }
            set { m_SmallChange = value; }
        }

        private int m_LargeChange = 16;
        public int LargeChange
        {
            get { return m_LargeChange; }
            set
            {
                m_LargeChange = value;
                DisposeCashedView();
                UpdateLayout();
            }
        }

        private int m_Value = 0;
        public int Value
        {
            get { return m_Value; }
            set
            {
                value = NormalizeValue(value);
                if (m_Value == value && (!m_IsScrollBarParent || m_IsAdvScrollBar)) return;

                m_Value = value;
                if (m_PassiveScrollBar) return;
                DisposeCashedView();
                UpdateLayout();
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
                this.Invalidate();
            }
        }

        private bool _RaiseEndScroll = false;
        private void SetValue(int value, ScrollEventType type)
        {
            value = NormalizeValue(value);
#if FRAMEWORK20
            OnScroll(new ScrollEventArgs(type, m_Value, value, this.ScrollOrientation));
#else
			OnScroll(new ScrollEventArgs(type, value));
#endif
            Value = value;
            _RaiseEndScroll = true;
        }

        private int NormalizeValue(int value)
        {
            int max = m_IsAdvScrollBar ? GetMaximumValue() : m_Maximum;
            if (value < m_Minimum) value = m_Minimum;
            if (value > max) value = max;
            return value;
        }

        private void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler h = this.Scroll;
            if (h != null)
                h(this, e);
        }

        internal int GetMaximumValue()
        {
            if (m_IsScrollBarParent)
            {
                return m_Maximum - m_LargeChange + 1;
            }
            else
                return m_Maximum;
        }
        
        internal eScrollPart MouseOverPart
        {
            get
            {
                return m_MouseOverPart;
            }
            set
            {
                m_MouseOverPart = value;
            }
        }

        private bool m_Visible = true;
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        public Control ParentControl
        {
            get { return m_ParentControl; }
            set 
            { 
                m_ParentControl = value;
                if (m_ParentControl == null) DisposeTimer();
            }
        }

        private bool m_Enabled = true;
        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                if (m_Enabled != value)
                {
                    m_Enabled = value;
                    this.Invalidate();
                }
            }
        }

        public bool IsAppScrollBarStyle
        {
            get { return m_IsAppScrollBarStyle; }
            set { m_IsAppScrollBarStyle = value; DisposeCashedView(); }
        }

        internal enum eScrollPart
        {
            ThumbIncrease,
            ThumbDecrease,
            Track,
            Control,
            None
        }

        internal bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.PageDown)
            {
                SetValue(this.Value + m_LargeChange, ScrollEventType.LargeIncrement);
                return true;
            }
            else if (keyData == Keys.PageUp)
            {
                SetValue(this.Value - m_LargeChange, ScrollEventType.LargeIncrement);
                return true;
            }
            else if (keyData == Keys.Up || keyData == Keys.Left)
            {
                SetValue(this.Value - m_SmallChange, ScrollEventType.SmallDecrement);
                return true;
            }
            else if (keyData == Keys.Down || keyData == Keys.Right)
            {
                SetValue(this.Value + m_SmallChange, ScrollEventType.SmallIncrement);
                return true;
            }
            else if (keyData == Keys.Home)
            {
                SetValue(m_Minimum, ScrollEventType.First);
                return true;
            }
            else if (keyData == Keys.End)
            {
                SetValue(m_Maximum, ScrollEventType.Last);
                return true;
            }


            return false;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            DisposeTimer();
        }

        #endregion

        internal void InvalidateScrollBar()
        {
            DisposeCashedView();
            Control parentControl = m_ParentControl;
            if (parentControl != null)
                parentControl.Invalidate();
        }
    }

#if !FRAMEWORK20
	public enum ScrollOrientation
	{
		HorizontalScroll,
		VerticalScroll
	}
#endif
}
