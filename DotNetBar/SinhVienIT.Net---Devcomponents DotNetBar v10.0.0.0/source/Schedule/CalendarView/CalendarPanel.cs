#if FRAMEWORK20
using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Schedule
{
    public class CalendarPanel : BaseItem
    {
        #region Private variables

        private CalendarView _CalendarView;         // Associated CalendarView

        private HScrollBarAdv _HScrollBar;          // Horizontal scroll bar
        private int _HScrollPos;
        private int _HsMaximum;

        private VScrollBarAdv _VScrollBar;          // Vertical scroll bar
        private int _VScrollPos;
        private int _VsMaximum;

        #endregion

        /// <summary>
        /// Constructor.
        /// This is the main container panel for all
        /// BaseView and TimeRulerPanel objects
        /// </summary>
        /// <param name="calendarView">CalendarView</param>
        public CalendarPanel(CalendarView calendarView)
        {
            _CalendarView = calendarView;

            SetIsContainer(true);

            _CalendarView.SelectedViewChanged += _CalendarView_SelectedViewChanged;
        }

        #region Internal properties

        /// <summary>
        /// Gets the Horizontal scrollbar
        /// </summary>
        internal HScrollBarAdv HScrollBar
        {
            get { return (_HScrollBar); }
        }

        /// <summary>
        /// Gets the Vertical scrollbar
        /// </summary>
        internal VScrollBarAdv VScrollBar
        {
            get { return (_VScrollBar); }
        }

        internal bool Checkit
        {
            get { return (m_CheckMouseMovePressed); }
            set { m_CheckMouseMovePressed = value; }
        }

        #endregion

        #region Event processing

        /// <summary>
        /// Handles SelectedView changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _CalendarView_SelectedViewChanged(object sender, SelectedViewEventArgs e)
        {
            if (e.OldValue == eCalendarView.TimeLine)
            {
                if (_VScrollBar != null)
                    _VScrollBar.Visible = false;
            }
            else if (e.NewValue == eCalendarView.TimeLine)
            {
                if (_HScrollBar != null)
                    _HScrollBar.Visible = false;
            }
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs object recalc processing
        /// </summary>
        public override void RecalcSize()
        {
            if (SubItems.Count > 0)
            {
                if (Bounds.Width > 0 && Bounds.Height > 0)
                {
                    switch (_CalendarView.SelectedView)
                    {
                        case eCalendarView.Day:
                        case eCalendarView.Week:
                            RecalcWeekDay();
                            break;

                        case eCalendarView.Month:
                            RecalcMonth();
                            break;

                        case eCalendarView.Year:
                            RecalcYear();
                            break;

                        case eCalendarView.TimeLine:
                            RecalcTimeLine();
                            break;
                    }

                    base.RecalcSize();
                }
            }
        }

        #region RecalcWeekDay

        /// <summary>
        /// Recalculates all WeekDay views
        /// </summary>
        private void RecalcWeekDay()
        {
            // Calculate the necessary params for calculating
            // the divided width for each subItems in our panel

            int trWidth = 0;
            int vsWidth = 0;

            int vCount = SubItems.Count;

            if (SubItems[0] is VScrollPanel)
            {
                vCount--;
                vsWidth = _CalendarView.VsWidth;

                if (SubItems[1] is TimeRulerPanel)
                {
                    vCount--;
                    trWidth = _CalendarView.TimeRulerWidth;

                }
            }

            // Configure the subItems based upon the
            // previously calculated display params

            if (vCount > 0)
                ConfigureWeekDayItems(vCount, trWidth, vsWidth);
        }

        #region ConfigureWeekDayItems

        /// <summary>
        /// Configures the CalendarPanel subItems
        /// </summary>
        /// <param name="vCount">View count</param>
        /// <param name="trWidth">TimeRuler width</param>
        /// <param name="vsWidth">Vert Scrollbar width</param>
        private void ConfigureWeekDayItems(int vCount, int trWidth, int vsWidth)
        {
            Rectangle r = Bounds;

            // Calculate our default viewWidth

            int bvWidth = Bounds.Width - (trWidth + vsWidth);
            float viewWidth = (float)bvWidth / vCount;

            // Sync our current item params with our current
            // horizontal scrollBar states

            ConfigDhScrollBar(ref r,
                ref viewWidth, vCount, trWidth, vsWidth);

            // If we have a TimRulerPanel, then
            // set it's location and size

            if (trWidth > 0)
            {
                r.X = Bounds.X;
                r.Width = trWidth;

                SubItems[1].Bounds = r;
                SubItems[1].RecalcSize();
            }

            // Loop through each BaseView item,
            // setting it's location and size 

            _CalendarView.StartSlice = -1;
            _CalendarView.NumberOfSlices = 0;

            float fxPos = Bounds.X + trWidth + _HScrollPos;
            BaseView view = null;

            int n = vCount;

            for (int i = 0; i < SubItems.Count; i++)
            {
                if (SubItems[i] is BaseView)
                {
                    r.X = (int)fxPos;
                    fxPos += viewWidth;

                    r.Width = (int)(fxPos - r.X - (--n > 0 ? 2 : 0));

                    SubItems[i].Bounds = r;
                    SubItems[i].Displayed = r.IntersectsWith(Bounds);

                    if (SubItems[i].Displayed == true)
                    {
                        view = SubItems[i] as BaseView;

                        if (view != null)
                        {
                            view.NeedRecalcLayout = true;
                            view.RecalcSize();
                        }
                    }
                }
            }

            // If we need to align the WorkDay times, then
            // make sure all displayed WeekDay views are kept in sync

            if (_CalendarView.ShowOnlyWorkDayHours == true && _CalendarView.IsMultiCalendar == true)
            {
                for (int i = 0; i < SubItems.Count; i++)
                {
                    WeekDayView wv = SubItems[i] as WeekDayView;

                    if (wv != null && wv.Displayed == true)
                    {
                        if (wv.LocalStartSlice != _CalendarView.StartSlice ||
                            wv.LocalNumberOfSlices != _CalendarView.NumberOfSlices)
                        {
                            wv.NeedRecalcLayout = true;
                            wv.RecalcSize();
                        }
                    }
                }
            }

            // If we have a VScrollPanel, then
            // set it's location and size

            if (vsWidth > 0)
            {
                r.X = Bounds.Right - vsWidth;
                r.Width = vsWidth;

                int delta = _CalendarView.AllDayPanelHeight;

                r.Y = delta;
                r.Height = 50;

                if (view != null)
                {
                    r.Y += view.ClientRect.Y + view.DayOfWeekHeaderHeight;
                    r.Height = Math.Max(r.Height, view.ClientRect.Height - (delta + view.DayOfWeekHeaderHeight));
                }

                SubItems[0].Bounds = r;
                SubItems[0].RecalcSize();
            }
        }

        #endregion

        #endregion

        #region RecalcMonth

        /// <summary>
        /// Recalculates Month views
        /// </summary>
        private void RecalcMonth()
        {
            // Configure the subItems based upon the
            // previously calculated display params

            if (SubItems.Count > 0)
                ConfigureMonthItems(SubItems.Count);
        }

        #region ConfigureMonthItems

        /// <summary>
        /// Configures the CalendarPanel subItems
        /// </summary>
        /// <param name="vCount">View count</param>
        private void ConfigureMonthItems(int vCount)
        {
            Rectangle r = Bounds;

            // Calculate our default viewWidth

            float viewWidth = (float)Bounds.Width / vCount;

            // Sync our current item params with our current
            // horizontal scrollBar states

            ConfigDhScrollBar(ref r, ref viewWidth, vCount, 0, 0);

            // Loop through each BaseView item,
            // setting it's location and size 

            float fxPos = Bounds.X + _HScrollPos;
            int n = vCount;

            for (int i = 0; i < SubItems.Count; i++)
            {
                if (SubItems[i] is BaseView)
                {
                    r.X = (int)fxPos;
                    fxPos += viewWidth;

                    r.Width = (int)(fxPos - r.X - (--n > 0 ? 2 : 0));

                    SubItems[i].Bounds = r;
                    SubItems[i].Displayed = r.IntersectsWith(Bounds);

                    if (SubItems[i].Displayed == true)
                    {
                        BaseView view = SubItems[i] as BaseView;

                        if (view != null)
                        {
                            view.NeedRecalcLayout = true;
                            view.RecalcSize();
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region RecalcYear

        /// <summary>
        /// Recalculates all Year views
        /// </summary>
        private void RecalcYear()
        {
            // Calculate the necessary params for calculating
            // the divided width for each subItems in our panel

            int vsWidth = 0;

            int vCount = SubItems.Count;

            if (SubItems[0] is VScrollPanel)
            {
                vCount--;
                vsWidth = _CalendarView.VsWidth;
            }

            // Configure the subItems based upon the
            // previously calculated display params

            if (vCount > 0)
                ConfigureYearItems(vCount, vsWidth);
        }

        #region ConfigureYearItems

        /// <summary>
        /// Configures the CalendarPanel subItems
        /// </summary>
        /// <param name="vCount">View count</param>
        /// <param name="vsWidth">Vert Scrollbar width</param>
        private void ConfigureYearItems(int vCount, int vsWidth)
        {
            Rectangle r = Bounds;

            // Calculate our default viewWidth

            int bvWidth = Bounds.Width;
            float viewWidth = (float) bvWidth/vCount;

            // Sync our current item params with our current
            // horizontal scrollBar states

            ConfigDhScrollBar(ref r, ref viewWidth, vCount, 0, vsWidth);

            // Locate and calculate the default size of our YearView

            BaseView view = null;

            for (int i = 0; i < SubItems.Count; i++)
            {
                view = SubItems[i] as BaseView;

                if (view != null)
                {
                    Rectangle t = r;
                    t.Width = (int) viewWidth;

                    SubItems[i].Bounds = t;

                    view.NeedRecalcLayout = true;
                    view.RecalcSize();

                    break;
                }
            }

            if (view != null)
            {
                // If we have a VScrollPanel, then
                // set it's location and size

                if (vsWidth > 0)
                {
                    if (_CalendarView.YearViewMax + 1 == view.ClientRect.Height)
                    {
                        SubItems[0].Visible = false;
                        ((VScrollPanel)SubItems[0]).DisableScrollBar();
                    }
                    else
                    {
                        bvWidth = Bounds.Width - vsWidth;
                        viewWidth = (float)bvWidth / vCount;

                        r = Bounds;
                        ConfigDhScrollBar(ref r, ref viewWidth, vCount, 0, vsWidth);

                        Rectangle t = r;
                        t.Y += view.ClientRect.Y;
                        t.Height = Math.Max(50, view.ClientRect.Height);

                        t.X = Bounds.Right - vsWidth;
                        t.Width = vsWidth;

                        SubItems[0].Bounds = t;
                        SubItems[0].RecalcSize();
                        SubItems[0].Visible = true;
                    }
                }

                // Loop through each BaseView item,
                // setting it's location and size

                float fxPos = Bounds.X + _HScrollPos;
                int n = vCount;

                for (int i = 0; i < SubItems.Count; i++)
                {
                    if (SubItems[i] is BaseView)
                    {
                        r.X = (int) fxPos;
                        fxPos += viewWidth;

                        r.Width = (int) (fxPos - r.X - (--n > 0 ? 2 : 0));

                        SubItems[i].Bounds = r;
                        SubItems[i].Displayed = r.IntersectsWith(Bounds);

                        if (SubItems[i].Displayed == true)
                        {
                            view = SubItems[i] as BaseView;

                            if (view != null)
                            {
                                view.NeedRecalcLayout = true;
                                view.RecalcSize();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region RecalcTimeLine

        /// <summary>
        /// Recalculates TimeLine views
        /// </summary>
        private void RecalcTimeLine()
        {
            int vCount = SubItems.Count;

            if (SubItems.Contains("TimeLineHScrollPanel"))
                vCount--;

            if (SubItems.Contains("TimeLineHeaderPanel"))
                vCount--;

            if (vCount > 0)
                ConfigureTItems(vCount);
        }

        #region ConfigureTItems

        /// <summary>
        /// Configures TimeLine view items
        /// </summary>
        /// <param name="vCount"></param>
        private void ConfigureTItems(int vCount)
        {
            Rectangle r = Bounds;

            r.Height -= _CalendarView.HsHeight;

            // TimeLineHeaderPanel

            int headerHeight = 0;
            int iHeaderPanel = SubItems.IndexOf("TimeLineHeaderPanel");

            if (iHeaderPanel >= 0)
            {
                if (iHeaderPanel != 0)
                {
                    BaseItem bi = SubItems[iHeaderPanel];

                    SubItems.RemoveAt(iHeaderPanel);
                    SubItems.Insert(0, bi);

                    iHeaderPanel = 0;
                }

                if (_CalendarView.TimeLineShowIntervalHeader == true)
                    headerHeight += _CalendarView.TimeLineIntervalHeaderHeight;

                if (_CalendarView.TimeLineShowPeriodHeader == true)
                    headerHeight += _CalendarView.TimeLinePeriodHeaderHeight;

                r.Y += headerHeight;
                r.Height -= headerHeight;
            }

            // Calculate our viewHeight

            int vsWidth = 0;

            float viewHeight = (float)r.Height / vCount;

            if (_CalendarView.TimeLineHeight > viewHeight)
            {
                viewHeight = _CalendarView.TimeLineHeight;
                vsWidth = _CalendarView.VsWidth;
            }

            // VScrollPanel

            ConfigureTvScrollBar(r, vCount, vsWidth);

            // Sync our current item params with our current
            // horizontal scrollBar states

            r.Width -= vsWidth;

            Rectangle r2 = r;

            if (_CalendarView.IsMultiCalendar == true &&
                _CalendarView.ShowTabs == true)
            {
                if (_CalendarView.TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                {
                    r2.X += _CalendarView.MultiUserTabHeight + 4;
                    r2.Width -= (_CalendarView.MultiUserTabHeight + 4);
                }
                else
                {
                    r2.X += _CalendarView.TimeLineMultiUserTabWidth + 4;
                    r2.Width -= (_CalendarView.TimeLineMultiUserTabWidth + 4);
                }
            }

            // TimeLineHeaderPanel

            if (iHeaderPanel >= 0)
            {
                r2.Y = Bounds.Y;
                r2.Height = headerHeight;

                SubItems[iHeaderPanel].Bounds = r2;
                SubItems[iHeaderPanel].RecalcSize();
            }

            // TimeLineView

            r2 = r;

            float fxPos = r2.Y + _VScrollPos;

            for (int i = 0; i < SubItems.Count; i++)
            {
                if (SubItems[i] is TimeLineView)
                {
                    r2.Y = (int)fxPos;
                    fxPos += viewHeight;

                    r2.Height = (int)(fxPos - r2.Y);

                    SubItems[i].Bounds = r2;
                    SubItems[i].Displayed = r2.IntersectsWith(Bounds);

                    if (SubItems[i].Displayed == true)
                        SubItems[i].RecalcSize();
                }
            }

            // If we have an HScrollPanel, then
            // set it's location and size

            int iScrollPanel = SubItems.IndexOf("TimeLineHScrollPanel");

            if (iScrollPanel >= 0)
            {
                if (_CalendarView.IsMultiCalendar == true &&
                    _CalendarView.ShowTabs == true)
                {
                    if (_CalendarView.TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                    {
                        r.X += (_CalendarView.MultiUserTabHeight + 4);
                        r.Width -= (_CalendarView.MultiUserTabHeight + 4);
                    }
                    else
                    {
                        r.X += (_CalendarView.TimeLineMultiUserTabWidth + 4);
                        r.Width -= (_CalendarView.TimeLineMultiUserTabWidth + 4);
                    }
                }

                r.Y = Bounds.Bottom - _CalendarView.HsHeight;
                r.Height = _CalendarView.HsHeight;

                SubItems[iScrollPanel].Bounds = r;
                SubItems[iScrollPanel].RecalcSize();
            }
        }

        #endregion

        #endregion

        #region Horizontal scrollbar support

        #region ConfigDhScrollBar

        /// <summary>
        /// Configures default view horizontal scrollBar
        /// </summary>
        /// <param name="r"></param>
        /// <param name="viewWidth"></param>
        /// <param name="vCount"></param>
        /// <param name="trWidth"></param>
        /// <param name="vsWidth"></param>
        private void ConfigDhScrollBar(ref Rectangle r,
            ref float viewWidth, int vCount, int trWidth, int vsWidth)
        {
            if (_CalendarView.ViewWidth > viewWidth)
            {
                viewWidth = _CalendarView.ViewWidth + 2;
                r.Height -= _CalendarView.HsHeight;

                _HsMaximum = (int)(viewWidth * vCount);

                if (_HScrollBar == null)
                    SetUpDhScrollBar();

                if (_HScrollBar != null)
                {
                    UpdateDhScrollBar(trWidth, vsWidth);

                    if (_HScrollBar.Value > _HsMaximum - _HScrollBar.LargeChange + 1)
                    {
                        _HScrollPos = -_HsMaximum - _HScrollBar.LargeChange;
                        _HScrollBar.Value = _HsMaximum - _HScrollBar.LargeChange;
                    }
                }
            }
            else
            {
                if (_HScrollBar != null)
                {
                    _HScrollPos = 0;
                    _HScrollBar.Value = 0;

                    _HScrollBar.Visible = false;
                }
            }
        }

        #endregion

        #region SetUpDhScrollBar

        /// <summary>
        /// Sets up default view horizontal scrollbar
        /// </summary>
        private void SetUpDhScrollBar()
        {
            _HScrollBar = new HScrollBarAdv();
            _HScrollBar.Height = _CalendarView.HsHeight;

            Control c = (Control)this.GetContainerControl(true);

            if (c != null)
                c.Controls.Add(_HScrollBar);

            _HScrollBar.ValueChanged += DhScrollBarValueChanged;
        }

        #endregion

        #region UpdateDhScrollBar

        /// <summary>
        /// Updates default view horizontal scrollbar
        /// </summary>
        private void UpdateDhScrollBar(int trWidth, int vsWidth)
        {
            Point pt = new Point(Bounds.X + trWidth,
                Bounds.Bottom - _CalendarView.HsHeight);

            _HScrollBar.Location = pt;
            _HScrollBar.Width = Math.Max(50, Bounds.Width - (trWidth + vsWidth));

            _HScrollBar.SmallChange = 10;
            _HScrollBar.LargeChange = _HScrollBar.Width;
            _HScrollBar.Maximum = _HsMaximum;

            _HScrollBar.Visible = true;
            _HScrollBar.BringToFront();

            _HScrollBar.Refresh();
        }

        #endregion

        #region DhScrollBarValueChanged

        /// <summary>
        /// Processes default view horizontal
        /// scrollbar value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DhScrollBarValueChanged(object sender, EventArgs e)
        {
            if (_HScrollPos != -_HScrollBar.Value)
            {
                _HScrollPos = -_HScrollBar.Value;

                NeedRecalcSize = true;
                Refresh();
            }
        }

        #endregion

        #endregion

        #region Vertical scrollbar support

        #region ConfigureTvScrollBar

        /// <summary>
        /// Configures TimeLine View vertical scrollbar
        /// </summary>
        /// <param name="r"></param>
        /// <param name="vCount"></param>
        /// <param name="vsWidth"></param>
        private void ConfigureTvScrollBar(Rectangle r, int vCount, int vsWidth)
        {
            if (vsWidth > 0)
            {
                _VsMaximum = _CalendarView.TimeLineHeight * vCount + 2;

                if (_VScrollBar == null)
                    SetUpTvScrollBar();

                if (_VScrollBar != null)
                {
                    UpdateTvScrollBar(r, vsWidth);

                    if (_VScrollBar.Value > _VsMaximum - _VScrollBar.LargeChange + 2)
                        _VScrollBar.Value = _VsMaximum - _VScrollBar.LargeChange + 2;
                }
            }
            else
            {
                if (_VScrollBar != null)
                {
                    _VScrollPos = 0;
                    _VScrollBar.Value = 0;

                    _VScrollBar.Visible = false;
                }
            }
        }

        #endregion

        #region SetUpTvScrollBar

        /// <summary>
        /// Sets up TimeLine vertical scrollbar
        /// </summary>
        private void SetUpTvScrollBar()
        {
            _VScrollBar = new VScrollBarAdv();
            _VScrollBar.Width = _CalendarView.VsWidth;

            Control c = (Control)this.GetContainerControl(true);

            if (c != null)
                c.Controls.Add(_VScrollBar);

            _VScrollBar.ValueChanged += VScrollBar_ValueChanged;
        }

        #endregion

        #region UpdateTvScrollBar

        /// <summary>
        /// Updates our vertical scrollbar
        /// </summary>
        /// <param name="r"></param>
        /// <param name="vsWidth"></param>
        private void UpdateTvScrollBar(Rectangle r, int vsWidth)
        {
            Point pt = new Point(r.Right - vsWidth, r.Top);

            _VScrollBar.Location = pt;
            _VScrollBar.Height = Math.Max(30, r.Height);

            _VScrollBar.SmallChange = _CalendarView.TimeLineHeight;
            _VScrollBar.LargeChange = _VScrollBar.Height;
            _VScrollBar.Maximum = _VsMaximum;

            _VScrollBar.Visible = true;
            _VScrollBar.BringToFront();

            _VScrollBar.Refresh();
        }

        #endregion

        #region VScrollBar_ValueChanged

        /// <summary>
        /// Processes TimeLine vertical scrollbar changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_VScrollPos != -_VScrollBar.Value)
            {
                _VScrollPos = -_VScrollBar.Value;

                NeedRecalcSize = true;
                Refresh();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(ItemPaintArgs e)
        {
            if (SubItems.Count > 0)
            {
                PaintNonViewItems(e);
                PaintBaseViewItems(e);
            }
        }

        /// <summary>
        /// Paints the non-BaseView items
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        private void PaintNonViewItems(ItemPaintArgs e)
        {
            // Loop through each item, initiating
            // a paint on each one

            for (int i = 0; i < SubItems.Count; i++)
            {
                if ((SubItems[i] is BaseView) == false)
                {
                    if (SubItems[i].Bounds.IntersectsWith(e.ClipRectangle))
                        SubItems[i].Paint(e);
                }
            }
        }

        /// <summary>
        /// Paints the BaseView items
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        private void PaintBaseViewItems(ItemPaintArgs e)
        {
            Rectangle r = Bounds;

            if (_CalendarView.SelectedView == eCalendarView.TimeLine)
            {
                int n = 0;

                if (_CalendarView.TimeLineShowIntervalHeader == true)
                    n += _CalendarView.TimeLineIntervalHeaderHeight;
                
                if (_CalendarView.TimeLineShowPeriodHeader == true)
                    n += _CalendarView.TimeLinePeriodHeaderHeight;

                r.Y += n;
                r.Height -= (_CalendarView.HsHeight + n);
            }
            else
            {
                if (SubItems[0] is VScrollPanel)
                {
                    if (SubItems[0].Visible == true)
                        r.Width -= _CalendarView.VsWidth;
                }

                if (SubItems.Count > 1 && SubItems[1] is TimeRulerPanel)
                {
                    r.X = _CalendarView.TimeRulerWidth;
                    r.Width -= _CalendarView.TimeRulerWidth;
                }
            }

            Region rgnSave = e.Graphics.Clip;

            using (Region rgn = new Region(r))
            {
                e.Graphics.Clip = rgn;

                for (int i = 0; i < SubItems.Count; i++)
                {
                    if (SubItems[i] is BaseView)
                    {
                        if (SubItems[i].Displayed == true)
                            SubItems[i].Paint(e);
                    }
                }
            }

            e.Graphics.Clip = rgnSave;
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CalendarPanel objCopy = new CalendarPanel(_CalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the CalendarPanel specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New CalendarPanel instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            CalendarPanel objCopy = copy as CalendarPanel;
            base.CopyToItem(objCopy);
        }

        #endregion
    }
}
#endif

