#if FRAMEWORK20
using System;
using System.Collections.Generic;
using DevComponents.DotNetBar.ScrollBar;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    public class TimeLineView : BaseView
    {
        #region Static variables

        static private AppointmentColor _appointmentColor = new AppointmentColor();

        #endregion

        #region Private variables

        private CalendarWeekDayColor _ViewColor =       // View display color table
            new CalendarWeekDayColor(eCalendarColor.Automatic);

        private List<CalendarItem>
            _CalendarItems = new List<CalendarItem>();

        private Point _LastMovePoint;           // Last mouse move Point
        private Point _LastPointOffset;         // Last Point offset
        private Rectangle _LastBounds;          // MouseDown item bounds

        private int _SelectedColStart;          // Column start selection
        private int _SelectedColEnd;            // Column end selection

        private Timer _ScrollViewTimer;         // Timer used to implement auto view scrolling
        private int _ScrollDwell;               // Scroll dwell (pause, throttle) counter

        private Color _WorkColor;               // Cached Brush colors
        private Color _OffWorkColor;
        private Color _SelectedColor;

        private Brush _WorkBrush;               // Cached Brushes
        private Brush _OffWorkBrush;
        private Brush _SelectedBrush;

        private int _HScrollPos;                // Horizontal scroll position

        private List<ColumnList> _CondensedColList;
        private List<int> _CollateLines = new List<int>();

        private bool _ModelReloaded = true;

        #endregion

        public TimeLineView(CalendarView calendarView, eCalendarView eCalendarView)
            : base(calendarView, eCalendarView)
        {
            // Set our non-client drawing info and CalendarColor

            NClientData = new NonClientData(
                eTabOrientation.Vertical,
                (int)eCalendarWeekDayPart.OwnerTabBorder,
                (int)eCalendarWeekDayPart.OwnerTabForeground,
                (int)eCalendarWeekDayPart.OwnerTabBackground,
                (int)eCalendarWeekDayPart.OwnerTabContentBackground,
                (int)eCalendarWeekDayPart.OwnerTabSelectedForeground,
                (int)eCalendarWeekDayPart.OwnerTabSelectedBackground);

            CalendarColorTable = _ViewColor;

            // Hook onto our events

            HookEvents(true);

            if (calendarView.TimeLineHScrollPanel.ScrollBar != null)
            {
                _HScrollPos = -calendarView.TimeLineHScrollPanel.ScrollBar.Value *
                    calendarView.TimeLineColumnWidth;
            }
        }

        #region Public properties

        #region CalendarItems

        /// <summary>
        /// Gets array of CalendarItems
        /// </summary>
        public List<CalendarItem> CalendarItems
        {
            get { return (_CalendarItems); }
        }

        #endregion

        #region StartDate

        /// <summary>
        /// Start date - readonly
        /// </summary>
        public new DateTime StartDate
        {
            get { return (base.StartDate); }
            internal set { base.StartDate = value; }
        }

        #endregion

        #region EndDate

        /// <summary>
        /// End date - readonly
        /// </summary>
        public new DateTime EndDate
        {
            get { return (base.EndDate); }
            internal set { base.EndDate = value; }
        }

        #endregion

        #region ColumnWidth

        /// <summary>
        /// Gets the ColumnWidth
        /// </summary>
        public int ColumnWidth
        {
            get { return (CalendarView.TimeLineColumnWidth); }
        }

        #endregion

        #region BaseInterval

        /// <summary>
        /// Gets the BaseInterval (interval in total minutes)
        /// </summary>
        public double BaseInterval
        {
            get { return (CalendarView.BaseInterval); }
        }

        #endregion

        #region TimeLineColumnCount

        /// <summary>
        /// Gets the number of Columns
        /// </summary>
        public int TimeLineColumnCount
        {
            get { return (CalendarView.TimeLineColumnCount); }
        }

        #endregion

        #endregion

        #region Private properties

        #region ShowCondensed

        /// <summary>
        /// Gets the CondensedView visibility state
        /// </summary>
        private bool ShowCondensed
        {
            get
            {
                if (CalendarView.TimeLineCondensedViewVisibility == eCondensedViewVisibility.Hidden)
                    return (false);

                if (CalendarView.TimeLineCondensedViewHeight < 10)
                    return (false);

                if (CalendarView.TimeLineCondensedViewVisibility == eCondensedViewVisibility.AllResources)
                    return (true);

                return (CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex);
            }
        }

        #endregion

        #region Cached Brushes

        #region WorkBrush

        /// <summary>
        /// Gets and sets the Work time brush
        /// </summary>
        private Brush WorkBrush
        {
            get
            {
                Color color = _ViewColor.GetColor(
                    (int)eCalendarWeekDayPart.DayWorkHoursBackground);

                if (_WorkColor != color)
                {
                    _WorkColor = color;

                    WorkBrush = new SolidBrush(color);
                }

                return (_WorkBrush);
            }

            set
            {
                if (_WorkBrush != value)
                {
                    if (_WorkBrush != null)
                        _WorkBrush.Dispose();

                    _WorkBrush = value;
                }
            }
        }

        #endregion

        #region OffWorkBrush

        /// <summary>
        /// Gets and sets the Off-hours work time brush
        /// </summary>
        private Brush OffWorkBrush
        {
            get
            {
                Color color = _ViewColor.GetColor(
                    (int)eCalendarWeekDayPart.DayOffWorkHoursBackground);

                if (_OffWorkColor != color)
                {
                    _OffWorkColor = color;

                    OffWorkBrush = new SolidBrush(color);
                }

                return (_OffWorkBrush);
            }

            set
            {
                if (_OffWorkBrush != value)
                {
                    if (_OffWorkBrush != null)
                        _OffWorkBrush.Dispose();

                    _OffWorkBrush = value;
                }
            }
        }

        #endregion

        #region SelectedBrush

        /// <summary>
        /// Gets and sets the selected brush
        /// </summary>
        private Brush SelectedBrush
        {
            get
            {
                Color color = _ViewColor.GetColor(
                    (int)eCalendarWeekDayPart.SelectionBackground);

                if (_SelectedColor != color)
                {
                    _SelectedColor = color;

                    SelectedBrush = new SolidBrush(color);
                }

                return (_SelectedBrush);
            }

            set
            {
                if (_SelectedBrush != value)
                {
                    if (_SelectedBrush != null)
                        _SelectedBrush.Dispose();

                    _SelectedBrush = value;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the first visible timeline column
        /// </summary>
        internal int FirstVisibleColumn
        {
            get { return (-_HScrollPos / ColumnWidth); }
        }

        /// <summary>
        /// Gets the condensed time line height
        /// </summary>
        internal int CondensedLineHeight
        {
            get { return (CalendarView.TimeLineCondensedViewHeight); }
        }

        /// <summary>
        /// Gets and sets the model reload state
        /// </summary>
        internal bool ModelReloaded
        {
            get { return (_ModelReloaded); }
            set { _ModelReloaded = value; }
        }

        #endregion

        #region Hook / Unhook Events

        /// <summary>
        /// Routine hooks all necessary events for this control
        /// </summary>
        /// <param name="hook">True to hook, false to unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                CalendarView.SelectedViewChanged += SelectedViewChanged;
                CalendarView.TimeLineViewStartDateChanged += TimeLineViewStartDateChanged;
                CalendarView.TimeLineViewEndDateChanged += TimeLineViewEndDateChanged;
                CalendarView.TimeLineIntervalChanged += TimeLineIntervalChanged;
                CalendarView.TimeLineIntervalPeriodChanged += TimeLineIntervalPeriodChanged;
                CalendarView.TimeLineHScrollPanel.ScrollPanelChanged += ScrollPanelChanged;
            }
            else
            {
                CalendarView.SelectedViewChanged -= SelectedViewChanged;
                CalendarView.TimeLineViewStartDateChanged -= TimeLineViewStartDateChanged;
                CalendarView.TimeLineViewEndDateChanged -= TimeLineViewEndDateChanged;
                CalendarView.TimeLineIntervalChanged -= TimeLineIntervalChanged;
                CalendarView.TimeLineIntervalPeriodChanged -= TimeLineIntervalPeriodChanged;
                CalendarView.TimeLineHScrollPanel.ScrollPanelChanged -= ScrollPanelChanged;
            }
        }

        #endregion

        #region Event handling routines

        #region SelectedViewChanged

        /// <summary>
        /// Processes view changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectedViewEventArgs</param>
        void SelectedViewChanged(object sender, SelectedViewEventArgs e)
        {
            // Update our IsViewSelected state

            IsViewSelected = (e.NewValue == ECalendarView);

            if (IsViewSelected == true)
            {
                AutoSyncViewDate(e.OldValue);
                UpdateDateSelection();
            }
            else
            {
                ResetView();
            }
        }

        #endregion

        #region TimeLineViewStartDateChanged

        /// <summary>
        /// Processes StartDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineViewStartDateChanged(object sender, DateChangeEventArgs e)
        {
            StartDate = e.NewValue;
        }

        #endregion

        #region TimeLineViewEndDateChanged

        /// <summary>
        /// Processes EndDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineViewEndDateChanged(object sender, DateChangeEventArgs e)
        {
            EndDate = e.NewValue;
        }

        #endregion

        #region TimeLineIntervalPeriodChanged

        /// <summary>
        /// Handles IntervalPeriodChange notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineIntervalPeriodChanged(object sender, TimeLineIntervalPeriodChangedEventArgs e)
        {
            NeedRecalcSize = true;
            NeedRecalcLayout = true;
        }

        #endregion

        #region TimeLineIntervalChanged

        /// <summary>
        /// Handles IntervalChange notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineIntervalChanged(object sender, TimeLineIntervalChangedEventArgs e)
        {
            NeedRecalcSize = true;
            NeedRecalcLayout = true;
        }

        #endregion

        #region ScrollPanelChanged

        /// <summary>
        /// Handles ScrollPanel change notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollPanelChanged(object sender, EventArgs e)
        {
            TimeLineHScrollPanel panel = sender as TimeLineHScrollPanel;

            if (panel != null)
            {
                HScrollBarAdv hScrollBar = panel.ScrollBar;

                _HScrollPos = -hScrollBar.Value * CalendarView.TimeLineColumnWidth;

                // Redraw our view

                if (Displayed == true)
                {
                    NeedRecalcLayout = true;

                    InvalidateRect(true);
                    Refresh();
                }
            }
        }

        #endregion

        #endregion

        #region GetViewAreaFromPoint

        /// <summary>
        /// Gets the view area under the given mouse
        /// point (tab, header, content, etc)
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>eViewArea</returns>
        public override eViewArea GetViewAreaFromPoint(Point pt)
        {
            if (Bounds.Contains(pt) == true)
            {
                if (pt.X >= ClientRect.X)
                {
                    // CondensedView

                    if (ShowCondensed == true)
                    {
                        if (GetCondensedRect().Contains(pt) == true)
                            return (eViewArea.InCondensedView);
                    }

                    return (eViewArea.InContent);
                }

                return (base.GetViewAreaFromPoint(pt));
            }

            return (eViewArea.NotInView);
        }

        #endregion

        #region GetDateSelectionFromPoint

        /// <summary>
        /// Gets the date selection from the given point. The startDate
        /// and endDate will vary based upon the view type
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <param name="startDate">out start date</param>
        /// <param name="endDate">out end date</param>
        /// <returns>True if a valid selection exists
        /// at the given point</returns>
        public override bool GetDateSelectionFromPoint(
            Point pt, out DateTime startDate, out DateTime endDate)
        {
            base.GetDateSelectionFromPoint(pt, out startDate, out endDate);

            int col;

            if (GetPointItem(pt, out col, true) == true)
            {
                startDate = CalendarView.TimeLineAddInterval(StartDate, col);
                endDate = CalendarView.TimeLineAddInterval(startDate, 1);

                return (true);
            }

            return (false);
        }

        #endregion

        #region SetSelectedItem

        /// <summary>
        /// Handles selected item changes
        /// </summary>
        /// <param name="sender">CalendarItem</param>
        /// <param name="e">EventArgs</param>
        public void ItemIsSelectedChanged(object sender, EventArgs e)
        {
            CalendarItem ci = sender as CalendarItem;

            if (ci != null)
            {
                if (ci.IsSelected == true)
                {
                    if (SelectedItem != null && ci != SelectedItem)
                        SelectedItem.IsSelected = false;

                    SelectedItem = ci;
                }
                else
                {
                    if (ci == SelectedItem)
                        SelectedItem = null;
                }

                // Make sure the selection is reflected
                // in the condensed view (if present)

                if (ShowCondensed == true)
                    InvalidateRect(GetCondensedRect());
            }
        }

        /// <summary>
        /// Sets the current selected item
        /// </summary>
        /// <param name="pci">Previous CalendarItem</param>
        /// <param name="nci">New CalendarItem</param>
        /// <returns>New selected CalendarItem</returns>
        protected override CalendarItem SetSelectedItem(CalendarItem pci, CalendarItem nci)
        {
            if (nci != null)
                nci.IsSelected = true;

            else if (pci != null)
                pci.IsSelected = false;

            return (nci);
        }

        #endregion

        #region UpdateDateSelection

        /// <summary>
        /// Updates our slice selection range to reflect
        /// the given date selection start and end values
        /// </summary>
        protected override void UpdateDateSelection()
        {
            if (IsViewSelected == true)
            {
                // Get the new absolute slice selection range

                int colStart = GetDateCol(DateSelectionStart);
                int colEnd = GetDateCol(DateSelectionEnd);

                // Limit our range to only those columns
                // that are visible on the screen

                int c1 = -_HScrollPos / ColumnWidth;
                int c2 = c1 + ClientRect.Width / ColumnWidth + 1;

                if (c2 > c1)
                {
                    ProcessSelRange(colStart, colEnd, c1, c2);

                    // Save our new selection range

                    _SelectedColStart = colStart;
                    _SelectedColEnd = colEnd;
                }

                if (ShowCondensed == true)
                {
                    Rectangle r = GetCondensedRect();

                    this.InvalidateRect(r);
                }
            }
        }

        /// <summary>
        /// Processes the selection time column range
        /// </summary>
        /// <param name="colStart">Column range start</param>
        /// <param name="colEnd">Column range end</param>
        /// <param name="c1">Column start limit</param>
        /// <param name="c2">Column end limit</param>
        private void ProcessSelRange(int colStart, int colEnd, int c1, int c2)
        {
            bool[] oldSelected = SelectedColumns(c1, c2, _SelectedColStart, _SelectedColEnd);
            bool[] newSelected = SelectedColumns(c1, c2, colStart, colEnd);

            // Invalidate those slices whose
            // selection status changed

            Rectangle r = GetColRect(c1);

            if (r.Width > 0 && r.Height > 0)
            {
                for (int i = 0; i < c2 - c1; i++)
                {
                    if (oldSelected[i] != newSelected[i])
                        InvalidateRect(r);

                    r.X += ColumnWidth;
                }
            }
        }

        /// <summary>
        /// Gets an array of column selection values
        /// over the given range of columns
        /// </summary>
        /// <param name="c1">Column start limit</param>
        /// <param name="c2">Column end limit</param>
        /// <param name="colStart">Slice range start</param>
        /// <param name="colEnd">Slice range end</param>
        /// <returns>Array of selection values</returns>
        private bool[] SelectedColumns(int c1, int c2, int colStart, int colEnd)
        {
            // Calculate our number of entries and
            // allocate our IsSelected array accordingly

            int n = c2 - c1;
            bool[] sel = new bool[n + 1];

            // Loop through the range of entries determining if
            // the specific col is within the selection range

            for (int i = 0; i < n; i++)
                sel[i] = (c1 + i >= colStart && c1 + i < colEnd);

            // Return the array to the caller

            return (sel);
        }

        #endregion

        #region RecalcSize routines

        /// <summary>
        /// Performs NeedRecalcSize requests
        /// </summary>
        public override void RecalcSize()
        {
            base.RecalcSize();

            if (IsViewSelected == true)
            {
                // Normalize our start and end dates

                DateTime startDate;
                DateTime endDate;
                NormalizeDates(out startDate, out endDate);

                // Update our Model connection view,
                // CalendarItems, and DateSelection

                UpdateView();
                UpdateDateSelection();
            }

            NeedRecalcLayout = false;
        }

        #region NormalizeDates

        /// <summary>
        /// Normalizes the user specified start and end dates
        /// </summary>
        /// <param name="startDate">[out] Normalized start date</param>
        /// <param name="endDate">[out] Normalized end date</param>
        protected virtual void NormalizeDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = this.StartDate;
            endDate = this.EndDate;
        }

        #endregion

        #region UpdateView

        /// <summary>
        /// Updates our connection model view
        /// </summary>
        private void UpdateView()
        {
            // Make sure we have a model connection

            if (Connector == null)
            {
                if (CalendarModel != null)
                    Connector = new ModelTimeLineViewConnector(CalendarModel, this);
            }
            else
            {
                // The timeline range could have changed, so let
                // the connection refresh the data if needed

                ((ModelTimeLineViewConnector)Connector).RefreshData(false);
            }

            // We have a connection, so update our CalendarItems

            if (Connector != null)
            {
                UpdateCalendarItems();

                if (ShowCondensed == true)
                    UpdateCondensedColumnList();

                CalendarView.DoViewLoadComplete(this);
            }
        }

        #endregion

        #region ResetView

        /// <summary>
        /// Disconnects and resets the Model connection
        /// </summary>
        internal override void ResetView()
        {
            _ModelReloaded = true;

            base.ResetView();
        }

        #endregion

        #region UpdateCalendarItems

        /// <summary>
        /// Updates our CalendarItems list
        /// </summary>
        private void UpdateCalendarItems()
        {
            if (NeedRecalcLayout == true)
            {
                _CollateLines.Clear();

                if (_CalendarItems.Count > 0)
                {
                    List<CalendarItem> items = SortCalendarItems(_CalendarItems);

                    if (CalendarView.HasTimeLineGetRowCollateIdCallout)
                    {
                        SortedDictionary<int, List<CalendarItem>> ditems = CollateCalendarItems(items);

                        int dy = 0;

                        foreach (KeyValuePair<int, List<CalendarItem>> kvp in ditems)
                        {
                            ColumnList colList = new ColumnList();

                            for (int i = 0; i < kvp.Value.Count; i++)
                                colList.AddColumnSlot(kvp.Value[i], 0);

                            colList.CountColumns();

                            dy = CalcAppointmentBounds(colList, dy, true);

                            _CollateLines.Add(dy);
                        }
                    }
                    else
                    {
                        ColumnList colList = new ColumnList();

                        for (int i = 0; i < items.Count; i++)
                            colList.AddColumnSlot(items[i], 0);

                        colList.CountColumns();

                        CalcAppointmentBounds(colList, 0, false);
                    }
                }

                NeedRecalcLayout = false;

                InvalidateRect();
            }
        }

        #region CollateCalendarItems

        private SortedDictionary<int, List<CalendarItem>>
            CollateCalendarItems(List<CalendarItem> items)
        {
            SortedDictionary<int, List<CalendarItem>> citems =
                new SortedDictionary<int, List<CalendarItem>>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].CollateId < 0)
                {
                    TimeLineGetRowCollateIdEventArgs e =
                        new TimeLineGetRowCollateIdEventArgs(items[i]);

                    CalendarView.DoTimeLineGetRowCollateId(e);

                    items[i].CollateId = e.CollateId;
                }

                int collateId = items[i].CollateId;

                if (citems.ContainsKey(collateId) == false)
                    citems.Add(collateId, new List<CalendarItem>());

                citems[collateId].Add(items[i]);
            }

            return (citems);
        }

        #endregion

        #region SortCalendarItems

        /// <summary>
        /// Sorts the provided CalendarItems
        /// </summary>
        /// <returns>Sorted CalendarItems</returns>
        private List<CalendarItem> SortCalendarItems(IEnumerable<CalendarItem> clist)
        {
            List<CalendarItem> items = new List<CalendarItem>();

            items.AddRange(clist);

            if (items.Count > 0)
            {
                items.Sort(

                   delegate(CalendarItem c1, CalendarItem c2)
                   {
                       if (c1.StartTime > c2.StartTime)
                           return (1);

                       if (c1.StartTime < c2.StartTime)
                           return (-1);

                       if (c1.EndTime < c2.EndTime)
                           return (1);

                       if (c1.EndTime > c2.EndTime)
                           return (-1);

                       return (0);
                   }
               );
            }

            return (items);
        }

        #endregion

        #region CalcAppointmentBounds

        /// <summary>
        /// Calculates normal appointment bounds
        /// </summary>
        /// <param name="colList">Accumulated ColumnList</param>
        /// <param name="dy"></param>
        /// <param name="collate"></param>
        private int CalcAppointmentBounds(ColumnList colList, int dy, bool collate)
        {
            if (colList.SList.Count > 0)
            {
                int rowSpread = 0;
                int rowHeight = AppointmentHeight;

                // If we are stretching the rows height, then calculate
                // the default row height and the row spread, to evenly
                // distribute the fill remainder

                if (collate == false && 
                    CalendarView.TimeLineStretchRowHeight == true)
                {
                    int height = GetColRect(0).Height;
                    rowHeight = height / colList.SList.Count;

                    if (rowHeight < AppointmentHeight)
                        rowHeight = AppointmentHeight;
                    else
                        rowSpread = height - (colList.SList.Count * rowHeight);
                }

                for (int i = 0; i < colList.SList.Count; i++)
                {
                    int maxRowHeight = 0;

                    for (int j = 0; j < colList.SList[i].Count; j++)
                    {
                        CalendarItem item = colList.SList[i][j].CItem;

                        Rectangle r = new Rectangle();

                        r.Y = ClientRect.Y + dy;
                        r.Height = rowHeight;

                        if (collate == false &&
                            CalendarView.TimeLineStretchRowHeight == true)
                        {
                            if (i < rowSpread)
                                r.Height++;
                        }
                        else
                        {
                            r.Height = GetRowHeight(item, rowHeight);
                        }

                        TimeSpan ts1 = item.StartTime - StartDate;
                        TimeSpan ts2 = item.EndTime - item.StartTime;

                        int pos = (int) ((ts1.TotalMinutes * ColumnWidth) / BaseInterval);
                        int width = (int) ((ts2.TotalMinutes * ColumnWidth) / BaseInterval);

                        if (CalendarView.TimeLinePeriod == eTimeLinePeriod.Years)
                        {
                            int years = item.StartTime.Year - StartDate.Year;

                            pos = (years * ColumnWidth) / CalendarView.TimeLineInterval;
                        }

                        if (width < 20)
                            width = 20;

                        r.X = ClientRect.X + pos + _HScrollPos;
                        r.Width = width;

                        // Now that we have calculated the items height and
                        // width, invoke a Recalc on the item

                        item.WidthInternal = r.Width;
                        item.HeightInternal = r.Height - 1;

                        item.RecalcSize();

                        // Set our bounds for the item

                        r.Width = item.WidthInternal;
                        r.Height = item.HeightInternal;

                        item.Bounds = r;

                        if (item.StartTime != item.EndTime)
                        {
                            if (r.Height + 1 > maxRowHeight)
                                maxRowHeight = r.Height + 1;
                        }

                        // Set it's display state

                        item.Displayed = r.IntersectsWith(ClientRect);
                    }

                    dy += maxRowHeight;
                }
            }

            return (dy);
        }

        #region GetRowHeight

        /// <summary>
        /// Get the RowHeight for the given CalendarItem
        /// </summary>
        /// <param name="item">CalendarItem</param>
        /// <param name="height">Calculated height</param>
        /// <returns></returns>
        private int GetRowHeight(CalendarItem item, int height)
        {
            TimeLineGetRowHeightEventArgs e =
                new TimeLineGetRowHeightEventArgs(item, height);

            CalendarView.DoTimeLineGetRowHeight(e);

            return (e.Height);
        }

        #endregion

        #endregion

        #endregion

        #region UpdateCondensedColumnList

        /// <summary>
        /// Updates the condensed view column list
        /// </summary>
        internal void UpdateCondensedColumnList()
        {
            if (_ModelReloaded == true && Connector != null)
            {
                _CondensedColList = new List<ColumnList>();

                List<CalendarItem> items = new List<CalendarItem>();

                // Get our appointment collection

                GetCondensedAppts(items);
                GetCondensedCustomItems(items);

                // If we have any items, sort them and
                // create a corresponding ColumnList

                if (items.Count > 0)
                {
                    items = SortCalendarItems(items);

                    if (CalendarView.HasTimeLineGetRowCollateIdCallout)
                    {
                        SortedDictionary<int, List<CalendarItem>> ditems = CollateCalendarItems(items);

                        foreach (KeyValuePair<int, List<CalendarItem>> kvp in ditems)
                        {
                            ColumnList colList = new ColumnList();

                            for (int i = 0; i < kvp.Value.Count; i++)
                                colList.AddColumnSlot(kvp.Value[i], 0);

                            colList.CountColumns();

                            _CondensedColList.Add(colList);
                        }
                    }
                    else
                    {
                        _CondensedColList.Add(new ColumnList());

                        for (int i = 0; i < items.Count; i++)
                            _CondensedColList[0].AddColumnSlot(items[i], 0);

                        _CondensedColList[0].CountColumns();
                    }
                }

                _ModelReloaded = false;
            }
        }

        #region GetCondensedAppts

        private void GetCondensedAppts(List<CalendarItem> items)
        {
            AppointmentSubsetCollection appts =
                ((ModelTimeLineViewConnector)Connector).Appts;

            if (appts != null && appts.Count > 0)
            {
                int n = Math.Min(appts.Count, 500);
                n = Math.Max(appts.Count / n, 1);

                for (int i = 0; i < appts.Count; i += n)
                {
                    if (IsAppointmentVisible(appts[i]) == true)
                    {
                        CalendarItem ci = new CalendarItem();

                        ci.StartTime = appts[i].StartTime;
                        ci.EndTime = appts[i].EndTime;
                        ci.ModelItem = appts[i];

                        items.Add(ci);
                    }
                }
            }
        }

        /// <summary>
        /// Determines if an appointment is visible
        /// for the given DisplayOwner
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private bool IsAppointmentVisible(Appointment app)
        {
            if (string.IsNullOrEmpty(DisplayedOwnerKey))
                return (true);

            return (app.OwnerKey == DisplayedOwnerKey);
        }

        #endregion

        #region GetCondensedCustomItems

        private void GetCondensedCustomItems(List<CalendarItem> items)
        {
            for (int i = 0; i < CalendarView.CustomItems.Count; i++)
            {
                CustomCalendarItem item = CalendarView.CustomItems[i];

                if (IsCustomItemVisible(item) == true &&
                    (item.StartTime < EndDate && item.EndTime > StartDate))
                {
                    CalendarItem ci = new CalendarItem();

                    ci.StartTime = item.StartTime;
                    ci.EndTime = item.EndTime;
                    ci.ModelItem = item;

                    items.Add(ci);
                }
            }
        }

        private bool IsCustomItemVisible(CustomCalendarItem item)
        {
            if (string.IsNullOrEmpty(Connector.DisplayOwnerKey))
                return (true);

            return (item.OwnerKey.Equals(Connector.DisplayOwnerKey));
        }

        #endregion

        #endregion

        #endregion

        #region Paint processing

        #region Root paint code

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        public override void Paint(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            // Set our current color table

            _ViewColor.SetColorTable();

            // calculate our Column count and range

            int colStart, colEnd;

            if (GetColRange(e, out colStart, out colEnd) > 0)
            {
                Region rgnSave = g.Clip;

                g.SetClip(ClientRect, CombineMode.Intersect);

                // Draw the TimeLine

                DrawTimeLine(g, colStart, colEnd);

                if (Connector != null && ShowCondensed == true)
                    DrawCondensedLine(g);

                // If we have a model connection then
                // draw our appointments

                if (Connector != null && Connector.IsConnected)
                {
                    Rectangle r = ClientRect;
                    r.X += 1;

                    if (ShowCondensed == true)
                    {
                        Rectangle r2 = GetCondensedRect();

                        if (r.Bottom > r2.Top)
                            r.Height -= (r.Bottom - r2.Top);
                    }

                    g.SetClip(r, CombineMode.Intersect);

                    DrawAppointments(e);
                }

                g.Clip = rgnSave;

                // Update our pos window

                UpdatePosWin(ClientRect);
            }

            // Let the base painting take place

            base.Paint(e);
        }

        #endregion

        #region DrawTimeLine

        /// <summary>
        /// Initiates the drawing of the TimeLine
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="colStart">Starting column</param>
        /// <param name="colEnd">Ending column</param>
        private void DrawTimeLine(Graphics g, int colStart, int colEnd)
        {
            int n = colEnd - colStart;

            eSlotDisplayState[] states = new eSlotDisplayState[n + 1];

            for (int i = 0; i <= n; i++)
                states[i] = GetSlotState(colStart + i);

            DrawContent(g, colStart, colEnd, states);
            DrawBorder(g, colStart, colEnd, states);

            DrawTimeIndicators(g, colStart, colEnd);
        }

        #region DrawContent

        /// <summary>
        /// Draws the content area of the TimeLine
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart">Starting column</param>
        /// <param name="colEnd">Ending column</param>
        /// <param name="states"></param>
        private void DrawContent(Graphics g,
            int colStart, int colEnd, eSlotDisplayState[] states)
        {
            Rectangle r = GetColRect(colStart);

            if (CalendarView.HasTimeLineSlotBackgroundCallout == true)
            {
                for (int i = colStart; i <= colEnd; i++)
                {
                    int n = i - colStart;

                    DateTime startTime = StartDate.AddMinutes(i * BaseInterval);
                    DateTime endTime = startTime.AddMinutes(BaseInterval);

                    if (CalendarView.DoTimeLineViewPreRenderSlotBackground(
                        g, this, startTime, endTime, r, ref states[n]) == false)
                    {
                        g.FillRectangle(GetContentBrush(states[n]), r);

                        DrawCollateLines(g, r);

                        CalendarView.DoTimeLineViewPostRenderSlotBackground(
                            g, this, startTime, endTime, r, states[n]);
                    }

                    r.X += ColumnWidth;
                }
            }
            else
            {
                Rectangle t = r;

                for (int i = 0; i <= colEnd - colStart; i++)
                {
                    g.FillRectangle(GetContentBrush(states[i]), t);

                    t.X += ColumnWidth;
                }

                r.Width += (t.Right - r.Right);

                DrawCollateLines(g, r);
            }
        }

        #region DrawCollateLines

        private void DrawCollateLines(Graphics g, Rectangle r)
        {
            if (CalendarView.TimeLineShowCollateLines == true &&
                _CollateLines.Count > 1)
            {
                using (Pen pen = new Pen(
                    _ViewColor.GetColor((int) eCalendarWeekDayPart.DayHalfHourBorder)))
                {
                    for (int i = 0; i < _CollateLines.Count - 1; i++)
                    {
                        int y = _CollateLines[i];

                        g.DrawLine(pen, r.X, r.Y + y, r.Right, r.Y + y);
                    }
                }
            }
        }

        #endregion

        #region GetSlotState

        /// <summary>
        /// GetSlotState
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private eSlotDisplayState GetSlotState(int col)
        {
            eSlotDisplayState state = eSlotDisplayState.None;

            if (DisplayedOwnerKeyIndex == CalendarView.SelectedOwnerIndex)
            {
                if (col >= _SelectedColStart && col < _SelectedColEnd)
                    state |= eSlotDisplayState.Selected;
            }

            DateTime date = StartDate.AddMinutes(col * CalendarView.BaseInterval);
            WorkTime workTime = new WorkTime(date.Hour, date.Minute);

            if (IsWorkTime((int)date.DayOfWeek, workTime) == true)
                state |= eSlotDisplayState.Work;

            return (state);
        }

        #endregion

        #region GetContentBrush

        /// <summary>
        /// Gets the background content brush
        /// for the given time slice
        /// </summary>
        /// <returns>Background brush</returns>
        private Brush GetContentBrush(eSlotDisplayState state)
        {
            if ((state & eSlotDisplayState.Selected) == eSlotDisplayState.Selected)
                return (SelectedBrush);

            if ((state & eSlotDisplayState.Work) == eSlotDisplayState.Work)
                return (WorkBrush);

            return (OffWorkBrush);
        }

        #endregion

        #region IsWorkTime

        /// <summary>
        /// Determines if the given time is tagged as a "Work time"
        /// </summary>
        /// <param name="day">Day of week</param>
        /// <param name="time">WorkTime to test</param>
        /// <returns>true if specified "time" is a Work time</returns>
        private bool IsWorkTime(int day, WorkTime time)
        {
            ModelTimeLineViewConnector tlc =
                (ModelTimeLineViewConnector)Connector;

            WorkTime workStartTime = tlc.DayInfo[day].WorkStartTime;
            WorkTime workEndTime = tlc.DayInfo[day].WorkEndTime;

            return (time >= workStartTime && time < workEndTime);
        }

        #endregion

        #endregion

        #region DrawBorder

        /// <summary>
        /// Draws the TimeLine border
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart">Starting column</param>
        /// <param name="colEnd">Ending column</param>
        /// <param name="states"></param>
        private void DrawBorder(Graphics g,
            int colStart, int colEnd, eSlotDisplayState[] states)
        {
            Rectangle r = GetColRect(colStart);

            // Draw the vertical borders

            if (CalendarView.TimeLinePeriod == eTimeLinePeriod.Minutes)
                DrawHalfHourBorders(g, colStart, colEnd, r, states);
            else
                DrawHourBorders(g, colStart, colEnd, r, states);

            // Now draw the horizontal border

            using (Pen pen1 = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayViewBorder)))
            {
                Point pt1 = new Point(r.X, ClientRect.Bottom - 1);
                Point pt2 = new Point(r.X + (colEnd - colStart + 1) * ColumnWidth, pt1.Y);

                g.DrawLine(pen1, pt1, pt2);

                if (CalendarView.TimeLineShowPeriodHeader == false &&
                    CalendarView.TimeLineShowIntervalHeader == false)
                {
                    pt1 = new Point(r.X, ClientRect.Top);
                    pt2 = new Point(r.X + (colEnd - colStart + 1) * ColumnWidth, pt1.Y);

                    g.DrawLine(pen1, pt1, pt2);
                }
            }
        }

        #region DrawHalfHourBorders

        /// <summary>
        /// DrawHalfHourBorders
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        /// <param name="bounds"></param>
        /// <param name="states"></param>
        private void DrawHalfHourBorders(Graphics g,
            int colStart, int colEnd, Rectangle bounds, eSlotDisplayState[] states)
        {
            // Draw the vertical hour and half hour borders

            using (Pen pen1 = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHourBorder)))
            {
                using (Pen pen2 = new Pen(
                    _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHalfHourBorder)))
                {
                    Point pt1 = new Point(bounds.X, ClientRect.Y);
                    Point pt2 = new Point(bounds.X, ClientRect.Bottom - 1);

                    int n = 60 / CalendarView.TimeLineInterval;
                    int start = -_HScrollPos / ColumnWidth;

                    for (int i = colStart; i <= colEnd; i++)
                    {
                        bool isHour = (i == start || (i % n) == 0);
                        Pen pen = (isHour ? pen1 : pen2);

                        if (CalendarView.DoTimeLineViewRenderSlotBorder(
                            g, this, i, isHour, states[i - colStart], pt1, pt2, pen) == false)
                        {
                            g.DrawLine((isHour ? pen1 : pen2), pt1, pt2);
                        }

                        pt1.X = pt2.X = (pt1.X + ColumnWidth);
                    }
                }
            }
        }

        #endregion

        #region DrawHourBorders

        /// <summary>
        /// DrawHourBorders
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        /// <param name="bounds"></param>
        /// <param name="states"></param>
        private void DrawHourBorders(Graphics g,
            int colStart, int colEnd, Rectangle bounds, eSlotDisplayState[] states)
        {
            // Draw the vertical hour borders

            using (Pen pen = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHourBorder)))
            {
                Point pt1 = new Point(bounds.X, ClientRect.Y);
                Point pt2 = new Point(bounds.X, ClientRect.Bottom - 1);

                for (int i = colStart; i <= colEnd; i++)
                {
                    if (CalendarView.DoTimeLineViewRenderSlotBorder(
                        g, this, i, true, states[i - colStart], pt1, pt2, pen) == false)
                    {
                        g.DrawLine(pen, pt1, pt2);
                    }

                    pt1.X = pt2.X = (pt1.X + ColumnWidth);
                }
            }
        }

        #endregion

        #endregion

        #region DrawTimeIndicators

        #region DrawTimeIndicators

        /// <summary>
        /// Draws view TimeIndicators
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        private void DrawTimeIndicators(Graphics g, int colStart, int colEnd)
        {
            DateTime start = CalendarView.TimeLineAddInterval(StartDate, colStart);
            DateTime end = CalendarView.TimeLineAddInterval(StartDate, colEnd);

            Rectangle r = Rectangle.Union(GetColRect(colStart), GetColRect(colEnd));

            for (int i = 0; i < CalendarView.TimeIndicators.Count; i++)
            {
                TimeIndicator ti = CalendarView.TimeIndicators[i];

                if (ti.IndicatorArea == eTimeIndicatorArea.All ||
                    ti.IndicatorArea == eTimeIndicatorArea.Content)
                {
                    if (ti.IsVisible(CalendarView, this))
                    {
                        DateTime time = ti.IndicatorDisplayTime;

                        if (time >= start && time < end)
                            DrawTimeIndicator(g, start, r, ti);
                    }
                }
            }
        }

        #endregion

        #region DrawTimeIndicator

        #region DrawTimeIndicator

        /// <summary>
        /// Draws individual view TimeIndicator
        /// </summary>
        /// <param name="g"></param>
        /// <param name="startDate"></param>
        /// <param name="sRect"></param>
        /// <param name="ti"></param>
        private void DrawTimeIndicator(Graphics g,
            DateTime startDate, Rectangle sRect, TimeIndicator ti)
        {
            Rectangle r = GetIndicatorRect(ti, startDate, sRect);

            if (r.IntersectsWith(sRect) == true)
            {
                if (r.Width > 0)
                {
                    ColorDef cdef = GetIndicatorColor(ti);

                    if (cdef != null)
                    {
                        float angle = cdef.Angle - 90;

                        using (Brush br = _ViewColor.BrushPart(cdef, r, angle))
                        {
                            if (br is LinearGradientBrush)
                                ((LinearGradientBrush) br).WrapMode = WrapMode.TileFlipX;

                            g.FillRectangle(br, r);
                        }
                    }
                }

                Color color = GetIndicatorBorder(ti);

                if (color.IsEmpty == false)
                {
                    using (Pen pen = new Pen(color))
                        g.DrawLine(pen, r.Right, r.Top, r.Right, r.Bottom - 1);
                }
            }
        }

        #endregion

        #region GetIndicatorColor

        /// <summary>
        /// Gets the Indicator Back color
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        private ColorDef GetIndicatorColor(TimeIndicator ti)
        {
            ColorDef cdef = ti.IndicatorColor;

            if (cdef == null || cdef.IsEmpty == true)
                cdef = _ViewColor.GetColorDef((int)eCalendarWeekDayPart.TimeIndicator);

            return (cdef);
        }

        #endregion

        #region GetIndicatorBorder

        /// <summary>
        /// Gets the Indicator Border color
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        private Color GetIndicatorBorder(TimeIndicator ti)
        {
            return (ti.BorderColor.IsEmpty == false ? ti.BorderColor :
                _ViewColor.GetColor((int)eCalendarWeekDayPart.TimeIndicatorBorder));
        }

        #endregion

        #endregion

        #region GetIndicatorRect

        /// <summary>
        /// Gets the TimeIndicator Rectangle
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="startDate"></param>
        /// <param name="sRect"></param>
        /// <returns></returns>
        private Rectangle GetIndicatorRect(
            TimeIndicator ti, DateTime startDate, Rectangle sRect)
        {
            double x = ColumnWidth / CalendarView.BaseInterval;

            int offset = (int)((ti.IndicatorDisplayTime - startDate).TotalMinutes * x);

            sRect.X += (offset - ti.Thickness - 1);
            sRect.Width = ti.Thickness;

            return (sRect);
        }

        /// <summary>
        /// Gets the TimeIndicator Rectangle
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        internal override Rectangle GetIndicatorRect(TimeIndicator ti)
        {
            return (GetIndicatorRect(ti, ti.IndicatorDisplayTime));
        }

        /// <summary>
        /// Gets the TimeIndicator Rectangle for the given date
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        internal override Rectangle GetIndicatorRect(TimeIndicator ti, DateTime time)
        {
            int colStart = -_HScrollPos / ColumnWidth;
            DateTime startDate = CalendarView.TimeLineAddInterval(StartDate, colStart);

            double x = ColumnWidth / CalendarView.BaseInterval;
            int offset = (int)((time - startDate).TotalMinutes * x);

            Rectangle r = GetColRect(colStart);

            r.X += (offset - ti.Thickness - 1);
            r.Width = ti.Thickness;

            return (r);
        }

        #endregion

        #endregion

        #endregion

        #region DrawCondensedLine

        /// <summary>
        /// Draws the condensed TimeLine
        /// </summary>
        /// <param name="g"></param>
        private void DrawCondensedLine(Graphics g)
        {
            TimeSpan ts0 = EndDate - StartDate;
            float scale = (float)(ClientRect.Width / (ts0.TotalMinutes + BaseInterval));

            Rectangle r = GetCondensedRect();

            using (GraphicsPath path = GetCondensedViewPath(r, scale))
            {
                using (Brush br =
                    _ViewColor.BrushPart((int)eCalendarWeekDayPart.CondensedViewBackground, r))
                {
                    g.FillRectangle(br, r);
                }

                g.FillPath(Brushes.White, path);

                DrawCondensedContent(g, r, scale);
                DrawCondensedAppointments(g, scale);

                g.DrawPath(Pens.Gray, path);
            }
        }

        #region DrawCondensedContent

        /// <summary>
        /// Draws the Condensed Content area
        /// </summary>
        /// <param name="g"></param>
        /// <param name="vRect"></param>
        /// <param name="scale"></param>
        private void DrawCondensedContent(Graphics g, Rectangle vRect, float scale)
        {
            using (Pen pen = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHourBorder)))
            {
                int selCols = _SelectedColEnd - _SelectedColStart;

                if (selCols > 0)
                {
                    Rectangle r = vRect;

                    r.X += (int)(_SelectedColStart * BaseInterval * scale);
                    r.Width = (int)(selCols * BaseInterval * scale);

                    using (Brush br =
                        _ViewColor.BrushPart((int)eCalendarWeekDayPart.DayOffWorkHoursBackground, r))
                    {
                        g.FillRectangle(br, r);
                    }

                    g.DrawRectangle(pen, r);
                }

                g.DrawLine(pen, vRect.Left, vRect.Y, vRect.Right, vRect.Y);
            }
        }

        #endregion

        #region GetCondensedViewPath

        /// <summary>
        /// Gets the condensed view display path
        /// </summary>
        /// <param name="scale">Scale factor</param>
        /// <param name="vRect">Condensed view rect</param>
        /// <returns>Path</returns>
        private GraphicsPath GetCondensedViewPath(Rectangle vRect, float scale)
        {
            int pos = (int)(FirstVisibleColumn * BaseInterval * scale);
            int width = (int)((BaseInterval * ClientRect.Width) / ColumnWidth * scale);

            Rectangle r = new
                Rectangle(ClientRect.X + pos, vRect.Y, width, vRect.Height);

            r.Inflate(0, -1);

            return (DisplayHelp.GetRoundedRectanglePath(r, 2, 2, 2, 2));
        }

        #endregion

        #region DrawCondensedAppointments

        /// <summary>
        /// Draws condensed appointments
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="scale">Scale factor</param>
        private void DrawCondensedAppointments(Graphics g, float scale)
        {
            // Display each item

            SmoothingMode saveMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;

            int dy = 3;

            foreach (ColumnList clist in _CondensedColList)
            {
                for (int i = 0; i < clist.SList.Count; i++)
                {
                    for (int j = 0; j < clist.SList[i].Count; j++)
                    {
                        CalendarItem item = clist.SList[i][j].CItem;

                        TimeSpan ts1 = item.StartTime - StartDate;
                        TimeSpan ts2 = item.EndTime - item.StartTime;

                        int pos = (int)(ts1.TotalMinutes * scale);
                        int width = (int)(ts2.TotalMinutes * scale);

                        if (width < 4)
                            width = 4;

                        Point pt1 = new Point(ClientRect.X + pos,
                                              ClientRect.Bottom - CondensedLineHeight + (i * 3) + dy);

                        Point pt2 = new Point(pt1.X + width, pt1.Y);

                        using (Pen pen = GetCondensedPen(item.ModelItem))
                            g.DrawLine(pen, pt1, pt2);
                    }
                }

                dy += (clist.SList.Count * 3);
            }

            g.SmoothingMode = saveMode;
        }

        #region GetCondensedPen

        /// <summary>
        /// Gets the appointments condensed pen
        /// </summary>
        /// <param name="o">Appointment object</param>
        /// <returns></returns>
        private Pen GetCondensedPen(object o)
        {
            string category = null;

            if (o is Appointment)
                category = (o as Appointment).CategoryColor;

            else if (o is CustomCalendarItem)
                category = (o as CustomCalendarItem).CategoryColor;

            if (category != null)
            {
                // Check to see if we have any user defined
                // AppointmentCategoryColors

                if (CalendarView.HasCategoryColors == true)
                {
                    AppointmentCategoryColor acc =
                        CalendarView.CategoryColors[category];

                    if (acc != null)
                    {
                        ColorDef cdef = acc.BackColor;

                        return (new Pen(cdef.Colors[cdef.Colors.Length - 1], 2));
                    }
                }

                if (category.Equals(Appointment.CategoryBlue))
                    return (GetCategoryPen(eAppointmentPart.BlueBackground));

                if (category.Equals(Appointment.CategoryGreen))
                    return (GetCategoryPen(eAppointmentPart.GreenBackground));

                if (category.Equals(Appointment.CategoryOrange))
                    return (GetCategoryPen(eAppointmentPart.OrangeBackground));

                if (category.Equals(Appointment.CategoryPurple))
                    return (GetCategoryPen(eAppointmentPart.PurpleBackground));

                if (category.Equals(Appointment.CategoryRed))
                    return (GetCategoryPen(eAppointmentPart.RedBackground));

                if (category.Equals(Appointment.CategoryYellow))
                    return (GetCategoryPen(eAppointmentPart.YellowBackground));
            }

            return (GetCategoryPen(eAppointmentPart.DefaultBackground));
        }

        #endregion

        #region GetCategoryPen

        /// <summary>
        /// GetCategoryPen
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private Pen GetCategoryPen(eAppointmentPart part)
        {
            ColorDef cdef = _appointmentColor.GetColorDef((int)part);

            return (new Pen(cdef.Colors[cdef.Colors.Length - 1], 2));
        }

        #endregion

        #endregion

        #endregion

        #region DrawAppointments

        /// <summary>
        /// Draws TimeLine appointments
        /// </summary>
        /// <param name="e"></param>
        private void DrawAppointments(ItemPaintArgs e)
        {
            List<CalendarItem> items = CalendarItems;

            if (items.Count > 0)
            {
                // Loop through each CalendarItem in the week

                int selItem = -1;

                for (int i = 0; i < items.Count; i++)
                {
                    // If we can display the item, then initiate the paint

                    if (items[i].Displayed == true)
                    {
                        if (e.ClipRectangle.IsEmpty ||
                            e.ClipRectangle.IntersectsWith(items[i].DisplayRectangle))
                        {
                            if (items[i].IsSelected == true)
                                selItem = i;
                            else
                                items[i].Paint(e);
                        }
                    }
                }

                if (selItem >= 0)
                    items[selItem].Paint(e);
            }
        }

        #endregion

        #region GetColRange

        /// <summary>
        /// Calculates the range of columns needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="colStart">[out] Column start index</param>
        /// <param name="colEnd">[out] Column end index</param>
        /// <returns>Column range count (end - start)</returns>
        private int GetColRange(ItemPaintArgs e, out int colStart, out int colEnd)
        {
            // Calc our starting index

            int start = -_HScrollPos / ColumnWidth;
            int x = ClientRect.X + start * ColumnWidth + _HScrollPos;

            while (start < TimeLineColumnCount)
            {
                if (x + ColumnWidth > e.ClipRectangle.X)
                    break;

                x += ColumnWidth;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < TimeLineColumnCount)
            {
                if (x >= e.ClipRectangle.Right)
                    break;

                x += ColumnWidth;

                end++;
            }

            end++;

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                colStart = 0;
                colEnd = 0;

                return (0);
            }

            colStart = start;
            colEnd = end;

            return (end - start);
        }

        #endregion

        #endregion

        #region GetColRect

        /// <summary>
        /// Gets the display rectangle for the given column
        /// </summary>
        /// <param name="col">Column</param>
        /// <returns>Display rectangle</returns>
        private Rectangle GetColRect(int col)
        {
            Rectangle r = ClientRect;

            r.X += ((col * ColumnWidth) + _HScrollPos);
            r.Width = ColumnWidth;

            if (ShowCondensed == true)
                r.Height -= CondensedLineHeight;

            return (r);
        }

        #endregion

        #region GetCondensedRect

        /// <summary>
        /// Gets the CondensedView rectangle
        /// </summary>
        /// <returns>CondensedView rectangle</returns>
        private Rectangle GetCondensedRect()
        {
            Rectangle r = ClientRect;

            r.X += 1;
            r.Width -= 1;

            r.Y = r.Bottom - CondensedLineHeight;
            r.Height = CondensedLineHeight - 1;

            return (r);
        }

        #endregion

        #region GetDateCol

        /// <summary>
        /// Gets the absolute column value for the given date
        /// </summary>
        /// <param name="selDate">Selection date</param>
        /// <returns>Absolute column</returns>
        private int GetDateCol(DateTime? selDate)
        {
            if (selDate.HasValue)
            {
                TimeSpan ts = selDate.Value - StartDate;

                return (int)(ts.TotalMinutes / CalendarView.BaseInterval);
            }

            return (0);
        }

        #endregion

        #region Mouse routines

        #region MouseDown processing

        /// <summary>
        /// MouseDown event processing
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            // Forward on the event

            base.InternalMouseDown(objArg);

            if (objArg.Button == MouseButtons.Left)
            {
                if (IsTabMoving == false)
                {
                    // Locate where the event took place

                    if (ClientRect.Contains(objArg.Location) == true)
                    {
                        if (PointInCondensedView(objArg.Location) == true)
                        {
                            // User is mousing in the CondensedView area

                            MyCursor = Cursors.Hand;

                            ProcessCvlButtonDown(objArg);

                            IsCondMoving = true;
                        }
                        else
                        {
                            int col;

                            if (GetPointItem(objArg.Location, out col, true) == true)
                            {
                                // User is mouseing in the Content area

                                MyCursor = GetContentCursor();

                                if (ProcessCilButtonDown(objArg) == false)
                                    ProcessTvlButtonDown(col);

                                IsMouseDown = true;
                            }

                            IsCopyDrag = false;
                        }
                    }
                }
            }
        }

        #region CondensedView MouseDown processing

        /// <summary>
        /// Handles CondensedView Left Button Down events
        /// </summary>
        /// <param name="objArg"></param>
        private void ProcessCvlButtonDown(MouseEventArgs objArg)
        {
            ProcessCvlPoint(objArg);
        }

        #region ProcessCvlPoint

        /// <summary>
        /// Processes CondensedView point selection
        /// </summary>
        /// <param name="objArg"></param>
        private void ProcessCvlPoint(MouseEventArgs objArg)
        {
            TimeSpan ts0 = EndDate - StartDate;

            float scale = (float)((ClientRect.Width + 2) / ts0.TotalMinutes);
            int width = (int)((BaseInterval * ClientRect.Width) / ColumnWidth * scale);

            double x = (objArg.Location.X - ClientRect.X - (width / 2)) / scale;
            int col = (int)(x / BaseInterval);

            // Make sure we stay within our bounds, and
            // then set our new scroll value accordingly

            HScrollBarAdv hScrollBar = CalendarView.TimeLineHScrollPanel.ScrollBar;

            if (col < hScrollBar.Minimum)
                col = hScrollBar.Minimum;

            if (col > hScrollBar.Maximum - hScrollBar.LargeChange)
                col = hScrollBar.Maximum - hScrollBar.LargeChange;

            hScrollBar.Value = col;
        }

        #endregion

        #endregion

        #region CalendarItem MouseDown processing

        /// <summary>
        /// CalendarItem left mouseDown processing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        private bool ProcessCilButtonDown(MouseEventArgs objArg)
        {
            CalendarItem item = m_HotSubItem as CalendarItem;

            if (item != null)
            {
                if (item.HitArea != CalendarItem.eHitArea.None)
                {
                    // Give the user a chance to cancel the
                    // operation before it starts

                    if (CalendarView.DoBeforeAppointmentViewChange(this, item,
                        ((item.HitArea == CalendarItem.eHitArea.Move) ?
                        eViewOperation.AppointmentMove : eViewOperation.AppointmentResize)) == false)
                    {
                        _LastBounds = item.Bounds;
                        _LastPointOffset = objArg.Location;
                        _LastMovePoint = objArg.Location;

                        if (IsResizing == false && IsMoving == false)
                        {
                            OldStartTime = item.StartTime;
                            OldEndTime = item.EndTime;
                            OldOwnerKey = OwnerKey;
                        }

                        // Flag appropriate action

                        if (item.HitArea == CalendarItem.eHitArea.LeftResize)
                            IsStartResizing = true;

                        else if (item.HitArea == CalendarItem.eHitArea.RightResize)
                            IsEndResizing = true;

                        else if (item.HitArea == CalendarItem.eHitArea.Move)
                            IsMoving = true;

                        // Update our initial PosWin display

                        UpdatePosWin(ClientRect);
                    }
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #region TimeLineView MouseDown processing

        /// <summary>
        /// Handles TimeLineView left MouseDown events
        /// </summary>
        /// <param name="col">Column index</param>
        private void ProcessTvlButtonDown(int col)
        {
            DateTime startDate = CalendarView.TimeLineAddInterval(StartDate, col);
            DateTime endDate = CalendarView.TimeLineAddInterval(startDate, 1);

            ExtendSelection(ref startDate, ref endDate);

            CalendarView.DateSelectionStart = startDate;
            CalendarView.DateSelectionEnd = endDate;

            SelectedItem = null;
        }

        #endregion

        #endregion

        #region MouseUp processing

        /// <summary>
        /// MouseUp event processing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);

            // Cancel our scroll timer

            CancelScrollTimer();
        }

        #endregion

        #region MouseMove processing

        /// <summary>
        /// MouseMove event processing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            // Forward on the event, but only if we are not in
            // the middle of moving or resizing a CalendarItem

            if (Control.MouseButtons == MouseButtons.None)
                ClearMouseStates();

            if (!IsMoving && !IsStartResizing && !IsEndResizing)
                base.InternalMouseMove(objArg);

            if (IsTabMoving == false)
            {
                if (IsCondMoving == true || PointInCondensedView(objArg.Location) == true)
                {
                    // The user in the CondensedView area

                    MyCursor = Cursors.Hand;

                    if (IsCondMoving == true)
                        ProcessCvlMouseMove(objArg);
                }
                else
                {
                    // The user in the Content area

                    MyCursor = GetContentCursor();

                    if (objArg.Button == MouseButtons.Left)
                        ProcessContentMove(objArg);
                }
            }
        }

        #region ProcessCvlMouseMove

        /// <summary>
        /// Handles CondensedView mouse moves
        /// </summary>
        /// <param name="objArg"></param>
        private void ProcessCvlMouseMove(MouseEventArgs objArg)
        {
            ProcessCvlPoint(objArg);
        }

        #endregion

        #region ProcessContentMove

        /// <summary>
        /// Processes content mouse moves
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        private void ProcessContentMove(MouseEventArgs objArg)
        {
            // Locate where the event took place
            // and process it accordingly

            int col;

            if (GetPointItem(objArg.Location, out col, false) == true)
            {
                // The col is visible, so no need to
                // enable scrolling - just process the event

                EnableViewScrolling(false);

                ProcessMouseMove(col, objArg);
            }
            else if (IsMouseDown == true)
            {
                // Check to see if the user is performing
                // a "DragDrop" operation

                if (DragDropAppointment(objArg) == false)
                {
                    // The selected slice is not visible,
                    // so we need to enable scrolling

                    EnableViewScrolling(true);

                    // Only process the event if the user is selecting
                    // time cells (auto moving apps is intrusive looking)

                    if (DateSelectionAnchor != null)
                        ProcessMouseMove(col, objArg);
                }
            }
        }

        #region DragDropAppointment

        /// <summary>
        /// Initiates a user "DragDrop" operation - if enabled
        /// </summary>
        /// <param name="objArgs"></param>
        /// <returns>True if operation started</returns>
        private bool DragDropAppointment(MouseEventArgs objArgs)
        {
            if (IsMoving == true && CanDrag == true)
            {
                Point pt = objArgs.Location;
                BaseView bv = CalendarView.GetViewFromPoint(pt);
                TimeLineView tlv = bv as TimeLineView;

                if (tlv != null && tlv != this)
                {
                    eViewArea area = bv.GetViewAreaFromPoint(pt);

                    if (area == eViewArea.InContent)
                    {
                        DragCopy();

                        ClearMouseStates();
                        CancelScrollTimer();

                        AppointmentView av = SelectedItem as AppointmentView;

                        if (av != null)
                            return (tlv.DragAppointment(this, av));

                        CustomCalendarItem ci = SelectedItem as CustomCalendarItem;

                        if (ci != null)
                            return (tlv.DragCustomItem(this, ci));
                    }
                }
            }

            return (false);
        }

        #region DragCustomItem

        private bool DragCustomItem(TimeLineView pv, CustomCalendarItem ci)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            ci.OwnerKey = OwnerKey;

            NeedRecalcLayout = true;
            RecalcSize();

            CalendarView.SelectedOwnerIndex = this.DisplayedOwnerKeyIndex;

            // Get the new view

            CustomCalendarItem view = GetCustomCalendarItem(ci);

            if (view != null)
                SetNewDragItem(pv, view);

            return (true);
        }

        #endregion

        #region DragAppointment

        /// <summary>
        /// Drags the given appointment from one view to another
        /// </summary>
        /// <param name="pv">Previous view</param>
        /// <param name="av">Item to drag</param>
        private bool DragAppointment(TimeLineView pv, AppointmentView av)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            av.Appointment.OwnerKey = OwnerKey;

            NeedRecalcLayout = true;
            RecalcSize();

            CalendarView.SelectedOwnerIndex = this.DisplayedOwnerKeyIndex;

            // Get the new view

            AppointmentView view = GetAppointmentView(av.Appointment);

            if (view != null)
                SetNewDragItem(pv, view);

            return (true);
        }

        #endregion

        #region SetNewDragItem

        private void SetNewDragItem(TimeLineView pv, CalendarItem view)
        {
            _LastBounds = pv._LastBounds;
            _LastMovePoint = pv._LastMovePoint;
            _LastPointOffset = pv._LastPointOffset;

            int dx = _LastPointOffset.X - _LastBounds.X;

            CalendarView.CalendarPanel.InternalMouseMove(new
                MouseEventArgs(MouseButtons.None, 0, view.Bounds.X + dx, view.Bounds.Y, 0));

            MouseEventArgs args = new
                MouseEventArgs(MouseButtons.Left, 1, view.Bounds.X + dx, view.Bounds.Y, 0);

            IsMoving = true;

            InternalMouseMove(args);
            CalendarView.CalendarPanel.InternalMouseDown(args);

            SelectedItem = view;

            IsCopyDrag = true;
        }

        #endregion

        #endregion

        #endregion

        #region GetContentCursor
        /// <summary>
        /// Gets the cursor
        /// </summary>
        /// <returns>Cursor</returns>
        private Cursor GetContentCursor()
        {
            CalendarItem item = m_HotSubItem as CalendarItem;

            if (item != null)
            {
                switch (item.HitArea)
                {
                    case CalendarItem.eHitArea.LeftResize:
                    case CalendarItem.eHitArea.RightResize:
                        return (Cursors.SizeWE);

                    case CalendarItem.eHitArea.Move:
                        return (IsMoving ? Cursors.SizeAll : DefaultCursor);
                }
            }

            return (DefaultCursor);
        }

        #endregion

        #region ProcessMouseMove

        /// <summary>
        /// Processes user MouseMove
        /// </summary>
        /// <param name="col">DayColumn</param>
        /// <param name="objArg"></param>
        private void ProcessMouseMove(int col, MouseEventArgs objArg)
        {
            if (DateSelectionAnchor != null)
            {
                ProcessTvMouseMove(col);
            }
            else if (SelectedItem != null)
            {
                if (objArg.Location.Equals(_LastMovePoint) == false)
                {
                    if (IsMoving == true)
                        ProcessItemMove(col, objArg);

                    else if (IsStartResizing == true)
                        ProcessItemLeftResize(col, objArg);

                    else if (IsEndResizing == true)
                        ProcessItemRightResize(col, objArg);

                    _LastMovePoint = objArg.Location;
                }
            }
        }

        #endregion

        #region ProcessTvMouseMove

        /// <summary>
        /// Processes TimeLineView mouseMove events
        /// </summary>
        /// <param name="col">Column</param>
        private void ProcessTvMouseMove(int col)
        {
            if (DateSelectionAnchor != null)
            {
                DateTime date = CalendarView.TimeLineAddInterval(StartDate, col);

                // Let the user select forwards or backwards

                if (date >= DateSelectionAnchor)
                {
                    CalendarView.DateSelectionStart = DateSelectionAnchor.Value;
                    CalendarView.DateSelectionEnd = CalendarView.TimeLineAddInterval(date, 1);
                }
                else
                {
                    CalendarView.DateSelectionStart = date;
                    CalendarView.DateSelectionEnd = CalendarView.TimeLineAddInterval(DateSelectionAnchor.Value, 1);
                }
            }
        }

        #endregion

        #region CalendarItem MouseMove processing

        /// <summary>
        /// Processes CalendarItem mouseMove events
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="objArg">MouseEventArgs</param>
        private void ProcessItemMove(int col, MouseEventArgs objArg)
        {
            // Calculate our new item date

            DateTime newDate;

            if (CalendarView.TimeLinePeriod == eTimeLinePeriod.Years)
            {
                newDate = StartDate.AddYears(
                    GetDeltaYears(col, true, objArg));
            }
            else
            {
                newDate = StartDate.AddMinutes(
                    GetDeltaMinutes(col, true, objArg));
            }

            if (newDate != SelectedItem.StartTime)
            {
                if (DragCopy() == false)
                {
                    TimeSpan ts = SelectedItem.EndTime - SelectedItem.StartTime;

                    try
                    {
                        if (SelectedItem is CustomCalendarItem)
                            CalendarView.CustomItems.BeginUpdate();
                        else
                            CalendarModel.BeginUpdate();

                        // Make the move

                        if (CalendarView.DoAppointmentViewChanging(SelectedItem, newDate,
                            newDate + ts, eViewOperation.AppointmentMove, IsCopyDrag) == false)
                        {
                            SelectedItem.StartTime = newDate;
                            SelectedItem.EndTime = newDate + ts;

                            NeedRecalcLayout = true;
                        }
                    }
                    finally
                    {
                        if (SelectedItem is CustomCalendarItem)
                            CalendarView.CustomItems.EndUpdate();
                        else
                            CalendarModel.EndUpdate();
                    }
                }
            }
        }

        #endregion

        #region CalendarItem MouseResize processing

        /// <summary>
        /// Processes CalendarItem left resizing
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="objArg"></param>
        private void ProcessItemLeftResize(int col, MouseEventArgs objArg)
        {
            DateTime date = (CalendarView.TimeLinePeriod == eTimeLinePeriod.Years) ?
                StartDate.AddYears(GetDeltaYears(col, true, objArg)) :
                StartDate.AddMinutes(GetDeltaMinutes(col, true, objArg));

            if (date < SelectedItem.EndTime && SelectedItem.StartTime != date)
                ResizeItem(date, SelectedItem.EndTime);
        }

        /// <summary>
        /// Processes CalendarItem right resizing
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="objArg"></param>
        private void ProcessItemRightResize(int col, MouseEventArgs objArg)
        {
            DateTime date = (CalendarView.TimeLinePeriod == eTimeLinePeriod.Years) ?
                StartDate.AddYears(GetDeltaYears(col, false, objArg)) :
                StartDate.AddMinutes(GetDeltaMinutes(col, false, objArg));

            if (date > SelectedItem.StartTime && SelectedItem.EndTime != date)
                ResizeItem(SelectedItem.StartTime, date);
        }

        /// <summary>
        /// Initiates the resize of the selected item
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        private void ResizeItem(DateTime startTime, DateTime endTime)
        {
            // Let the user cancel the operation if desired

            if (CalendarView.DoAppointmentViewChanging(SelectedItem, startTime,
                endTime, eViewOperation.AppointmentResize, IsCopyDrag) == false)
            {
                SelectedItem.StartTime = startTime;
                SelectedItem.EndTime = endTime;

                NeedRecalcLayout = true;
            }
        }

        #endregion

        #region GetDeltaMinutes

        /// <summary>
        /// Gets the change from the last offset (in minutes)
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="left">Are we going left or right</param>
        /// <param name="objArg">MouseEventArgs</param>
        /// <returns>Change in mnutes</returns>
        private double GetDeltaMinutes(int col, bool left, MouseEventArgs objArg)
        {
            // Calculate our delta minutes

            Rectangle r = GetColRect(col);

            double dm = (objArg.Location.X - _LastPointOffset.X);

            dm += left ? _LastBounds.Left - r.Left
                       : _LastBounds.Right - r.Right;

            dm = BaseInterval * dm / ColumnWidth;

            // If the Alt key is not pressed, then round
            // our value off to the nearest column

            if ((Control.ModifierKeys & Keys.Alt) != Keys.Alt)
            {
                col += (int)(dm / BaseInterval);
                dm = 0;
            }

            dm += (col * BaseInterval);

            // Make sure we don't cross our day boundaries

            if (left == true)
            {
                if (dm < 0)
                    dm = 0;
            }
            else
            {
                dm += BaseInterval;
            }

            return (dm);
        }

        #endregion

        #region GetDeltaYears

        /// <summary>
        /// Gets the change from the last offset (in years)
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="left">Are we going left or right</param>
        /// <param name="objArg">MouseEventArgs</param>
        /// <returns>Change in years</returns>
        private int GetDeltaYears(int col, bool left, MouseEventArgs objArg)
        {
            // Calculate our delta minutes

            Rectangle r = GetColRect(col);

            int dm = (objArg.Location.X - _LastPointOffset.X);

            dm += left ? _LastBounds.Left - r.Left
                       : _LastBounds.Right - r.Right;

            dm = CalendarView.TimeLineInterval * dm / ColumnWidth;

            // If the Alt key is not pressed, then round
            // our value off to the nearest column

            if ((Control.ModifierKeys & Keys.Alt) != Keys.Alt)
            {
                col += dm / CalendarView.TimeLineInterval;
                dm = 0;
            }

            dm += (col * CalendarView.TimeLineInterval);

            // Make sure we don't cross our day boundaries

            if (left == true)
            {
                if (dm < 0)
                    dm = 0;
            }
            else
            {
                dm += CalendarView.TimeLineInterval;
            }

            return (dm);
        }

        #endregion

        #region View Scrolling support

        /// <summary>
        /// Routine to enable or disable view scrolling
        /// </summary>
        /// <param name="enable">true to enable</param>
        private void EnableViewScrolling(bool enable)
        {
            if (enable == true)
            {
                if (_ScrollViewTimer == null)
                {
                    _ScrollViewTimer = new Timer();

                    _ScrollViewTimer.Interval = 10;
                    _ScrollViewTimer.Tick += ScrollViewTimerTick;
                    _ScrollViewTimer.Start();
                }

                _ScrollDwell = 0;
            }
            else
            {
                CancelScrollTimer();
            }
        }

        /// <summary>
        /// Cancels the view scroll timer
        /// </summary>
        private void CancelScrollTimer()
        {
            // Dispose of our scroll timer

            if (_ScrollViewTimer != null)
            {
                _ScrollViewTimer.Stop();
                _ScrollViewTimer.Tick -= ScrollViewTimerTick;

                _ScrollViewTimer = null;
            }
        }

        /// <summary>
        /// Determines the amount to scroll (which is
        /// based loosely upon the delta magnitude)
        /// </summary>
        /// <param name="delta">Point delta</param>
        /// <returns>Scroll amount</returns>
        private int ScrollAmount(int delta)
        {
            _ScrollDwell++;

            int dx = Math.Abs(delta);
            int n = (dx < 16 ? 8 : dx < 32 ? 4 : 1);

            if (_ScrollDwell % n == 0)
                return (delta < 0 ? -1 : 1);

            return (0);
        }

        /// <summary>
        /// Handles view scroll timer ticks
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void ScrollViewTimerTick(object sender, EventArgs e)
        {
            Control c = (Control)this.GetContainerControl(true);

            if (c != null)
            {
                // Calculate our delta

                Point pt = c.PointToClient(Cursor.Position);
                Rectangle r = ClientRect;

                int n = r.X + (ClientRect.Width / ColumnWidth) * ColumnWidth;

                int dx = (pt.X < r.Left) ? ScrollAmount(pt.X - r.Left) :
                    (pt.X >= n) ? ScrollAmount(pt.X - n) : 0;

                // Make sure we stay within our upper bounds

                if (dx < 0)
                {
                    if (CalendarView.TimeLineHScrollPanel.ScrollBar.Value + dx < 0)
                        dx = -CalendarView.TimeLineHScrollPanel.ScrollBar.Value;
                }
                else
                {
                    int maxValue = CalendarView.TimeLineHScrollPanel.ScrollBar.Maximum -
                        CalendarView.TimeLineHScrollPanel.ScrollBar.LargeChange;

                    if (CalendarView.TimeLineHScrollPanel.ScrollBar.Value + dx > maxValue)
                        dx = maxValue - CalendarView.TimeLineHScrollPanel.ScrollBar.Value;
                }

                // Scroll if necessary

                if (dx != 0)
                {
                    CalendarView.TimeLineHScrollPanel.ScrollBar.Value += dx;

                    if (PosWin != null)
                        PosWin.Hide();

                    InternalMouseMove(new
                        MouseEventArgs(MouseButtons.Left, 0, pt.X, pt.Y, 0));
                }
            }
        }

        #endregion

        #endregion

        #region GetPointItem

        /// <summary>
        /// Gets the item column at the given point
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <param name="col">[out] Column</param>
        /// <param name="partial">True if partial hits are ok</param>
        /// <returns>True if valid item</returns>
        private bool GetPointItem(Point pt, out int col, bool partial)
        {
            int start = -_HScrollPos / ColumnWidth;
            int end = start + ClientRect.Width / ColumnWidth;

            Rectangle r = GetColRect(start);

            for (int i = start; i <= end; i++)
            {
                if (r.Contains(pt) == true)
                {
                    col = i;

                    return (IsColVisible(r, partial));
                }

                r.X += ColumnWidth;
            }

            col = (pt.X < ClientRect.X) ? 0 : TimeLineColumnCount;

            return (false);
        }

        /// <summary>
        /// Determines if a given column is visible
        /// </summary>
        /// <param name="r">Display rectangle</param>
        /// <param name="partial">True if partial visibility is ok</param>
        /// <returns>True if visible</returns>
        private bool IsColVisible(Rectangle r, bool partial)
        {
            if (partial == true)
            {
                if (r.Right < ClientRect.Left)
                    return (false);

                if (r.Left > ClientRect.Right)
                    return (false);
            }
            else
            {
                if (r.Left < ClientRect.Left)
                    return (false);

                if (r.Right > ClientRect.Right)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region PointInCondensedView

        /// <summary>
        /// Determines if the given point in in
        /// the CondensedView area
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private bool PointInCondensedView(Point pt)
        {
            if (ShowCondensed == true)
                return (GetCondensedRect().Contains(pt));

            return (false);
        }

        #endregion

        #endregion

        #region InternalKeyDown

        #region InternalKeyDown

        /// <summary>
        /// Processes KeyDown events
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalKeyDown(KeyEventArgs objArg)
        {
            switch (objArg.KeyData)
            {
                case Keys.Up:
                case Keys.Up | Keys.Shift:
                case Keys.Up | Keys.Control:
                case Keys.Up | Keys.Control | Keys.Shift:
                    objArg.Handled = true;
                    break;

                case Keys.Down:
                case Keys.Down | Keys.Shift:
                case Keys.Down | Keys.Control:
                case Keys.Down | Keys.Control | Keys.Shift:
                    objArg.Handled = true;
                    break;

                case Keys.Left:
                case Keys.Left | Keys.Shift:
                case Keys.Left | Keys.Control:
                case Keys.Left | Keys.Control | Keys.Shift:
                    ProcessLeftRightKey(objArg, -1);
                    break;

                case Keys.Right:
                case Keys.Right | Keys.Shift:
                case Keys.Right | Keys.Control:
                case Keys.Right | Keys.Control | Keys.Shift:
                    ProcessLeftRightKey(objArg, 1);
                    break;

                case Keys.Home:
                case Keys.Home | Keys.Shift:
                case Keys.Home | Keys.Control:
                case Keys.Home | Keys.Control | Keys.Shift:
                    ProcessHomeKey(objArg);
                    break;

                case Keys.End:
                case Keys.End | Keys.Shift:
                case Keys.End | Keys.Control:
                case Keys.End | Keys.Control | Keys.Shift:
                    ProcessEndKey(objArg);
                    break;
            }
        }

        #endregion

        #region ProcessLeftRightKey

        /// <summary>
        /// Processes Left and Right Key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dx"></param>
        protected virtual void ProcessLeftRightKey(KeyEventArgs objArg, int dx)
        {
            if (ValidDateSelection())
            {
// ReSharper disable PossibleInvalidOperationException

                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if (startDate.Equals(DateSelectionAnchor.Value) == true)
                    startDate = CalendarView.TimeLineAddInterval(endDate, -1);

// ReSharper restore PossibleInvalidOperationException

                startDate = CalendarView.TimeLineAddInterval(startDate, dx);
                endDate = CalendarView.TimeLineAddInterval(startDate, 1);

                DateTime viewStart = CalendarView.TimeLineViewScrollStartDate;
                DateTime viewEnd = CalendarView.TimeLineViewScrollEndDate;

                if (startDate < viewStart)
                {
                    CalendarView.TimeLineViewScrollStartDate = startDate;
                }
                else if (endDate > viewEnd)
                {
                    CalendarView.TimeLineViewScrollStartDate =
                        CalendarView.TimeLineAddInterval(CalendarView.TimeLineViewScrollStartDate, 1);
                }

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessHomeKey

        /// <summary>
        /// Handles Home key events
        /// </summary>
        /// <param name="objArg"></param>
        protected virtual void ProcessHomeKey(KeyEventArgs objArg)
        {
            DateTime startDate = CalendarView.TimeLineViewScrollStartDate;
            DateTime endDate = CalendarView.TimeLineAddInterval(startDate, 1);

            ExtendSelection(ref startDate, ref endDate);

            CalendarView.DateSelectionStart = startDate;
            CalendarView.DateSelectionEnd = endDate;

            SelectedItem = null;

            objArg.Handled = true;
        }

        #endregion

        #region ProcessEndKey

        /// <summary>
        /// Processes End key events
        /// </summary>
        /// <param name="objArg"></param>
        protected virtual void ProcessEndKey(KeyEventArgs objArg)
        {
            DateTime endDate = CalendarView.TimeLineViewScrollEndDate;
            DateTime startDate = CalendarView.TimeLineAddInterval(endDate, -1);

            ExtendSelection(ref startDate, ref endDate);

            CalendarView.DateSelectionStart = startDate;
            CalendarView.DateSelectionEnd = endDate;

            SelectedItem = null;

            objArg.Handled = true;
        }

        #endregion

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                ResetView();

                WorkBrush = null;
                OffWorkBrush = null;
                SelectedBrush = null;

                HookEvents(false);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
#endif