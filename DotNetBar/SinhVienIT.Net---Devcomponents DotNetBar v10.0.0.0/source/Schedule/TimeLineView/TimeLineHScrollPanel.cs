#if FRAMEWORK20
using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Schedule
{
    public class TimeLineHScrollPanel : BaseItem
    {
        #region Events

        public event EventHandler<EventArgs> ScrollPanelChanged;

        #endregion

        #region Private variables

        private CalendarView _CalendarView;      // CalendarView

        private HScrollBarAdv _ScrollBar;       // Scroll bar
        private PageNavigator _PageNavigator;   // PageNavigator

        private int _UpdateCount;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">_CalendarView</param>
        public TimeLineHScrollPanel(CalendarView calendarView)
        {
            _CalendarView = calendarView;
            Name = "TimeLineHScrollPanel";

            SetUpPanel();

            Visible = false;
        }

        #region Public properties

        /// <summary>
        /// Gets and sets the display bounds
        /// </summary>
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                if (base.Bounds != value)
                {
                    base.Bounds = value;

                    UpdatePanel();
                }
            }
        }

        /// <summary>
        /// Gets and sets the visible status
        /// </summary>
        public override bool Visible
        {
            get { return (base.Visible); }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    _ScrollBar.Visible = value;

                    if (_PageNavigator != null)
                        _PageNavigator.Visible = value;
                }
            }
        }

        /// <summary>
        /// Gets the ScrollBar
        /// </summary>
        public HScrollBarAdv ScrollBar
        {
            get { return (_ScrollBar); }
        }

        #endregion

        #region Internal properties

        internal PageNavigator PageNavigator
        {
            get { return (_PageNavigator); }
        }

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the scrollBar SmallChange value
        /// </summary>
        private int ScrollPanelSmallChange
        {
            get { return (1); }
        }

        /// <summary>
        /// Gets the scrollBar LargeChange value
        /// </summary>
        private int ScrollPanelLargeChange
        {
            get { return (ScrollBar.Width / _CalendarView.TimeLineColumnWidth); }
        }

        /// <summary>
        /// Gets the scrollBar Maximum value
        /// </summary>
        private int ScrollPanelMaximum
        {
            get { return (_CalendarView.TimeLineColumnCount); }
        }

        #endregion

        #region HookScrollEvents

        /// <summary>
        /// Hooks our ScrollBar events
        /// </summary>
        /// <param name="hook"></param>
        private void HookScrollEvents(bool hook)
        {
            if (hook == true)
            {
                ScrollBar.Scroll += ScrollBarScroll;
                ScrollBar.ValueChanged += ScrollBarValueChanged;
            }
            else
            {
                ScrollBar.Scroll += ScrollBarScroll;
                ScrollBar.ValueChanged += ScrollBarValueChanged;
            }
        }

        #endregion

        #region HookNavigateEvents

        /// <summary>
        /// Hooks our PageNavigator events
        /// </summary>
        /// <param name="hook"></param>
        private void HookNavigateEvents(bool hook)
        {
            if (_PageNavigator != null)
            {
                if (hook == true)
                {
                    _PageNavigator.NavigateNextPage += NavigateNextPage;
                    _PageNavigator.NavigatePreviousPage += NavigatePreviousPage;
                    _PageNavigator.NavigateToday += NavigateToday;
                }
                else
                {
                    _PageNavigator.NavigateNextPage -= NavigateNextPage;
                    _PageNavigator.NavigatePreviousPage -= NavigatePreviousPage;
                    _PageNavigator.NavigateToday -= NavigateToday;
                }
            }
        }

        #endregion

        #region Event handling

        #region ScrollBar_Scroll

        /// <summary>
        /// ScrollBar Scroll event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollBarScroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.SmallIncrement)
            {
                // The user has right-arrow scrolled to the very end of
                // the scrollbar, so add another time slice to timeline
                
                if (ScrollBar.Value >= ScrollBar.Maximum - ScrollBar.LargeChange)
                    IncreaseEndDate(1);
            }
            else if (e.Type == ScrollEventType.SmallDecrement)
            {
                // The user has left-arrow scrolled to the very beginning of
                // the scrollbar, so add another time slice to timeline

                if (ScrollBar.Value == 0)
                    DecreaseStartDate(1);
            }
        }

        #endregion

        #region ScrollBar_ValueChanged

        /// <summary>
        /// Processes ScrollBar ValueChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollBarValueChanged(object sender, EventArgs e)
        {
            OnScrollPanelUpdate();
        }

        #endregion

        #region NavigatePreviousPage

        /// <summary>
        /// Navigates to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NavigatePreviousPage(object sender, EventArgs e)
        {
            DateTime startDate;

            int n = ScrollBar.LargeChange;
            int value = ScrollBar.Value - n;

            if (value < 0)
            {
                startDate = GetDecreaseStartDate(n);
            }
            else
            {
                startDate = 
                    _CalendarView.TimeLineViewStartDate.AddMinutes(
                    _CalendarView.BaseInterval * -n);
            }

            if (_CalendarView.DoPageNavigatorClick(_CalendarView.TimeLineView,
                _PageNavigator, PageNavigatorButton.PreviousPage, ref startDate) == false)
            {
                _CalendarView.TimeLineViewStartDate = GetIntervalStartDate(startDate);
            }
        }

        #endregion

        #region NavigateToday

        /// <summary>
        /// Navigates to Today
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NavigateToday(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Today;

            if (_CalendarView.DoPageNavigatorClick(_CalendarView.TimeLineView,
                _PageNavigator, PageNavigatorButton.Today, ref startDate) == false)
            {
                if (startDate < _CalendarView.TimeLineViewScrollStartDate ||
                    startDate > _CalendarView.TimeLineViewScrollEndDate)
                {
                    _CalendarView.TimeLineViewScrollStartDate =
                        GetIntervalStartDate(startDate);
                }
            }
        }

        #endregion

        #region NavigateNextPage

        /// <summary>
        /// Navigates to the Next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NavigateNextPage(object sender, EventArgs e)
        {
            DateTime startDate;

            int n = ScrollBar.LargeChange;
            int value = ScrollBar.Value + n;

            if (value > ScrollBar.Maximum - n)
            {
                startDate = GetIncreaseEndDate(n);
            }
            else
            {
                startDate =
                    _CalendarView.TimeLineViewStartDate.AddMinutes(
                    _CalendarView.BaseInterval * n);
            }

            if (_CalendarView.DoPageNavigatorClick(_CalendarView.TimeLineView,
                _PageNavigator, PageNavigatorButton.NextPage, ref startDate) == false)
            {
                _CalendarView.TimeLineViewStartDate = GetIntervalStartDate(startDate);
            }
        }

        #endregion

        #region IncreaseEndDate

        /// <summary>
        /// Increases timeline EndDate
        /// </summary>
        /// <param name="n">Amount to add</param>
        private void IncreaseEndDate(int n)
        {
            if (_CalendarView.TimeLineCanExtendRange == true)
            {
                _CalendarView.TimeLineViewEndDate = GetIncreaseEndDate(n);

                int value = ScrollBar.Value + n;

                if (value > ScrollBar.Maximum - ScrollBar.LargeChange)
                    value = ScrollBar.Maximum - ScrollBar.LargeChange;

                if (ScrollBar.Value != value)
                    ScrollBar.Value = value;
                else
                    OnScrollPanelUpdate();
            }
        }

        #endregion

        #region GetIncreaseEndDate

        /// <summary>
        /// Increases timeline EndDate
        /// </summary>
        /// <param name="n">Amount to add</param>
        private DateTime GetIncreaseEndDate(int n)
        {
            if (_CalendarView.TimeLineCanExtendRange == true)
            {
                DateTime end = new DateTime(9900, 1, 1);

                try
                {
                    DateTime date =
                        _CalendarView.TimeLineViewEndDate.AddMinutes(
                            _CalendarView.BaseInterval * n);

                    if (date > end)
                        date = end;

                    return (date);
                }
                catch
                {
                    return (end);
                }
            }

            return (_CalendarView.TimeLineViewEndDate);
        }

        #endregion

        #region DecreaseStartDate

        /// <summary>
        /// Decreases the timeline StartDate
        /// </summary>
        /// <param name="n">Amount to del</param>
        private void DecreaseStartDate(int n)
        {
            if (_CalendarView.TimeLineCanExtendRange == true)
            {
                DateTime startDate = GetDecreaseStartDate(n);

                _CalendarView.TimeLineViewStartDate = startDate;

                UpdatePanel();
            }
        }

        #endregion

        #region GetDecreaseStartDate

        /// <summary>
        /// Decreases the timeline StartDate
        /// </summary>
        /// <param name="n">Amount to del</param>
        private DateTime GetDecreaseStartDate(int n)
        {
            if (_CalendarView.TimeLineCanExtendRange == true)
            {
                try
                {
                    return (_CalendarView.TimeLineViewStartDate.AddMinutes(
                            -_CalendarView.BaseInterval * n));
                }
                catch
                {
                    return (new DateTime(1, 1, 1));
                }
            }

            return (_CalendarView.TimeLineViewStartDate);
        }

        #endregion

        #region GetIntervalStartDate

        private DateTime GetIntervalStartDate(DateTime startDate)
        {
            TimeSpan ts = new TimeSpan(startDate.Hour, startDate.Minute, 0);

            int interval = _CalendarView.TimeLineInterval;
            int minutes = ((int)ts.TotalMinutes / interval) * interval;

            return (startDate.Date.AddMinutes(minutes));
        }

        #endregion

        #endregion

        #region Begin/EndUpdate

        /// <summary>
        /// Begins Update block
        /// </summary>
        public void BeginUpdate()
        {
            _UpdateCount++;
        }

        /// <summary>
        /// Ends update block
        /// </summary>
        public void EndUpdate()
        {
            if (_UpdateCount == 0)
            {
                throw new InvalidOperationException(
                    "EndUpdate must be called After BeginUpdate");
            }

            _UpdateCount--;

            if (_UpdateCount == 0)
                OnScrollPanelUpdate();
        }

        #endregion

        #region SetUpPanel

        /// <summary>
        /// Performs panel setup
        /// </summary>
        private void SetUpPanel()
        {
            Control c = (Control)_CalendarView.CalendarPanel.GetContainerControl(true);

            if (c != null)
            {
                SetupPageNavigator(c);
                SetupScrollBar(c);

                UpdatePanel();
            }
        }

        #endregion

        #region SetupPageNavigator

        /// <summary>
        /// Sets-up the PageNavigator
        /// </summary>
        /// <param name="c"></param>
        private void SetupPageNavigator(Control c)
        {
            _PageNavigator = new PageNavigator();

            _PageNavigator.Visible = false;
            _PageNavigator.FocusCuesEnabled = false;

            HookNavigateEvents(true);

            c.Controls.Add(_PageNavigator);

            _PageNavigator.NextPageTooltip = _CalendarView.TimeLinePageNavigatorNextPageTooltip;
            _PageNavigator.PreviousPageTooltip = _CalendarView.TimeLinePageNavigatorPreviousPageTooltip;
            _PageNavigator.TodayTooltip = _CalendarView.TimeLinePageNavigatorTodayTooltip;
        }

        #endregion

        #region SetupScrollBar

        private void SetupScrollBar(Control c)
        {
            _ScrollBar = new HScrollBarAdv();

            _ScrollBar.Visible = false;
            _ScrollBar.Height = SystemInformation.HorizontalScrollBarHeight;

            HookScrollEvents(true);

            c.Controls.Add(_ScrollBar);
        }

        #endregion

        #region UpdatePanel

        /// <summary>
        /// Updates the panel
        /// </summary>
        public void UpdatePanel()
        {
            if (Bounds.Width > 0)
            {
                UpdatePageNavigator();
                UpdateScrollBar();

                OnScrollPanelUpdate();
            }
        }

        #endregion

        #region UpdatePageNavigator

        /// <summary>
        /// Updates the PageNavigator
        /// </summary>
        private void UpdatePageNavigator()
        {
            if (_CalendarView.TimeLineShowPageNavigation == true)
            {
                // We are to show the PageNavigator, so allocate it
                // if we haven't done so already

                if (_PageNavigator == null)
                {
                    Control c = (Control)_CalendarView.CalendarPanel.GetContainerControl(true);

                    if (c != null)
                    {
                        SetupPageNavigator(c);

                        if (_PageNavigator != null)
                            _PageNavigator.Visible = Visible;
                    }
                }
            }
            else
            {
                // We are not to show a PageNavigator, so if we have already
                // allocated one, see that we release it appropriately

                if (_PageNavigator != null)
                {
                    Control c = (Control)_CalendarView.CalendarPanel.GetContainerControl(true);

                    if (c != null)
                    {
                        c.Controls.Remove(_PageNavigator);

                        HookNavigateEvents(false);

                        _PageNavigator = null;
                    }
                }
            }
        }

        #endregion

        #region UpdateScrollBar

        /// <summary>
        /// Updates our ScrollBar
        /// </summary>
        private void UpdateScrollBar()
        {
            _ScrollBar.Maximum = ScrollPanelMaximum;
            _ScrollBar.SmallChange = ScrollPanelSmallChange;
            _ScrollBar.LargeChange = ScrollPanelLargeChange;

            if (_ScrollBar.Value > _ScrollBar.Maximum - _ScrollBar.LargeChange)
                _ScrollBar.Value = _ScrollBar.Maximum - _ScrollBar.LargeChange;

            _ScrollBar.Refresh();
        }

        #endregion

        #region OnScrollPanelUpdate

        /// <summary>
        /// Passes the scroll onto others
        /// </summary>
        protected void OnScrollPanelUpdate()
        {
            if (_UpdateCount == 0)
            {
                if (ScrollPanelChanged != null)
                    ScrollPanelChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs control recalc
        /// </summary>
        public override void RecalcSize()
        {
            // Update our PageNavigator

            UpdatePageNavigator();

            int navWidth = 0;

            if (_PageNavigator != null)
            {
                navWidth = _PageNavigator.Width;

                _PageNavigator.Location =
                    new Point(Bounds.Right - navWidth, Bounds.Y);
            }

            // Update our ScrollBar

            UpdateScrollBar();

            if (_ScrollBar != null)
            {
                _ScrollBar.Location = Bounds.Location;
                _ScrollBar.Width = Bounds.Width - navWidth;
            }

            base.RecalcSize();
        }

        #endregion

        #region Paint

        public override void Paint(ItemPaintArgs p)
        {
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item
        /// </summary>
        public override BaseItem Copy()
        {
            TimeLineHScrollPanel objCopy = new TimeLineHScrollPanel(_CalendarView);

            this.CopyToItem(objCopy);

            return (objCopy);
        }
        /// <summary>
        /// Copies the TimeLineHScrollPanel specific properties to
        /// new instance of the item
        /// </summary>
        /// <param name="copy">New PageNavigatorItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            TimeLineHScrollPanel c = copy as TimeLineHScrollPanel;
            base.CopyToItem(c);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                HookScrollEvents(false);
                HookNavigateEvents(false);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
#endif