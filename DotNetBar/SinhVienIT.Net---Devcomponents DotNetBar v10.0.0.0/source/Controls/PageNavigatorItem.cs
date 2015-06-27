#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the PageNavigate item
    /// </summary>
    [Browsable(false), Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class PageNavigatorItem : BaseItem
    {
        #region Events

        /// <summary>
        /// Occurs when NavigateNextPage button is clicked
        /// </summary>
        [Description("Occurs when NavigateNextPage button is clicked.")]
        public event EventHandler NavigateNextPage;

        /// <summary>
        /// Occurs when NavigateToday button is clicked
        /// </summary>
        [Description("Occurs when NavigateToday button is clicked.")]
        public event EventHandler NavigateToday;

        /// <summary>
        /// Occurs when NavigatePreviousPage button is clicked
        /// </summary>
        [Description("Occurs when NavigatePreviousPage button is clicked.")]
        public event EventHandler NavigatePreviousPage;

        #endregion

        #region Private Variables

        private ePageNavigatorPart _MouseOverPart = ePageNavigatorPart.None;
        private ePageNavigatorPart _MouseDownPart = ePageNavigatorPart.None;

        private Rectangle _PreviousPageBounds;
        private Rectangle _TodayBounds;
        private Rectangle _NextPageBounds;

        private string _PreviousPageTooltip = "";
        private string _TodayTooltip = "";
        private string _NextPageTooltip = "";

        private Bitmap _PreviousPageBitmap;
        private Bitmap _TodayBitmap;
        private Bitmap _NextPageBitmap;

        private Timer _ClickTimer;
        private int _AutoClickCount;

        private int _ButtonSize = SystemInformation.VerticalScrollBarWidth;
        private int _MinWidth = SystemInformation.VerticalScrollBarWidth * 3;

        #endregion

        /// <summary>
        /// Creates new instance of PageNavigateItem
        /// </summary>
        public PageNavigatorItem() : this("") { }

        /// <summary>
        /// Creates new instance of PageNavigateItem and assigns the name to it
        /// </summary>
        /// <param name="itemName">Item name</param>
        public PageNavigatorItem(string itemName)
        {
            this.Name = itemName;

            this.ClickRepeatInterval = 200;

            this.MouseUpNotification = true;
            this.MouseDownCapture = true;

            this.Size = new Size(_MinWidth, _ButtonSize);
        }

        #region Internal properties

        #region MouseOverPart

        /// <summary>
        /// Gets or sets the MouseOverPart
        /// </summary>
        internal ePageNavigatorPart MouseOverPart
        {
            get { return _MouseOverPart; }

            set
            {
                if (_MouseOverPart != value)
                {
                    _MouseOverPart = value;

                    UpdateTooltip();

                    this.Refresh();
                }
            }
        }

        #endregion

        #region MouseDownPart

        /// <summary>
        /// Gets or sets the MouseDownPart
        /// </summary>
        internal ePageNavigatorPart MouseDownPart
        {
            get { return _MouseDownPart; }

            set
            {
                if (_MouseDownPart != value)
                {
                    _MouseDownPart = value;

                    this.Refresh();
                }
            }
        }

        #endregion

        #endregion

        #region Public properties

        #region Orientation

        /// <summary>
        /// Gets or sets the control orientation. Default value is Horizontal
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance")]
        [Description("Indicates PageNavigator orientation.")]
        public override eOrientation Orientation
        {
            get { return (base.Orientation); }

            set
            {
                if (base.Orientation != value)
                {
                    base.Orientation = value;

                    // Release our current cached bitmaps

                    ReleaseBitmap(ref _NextPageBitmap);
                    ReleaseBitmap(ref _PreviousPageBitmap);

                    // Reset our size back to the
                    // default for the new orientation

                    this.Size = (value == eOrientation.Horizontal) ? 
                        new Size(_MinWidth, _ButtonSize) : new Size(_ButtonSize, _MinWidth);

                    NeedRecalcSize = true;

                    this.Refresh();
                }
            }
        }

        #endregion

        #region ClickAutoRepeat

        /// <summary>
        /// Gets or sets whether Click event will be auto repeated
        /// when mouse button is kept pressed over the item
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(false), Category("Behavior")]
        [Description("Gets or sets whether Click event will be auto repeated when the mouse button is kept pressed over the item.")]
        public override bool ClickAutoRepeat
        {
            get { return (base.ClickAutoRepeat); }
            set { base.ClickAutoRepeat = value; }
        }

        #endregion

        #region ClickRepeatInterval

        /// <summary>
        /// Gets or sets the auto-repeat interval for the click event
        /// when mouse button is kept pressed over the item
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(200), Category("Behavior")]
        [Description("Gets or sets the auto-repeat interval for the click event when the mouse button is kept pressed over the item.")]
        public override int ClickRepeatInterval
        {
            get { return (base.ClickRepeatInterval); }
            set { base.ClickRepeatInterval = value; }
        }

        #endregion

        #region PreviousPageTooltip

        /// <summary>
        /// Gets or sets the tooltip for the PreviousPage button of the control
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the PreviousPage button of the control.")]
        public string PreviousPageTooltip
        {
            get { return _PreviousPageTooltip; }

            set
            {
                _PreviousPageTooltip = value;

                UpdateTooltip();
            }
        }

        #endregion

        #region TodayTooltip

        /// <summary>
        /// Gets or sets the tooltip for the Today button
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the TodayPage button of the control.")]
        public string TodayTooltip
        {
            get { return (_TodayTooltip); }

            set
            {
                _TodayTooltip = value;

                UpdateTooltip();
            }
        }

        #endregion

        #region NextPageTooltip

        /// <summary>
        /// Gets or sets the tooltip for the NextPage button
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the NextPage button of the control.")]
        public string NextPageTooltip
        {
            get { return (_NextPageTooltip); }

            set
            {
                _NextPageTooltip = value;

                UpdateTooltip();
            }
        }

        #endregion

        #endregion

        #region RecalcSize

        /// <summary>
        /// Handles size recalc
        /// </summary>
        public override void RecalcSize()
        {
            // Update our layout

            UpdateLayout();

            base.RecalcSize();
        }

        #endregion

        #region OnExternalSizeChange

        /// <summary>
        /// Handles external size changes
        /// </summary>
        protected override void OnExternalSizeChange()
        {
            UpdateLayout();

            base.OnExternalSizeChange();
        }

        #endregion

        #region UpdateLayout

        /// <summary>
        /// Lays out our control based upon its
        /// vertical / horizontal orientation
        /// </summary>
        private void UpdateLayout()
        {
            Rectangle r = Bounds;

            if (Orientation == eOrientation.Horizontal)
            {
                int dx = r.Width / 3;

                _PreviousPageBounds = new Rectangle(r.Left, r.Top, dx, r.Height);
                _NextPageBounds = new Rectangle(r.Right - dx, r.Top, dx, r.Height);

                _TodayBounds = new Rectangle(r.Left + dx, r.Top, r.Width - dx * 2, r.Height);
            }
            else
            {
                int dy = r.Height / 3;

                _PreviousPageBounds = new Rectangle(r.Left, r.Top, r.Width, dy);
                _NextPageBounds = new Rectangle(r.Left, r.Bottom - dy, r.Width, dy);

                _TodayBounds = new Rectangle(r.Left, r.Y + dy, r.Width, r.Height - dy * 2);
            }
        }

        #endregion

        #region Paint

        /// <summary>
        /// Handles control rendering
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            Office2007ScrollBarColorTable colorTable =
                ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ScrollBar;

            DrawPreviousPage(g, colorTable);
            DrawToday(g, colorTable);
            DrawNextPage(g, colorTable);
        }

        #region DrawPreviousPage

        /// <summary>
        /// Draws the PreviousPage button
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="ct">Office2007ScrollBarColorTable</param>
        private void DrawPreviousPage(Graphics g, Office2007ScrollBarColorTable ct)
        {
            Office2007ScrollBarStateColorTable cst =
                GetPageColorTable(ct, ePageNavigatorPart.PreviousPage);

            float angle = (Orientation == eOrientation.Horizontal) ? 90f : 0f;
            
            using (LinearGradientBrush lbr = new LinearGradientBrush(
                _PreviousPageBounds, cst.Background.Start, cst.Background.End, angle))
            {
                lbr.InterpolationColors =
                    cst.TrackBackground.GetColorBlend();

                g.FillRectangle(lbr, _PreviousPageBounds);
            }

            g.DrawImageUnscaled(
                GetPreviousPageBitmap(g, cst),
                CenterRect(_PreviousPageBounds));
        }

        #region GetPreviousPageBitmap

        /// <summary>
        /// Gets the PreviousPage bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap GetPreviousPageBitmap(
            Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            if (_PreviousPageBitmap == null)
                _PreviousPageBitmap = CreatePreviousPageBitmap(g, cst);

            return (_PreviousPageBitmap);
        }

        /// <summary>
        /// Creates the PreviousPage bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap CreatePreviousPageBitmap(
            Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            Bitmap bmp = new Bitmap(10, 10, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    float angle;

                    if (Orientation == eOrientation.Horizontal)
                    {
                        angle = 90;

                        path.AddLines(new Point[] {
                            new Point(8,0),
                            new Point(4,4),
                            new Point(8,8)
                            });

                        path.CloseFigure();

                        path.AddLines(new Point[] {
                            new Point(4,0),
                            new Point(0,4),
                            new Point(4,8)
                            });

                        path.CloseFigure();
                    }
                    else
                    {
                        angle = 0;

                        path.AddLines(new Point[] {
                            new Point(0,5),
                            new Point(4,0),
                            new Point(8,5)
                            });

                        path.CloseFigure();

                        path.AddLines(new Point[] {
                            new Point(0,9),
                            new Point(4,4),
                            new Point(8,9)
                            });

                        path.CloseFigure();
                    }

                    Rectangle r = new Rectangle(0, 0, 10, 10);

                    using (LinearGradientBrush lbr = new
                        LinearGradientBrush(r, cst.ThumbSignBackground.Start, cst.ThumbSignBackground.End, angle))
                    {
                        gBmp.FillPath(lbr, path);
                    }
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region DrawToday

        /// <summary>
        /// Draws the Today button
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="ct">Office2007ScrollBarColorTable</param>
        private void DrawToday(Graphics g, Office2007ScrollBarColorTable ct)
        {
            Office2007ScrollBarStateColorTable cst =
                GetPageColorTable(ct, ePageNavigatorPart.Today);

            float angle = (Orientation == eOrientation.Horizontal) ? 90f : 0f;

            using (LinearGradientBrush lbr = new LinearGradientBrush(
                _TodayBounds, cst.Background.Start, cst.Background.End, angle))
            {
                lbr.InterpolationColors = cst.TrackBackground.GetColorBlend();

                g.FillRectangle(lbr, _TodayBounds);
            }

            g.DrawImageUnscaled(
                GetTodayBitmap(g, cst),
                CenterRect(_TodayBounds));
        }

        #region GetTodayBitmap

        /// <summary>
        /// Gets the Today Bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap GetTodayBitmap(
            Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            if (_TodayBitmap == null)
                _TodayBitmap = CreateTodayBitmap(g, cst);

            return (_TodayBitmap);
        }

        /// <summary>
        /// Creates the Today Bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap CreateTodayBitmap(
            Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            Bitmap bmp = new Bitmap(10, 10, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                Rectangle r = new Rectangle(1, 1, 6, 6);

                gBmp.SmoothingMode = SmoothingMode.HighQuality;

                Color color = ControlPaint.LightLight(cst.ThumbSignBackground.Start);

                using (SolidBrush br = new SolidBrush(color))
                {
                    gBmp.FillEllipse(br, r);
                }

                using (Pen pen = new Pen(cst.ThumbSignBackground.End))
                {
                    gBmp.DrawEllipse(pen, r);
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region DrawNextPage

        /// <summary>
        /// Draws the NextPage button
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ct"></param>
        private void DrawNextPage(Graphics g, Office2007ScrollBarColorTable ct)
        {
            Office2007ScrollBarStateColorTable cst =
                GetPageColorTable(ct, ePageNavigatorPart.NextPage);

            float angle = (Orientation == eOrientation.Horizontal) ? 90f : 0f;

            using (LinearGradientBrush lbr = new LinearGradientBrush(
                _NextPageBounds, cst.Background.Start, cst.Background.End, angle))
            {
                lbr.InterpolationColors = cst.TrackBackground.GetColorBlend();

                g.FillRectangle(lbr, _NextPageBounds);
            }

            g.DrawImageUnscaled(
                GetNextPageBitmap(g, cst),
                CenterRect(_NextPageBounds));
        }

        #region GetNextPageBitmap

        /// <summary>
        /// Gets the NextPage Bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap GetNextPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            if (_NextPageBitmap == null)
                _NextPageBitmap = CreateNextPageBitmap(g, cst);

            return (_NextPageBitmap);
        }

        /// <summary>
        /// Creates the NextPage Bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        private Bitmap CreateNextPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
        {
            Bitmap bmp = new Bitmap(10, 10, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    float angle;

                    if (Orientation == eOrientation.Horizontal)
                    {
                        angle = 90;

                        path.AddLines(new Point[] {
                            new Point(1,0),
                            new Point(5,4), 
                            new Point(1,8)
                            });

                        path.CloseFigure();

                        path.AddLines(new Point[] {
                            new Point(5,0),
                            new Point(9,4),
                            new Point(5,8)
                            });

                        path.CloseFigure();
                    }
                    else
                    {
                        angle = 0;

                        path.AddLines(new Point[] {
                            new Point(1,0),
                            new Point(8,0),
                            new Point(4,4)
                            });

                        path.CloseFigure();

                        path.AddLines(new Point[] {
                            new Point(1,4),
                            new Point(8,4),
                            new Point(4,8)
                            });

                        path.CloseFigure();
                    }

                    Rectangle r = new Rectangle(0, 0, 10, 10);

                    using (LinearGradientBrush lbr = new
                        LinearGradientBrush(r, cst.ThumbSignBackground.Start, cst.ThumbSignBackground.End, angle))
                    {
                        gBmp.FillPath(lbr, path);
                    }
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region GetPageColorTable

        private Office2007ScrollBarStateColorTable
            GetPageColorTable(Office2007ScrollBarColorTable ct, ePageNavigatorPart part)
        {
            if (GetEnabled() == false)
                return (ct.Disabled);

            if (MouseDownPart == part)
                return (ct.Pressed);

            if (MouseOverPart == part)
                return (ct.MouseOver);

            return (ct.Default);
        }

        #endregion

        #region CenterRect

        /// <summary>
        /// Centers the given rect
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private Rectangle CenterRect(Rectangle r)
        {
            r.X += (r.Width - 10) / 2 + 1;
            r.Y += (r.Height - 10) / 2 + 1;

            if (r.X < 0)
                r.X = 0;

            if (r.Y < 0)
                r.Y = 0;

            return (r);
        }

        #endregion

        #endregion

        #region Mouse support

        #region InternalMouseMove

        /// <summary>
        /// Processes InternalMouseMove events
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            if (GetEnabled() == true && DesignMode == false)
            {
                if (MouseDownPart == ePageNavigatorPart.None)
                    MouseOverPart = HitTest(objArg.Location);
            }

            base.InternalMouseMove(objArg);
        }

        #region HitTest

        /// <summary>
        /// Returns the HitText area for the given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public ePageNavigatorPart HitTest(Point pt)
        {
            if (_PreviousPageBounds.Contains(pt))
                return (ePageNavigatorPart.PreviousPage);

            if (_NextPageBounds.Contains(pt))
                return (ePageNavigatorPart.NextPage);

            if (_TodayBounds.Contains(pt))
                return (ePageNavigatorPart.Today);

            return (ePageNavigatorPart.None);
        }

        #endregion

        #endregion

        #region InternalMouseLeave

        /// <summary>
        /// Processes Mouse Leave events
        /// </summary>
        public override void InternalMouseLeave()
        {
            DisposeClickTimer();

            MouseOverPart = ePageNavigatorPart.None;
            MouseDownPart = ePageNavigatorPart.None;

            base.InternalMouseLeave();
        }

        #endregion

        #region InternalMouseDown

        /// <summary>
        /// Processes Mouse Down events
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            if (objArg.Button == MouseButtons.Left)
            {
                if (GetEnabled() && DesignMode == false)
                {
                    MouseDownPart = HitTest(objArg.Location);

                    if (MouseDownPart != ePageNavigatorPart.None)
                    {
                        switch (MouseDownPart)
                        {
                            case ePageNavigatorPart.PreviousPage:
                                OnNavigatePreviousPage();
                                break;

                            case ePageNavigatorPart.NextPage:
                                OnNavigateNextPage();
                                break;

                            default:
                                OnNavigateToday();
                                break;
                        }
                    }
                }
            }

            base.InternalMouseDown(objArg);
        }

        #endregion

        #region InternalMouseUp

        /// <summary>
        /// Processes Mouse Up events
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            DisposeClickTimer();

            MouseDownPart = ePageNavigatorPart.None;

            base.InternalMouseUp(objArg);
        }

        #endregion

        #endregion

        #region OnNavigate

        /// <summary>
        /// Raises the NavigatePreviousPage event
        /// </summary>
        protected virtual void OnNavigatePreviousPage()
        {
            if (NavigatePreviousPage != null)
                NavigatePreviousPage(this, EventArgs.Empty);

            EnableClickTimer();
        }

        /// <summary>
        /// Raises the NavigateToday event
        /// </summary>
        protected virtual void OnNavigateToday()
        {
            if (NavigateToday != null)
                NavigateToday(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the NavigateNextPage event
        /// </summary>
        protected virtual void OnNavigateNextPage()
        {
            if (NavigateNextPage != null)
                NavigateNextPage(this, EventArgs.Empty);

            EnableClickTimer();
        }

        #endregion

        #region Timer support

        #region ClickTimerTick

        /// <summary>
        /// Handles timer click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickTimerTick(object sender, EventArgs e)
        {
            switch (MouseDownPart)
            {
                case ePageNavigatorPart.PreviousPage:
                    OnNavigatePreviousPage();
                    break;

                case ePageNavigatorPart.NextPage:
                    OnNavigateNextPage();
                    break;
            }

            // Auto ramp-up

            if (_ClickTimer != null)
            {
                _AutoClickCount++;

                if (_AutoClickCount > 4 && _ClickTimer.Interval > 20)
                    _ClickTimer.Interval -= 10;
            }
        }

        #endregion

        #region UpdateClickTimer

        /// <summary>
        /// Enables our click timer
        /// </summary>
        private void EnableClickTimer()
        {
            if (ClickRepeatInterval > 0)
            {
                if (_ClickTimer == null)
                {
                    _ClickTimer = new Timer();

                    _ClickTimer.Interval = ClickRepeatInterval;
                    _ClickTimer.Tick += ClickTimerTick;

                    _ClickTimer.Start();

                    _AutoClickCount = 0;
                }
            }
        }

        #endregion

        #region DisposeClickTimer

        /// <summary>
        /// Disposes of the click timer
        /// </summary>
        private void DisposeClickTimer()
        {
            if (_ClickTimer != null)
            {
                _ClickTimer.Tick -= ClickTimerTick;

                _ClickTimer.Stop();
                _ClickTimer.Dispose();

                _ClickTimer = null;
            }
        }

        #endregion

        #endregion

        #region OnTooltipChanged

        /// <summary>
        /// OnTooltipChanged
        /// </summary>
        protected override void OnTooltipChanged()
        {
            UpdateTooltip();

            base.OnTooltipChanged();
        }

        #endregion

        #region UpdateTooltip

        /// <summary>
        /// Updates the control tooltip
        /// </summary>
        private void UpdateTooltip()
        {
            if (this.DesignMode == false)
            {
                string tip = "";

                switch (MouseOverPart)
                {
                    case ePageNavigatorPart.PreviousPage:
                        tip = _PreviousPageTooltip;
                        break;

                    case ePageNavigatorPart.Today:
                        tip = _TodayTooltip;
                        break;

                    case ePageNavigatorPart.NextPage:
                        tip = _NextPageTooltip;
                        break;
                }

                if (tip.Equals("") == false)
                {
                    if (m_Tooltip != tip)
                    {
                        m_Tooltip = tip;

                        if (this.ToolTipVisible)
                            this.ShowToolTip();
                    }
                }
            }
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item
        /// </summary>
        public override BaseItem Copy()
        {
            PageNavigatorItem objCopy = new PageNavigatorItem(Name);

            this.CopyToItem(objCopy);

            return (objCopy);
        }
        /// <summary>
        /// Copies the PageNavigatorItem specific properties to
        /// new instance of the item
        /// </summary>
        /// <param name="copy">New PageNavigatorItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            PageNavigatorItem c = copy as PageNavigatorItem;

            if (c != null)
            {
                base.CopyToItem(c);

                c.PreviousPageTooltip = _PreviousPageTooltip;
                c.TodayTooltip = _TodayTooltip;
                c.NextPageTooltip = _NextPageTooltip;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            DisposeClickTimer();

            ReleaseBitmap(ref _PreviousPageBitmap);
            ReleaseBitmap(ref _TodayBitmap);
            ReleaseBitmap(ref _NextPageBitmap);

            base.Dispose(disposing);
        }

        /// <summary>
        /// Releases the given bitmap
        /// </summary>
        /// <param name="bmp">Bitmap to release</param>
        private void ReleaseBitmap(ref Bitmap bmp)
        {
            if (bmp != null)
            {
                bmp.Dispose();
                bmp = null;
            }
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// Defines the PageNavigator item parts
    /// </summary>
    public enum ePageNavigatorPart
    {
        /// <summary>
        /// Indicates no part
        /// </summary>
        None,

        /// <summary>
        /// Indicates the PreviousPage button of the control
        /// </summary>
        PreviousPage,

        /// <summary>
        /// Indicates the TodayPage button of the control
        /// </summary>
        Today,

        /// <summary>
        /// Indicates the NextPage button of the control
        /// </summary>
        NextPage,
    }

    #endregion

}
#endif