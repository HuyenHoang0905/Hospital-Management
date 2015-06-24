#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    public class YearView : BaseView
    {
        #region Constants

        private const int MaxNumberOfMonths = 120;

        #endregion

        #region Private variables

        private YearMonth[] _YearMonths;        // Array of YearMonths
        private int _NumberOfMonths;            // Number of months in display
        private int _NumberOfCols;
        private int _NumberOfRows;

        private CalendarMonthColor _ViewColor =
            new CalendarMonthColor(eCalendarColor.Automatic);

        private bool _RecalcInProgress;

        private int _MouseDownMonthIndex = -1;
        private int _MouseDownDayIndex = -1;
        private int _LastMouseDownDayIndex = -1;
        private DateTime _LastMouseUpTime = DateTime.MinValue;

        private int _VScrollPos;

        private Size _PreferredSize;
        private Size _CellSize;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">Parent CalendarView</param>
        public YearView(CalendarView calendarView)
            : base(calendarView, eCalendarView.Year)
        {
            // Set our non-client drawing info and our CalendarColor

            NClientData = new NonClientData(
                eTabOrientation.Horizontal,
                (int)eCalendarMonthPart.OwnerTabBorder,
                (int)eCalendarMonthPart.OwnerTabForeground,
                (int)eCalendarMonthPart.OwnerTabBackground,
                (int)eCalendarMonthPart.OwnerTabContentBackground,
                (int)eCalendarMonthPart.OwnerTabSelectedForeground,
                (int)eCalendarMonthPart.OwnerTabSelectedBackground);

            CalendarColorTable = _ViewColor;

            // Hook onto needed event

            HookEvents(true);
        }

        #region Internal properties

        #region AllowDateSelection

        /// <summary>
        /// Gets whether date selections are permitted
        /// </summary>
        internal bool AllowDateSelection
        {
            get { return (CalendarView.YearViewAllowDateSelection); }
        }

        #endregion

        #region CellSize

        /// <summary>
        /// Gets the default day cell size
        /// </summary>
        internal Size CellSize
        {
            get
            {
                if (_CellSize.IsEmpty)
                {
                    Control container = GetContainerControl(true) as Control;

                    if (container != null)
                    {
                        using (Graphics g = BarFunctions.CreateGraphics(container))
                            _CellSize = g.MeasureString("00", Font).ToSize();
                    }
                }

                return (_CellSize);
            }
        }

        #endregion

        #region ShowGridLines

        /// <summary>
        /// Gets whether Grid lines are to be displayed
        /// </summary>
        internal bool ShowGridLines
        {
            get { return (CalendarView.YearViewShowGridLines); }
        }

        #endregion

        #region YearViewAppointmentLink

        /// <summary>
        /// Gets the Appointment Link click style
        /// </summary>
        internal eYearViewDayLink YearViewAppointmentLink
        {
            get { return (CalendarView.YearViewAppointmentLink); }
        }

        #endregion

        #region YearViewNonAppointmentLink

        /// <summary>
        /// Gets the non-Appointment Link click style
        /// </summary>
        internal eYearViewDayLink YearViewNonAppointmentLink
        {
            get { return (CalendarView.YearViewNonAppointmentLink); }
        }

        #endregion

        #region ViewColor

        /// <summary>
        /// Gets the Month Color table
        /// </summary>
        internal CalendarMonthColor ViewColor
        {
            get { return (_ViewColor); }
        }

        #endregion

        #region VsPanel

        /// <summary>
        /// Gets the Year vertical scroll panel
        /// </summary>
        private VScrollPanel VsPanel
        {
            get { return (CalendarView.YearVScrollPanel); }
        }

        #endregion

        #endregion

        #region Public properties

        #region Font

        /// <summary>
        /// Gets or sets the display font
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates the display font.")]
        public override Font Font
        {
            get { return (base.Font ?? CalendarView.Font); }

            set
            {
                base.Font = value;

                if (DaysOfTheWeek != null)
                    DaysOfTheWeek.NeedsMeasured = true;

                _CellSize = Size.Empty;
                _PreferredSize = Size.Empty;
            }
        }

        #endregion

        #region YearMonths

        /// <summary>
        /// Gets the array of YearMonths
        /// </summary>
        public YearMonth[] YearMonths
        {
            get { return (_YearMonths); }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks (or unhooks) needed events
        /// </summary>
        /// <param name="hook">True to hook, false to unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                CalendarView.SelectedViewChanged += SelectedViewChanged;
                CalendarView.YearViewStartDateChanged += YearViewStartDateChanged;
                CalendarView.YearViewEndDateChanged += YearViewEndDateChanged;
                CalendarView.YearViewShowGridLinesChanged += YearViewShowGridLinesChanged;
                CalendarView.YearViewAllowDateSelectionChanged += YearViewAllowDateSelectionChanged;
                CalendarView.YearVScrollPanel.ScrollBarChanged += VScrollPanelScrollBarChanged;
            }
            else
            {
                CalendarView.SelectedViewChanged -= SelectedViewChanged;
                CalendarView.YearViewStartDateChanged -= YearViewStartDateChanged;
                CalendarView.YearViewEndDateChanged -= YearViewEndDateChanged;
                CalendarView.YearViewShowGridLinesChanged -= YearViewShowGridLinesChanged;
                CalendarView.YearViewAllowDateSelectionChanged -= YearViewAllowDateSelectionChanged;
                CalendarView.YearVScrollPanel.ScrollBarChanged -= VScrollPanelScrollBarChanged;
            }
        }

        #endregion

        #region Event handling

        #region SelectedViewChanged

        /// <summary>
        /// Processes CalendarView SelectedViewChanged events
        /// </summary>
        /// <param name="sender">CalendarView</param>
        /// <param name="e">SelectedViewEventArgs</param>
        void SelectedViewChanged(object sender, SelectedViewEventArgs e)
        {
            IsViewSelected = (e.NewValue == eCalendarView.Year);

            if (IsViewSelected == true && Displayed == true)
            {
                AutoSyncViewDate(e.OldValue);

                UpdateView();
                UpdateDateSelection();
            }
            else
            {
                ResetView();
            }
        }

        #endregion

        #region YearViewStartDateChanged

        /// <summary>
        /// Processes StartDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void YearViewStartDateChanged(object sender, DateChangeEventArgs e)
        {
            StartDate = e.NewValue;
        }

        #endregion

        #region YearViewEndDateChanged

        /// <summary>
        /// Processes EndDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void YearViewEndDateChanged(object sender, DateChangeEventArgs e)
        {
            EndDate = e.NewValue;
        }

        #endregion

        #region YearViewAllowDateSelectionChanged

        /// <summary>
        /// Handles YearViewAllowDateSelectionChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void YearViewAllowDateSelectionChanged(object sender, AllowDateSelectionChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region YearViewShowGridLinesChanged

        /// <summary>
        /// Processes CalendarView YearViewShowGridLinesChanged events
        /// </summary>
        /// <param name="sender">CalendarView</param>
        /// <param name="e">SelectedViewEventArgs</param>
        void YearViewShowGridLinesChanged(object sender, ShowGridLinesChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region VScrollPanel_ScrollBarChanged

        /// <summary>
        /// Handles ScrollBarChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VScrollPanelScrollBarChanged(object sender, EventArgs e)
        {
            int vdelta = -VsPanel.ScrollBar.Value - _VScrollPos;

            if (vdelta != 0)
            {
                _VScrollPos = -VsPanel.ScrollBar.Value;

                // Offset our DayColumn accordingly

                for (int i = 0; i < _NumberOfMonths; i++)
                {
                    YearMonth ym = _YearMonths[i];

                    Rectangle r = ym.Bounds;
                    r.Y += vdelta;

                    ym.Bounds = r;
                }

                // Redraw our view

                InvalidateRect();
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
                if (ClientRect.Contains(pt))
                {
                    for (int i = 0; i < _NumberOfMonths; i++)
                    {
                        eViewArea area = _YearMonths[i].GetViewAreaFromPoint(pt);

                        if (area != eViewArea.NotInView)
                            return (area);
                    }

                    return (eViewArea.NotInView);
                }

                return (base.GetViewAreaFromPoint(pt));
            }

            return (eViewArea.NotInView);
        }

        #endregion

        #region GetDateSelectionFromPoint

        /// <summary>
        /// Gets the date selection from the given point.
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

            int monthIndex = GetMonthIndexFromPoint(pt);

            if (monthIndex >= 0)
            {
                if (_YearMonths[monthIndex].GetDateFromPoint(pt, ref startDate))
                {
                    endDate = startDate.AddDays(1);

                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region UpdateDateSelection

        /// <summary>
        /// Updates each monthWeeks DayRects to reflect
        /// the date selection start and end values
        /// </summary>
        protected override void UpdateDateSelection()
        {
            if (IsViewSelected == true)
            {
                // Loop through each month, setting
                // the selection status according to the date
                // selection range

                for (int i = 0; i < _NumberOfMonths; i++)
                    _YearMonths[i].UpdateDateSelection();
            }
        }

        #endregion

        #region RecalcSize routines

        /// <summary>
        /// Performs NeedRecalcSize requests
        /// </summary>
        public override void RecalcSize()
        {
            if (_RecalcInProgress == false)
            {
                try
                {
                    _RecalcInProgress = true;

                    RecalcDisplaySize();
                }
                finally
                {
                    _RecalcInProgress = false;
                }
            }
        }

        #region RecalcDisplaySize

        /// <summary>
        /// Performs all necessary recalc operations
        /// </summary>
        private void RecalcDisplaySize()
        {
            base.RecalcSize();

            if (IsViewSelected == true)
            {
                // Normalize our start and end dates

                DateTime startDate;
                DateTime endDate;

                NormalizeDates(out startDate, out endDate);

                // Calculate each months display info and then
                // initiate a YearMonth layout 

                CalcYearMonths(startDate, endDate);
                LayoutYearMonths(startDate);

                // Update our Model connection and views

                UpdateView();
                UpdateDateSelection();

                NeedRecalcLayout = false;
            }
        }

        #endregion

        #region NormalizeDates

        /// <summary>
        /// Normalizes the user specified start and end dates
        /// </summary>
        /// <param name="startDate">[out] Normalized start date</param>
        /// <param name="endDate">[out] Normalized end date</param>
        private void NormalizeDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = this.StartDate.Date;
            endDate = this.EndDate.Date;

            // If both values are unset, then set them to
            // today's date / + 12 months

            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                startDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
                endDate = startDate.AddMonths(12).AddDays(-1);
            }

            if (DaysOfTheWeek == null)
                DaysOfTheWeek = new DaysOfTheWeek();
        }

        #endregion

        #region Update View

        /// <summary>
        /// Updates our connection model view
        /// </summary>
        private void UpdateView()
        {
            // If we have had a date range change, just
            // reset our entire view

            if (DateRangeChanged == true)
                ResetView();

            // If we have no Model connection, attempt
            // to reconnect if this view is selected

            if (Connector == null)
            {
                if (CalendarModel != null)
                {
                    Connector = new ModelYearViewConnector(CalendarModel, this);

                    CalendarView.DoViewLoadComplete(this);
                }
            }
        }

        #endregion

        #region ResetView

        /// <summary>
        /// Disconnects and resets the Model connection
        /// </summary>
        internal override void ResetView()
        {
            base.ResetView();

            ModelYearViewConnector.ResetModelData();
        }

        #endregion

        #region CalcYearMonths

        /// <summary>
        /// Calculates display info for the YearMonth data
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        private void CalcYearMonths(DateTime startDate, DateTime endDate)
        {
            // Get the number of months for the date range

            _NumberOfMonths = (endDate.Year * 12 + endDate.Month) -
                (startDate.Year * 12 + startDate.Month) + 1;

            _NumberOfMonths = Math.Max(1, _NumberOfMonths);
            _NumberOfMonths = Math.Min(_NumberOfMonths, MaxNumberOfMonths);

            // Allocate our MonthWeeks array to
            // hold our weeks info

            if (_YearMonths == null || _YearMonths.Length != _NumberOfMonths)
                _YearMonths = new YearMonth[_NumberOfMonths];
        }

        #endregion

        #region LayoutYearMonths

        /// <summary>
        /// Performs size and positioning layout for the control
        /// </summary>
        /// <param name="startDate"></param>
        private void LayoutYearMonths(DateTime startDate)
        {
            Size size = GetPreferredSize(startDate);

            if (size.Width > 0 && size.Height > 0)
            {
                int maxCols = Math.Max(1, (ClientRect.Width - 1) / size.Width);
                int maxRows = Math.Max(1, (ClientRect.Height - 1) / size.Height);

                int targetCols = (int)Math.Ceiling(Math.Sqrt(_NumberOfMonths));

                if (targetCols >= maxCols)
                {
                    targetCols = maxCols;
                }
                else
                {
                    while (targetCols < maxCols)
                    {
                        int targetRows =
                            Math.Max(1, (_NumberOfMonths + targetCols - 1) / targetCols);

                        if (targetRows <= maxRows)
                            break;

                        targetCols++;
                    }
                }

                _NumberOfRows = Math.Max(1, (_NumberOfMonths + targetCols - 1) / targetCols);
                _NumberOfCols = (_NumberOfMonths + _NumberOfRows - 1) / _NumberOfRows;

                int monthHeight = (ClientRect.Height - 1) / _NumberOfRows;
                int monthWidth = (ClientRect.Width - 1) / _NumberOfCols;

                int rowSpread = 0;

                if (monthHeight < size.Height)
                    monthHeight = size.Height;
                else
                    rowSpread = ClientRect.Height - (_NumberOfRows * monthHeight) - 1;

                int colSpread = 0;

                if (monthWidth < size.Width)
                    monthWidth = size.Width;
                else
                    colSpread = ClientRect.Width - (_NumberOfCols * monthWidth) - 1;

                // Loop through each week

                int n = 0;
                DateTime date = startDate;

                Rectangle r = new Rectangle(ClientRect.X, ClientRect.Y, monthWidth, monthHeight);
                r.Y += _VScrollPos;

                for (int i = 0; i < _NumberOfRows; i++)
                {
                    if (i < rowSpread)
                        r.Height++;

                    for (int j = 0; j < _NumberOfCols; j++)
                    {
                        if (n >= _NumberOfMonths)
                            break;

                        if (_YearMonths[n] == null)
                            _YearMonths[n] = new YearMonth(this, date);
                        else
                            _YearMonths[n].StartDate = date;

                        // Calculate the bounding rect limits

                        r.Width = monthWidth;
                        
                        if (j < colSpread)
                            r.Width++;

                        // Set the bounding rect

                        _YearMonths[n].Bounds = r;

                        r.X += r.Width;
                        n++;

                        date = date.AddMonths(1);
                    }

                    r.X = ClientRect.X;
                    r.Y += r.Height;
                    r.Height = monthHeight;
                }

                CalendarView.YearViewMax = (r.Y - (ClientRect.Y + _VScrollPos));
            }
        }

        #endregion

        #region GetPreferredSize

        /// <summary>
        /// Gets the preferred size of the control
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private Size GetPreferredSize(DateTime startDate)
        {
            if (_PreferredSize.IsEmpty)
            {
                if (_YearMonths[0] == null)
                    _YearMonths[0] = new YearMonth(this, startDate);

                _PreferredSize = _YearMonths[0].GetPreferredSize();
            }

            return (_PreferredSize);
        }

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
            // Set our current color table

            _ViewColor.SetColorTable();

            // Only draw something if we have something to draw

            if (_NumberOfMonths > 0)
            {
                // Calculate our drawing ranges

                int colStart, colEnd;
                int rowStart, rowEnd;

                int colCount = GetColRange(e, out colStart, out colEnd);
                int rowCount = GetRowRange(e, out rowStart, out rowEnd);

                // Draw our calendar parts

                if (colCount > 0 && rowCount > 0)
                    DrawContent(e, rowStart, rowEnd, colStart, colEnd);
            }

            // Let the base painting take place

            base.Paint(e);
        }

        #endregion

        #region DrawContent

        /// <summary>
        /// Draws YearMonth header and content
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="rowStart">Row start index</param>
        /// <param name="rowEnd">Row end index</param>
        /// <param name="colStart">Col start index</param>
        /// <param name="colEnd">Col end index</param>
        private void DrawContent(ItemPaintArgs e,
            int rowStart, int rowEnd, int colStart, int colEnd)
        {
            Graphics g = e.Graphics;

            // Set our clipping region

            Rectangle r = ClientRect;

            r.X = ClientRect.X;
            r.Width = ClientRect.Width;

            Region regSave = g.Clip;
            g.SetClip(r, CombineMode.Intersect);

            // Loop through each day in each week, displaying
            // the associated day content and header

            DateTime date = DateTime.Now;

            int nowMonth = -1;

            for (int i = rowStart; i <= rowEnd; i++)
            {
                for (int j = colStart; j <= colEnd; j++)
                {
                    int n = (i * _NumberOfCols) + j;

                    if (n < _NumberOfMonths)
                    {
                        YearMonth ym = _YearMonths[n];

                        bool now = CalendarView.HighlightCurrentDay == true &&
                            ym.StartDate.Month == date.Month && ym.StartDate.Year == date.Year;

                        if (now == true)
                            nowMonth = n;
                        else
                            ym.Paint(e, false);
                    }
                }
            }

            if (nowMonth >= 0)
                _YearMonths[nowMonth].Paint(e, true);

            // Restore our original clip region

            g.Clip = regSave;
        }

        #endregion

        #region GetRange

        #region GetColRange

        /// <summary>
        /// Calculates the range of columns needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="colStart">[out] Col start index</param>
        /// <param name="colEnd">[out] COl end index</param>
        /// <returns>Col range count (end - start)</returns>
        private int GetColRange(ItemPaintArgs e, out int colStart, out int colEnd)
        {
            // Calc our starting index

            int nCols = Math.Min(_NumberOfMonths, _NumberOfCols);
            int start = 0;

            while (start < nCols)
            {
                Rectangle dr1 = _YearMonths[start].Bounds;

                if (dr1.Right > e.ClipRectangle.X)
                    break;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < nCols)
            {
                Rectangle dr2 = _YearMonths[end].Bounds;

                if (dr2.X >= e.ClipRectangle.Right)
                    break;

                end++;
            }

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                colStart = 0;
                colEnd = 0;

                return (0);
            }

            colStart = start;
            colEnd = end - 1;

            return (end - start);
        }

        #endregion

        #region GetRowRange

        /// <summary>
        /// Calculates the range of rows needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="rowStart">[out] Row start index</param>
        /// <param name="rowEnd">[out] Row end index</param>
        /// <returns>Row range count (end - start)</returns>
        private int GetRowRange(ItemPaintArgs e, out int rowStart, out int rowEnd)
        {
            // Calc our starting index

            int start = 0;

            while (start < _NumberOfRows)
            {
                if (_YearMonths[start * _NumberOfCols].Bounds.Bottom > e.ClipRectangle.Y)
                    break;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < _NumberOfRows)
            {
                if (_YearMonths[end * _NumberOfCols].Bounds.Y >= e.ClipRectangle.Bottom)
                    break;

                end++;
            }

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                rowStart = 0;
                rowEnd = 0;

                return (0);
            }

            rowStart = start;
            rowEnd = end - 1;

            return (end - start);
        }

        #endregion

        #endregion

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

            base.InternalMouseMove(objArg);

            if (IsTabMoving == false)
            {
                // Locate where the event took place
                // and process it accordingly

                int monthIndex, dayIndex, ndayIndex;

                if (GetPointItem(objArg.Location, out monthIndex, out dayIndex, out ndayIndex))
                {
                    if (objArg.Button == MouseButtons.Left)
                        ProcessMouseMove(monthIndex, ndayIndex);
                }

                // Set the cursor

                MyCursor = GetCursor(monthIndex, dayIndex, ndayIndex);
            }
        }

        #region ProcessMouseMove

        /// <summary>
        /// Processes view mouseMove events
        /// </summary>
        /// <param name="monthIndex"></param>
        /// <param name="dayIndex"></param>
        private void ProcessMouseMove(int monthIndex, int dayIndex)
        {
            if (AllowDateSelection == true)
            {
                if (DateSelectionAnchor != null)
                {
                    DateTime date = _YearMonths[monthIndex].GetDateFromIndex(dayIndex);

                    // Let the user select forwards or backwards

                    if (date >= DateSelectionAnchor)
                    {
                        CalendarView.DateSelectionStart = DateSelectionAnchor.Value;
                        CalendarView.DateSelectionEnd = date.AddDays(1);
                    }
                    else
                    {
                        CalendarView.DateSelectionStart = date;
                        CalendarView.DateSelectionEnd = DateSelectionAnchor.Value.AddDays(1);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region MouseDown processing

        #region InternalMouseDown

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

                    int monthIndex, dayIndex, ndayIndex;

                    if (GetPointItem(objArg.Location, out monthIndex, out dayIndex, out ndayIndex))
                    {
                        if (dayIndex == ndayIndex || 
                            (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            ProcessMouseDown(monthIndex, ndayIndex);
                        }
                        else
                        {
                            CalendarView.DateSelectionStart = null;
                            CalendarView.DateSelectionEnd = null;
                            DateSelectionAnchor = null; 
                        }
                    }

                    MyCursor = GetCursor(monthIndex, dayIndex, ndayIndex);
                }
            }
        }

        #endregion

        #region ProcessMouseDown

        /// <summary>
        /// Handles MonthView left MouseDown events
        /// </summary>
        /// <param name="monthIndex">Month index</param>
        /// <param name="dayIndex">Day index</param>
        private void ProcessMouseDown(int monthIndex, int dayIndex)
        {
            if (AllowDateSelection == true)
            {
                DateTime startDate = _YearMonths[monthIndex].GetDateFromIndex(dayIndex);
                DateTime endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;
            }

            _MouseDownMonthIndex = monthIndex;
            _MouseDownDayIndex = dayIndex;
        }

        #endregion

        #endregion

        #region MouseUp processing

        #region InternalMouseUp

        /// <summary>
        /// Handles InternalMouseUp events
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            // Forward on the event

            base.InternalMouseUp(objArg);

            ClearMouseStates();

            if (objArg.Button == MouseButtons.Left)
            {
                if (IsTabMoving == false)
                {
                    // Locate where the event took place

                    int monthIndex, dayIndex, ndayIndex;

                    if (GetPointItem(objArg.Location, out monthIndex, out dayIndex, out ndayIndex))
                        ProcessMouseUp(monthIndex, ndayIndex);

                    MyCursor = GetCursor(monthIndex, dayIndex, ndayIndex);
                }
            }
        }

        #endregion

        #region ProcessMouseUp

        /// <summary>
        /// Process mouse up events
        /// </summary>
        /// <param name="monthIndex"></param>
        /// <param name="dayIndex"></param>
        private void ProcessMouseUp(int monthIndex, int dayIndex)
        {
            // If the user is mousing-up on the same day that he
            // moused-down on, then check to see if we need to navigate
            // to a linked Calendar view

            if (_MouseDownMonthIndex == monthIndex && _MouseDownDayIndex == dayIndex)
            {
                YearMonth ym = _YearMonths[monthIndex];
                DateTime date = ym.GetDateFromIndex(dayIndex);

                eYearViewDayLink dl = (ym.DayIndexHasAppointments(dayIndex) == true)
                    ? YearViewAppointmentLink : YearViewNonAppointmentLink;

                if (((dl & eYearViewDayLink.DoubleClick) != eYearViewDayLink.DoubleClick) || _LastMouseDownDayIndex != dayIndex ||
                    _LastMouseUpTime == DateTime.MinValue ||
                    DateTime.Now.Subtract(_LastMouseUpTime).TotalMilliseconds > SystemInformation.DoubleClickTime)
                {
                    _LastMouseUpTime = DateTime.Now;

                    if ((dl & eYearViewDayLink.Click) == eYearViewDayLink.Click)
                    {
                        if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                            PerformLinkSelect(date);
                    }
                    else if ((dl & eYearViewDayLink.CtrlClick) == eYearViewDayLink.CtrlClick)
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                            PerformLinkSelect(date);
                    }
                }
                else
                {
                    _LastMouseUpTime = DateTime.MinValue;

                    PerformLinkSelect(date);
                }
            }

            _LastMouseDownDayIndex = _MouseDownDayIndex;

            _MouseDownMonthIndex = -1;
            _MouseDownDayIndex = -1;
        }

        #endregion

        #region PerformLinkSelect

        /// <summary>
        /// Performs a day link selection
        /// </summary>
        /// <param name="date"></param>
        private void PerformLinkSelect(DateTime date)
        {
            DateTime startDate = date;

            if (CalendarView.YearViewLinkAction == eYearViewLinkAction.GoToFirstCalendarItem)
            {
                if (Connector != null)
                {
                    Appointment app = ((ModelYearViewConnector) Connector).GetFirstAppointment(date);

                    if (app != null)
                        startDate = date.Date.AddHours(app.StartTime.Hour).AddMinutes(app.StartTime.Minute);

                    CustomCalendarItem cci = ((ModelYearViewConnector) Connector).GetFirstCustomItem(date);

                    if (cci != null)
                    {
                        if (app == null || cci.StartTime < startDate)
                            startDate = date.Date.AddHours(cci.StartTime.Hour).AddMinutes(cci.StartTime.Minute);
                    }
                }
            }

            DateTime endDate = startDate;
            eCalendarView calendarView = CalendarView.YearViewLinkView;

            if (CalendarView.DoYearViewLinkSelected(ref startDate, ref endDate, ref calendarView) == false)
            {
                if (CalendarView.YearViewLinkView != eCalendarView.None &&
                    CalendarView.YearViewLinkView != eCalendarView.Year)
                {
                    CalendarView.DateSelectionStart = startDate;
                    CalendarView.DateSelectionEnd = endDate;

                    CalendarView.SelectedView = calendarView;
                    CalendarView.EnsureVisible(startDate, endDate);
                }
            }
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
                    ProcessUpDownKey(objArg, -7);
                    break;

                case Keys.Down:
                case Keys.Down | Keys.Shift:
                case Keys.Down | Keys.Control:
                case Keys.Down | Keys.Control | Keys.Shift:
                    ProcessUpDownKey(objArg, 7);
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

                case Keys.LButton | Keys.ShiftKey | Keys.Control:
                    ProcessControlKey();
                    break;

                case Keys.Enter:
                case Keys.Space:
                    ProcessEnterKey();
                    break;
            }
        }

        #endregion

        #region ProcessUpDownKey

        /// <summary>
        /// Processes Up and Down Key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dy"></param>
        protected virtual void ProcessUpDownKey(KeyEventArgs objArg, int dy)
        {
            if (ValidDateSelection())
            {
                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if (DateSelectionAnchor != null)
                {
                    if (startDate.Equals(DateSelectionAnchor.Value) == true)
                        startDate = endDate.AddDays(-1);
                }

                startDate = startDate.AddDays(dy);
                endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessLeftRightKey

        /// <summary>
        /// Processes Left and Right key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dx"></param>
        protected virtual void ProcessLeftRightKey(KeyEventArgs objArg, int dx)
        {
            if (ValidDateSelection())
            {
                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if (DateSelectionAnchor != null)
                {
                    if (startDate.Equals(DateSelectionAnchor.Value) == true)
                        startDate = endDate.AddDays(-1);
                }

                startDate = startDate.AddDays(dx);
                endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessHomeKey

        /// <summary>
        /// Processes Hoe key events
        /// </summary>
        /// <param name="objArg"></param>
        protected virtual void ProcessHomeKey(KeyEventArgs objArg)
        {
            int monthIndex = ((objArg.Modifiers & Keys.Control) != Keys.Control)
                                 ? GetHomeEndMonth()
                                 : 0;

            if (monthIndex >= 0)
            {
                DateTime startDate = _YearMonths[monthIndex].StartDate;
                DateTime endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                SelectedItem = null;
            }

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
            int monthIndex = ((objArg.Modifiers & Keys.Control) != Keys.Control)
                                 ? GetHomeEndMonth()
                                 : _NumberOfMonths - 1;

            if (monthIndex >= 0)
            {
                DateTime startDate = _YearMonths[monthIndex].EndDate;
                DateTime endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                SelectedItem = null;
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessControlKey

        private void ProcessControlKey()
        {
            Point pt = Control.MousePosition;

            Control container = GetContainerControl(true) as Control;

            if (container != null)
            {
                pt = container.PointToClient(pt);

                CalendarView.CalendarPanel.InternalMouseMove(new
                    MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
            }
        }

        #endregion

        #region ProcessEnterKey

        private void ProcessEnterKey()
        {
            if (ValidDateSelection())
            {
                TimeSpan ts = CalendarView.DateSelectionEnd.Value -
                    CalendarView.DateSelectionStart.Value;

                if (ts.Days == 1)
                    PerformLinkSelect(CalendarView.DateSelectionStart.Value);
            }
        }

        #endregion

        #region GetMonthFromDate

        /// <summary>
        /// Gets the month containing the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>MonthIndex or -1</returns>
        private int GetMonthFromDate(DateTime date)
        {
            for (int i = 0; i < _NumberOfMonths; i++)
            {
                YearMonth ym = _YearMonths[i];

                if (ym.ContainsDate(date))
                    return (i);
            }

            return (-1);
        }

        #endregion

        #region GetHomeEndMonth

        /// <summary>
        /// Gets the Home and End month from the
        /// current selection range
        /// </summary>
        /// <returns></returns>
        private int GetHomeEndMonth()
        {
            if (ValidDateSelection() == true)
            {
                if (CalendarView.DateSelectionStart.Equals(DateSelectionAnchor.Value) == true)
                    return (GetMonthFromDate(DateSelectionEnd.Value.AddDays(-1)));

                return (GetMonthFromDate(DateSelectionStart.Value));
            }

            return (-1);
        }

        #endregion

        #endregion

        #region InternalKeyUp

        /// <summary>
        /// InternalKeyUp
        /// </summary>
        /// <param name="e"></param>
        internal override void InternalKeyUp(KeyEventArgs e)
        {
            base.InternalKeyUp(e);

            switch (e.KeyData)
            {
                case Keys.LButton | Keys.ShiftKey:
                    ProcessControlKey();
                    break;
            }
        }

        #endregion

        #region GetCursor

        /// <summary>
        /// Gets the cursor
        /// </summary>
        /// <returns>Cursor</returns>
        private Cursor GetCursor(int monthIndex, int dayIndex, int ndayIndex)
        {
            if (monthIndex >= 0 && dayIndex >= 0 && dayIndex == ndayIndex)
            {
                if (_MouseDownMonthIndex < 0 ||
                    (_MouseDownMonthIndex == monthIndex && _MouseDownDayIndex == dayIndex))
                {
                    YearMonth ym = _YearMonths[monthIndex];

                    eYearViewDayLink dl = (ym.DayIndexHasAppointments(dayIndex) == true)
                                              ? YearViewAppointmentLink
                                              : YearViewNonAppointmentLink;

                    if ((dl & eYearViewDayLink.Click) == eYearViewDayLink.Click)
                    {
                        if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                            return (Cursors.Hand);
                    }
                    else if ((dl & eYearViewDayLink.CtrlClick) == eYearViewDayLink.CtrlClick)
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                            return (Cursors.Hand);
                    }
                }
            }

            return (DefaultCursor);
        }

        #endregion

        #region GetPointItem

        /// <summary>
        /// Gets the month and dey index for the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="monthIndex">Month index</param>
        /// <param name="dayIndex">Day index</param>
        /// <param name="ndayIndex">Normalized day index</param>
        /// <returns></returns>
        private bool GetPointItem(Point pt,
            out int monthIndex, out int dayIndex, out int ndayIndex)
        {
            monthIndex = GetMonthIndexFromPoint(pt);

            if (monthIndex >= 0)
            {
                YearMonth ym = _YearMonths[monthIndex];

                dayIndex = ym.GetDayIndexFromPoint(pt);
                ndayIndex = ym.GetNormalizedDayIndex(dayIndex);
            }
            else
            {
                dayIndex = -1;
                ndayIndex = -1;
            }

            return (dayIndex >= 0);
        }

        #endregion

        #region GetMonthIndexFromPoint

        /// <summary>
        /// Gets the month index from the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>month index or -1</returns>
        private int GetMonthIndexFromPoint(Point pt)
        {
            for (int i = 0; i < _NumberOfMonths; i++)
            {
                if (_YearMonths[i].Bounds.Contains(pt))
                    return (i);
            }

            return (-1);
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                ResetView();
                HookEvents(false);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Copy object

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            YearView objCopy = new YearView(this.CalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the YearView specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New YearView instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            YearView objCopy = copy as YearView;

            if (objCopy != null)
                base.CopyToItem(objCopy);
        }

        #endregion
    }
}
#endif

