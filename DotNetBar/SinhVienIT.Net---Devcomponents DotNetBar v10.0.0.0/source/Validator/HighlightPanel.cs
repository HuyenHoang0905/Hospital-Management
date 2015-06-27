#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar.Validator
{
    internal class HighlightPanel : Control
    {
        #region Private Variables
        private Dictionary<Control, eHighlightColor> _Highlights = null;
        private List<HighlightRegion> _HighlightRegions = new List<HighlightRegion>();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the HighlightPanel class.
        /// </summary>
        /// <param name="highlights"></param>
        public HighlightPanel(Dictionary<Control, eHighlightColor> highlights)
        {
            _Highlights = highlights;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.Selectable, false);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        #endregion

        #region Implementation
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //g.FillRectangle(Brushes.Yellow, this.ClientRectangle);
            //return;

            foreach (HighlightRegion highlightRegion in _HighlightRegions)
            {
                Color[] colors = GetHighlightColors(highlightRegion.HighlightColor);
                Rectangle r = highlightRegion.Bounds;
                Color back = highlightRegion.BackColor;

                r.Inflate(1, 1);
                DisplayHelp.FillRectangle(g, r, back);
                r.Inflate(-1, -1);

                DisplayHelp.FillRoundedRectangle(g, r, 2, colors[0]);
                r.Inflate(-2, -2);
                DisplayHelp.DrawRectangle(g, colors[2], r);
                r.Inflate(1, 1);
                DisplayHelp.DrawRoundedRectangle(g, colors[1], r, 2);
            }

            base.OnPaint(e);
        }

        private Color[] GetHighlightColors(eHighlightColor color)
        {
            Color[] colors = new Color[3];
            if (color == eHighlightColor.Blue)
            {
                colors[0] = ColorScheme.GetColor(172, 0x6A9CD4);
                colors[1] = ColorScheme.GetColor(0x6A9CD4);
                colors[2] = ColorScheme.GetColor(0x5D7EA4);
            }
            else if (color == eHighlightColor.Orange)
            {
                colors[0] = ColorScheme.GetColor(172, 0xFF9C00);
                colors[1] = ColorScheme.GetColor(0xFF9C00);
                colors[2] = ColorScheme.GetColor(0xCC6600);
            }
            else if (color == eHighlightColor.Green)
            {
                colors[0] = ColorScheme.GetColor(172, 0x71B171);
                colors[1] = ColorScheme.GetColor(0x71B171);
                colors[2] = ColorScheme.GetColor(0x339933);
            }
            else if (color == eHighlightColor.Custom)
            {
                if (_CustomHighlightColors == null || _CustomHighlightColors.Length < 3)
                {
                    colors[0] = Color.Red;
                    colors[1] = Color.Red;
                    colors[2] = Color.Red;
                }
                else
                {
                    colors[0] = _CustomHighlightColors[0];
                    colors[1] = _CustomHighlightColors[1];
                    colors[2] = _CustomHighlightColors[2];
                }
            }
            else
            {
                colors[0] = ColorScheme.GetColor(172, 0xC63030);
                colors[1] = ColorScheme.GetColor(0xC63030);
                colors[2] = ColorScheme.GetColor(0x990000);
            }
            return colors;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible && !_UpdatingRegion) UpdateRegion();
            base.OnVisibleChanged(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (!_RegionInitialized) UpdateRegion();
            base.OnHandleCreated(e);
        }
        private bool _RegionInitialized = false;
        private bool _UpdatingRegion = false;
        internal void UpdateRegion()
        {
            if (_UpdatingRegion || !this.IsHandleCreated) return;

            try
            {
                _UpdatingRegion = true;
                this.Region = null;
                _HighlightRegions.Clear();
                if (_Highlights == null) return;
                if (_Highlights.Count == 0 && _FocusHighlightControl == null)
                {
                    this.Visible = false;
                    return;
                }

                bool processFocusControl = true;
                Region region = null;
                foreach (KeyValuePair<Control, eHighlightColor> item in _Highlights)
                {
                    if (item.Value == eHighlightColor.None || !GetIsVisible(item.Key)) continue;
                    if (item.Key == _FocusHighlightControl) processFocusControl = false;

                    Rectangle r = GetControlRect(item.Key);
                    if (r.IsEmpty) continue;
                    r.Inflate(2, 2);
                    _HighlightRegions.Add(new HighlightRegion(r, GetBackColor(item.Key.Parent), item.Value));
                    if (region == null)
                        region = new Region(r);
                    else
                        region.Union(r);
                    r.Inflate(-3, -3);
                    region.Exclude(r);
                }

                if (processFocusControl && _FocusHighlightControl != null && _FocusHighlightControl.Visible)
                {
                    Rectangle r = GetControlRect(_FocusHighlightControl);
                    if (!r.IsEmpty)
                    {
                        r.Inflate(2, 2);
                        _HighlightRegions.Add(new HighlightRegion(r, GetBackColor(_FocusHighlightControl.Parent), _FocusHighlightColor));
                        if (region == null)
                            region = new Region(r);
                        else
                            region.Union(r);
                        r.Inflate(-3, -3);
                        region.Exclude(r);
                    }
                }

                this.Region = region;
                if (region == null)
                    this.Visible = false;
                else if (!this.Visible)
                {
                    this.Visible = true;
                    this.BringToFront();
                }
                this.Invalidate();
            }
            finally
            {
                _UpdatingRegion = false;
                _RegionInitialized = true;
            }
        }

        private bool GetIsVisible(Control control)
        {
            if (!control.Visible) return false;
            if (control.Parent == null || !control.IsHandleCreated) return control.Visible;

            WinApi.RECT rect = new WinApi.RECT();
            WinApi.GetWindowRect(control.Handle, ref rect);
            Point pp = control.Parent.PointToClient(new Point(rect.Left + 3, rect.Top + 3));
            IntPtr handle = NativeFunctions.ChildWindowFromPoint(control.Parent.Handle, new NativeFunctions.POINT(pp.X, pp.Y));
            if (handle == IntPtr.Zero) return control.Visible;

            Control c = Control.FromHandle(handle);
            if (c != null && c != control && c != this && c != control.Parent)
            {
                return false;
            }

            return control.Visible;
        }
        private Color GetBackColor(Control control)
        {
            Color backColor = control.BackColor;
            if (backColor.IsEmpty || backColor == Color.Transparent)
                backColor = SystemColors.Control;
            else if (backColor.A < 255)
                backColor = Color.FromArgb(255, backColor);
            return backColor;
        }
        protected override void OnResize(EventArgs e)
        {
            UpdateRegion();
            base.OnResize(e);
        }

        private Rectangle GetControlRect(Control c)
        {
            if (!c.IsHandleCreated) return Rectangle.Empty;

            WinApi.RECT rect = new WinApi.RECT();
            WinApi.GetWindowRect(c.Handle, ref rect);
            Point p = this.PointToClient(rect.Location);
            return new Rectangle(p, rect.Size);

            //Point p = c.PointToScreen(Point.Empty);
            //p = this.PointToClient(p);
            //return new Rectangle(p, c.Size);
        }

        private struct HighlightRegion
        {
            public Rectangle Bounds;
            public Color BackColor;
            public eHighlightColor HighlightColor;
            /// <summary>
            /// Initializes a new instance of the HighlightRegion structure.
            /// </summary>
            /// <param name="bounds"></param>
            /// <param name="backColor"></param>
            /// <param name="highlightColor"></param>
            public HighlightRegion(Rectangle bounds, Color backColor, eHighlightColor highlightColor)
            {
                Bounds = bounds;
                BackColor = backColor;
                HighlightColor = highlightColor;
            }
        }

        private Control _FocusHighlightControl;
        public Control FocusHighlightControl
        {
            get { return _FocusHighlightControl; }
            set { _FocusHighlightControl = value; }
        }

        private eHighlightColor _FocusHighlightColor = eHighlightColor.Blue;
        public eHighlightColor FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set { _FocusHighlightColor = value; }
        }

        private Color[] _CustomHighlightColors = null;
        /// <summary>
        /// Gets or sets the array of colors used to render custom highlight color. Control expects 3 colors in array to be specified which define the highlight border.
        /// </summary>
        public Color[] CustomHighlightColors
        {
            get { return _CustomHighlightColors; }
            set
            {
                _CustomHighlightColors = value;
            }
        }
        #endregion
    }
}
#endif