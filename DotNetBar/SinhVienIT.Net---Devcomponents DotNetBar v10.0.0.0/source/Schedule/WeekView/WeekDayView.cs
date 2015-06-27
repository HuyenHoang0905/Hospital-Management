#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.Schedule.Model;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class WeekDayView : BaseView
    {
        #region Constants

        private const int HoursPerDay = 24;
        private const int MinutesPerHour = 60;
        private const int MaxNumberOfColumns = 3 * 31;

        #endregion

        #region Private variables

        private AllDayPanel _AllDayPanel;       // AllDay items panel

        private DayColumn[] _DayColumns;        // Array of DayColumns
        private int _NumberOfColumns;           // Number of DayColumns
        private Size[] _DayColumnSize;          // Array of day text sizes

        private CalendarWeekDayColor _ViewColor =     // View display color table
            new CalendarWeekDayColor(eCalendarColor.Automatic);

        private int _LastCol;                   // Last processed column
        private Point _LastMovePoint;           // Last mouse move Point
        private Point _LastPointOffset;         // Last Point offset
        private Rectangle _LastBounds;          // MouseDown item bounds

        private bool _IsPanelResizing;          // Flag denoting panel resizing

        private int _SelectedSliceStart;        // Absolute slice start
        private int _SelectedSliceEnd;          // Absolute slice end (e.g. 450 - 1440)

        private Timer _ScrollViewTimer;         // Timer used to implement auto view scrolling
        private int _VScrollPos;                // Vertical scroll bar pos

        private Color _BusyColor;               // Cached brush colors
        private Color _WorkColor;
        private Color _OffWorkColor;
        private Color _SelectedColor;

        private Brush _BusyBrush;               // Cached brushes
        private Brush _WorkBrush;
        private Brush _OffWorkBrush;
        private Brush _SelectedBrush;
        private Brush _AllDayBrush;
        private Color _AllDayColor;

        private int _LocalStartSlice;
        private int _LocalNumberOfSlices;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">CalendarView</param>
        /// <param name="eCalendarView"></param>
        public WeekDayView(CalendarView calendarView, eCalendarView eCalendarView)
            : base(calendarView, eCalendarView)
        {
            // Allocate our AllDayPanel

            _AllDayPanel = new AllDayPanel(this);
            _AllDayPanel.Displayed = true;

            // Set our non-client drawing info and CalendarColor

            NClientData = new NonClientData(
                eTabOrientation.Horizontal,
                (int)eCalendarWeekDayPart.OwnerTabBorder,
                (int)eCalendarWeekDayPart.OwnerTabForeground,
                (int)eCalendarWeekDayPart.OwnerTabBackground,
                (int)eCalendarWeekDayPart.OwnerTabContentBackground,
                (int)eCalendarWeekDayPart.OwnerTabSelectedForeground,
                (int)eCalendarWeekDayPart.OwnerTabSelectedBackground);

            CalendarColorTable = _ViewColor;

            // Hook onto our events

            HookEvents(true);
        }

        #region Public properties

        /// <summary>
        /// Gets the view DayColumns
        /// </summary>
        public DayColumn[] DayColumns
        {
            get { return (_DayColumns); }
        }

        /// <summary>
        /// Gets the view's number of DayColumns
        /// </summary>
        public int NumberOfColumns
        {
            get { return (_NumberOfColumns); }
        }

        #endregion

        #region Internal properties

        #region Base calendar properties

        /// <summary>
        /// Gets the Sub-Day view rectangle
        /// </summary>
        internal Rectangle ViewRect
        {
            get
            {
                Rectangle r = ClientRect;

                int n = DayOfWeekHeaderHeight + _AllDayPanel.PanelHeight;

                r.Y += n;
                r.Height -= (n + 1);

                return (r);
            }
        }

        /// <summary>
        /// Gets the CalendarColor
        /// </summary>
        internal CalendarWeekDayColor WeekDayColor
        {
            get { return (_ViewColor); }
        }

        #endregion

        #region DayColumn properties

        /// <summary>
        /// Gets the local StartSlice
        /// </summary>
        internal int LocalStartSlice
        {
            get { return (_LocalStartSlice); }
        }

        /// <summary>
        /// Gets the local NumberOfSlices
        /// </summary>
        internal int LocalNumberOfSlices
        {
            get { return (_LocalNumberOfSlices); }
        }

        #endregion

        #region AllDayPanel properties

        /// <summary>
        /// Gets the view's AllDayPanel
        /// </summary>
        internal AllDayPanel AllDayPanel
        {
            get { return (_AllDayPanel); }
        }

        #endregion

        #endregion

        #region Private properties

        #region DayColumnWidth

        /// <summary>
        /// Gets the DayColumnWidth
        /// </summary>
        private float DayColumnWidth
        {
            get { return (((float)ClientRect.Width - 1) / _NumberOfColumns); }
        }

        #endregion

        #region VsPanel

        /// <summary>
        /// Gets the WeekDay vertical scroll panel
        /// </summary>
        private VScrollPanel VsPanel
        {
            get { return (CalendarView.WeekDayVScrollPanel); }
        }

        #endregion

        #region AllDay Panel properties

        /// <summary>
        /// Gets the maximum AllDayPanel height
        /// </summary>
        private int MaximumAllDayPanelHeight
        {
            get { return (CalendarView.MaximumAllDayPanelHeight); }
        }

        #endregion

        #region Time Slice properties

        /// <summary>
        /// Gets the TimeSlotDuration
        /// </summary>
        private int TimeSlotDuration
        {
            get { return (CalendarView.TimeSlotDuration); }
        }

        /// <summary>
        /// Gets the default Time Slice height
        /// </summary>
        private float TimeSliceHeight
        {
            get { return (CalendarView.TimeSliceHeight); }
            set { CalendarView.TimeSliceHeight = value; }
        }

        /// <summary>
        /// Gets the SlotsPerHour
        /// </summary>
        private int SlotsPerHour
        {
            get { return (CalendarView.SlotsPerHour); }
        }

        /// <summary>
        /// Gets the NumberOfSlices
        /// </summary>
        private int NumberOfSlices
        {
            get { return (CalendarView.NumberOfSlices); }
            set { CalendarView.NumberOfSlices = value; }
        }

        /// <summary>
        /// Gets the NumberOfActiveSlices
        /// </summary>
        private int NumberOfActiveSlices
        {
            get { return (CalendarView.NumberOfActiveSlices); }
        }

        /// <summary>
        /// Gets the StartSlice
        /// </summary>
        private int StartSlice
        {
            get { return (CalendarView.StartSlice); }
            set { CalendarView.StartSlice = value; }
        }

        #endregion

        #region BusyBrush

        /// <summary>
        /// Gets and sets the Busy time brush
        /// </summary>
        private Brush BusyBrush
        {
            get
            {
                Color color = _ViewColor.GetColor(
                    (int)eCalendarWeekDayPart.DayAllDayEventBackground);

                if (_BusyColor != color)
                {
                    _BusyColor = color;

                    BusyBrush = new SolidBrush(color);
                }

                return (_BusyBrush);
            }

            set
            {
                if (_BusyBrush != value)
                {
                    if (_BusyBrush != null)
                        _BusyBrush.Dispose();

                    _BusyBrush = value;
                }
            }
        }

        #endregion

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

        #region AllDayBrush

        /// <summary>
        /// Gets and sets the Off-hours work time brush
        /// </summary>
        private Brush AllDayBrush
        {
            get
            {
                Color color = _ViewColor.GetColor(
                    (int)eCalendarWeekDayPart.DayAllDayEventBackground);

                if (_AllDayColor != color)
                {
                    _AllDayColor = color;

                    AllDayBrush = new SolidBrush(color);
                }

                return (_AllDayBrush);
            }

            set
            {
                if (_AllDayBrush != value)
                {
                    if (_AllDayBrush != null)
                        _AllDayBrush.Dispose();

                    _AllDayBrush = value;
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
                CalendarView.TimeSlotDurationChanged += TimeSlotDurationChanged;
                CalendarView.FixedAllDayPanelHeightChanged += FixedAllDayPanelHeightChanged;
                CalendarView.MaximumAllDayPanelHeightChanged += MaximumAllDayPanelHeightChanged;
                CalendarView.WeekDayVScrollPanel.ScrollBarChanged += VScrollPanelScrollBarChanged;

                if (this.ECalendarView == eCalendarView.Day)
                {
                    CalendarView.DayViewDateChanged += DayViewDateChanged;
                }
                else
                {
                    CalendarView.WeekViewStartDateChanged += WeekViewStartDateChanged;
                    CalendarView.WeekViewEndDateChanged += WeekViewEndDateChanged;
                }
            }
            else
            {
                CalendarView.SelectedViewChanged -= SelectedViewChanged;
                CalendarView.TimeSlotDurationChanged -= TimeSlotDurationChanged;
                CalendarView.FixedAllDayPanelHeightChanged -= FixedAllDayPanelHeightChanged;
                CalendarView.MaximumAllDayPanelHeightChanged -= MaximumAllDayPanelHeightChanged;
                CalendarView.WeekDayVScrollPanel.ScrollBarChanged -= VScrollPanelScrollBarChanged;

                if (this.ECalendarView == eCalendarView.Day)
                {
                    CalendarView.DayViewDateChanged -= DayViewDateChanged;
                }
                else
                {
                    CalendarView.WeekViewStartDateChanged -= WeekViewStartDateChanged;
                    CalendarView.WeekViewEndDateChanged -= WeekViewEndDateChanged;
                }
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

            IsViewSelected = (e.NewValue == this.ECalendarView);

            if (IsViewSelected == true)
            {
                AutoSyncViewDate(e.OldValue);

                UpdateDateSelection();
            }
            else
            {
                ResetView();
            }

            // Update our AllDayPanel view

            _AllDayPanel.UpdateView();

        }

        #endregion

        #region DayViewDateChanged

        /// <summary>
        /// Processes DayViewDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DayViewDateChanged(object sender, DateChangeEventArgs e)
        {
            StartDate = e.NewValue;
            EndDate = e.NewValue;
        }

        #endregion

        #region WeekViewStartDateChanged

        /// <summary>
        /// Processes StartDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WeekViewStartDateChanged(object sender, DateChangeEventArgs e)
        {
            StartDate = e.NewValue;
        }

        #endregion

        #region WeekViewEndDateChanged

        /// <summary>
        /// Processes EndDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WeekViewEndDateChanged(object sender, DateChangeEventArgs e)
        {
            EndDate = e.NewValue;
        }

        #endregion

        #region TimeSlotDurationChanged

        /// <summary>
        /// Handles TimeSlotDurationChanged events
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">TimeSlotDurationChangedEventArgs</param>
        void TimeSlotDurationChanged(
            object sender, TimeSlotDurationChangedEventArgs e)
        {
            // Reset our slice selection range and
            // invalidate our control

            _SelectedSliceStart = 0;
            _SelectedSliceEnd = 0;

            NeedRecalcLayout = true;
            RecalcSize();
        }

        #endregion

        #region FixedAllDayPanelHeightChanged

        /// <summary>
        /// Handles FixedAllDayPanelHeightChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FixedAllDayPanelHeightChanged(
            object sender, FixedAllDayPanelHeightChangedEventArgs e)
        {
            NeedRecalcLayout = true;
        }

        #endregion

        #region MaximumAllDayPanelHeightChanged

        /// <summary>
        /// Handles MaximumAllDayPanelHeightChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MaximumAllDayPanelHeightChanged(
            object sender, MaximumAllDayPanelHeightChangedEventArgs e)
        {
            NeedRecalcLayout = true;
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

                for (int i = 0; i < _NumberOfColumns; i++)
                {
                    DayColumn dayColumn = _DayColumns[i];

                    Rectangle r = dayColumn.Bounds;
                    r.Y += vdelta;

                    dayColumn.Bounds = r;

                    for (int j = 0; j < dayColumn.CalendarItems.Count; j++)
                    {
                        dayColumn.CalendarItems[j].Displayed =
                            dayColumn.CalendarItems[j].Bounds.IntersectsWith(ClientRect);
                    }
                }
                
                // Redraw our view

                InvalidateRect(ViewRect);
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
                if (pt.Y >= ClientRect.Y)
                {
                    if (AllDayPanel.Bounds.Contains(pt))
                        return (eViewArea.InAllDayPanel);

                    if (pt.Y < ClientRect.Y + DayOfWeekHeaderHeight)
                        return (eViewArea.InDayOfWeekHeader);

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
        /// and endDate will vary based upon the view type (WeekDay / Month)
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

            int col, slice;

            if (GetPointItem(pt, out col, out slice, true) == true)
            {
                startDate = GetTime(_DayColumns[col].Date, slice, 0);
                endDate = startDate.AddMinutes(TimeSlotDuration);

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
        internal void ItemIsSelectedChanged(object sender, EventArgs e)
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

                int sliceStart = 0;
                int sliceEnd = 0;

                if (DateSelectionStart.HasValue && DateSelectionEnd.HasValue)
                {
                    sliceStart = GetDateSlice(DateSelectionStart);
                    sliceEnd = GetDateSlice(DateSelectionEnd);

                    if (sliceStart < 0 && sliceEnd < 0)
                    {
                        sliceStart = 0;
                        sliceEnd = 0;
                    }

                    if (sliceStart < 0)
                        sliceStart = 0;

                    if (sliceEnd < 0)
                        sliceEnd = _NumberOfColumns * NumberOfActiveSlices;
                }

                // Limit our range to only those slices
                // that are visible on the screen

                int s1 = (int)(-_VScrollPos / TimeSliceHeight);
                int s2 = (int)(s1 + ViewRect.Height / TimeSliceHeight + 2);

                if (s2 > s1)
                {
                    ProcessSelRange(sliceStart, sliceEnd, s1, s2);

                    // Save our new selection range

                    _SelectedSliceStart = sliceStart;
                    _SelectedSliceEnd = sliceEnd;
                }
            }
        }

        #endregion

        #region GetDateSlice

        /// <summary>
        /// Gets the absolute slice value for the given date
        /// </summary>
        /// <param name="selDate">Selection date</param>
        /// <returns>Absolute slice</returns>
        private int GetDateSlice(DateTime? selDate)
        {
            if (selDate.HasValue)
            {
                // Loop through each column

                for (int i = 0; i < _NumberOfColumns; i++)
                {
                    DateTime date = _DayColumns[i].Date;

                    // If we have found our containing column, then
                    // calculate the absolute slice value and return it

                    if (selDate >= date && selDate < date.AddDays(1))
                    {
                        int slice = ((selDate.Value.Hour * MinutesPerHour) +
                            selDate.Value.Minute) / TimeSlotDuration;

                        return (i * NumberOfActiveSlices + slice - StartSlice);
                    }
                }
            }

            return (-1);
        }

        #endregion

        #region ProcessSelRange

        /// <summary>
        /// Processes the selection time slice range
        /// </summary>
        /// <param name="sliceStart">Slice range start</param>
        /// <param name="sliceEnd">Slice range end</param>
        /// <param name="s1">Slice start limit</param>
        /// <param name="s2">Slice end limit</param>
        private void ProcessSelRange(int sliceStart, int sliceEnd, int s1, int s2)
        {
            // Calculate our starting and ending column range

            int fcol = Math.Min(_SelectedSliceStart / NumberOfActiveSlices, sliceStart / NumberOfActiveSlices);
            int lcol = Math.Max(_SelectedSliceEnd / NumberOfActiveSlices, sliceEnd / NumberOfActiveSlices);

            lcol = Math.Min(_NumberOfColumns - 1, lcol);

            // Loop through each column

            for (int i = fcol; i <= lcol; i++)
            {
                // Get the selection status for the given range

                bool[] oldSelected = SelectedSlices(s1, s2, i, _SelectedSliceStart, _SelectedSliceEnd);
                bool[] newSelected = SelectedSlices(s1, s2, i, sliceStart, sliceEnd);

                // Invalidate those slices whose
                // selection status changed

                Rectangle vRect = ViewRect;

                for (int j = 0; j < s2 - s1; j++)
                {
                    if (oldSelected[j] != newSelected[j])
                    {
                        Rectangle r = GetSliceRect(i, s1 + j);

                        r.Intersect(vRect);

                        if (r.Width > 0 && r.Height > 0)
                            InvalidateRect(r);
                    }
                }
            }
        }

        #endregion

        #region SelectedSlices

        /// <summary>
        /// Gets an array of slice selection values
        /// over the given range of column slices
        /// </summary>
        /// <param name="s1">Slice start limit</param>
        /// <param name="s2">Slice end limit</param>
        /// <param name="col">Column</param>
        /// <param name="sliceStart">Slice range start</param>
        /// <param name="sliceEnd">Slice range end</param>
        /// <returns>Array of selection values</returns>
        private bool[] SelectedSlices(int s1, int s2, int col, int sliceStart, int sliceEnd)
        {
            // Calculate our number of entries and
            // allocate our IsSelected array accordingly

            int n = s2 - s1;

            bool[] sel = new bool[n + 1];

            // Loop through the range of entries determining if
            // the specific slice is within the selection range

            int slice = col * NumberOfActiveSlices + s1;

            for (int i = 0; i < n; i++)
            {
                sel[i] = (slice >= sliceStart && slice < sliceEnd);

                slice++;
            }

            // Return the array to the caller

            return (sel);
        }

        #endregion

        #endregion

        #region RecalcSize routines

        #region RecalcSize

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

                if (NeedRecalcLayout == true)
                    _DayColumnSize = null;

                // Allocate our DayColumns

                AllocateDayColumns(startDate, endDate);

                // Update our Model connection view,
                // AllDayPanel, CalendarItems, and DateSelection

                UpdateView();

                UpdateAllDayPanelItems();
                UpdateDayColumns(startDate, endDate);
                UpdateCalendarItems();

                UpdateDateSelection();

                // Set our display view rectangle

                Rectangle r = ClientRect;

                r.Y += DayOfWeekHeaderHeight;
                r.Height -= DayOfWeekHeaderHeight;

                SetViewRectangle(r);

                // Final Recalc cleanup

                if (DaysOfTheWeek != null)
                    DaysOfTheWeek.NeedsMeasured = true;

                NeedRecalcLayout = false;

                CalendarView.DoViewLoadComplete(this);
            }
        }

        #endregion

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

        #region AllocateDayColumns

        /// <summary>
        /// Allocates out DayColumns
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void AllocateDayColumns(DateTime startDate, DateTime endDate)
        {
            // Allocate our DayColumns array to
            // hold our info

            _NumberOfColumns = (endDate - startDate).Days + 1;
            _NumberOfColumns = Math.Max(_NumberOfColumns, 1);
            _NumberOfColumns = Math.Min(_NumberOfColumns, MaxNumberOfColumns);

            if (_DayColumns == null || _DayColumns.Length != _NumberOfColumns)
                _DayColumns = new DayColumn[_NumberOfColumns];

            // Prepare to update display info on each time slot

            Rectangle r = ViewRect;
            r.Y += _VScrollPos;

            // Loop through each day column

            float dcWidth = DayColumnWidth;
            float x1 = r.X;

            // Update each DayColumn

            for (int i = 0; i < _NumberOfColumns; i++)
            {
                if (_DayColumns[i] == null)
                    _DayColumns[i] = new DayColumn(0);

                _DayColumns[i].Date = startDate.AddDays(i);

                float x2 = (i == _NumberOfColumns - 1) ? r.Right - 1 : x1 + dcWidth;

                if (IsLayoutNeeded(i) == true)
                    _DayColumns[i].Bounds = new Rectangle((int) x1, r.Y, (int) x2 - (int) x1, r.Height);

                x1 = x2;
            }
        }

        #endregion

        #region UpdateView

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
                    Connector = new ModelWeekDayViewConnector(CalendarModel, this);

                    SubItems.Insert(0, _AllDayPanel);
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
            _AllDayPanel.ResetView();

            base.ResetView();
        }

        #endregion

        #region UpdateAllDayPanelItems

        /// <summary>
        /// Updates our AllDayPanel items
        /// </summary>
        private void UpdateAllDayPanelItems()
        {
            if (NeedRecalcLayout == true)
            {
                // Initiate a RecalcSize for the panel

                _AllDayPanel.WidthInternal = ClientRect.Width;
                _AllDayPanel.HeightInternal = MaximumAllDayPanelHeight;

                _AllDayPanel.RecalcSize();

                // Set the panel's bounding rect accordingly

                Rectangle r = ClientRect;

                r.Width = _AllDayPanel.WidthInternal;

                r.Y += DayOfWeekHeaderHeight;
                r.Height = _AllDayPanel.HeightInternal;

                _AllDayPanel.Bounds = r;
            }
        }

        private bool IsLayoutNeeded(int col)
        {
            return (NeedRecalcLayout == true ||
                _DayColumns[col].NeedRecalcLayout == true);
        }

        #endregion

        #region UpdateDayColumns

        /// <summary>
        /// Calculates and updates DayColumn bounds
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        protected void UpdateDayColumns(DateTime startDate, DateTime endDate)
        {
            // Establish our current TimeSlice range

            GetTimeSliceData();

            // Updating display info on each column

            Rectangle r = ViewRect;

            r.Y += _VScrollPos;
            r.Height = (int) (TimeSliceHeight * NumberOfSlices);

            for (int i = 0; i < _NumberOfColumns; i++)
            {
                _DayColumns[i].TimeSliceHeight = TimeSliceHeight;

                if (IsLayoutNeeded(i) == true)
                {
                    _DayColumns[i].Bounds = new
                        Rectangle(_DayColumns[i].Bounds.X, r.Y, _DayColumns[i].Bounds.Width, r.Height);

                    InvalidateRect(_DayColumns[i].Bounds);
                }
            }
        }

        #region GetTimeSliceData

        private void GetTimeSliceData()
        {
            _LocalStartSlice = 0;
            _LocalNumberOfSlices = (HoursPerDay * MinutesPerHour) / TimeSlotDuration + 1;

            if (CalendarView.ShowOnlyWorkDayHours == true)
            {
                WorkTime startTime = new WorkTime(23, 59);
                WorkTime endTime = new WorkTime(0, 0);

                for (int i = 0; i < NumberOfColumns; i++)
                {
                    DayColumn dc = _DayColumns[i];

                    if (dc.WorkStartTime.IsEmpty == false || dc.WorkEndTime.IsEmpty == false)
                    {
                        if (dc.WorkStartTime < startTime)
                            startTime = dc.WorkStartTime;

                        if (dc.WorkEndTime > endTime)
                            endTime = dc.WorkEndTime;
                    }
                }

                if (endTime > startTime)
                {
                    int startMinutes = (startTime.Hour * MinutesPerHour) + startTime.Minute;
                    int endMinutes = (endTime.Hour * MinutesPerHour) + endTime.Minute;

                    _LocalStartSlice = startMinutes / TimeSlotDuration;
                    _LocalNumberOfSlices = (endMinutes - startMinutes) / TimeSlotDuration + 1;
                }

                if (StartSlice >= 0)
                {
                    int endSlice = _LocalStartSlice + _LocalNumberOfSlices;

                    if (_LocalStartSlice > StartSlice)
                        _LocalStartSlice = StartSlice;

                    if (StartSlice + NumberOfActiveSlices > endSlice)
                        endSlice = StartSlice + NumberOfActiveSlices;

                    _LocalNumberOfSlices = endSlice - _LocalStartSlice;
                }
            }

            StartSlice = _LocalStartSlice;
            NumberOfSlices = _LocalNumberOfSlices;
            
            TimeSliceHeight = (float)ViewRect.Height / NumberOfActiveSlices;
        }

        #endregion

        #endregion

        #region UpdateCalendarItems

        /// <summary>
        /// Updates our CalendarItems list
        /// </summary>
        private void UpdateCalendarItems()
        {
            ColumnList colList = new ColumnList();

            for (int i = 0; i < _NumberOfColumns; i++)
            {
                if (IsLayoutNeeded(i) == true)
                {
                    if (_DayColumns[i].CalendarItems.Count > 0)
                    {
                        List<CalendarItem> items = SortCalendarItems(i);

                        colList.Clear();

                        for (int j = 0; j < items.Count; j++)
                            colList.AddColumnSlot(items[j], 0);

                        colList.CountColumns();

                        CalcAppointmentBounds(i, colList);
                    }

                    _DayColumns[i].NeedRecalcLayout = false;
                }
            }
        }

        #region SortCalendarItems

        /// <summary>
        /// Sorts the DayColumn CalendarItem list
        /// </summary>
        /// <param name="col">DayColumn index</param>
        private List<CalendarItem> SortCalendarItems(int col)
        {
            List<CalendarItem> items = new List<CalendarItem>();

            items.AddRange(_DayColumns[col].CalendarItems);

            if (items.Count > 0)
            {
                items.Sort(

                   delegate(CalendarItem c1, CalendarItem c2)
                   {
                       if (c1.StartTime > c2.StartTime)
                           return (1);

                       if (c1.StartTime < c2.StartTime)
                           return (-1);

                       if (c1.EndTime - c1.StartTime > c2.EndTime - c2.StartTime)
                           return (1);

                       if (c1.EndTime - c1.StartTime < c2.EndTime - c2.StartTime)
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
        /// <param name="col">DayColumn column</param>
        /// <param name="colList">Accumulated ColumnList</param>
        private void CalcAppointmentBounds(int col, ColumnList colList)
        {
            Rectangle r = _DayColumns[col].Bounds;

            r.X += 1;
            r.Width -= 8;

            for (int i = 0; i < colList.SList.Count; i++)
            {
                for (int j = 0; j < colList.SList[i].Count; j++)
                {
                    Rectangle r2 = r;
                    SlotItem si = colList.SList[i][j];
                    CalendarItem item = si.CItem;

                    float dx = (float)r2.Width / si.Count;
                    r2.X += (int)(dx * i);

                    if (si.SList != null && si.SList.Count == 1)
                        r2.Width = (int)(dx * (si.SList[0].Column - si.Column));

                    else if (si.SList == null)
                        r2.Width = r.Width - (r2.X - r.X);

                    else
                        r2.Width = (int)dx;

                    DateTime date = item.StartTime.Date;

                    TimeSpan ts1 = item.StartTime.AddMinutes(-StartSlice * TimeSlotDuration) - date;
                    TimeSpan ts2 = item.EndTime - item.StartTime;

                    int pos = (int)(ts1.TotalMinutes / TimeSlotDuration * TimeSliceHeight);
                    int height = (int)Math.Round(ts2.TotalMinutes / TimeSlotDuration * TimeSliceHeight);

                    AppointmentView view = item as AppointmentView;

                    if (view != null)
                    {
                        Font font = view.Font ?? CalendarView.Font;

                        if (font != null)
                            height = Math.Max(font.Height + 6, height);
                    }

                    r2.Y = r.Y + pos;
                    r2.Height = height;

                    // Now that we have calculated the items height and
                    // width, invoke a Recalc on the item

                    item.WidthInternal = r2.Width;
                    item.HeightInternal = r2.Height - 1;

                    item.RecalcSize();

                    // Set our bounds for the item

                    r2.Width = item.WidthInternal;
                    r2.Height = item.HeightInternal;

                    if (item.Bounds != r2)
                    {
                        InvalidateRect(item.Bounds);
                        InvalidateRect(r2);
                    }

                    item.Bounds = r2;

                    // Set it's display state

                    item.Displayed = r2.IntersectsWith(ClientRect);
                }
            }
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

            // Only draw something if we have something to draw

            if (_DayColumns != null)
            {
                // Calculate our drawing ranges

                int dayStart, dayEnd;
                int sliceStart, sliceEnd;

                int dayCount = GetDayRange(e, out dayStart, out dayEnd);
                int sliceCount = GetSliceRange(e, out sliceStart, out sliceEnd);

                // Draw our calendar parts

                if (dayCount > 0)
                    DrawDayOfTheWeekHeader(e, dayStart, dayEnd);

                if (sliceCount > 0 && dayCount > 0)
                {
                    // Set our clipping region

                    Rectangle r = ViewRect;

                    r.X = ClientRect.X;
                    r.Width = ClientRect.Width;

                    Region regSave = g.Clip;
                    g.SetClip(r, CombineMode.Intersect);

                    // Draw our slices and appointments

                    DrawSlices(e, sliceStart, sliceEnd, dayStart, dayEnd);

                    if (Connector != null && Connector.IsConnected)
                        DrawWeekAppointments(e, dayStart, dayEnd);

                    // Restore our original clip region

                    g.Clip = regSave;
                }

                // Initiate the painting of our AllDay Panel

                _AllDayPanel.Paint(e);

                // Update our Alt-Key window

                UpdatePosWin(ViewRect);
            }

            // Let the base painting take place

            base.Paint(e);
        }

        #region Clip range routines

        /// <summary>
        /// Calculates the range of days needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="dayStart">[out] Day start index</param>
        /// <param name="dayEnd">[out] Day end index</param>
        /// <returns>Day range count (end - start)</returns>
        private int GetDayRange(ItemPaintArgs e, out int dayStart, out int dayEnd)
        {
            // Calc our starting index

            int start = 0;

            while (start < _NumberOfColumns)
            {
                if (_DayColumns[start].Bounds.Right > e.ClipRectangle.X)
                    break;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < _NumberOfColumns)
            {
                if (_DayColumns[end].Bounds.X >= e.ClipRectangle.Right)
                    break;

                end++;
            }

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                dayStart = 0;
                dayEnd = 0;

                return (0);
            }

            dayStart = start;
            dayEnd = end - 1;

            return (end - start);
        }

        /// <summary>
        /// Calculates the range of slices needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="sliceStart">[out] Slice start index</param>
        /// <param name="sliceEnd">[out] Slice end index</param>
        /// <returns>Slice range count (end - start)</returns>
        internal int GetSliceRange(ItemPaintArgs e, out int sliceStart, out int sliceEnd)
        {
            // Calc our starting index

            int start = 0;

            Rectangle v = ViewRect;

            while (start < NumberOfSlices)
            {
                Rectangle r = GetSliceRect(0, start);

                if (r.Bottom > v.Top)
                {
                    if (r.Bottom > e.ClipRectangle.Y)
                        break;
                }

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < NumberOfSlices)
            {
                Rectangle r = GetSliceRect(0, end);

                if (r.Y >= e.ClipRectangle.Bottom)
                    break;

                end++;
            }

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                sliceStart = 0;
                sliceEnd = 0;

                return (0);
            }

            sliceStart = start;
            sliceEnd = end - 1;

            return (end - start);
        }

        private bool SliceIsSelected(int col, int slice)
        {
            int nslice = col * NumberOfActiveSlices + slice;

            return (nslice >= _SelectedSliceStart && nslice < _SelectedSliceEnd);
        }

        #endregion

        #endregion

        #region DayOfTheWeek header drawing

        #region DrawDayOfTheWeekHeader

        /// <summary>
        /// Draws the top Day of the week header
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="dayStart">Start day index</param>
        /// <param name="dayEnd">End day index</param>
        private void DrawDayOfTheWeekHeader(ItemPaintArgs e, int dayStart, int dayEnd)
        {
            Graphics g = e.Graphics;

            Rectangle r = new Rectangle(
                _DayColumns[dayStart].Bounds.X, ClientRect.Y,
                _DayColumns[dayEnd].Bounds.Right -
                _DayColumns[dayStart].Bounds.X, DayOfWeekHeaderHeight);

            if (r.Width > 0 && r.Height > 0)
            {
                // Establish our Days Of The Week text type

                DaysOfTheWeek.eDayType type = GetDaysOfTheWeekType(g);

                // Loop through each day, drawing the
                // day of the week text in the header area

                int nowHeader = -1;

                using (Brush br =
                    _ViewColor.BrushPart((int) eCalendarWeekDayPart.DayHeaderBackground, r))
                {
                    using (Pen pen = new Pen(
                        _ViewColor.GetColor((int) eCalendarWeekDayPart.DayHeaderBorder)))
                    {
                        for (int i = dayStart; i <= dayEnd; i++)
                        {
                            if (CalendarView.HighlightCurrentDay == true &&
                                _DayColumns[i].Date.Date.Equals(DateTime.Now.Date))
                            {
                                nowHeader = i;
                                continue;
                            }

                            DrawColumnHeader(g, i, type, r, br, pen, eCalendarWeekDayPart.DayHeaderForeground);
                        }
                    }
                }

                // Draw the current "Now" header

                if (nowHeader >= 0)
                {
                    using (Brush br =
                        _ViewColor.BrushPart((int) eCalendarWeekDayPart.NowDayHeaderBackground, r))
                    {
                        using (Pen pen = new Pen(
                            _ViewColor.GetColor((int) eCalendarWeekDayPart.NowDayHeaderBorder)))
                        {
                            DrawColumnHeader(g, nowHeader, type, r, br, pen, eCalendarWeekDayPart.NowDayHeaderForeground);
                        }
                    }
                }
            }
        }

        #region DrawColumnHeader

        /// <summary>
        /// DrawColumnHeader
        /// </summary>
        /// <param name="g"></param>
        /// <param name="i"></param>
        /// <param name="type"></param>
        /// <param name="r"></param>
        /// <param name="br"></param>
        /// <param name="pen"></param>
        /// <param name="part"></param>
        private void DrawColumnHeader(Graphics g, int i,
            DaysOfTheWeek.eDayType type, Rectangle r, Brush br, Pen pen, eCalendarWeekDayPart part)
        {
            r.X = _DayColumns[i].Bounds.X;
            r.Width = _DayColumns[i].Bounds.Width;

            g.FillRectangle(br, r);
            g.DrawRectangle(pen, r);

            // Draw the header text

            int n = i % DaysInWeek;

            eTextFormat tf = eTextFormat.VerticalCenter | eTextFormat.NoPadding;

            r.Inflate(-2, 0);
            r.X += 1;

            TextDrawing.DrawString(g,
                _DayColumns[i].Date.Day.ToString(), BoldFont, Color.Black, r, tf);

            r.X += _DayColumnSize[n].Width;
            r.Width -= _DayColumnSize[n].Width;

            if (type != DaysOfTheWeek.eDayType.None)
            {
                tf |= eTextFormat.HorizontalCenter;

                TextDrawing.DrawString(g, DaysOfTheWeek.DayText[(int) type][n],
                                       Font, _ViewColor.GetColor((int) part), r, tf);
            }
        }

        #endregion

        #endregion

        #region GetDaysOfTheWeekType

        /// <summary>
        /// Get the index for our day of the week text
        /// </summary>
        /// <param name="g">Graphics handle</param>
        /// <returns>Index to header text</returns>
        private DaysOfTheWeek.eDayType GetDaysOfTheWeekType(Graphics g)
        {
            // Resize our DayColumn day text if it
            // hasn't been resized previously

            if (_DayColumnSize == null)
            {
                _DayColumnSize = new Size[_NumberOfColumns];

                for (int i = 0; i < _NumberOfColumns; i++)
                {
                    _DayColumnSize[i] = TextDrawing.MeasureString(g,
                        _DayColumns[i].Date.Day.ToString(), BoldFont, 0, eTextFormat.NoPadding);
                }
            }

            // Determine if the current DayRect bounds
            // are constrained by the text threshold

            DaysOfTheWeek.MeasureText(g, Font);

            for (int i = 0; i < _NumberOfColumns; i++)
            {
                if (_DayColumns[i].Bounds.Width <
                    _DayColumnSize[i].Width + DaysOfTheWeek.DaySize[(int)DaysOfTheWeek.eDayType.Short][i % 7].Width)
                {
                    return (DaysOfTheWeek.eDayType.None);
                }
            }

            for (int i = 0; i < _NumberOfColumns; i++)
            {
                if (_DayColumns[i].Bounds.Width <
                    _DayColumnSize[i].Width + DaysOfTheWeek.DaySize[(int)DaysOfTheWeek.eDayType.Long][i % 7].Width)
                {
                    return (DaysOfTheWeek.eDayType.Short);
                }
            }

            return (DaysOfTheWeek.eDayType.Long);
        }

        #endregion

        #endregion

        #region DrawSlices

        #region DrawSlices

        /// <summary>
        /// Draws Normal Appointment time slices
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="sliceStart">Start slice</param>
        /// <param name="sliceEnd">End slice</param>
        /// <param name="dayStart">Day start</param>
        /// <param name="dayEnd">Day end</param>
        private void DrawSlices(ItemPaintArgs e,
            int sliceStart, int sliceEnd, int dayStart, int dayEnd)
        {
            Graphics g = e.Graphics;

            DaySlot[,] daySlots = GetDaySlots(sliceStart, sliceEnd, dayStart, dayEnd);

            DrawContent(g, sliceStart, sliceEnd, dayStart, dayEnd, daySlots);
            DrawDaySlot(g, sliceStart, sliceEnd, dayStart, dayEnd, daySlots, false);

            DrawBorders(g, sliceStart, sliceEnd, dayStart, dayEnd, daySlots);
            DrawDaySlot(g, sliceStart, sliceEnd, dayStart, dayEnd, daySlots, true);

            DrawTimeIndicators(g, sliceStart, sliceEnd);
        }

        #endregion

        #region GetDaySlots

        /// <summary>
        /// Gets the array of DaySlot information
        /// </summary>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        /// <param name="dayStart"></param>
        /// <param name="dayEnd"></param>
        /// <returns>array of DaySlots</returns>
        private DaySlot[,] GetDaySlots(int sliceStart, int sliceEnd, int dayStart, int dayEnd)
        {
            DaySlot[,] daySlots = new DaySlot[dayEnd - dayStart + 1, sliceEnd - sliceStart + 1];

            for (int i = dayStart; i <= dayEnd; i++)
            {
                for (int j = sliceStart; j <= sliceEnd; j++)
                {
                    DaySlot daySlot = new DaySlot();

                    daySlots[i - dayStart, j - sliceStart] = daySlot;

                    daySlot.Bounds = GetSliceRect(i, j);
                    daySlot.Selected = SliceIsSelected(i, j);

                    if (j < sliceEnd &&
                        CalendarView.HasViewDisplayCustomizations == true)
                    {
                        int start = (StartSlice + j) * TimeSlotDuration;
                        int end = Math.Min(start + TimeSlotDuration, 1439);

                        WorkTime wkStart = new WorkTime(start/MinutesPerHour, start%MinutesPerHour);
                        WorkTime wkEnd = new WorkTime(end/MinutesPerHour, end%MinutesPerHour);

                        daySlot.Appearance = CalendarView.ViewDisplayCustomizations.GetDaySlotAppearance(
                            OwnerKey, _DayColumns[i].Date, wkStart, wkEnd);
                    }
                }
            }

            return (daySlots);
        }

        #endregion

        #region DrawContent

        /// <summary>
        /// Time slice content drawing
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="sliceStart">Start slice</param>
        /// <param name="sliceEnd">End slice</param>
        /// <param name="dayStart">Day start</param>
        /// <param name="dayEnd">Day end</param>
        /// <param name="daySlots"></param>
        private void DrawContent(Graphics g,
            int sliceStart, int sliceEnd, int dayStart, int dayEnd, DaySlot[,] daySlots)
        {
            // Loop through each day in each week, displaying
            // the associated day content

            for (int i = dayStart; i <= dayEnd; i++)
            {
                for (int j = sliceStart; j <= sliceEnd; j++)
                {
                    DaySlot daySlot = daySlots[i - dayStart, j - sliceStart];
                    DaySlotAppearance dsa = daySlot.Appearance;

                    if (dsa != null && daySlot.Selected == false)
                    {
                        using (Brush br = new SolidBrush(dsa.BackColor))
                            g.FillRectangle(br, daySlot.Bounds);

                        continue;
                    }

                    g.FillRectangle(GetContentBrush(daySlot, i, j), daySlot.Bounds);
                }
            }
        }

        #region GetContentBrush

        /// <summary>
        /// Gets the background content brush
        /// for the given time slice
        /// </summary>
        /// <param name="daySlot"></param>
        /// <param name="col">Column index</param>
        /// <param name="slice">Time slice</param>
        /// <returns>Background brush</returns>
        private Brush GetContentBrush(DaySlot daySlot, int col, int slice)
        {
            int start = (StartSlice + slice) * TimeSlotDuration;

            if (slice < NumberOfActiveSlices)
            {
                if (daySlot.Selected == true)
                    return (SelectedBrush);

                WorkTime wkStart = new
                    WorkTime(start / MinutesPerHour, start % MinutesPerHour);

                if (_DayColumns[col].IsWorkTime(wkStart) == true)
                    return (WorkBrush);

                if (_DayColumns[col].IsBusyTime(wkStart) == true)
                    return (BusyBrush);
            }
            else
            {
                return (AllDayBrush);
            }

            return (OffWorkBrush);
        }

        #endregion

        #endregion

        #region DrawDaySlot

        /// <summary>
        /// Initiates DaySlot drawing
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        /// <param name="dayStart"></param>
        /// <param name="dayEnd"></param>
        /// <param name="daySlots"></param>
        /// <param name="onTop">On top of borders</param>
        private void DrawDaySlot(Graphics g, int sliceStart,
            int sliceEnd, int dayStart, int dayEnd, DaySlot[,] daySlots, bool onTop)
        {
            List<DaySlotAppearance> dsaList = null;
            Rectangle dRect = Rectangle.Empty;

            for (int i = dayStart; i <= dayEnd; i++)
            {
                DaySlot lastDaySlot = null;

                for (int j = sliceStart; j <= sliceEnd; j++)
                {
                    DaySlot daySlot = daySlots[i - dayStart, j - sliceStart];

                    Rectangle r = daySlot.Bounds;
                    DaySlotAppearance dsa = daySlot.Appearance;

                    if (lastDaySlot != null &&
                        (lastDaySlot.Appearance != dsa || lastDaySlot.Selected != daySlot.Selected))
                    {
                        FlushDaySlotText(g, i, dsaList, ref dRect, lastDaySlot.Selected);
                    }

                    lastDaySlot = daySlot;

                    if (dsa != null)
                    {
                        if (dsa.OnTop == onTop)
                        {
                           if (daySlot.Selected == true)
                           {
                               if (dsa.ShowTextWhenSelected == true)
                                   dRect = dRect.IsEmpty ? r : Rectangle.Union(dRect, r);
                           }
                           else
                           {
                               dRect = dRect.IsEmpty ? r : Rectangle.Union(dRect, r);
                           }

                           if (dsaList == null)
                                dsaList = new List<DaySlotAppearance>();

                            if (dsaList.Contains(dsa) == false)
                                dsaList.Add(dsa);
                        }

                        continue;
                    }
                }

                if (lastDaySlot != null)
                    FlushDaySlotText(g, i, dsaList, ref dRect, lastDaySlot.Selected);
            }
        }

        #region FlushDaySlotText

        /// <summary>
        /// Flushes out pending DaySlot drawing
        /// </summary>
        /// <param name="g"></param>
        /// <param name="dayCol"></param>
        /// <param name="dsaList"></param>
        /// <param name="dRect">Display rect</param>
        /// <param name="selected"></param>
        private void FlushDaySlotText(Graphics g,
            int dayCol, List<DaySlotAppearance> dsaList, ref Rectangle dRect, bool selected)
        {
            if (dsaList != null && dsaList.Count > 0)
            {
                if (dRect.IsEmpty == false)
                    DrawDaySlotText(g, dayCol, dsaList, dRect, selected);

                dsaList.Clear();

                dRect = Rectangle.Empty;
            }
        }

        #endregion

        #region DrawDaySlotText

        /// <summary>
        /// Draws the DaySlot Text
        /// </summary>
        /// <param name="g"></param>
        /// <param name="dayCol"></param>
        /// <param name="dsaList"></param>
        /// <param name="cRect"></param>
        /// <param name="selected"></param>
        private void DrawDaySlotText(Graphics g,
            int dayCol, IEnumerable<DaySlotAppearance> dsaList, Rectangle cRect, bool selected)
        {
            foreach (DaySlotAppearance dsa in dsaList)
            {
                DateTime startTime = GetSlotTime(dayCol, dsa.StartTime);
                DateTime endTime = GetSlotTime(dayCol, dsa.EndTime);

                DateTime e = endTime.AddMinutes(-1);
                WorkTime wkEnd = new WorkTime(e.Hour, e.Minute);

                int start = GetWorkTimeSlice(dsa.StartTime);
                int end = GetWorkTimeSlice(wkEnd);

                Rectangle r = GetSliceRect(dayCol, start);
                r = Rectangle.Union(r, GetSliceRect(dayCol, end));

                Region rgnSave = g.Clip;
                g.SetClip(cRect, CombineMode.Intersect);

                string text = dsa.Text;

                if (CalendarView.DoRenderDaySlotAppearanceText(g,
                    r, dsa, startTime, endTime, selected, ref text) == false)
                {
                    if (String.IsNullOrEmpty(dsa.Text) == false)
                    {
                        eTextFormat tf = GetTextFormat(dsa.TextAlignment);

                        Font font = dsa.Font ?? Font;

                        Color color = (selected == true)
                                          ? (dsa.SelectedTextColor.IsEmpty ? Color.Black : dsa.SelectedTextColor)
                                          : (dsa.TextColor.IsEmpty ? Color.Black : dsa.TextColor);

                        TextDrawing.DrawString(g, text, font, color, r, tf);
                    }
                }

                g.Clip = rgnSave;
            }
        }

        #region GetSlotTime

        private DateTime GetSlotTime(int dayCol, WorkTime wkTime)
        {
            DateTime time = _DayColumns[dayCol].Date;

            time = time.AddHours(wkTime.Hour);
            time = time.AddMinutes(wkTime.Minute);

            return (time);
        }

        #endregion

        #region GetWorkTimeSlice

        private int GetWorkTimeSlice(WorkTime wkTime)
        {
            int slice = ((wkTime.Hour * MinutesPerHour) +
                wkTime.Minute) / TimeSlotDuration;

            return (slice - StartSlice);
        }

        #region GetTextFormat

        private eTextFormat GetTextFormat(ContentAlignment alignment)
        {
            eTextFormat tf = eTextFormat.WordBreak;

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    tf |= eTextFormat.Top | eTextFormat.Left;
                    break;

                case ContentAlignment.TopCenter:
                    tf |= eTextFormat.Top | eTextFormat.HorizontalCenter;
                    break;

                case ContentAlignment.TopRight:
                    tf |= eTextFormat.Top | eTextFormat.Right;
                    break;

                case ContentAlignment.MiddleLeft:
                    tf |= eTextFormat.VerticalCenter | eTextFormat.Left;
                    break;

                case ContentAlignment.MiddleCenter:
                    tf |= eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter;
                    break;

                case ContentAlignment.MiddleRight:
                    tf |= eTextFormat.VerticalCenter | eTextFormat.Right;
                    break;

                case ContentAlignment.BottomLeft:
                    tf |= eTextFormat.Bottom | eTextFormat.Left;
                    break;

                case ContentAlignment.BottomCenter:
                    tf |= eTextFormat.Bottom | eTextFormat.HorizontalCenter;
                    break;

                case ContentAlignment.BottomRight:
                    tf |= eTextFormat.Bottom | eTextFormat.Right;
                    break;
            }

            return (tf);
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region DrawTimeIndicators

        #region DrawTimeIndicators

        /// <summary>
        /// Draw view TimeIndicators
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        private void DrawTimeIndicators(Graphics g, int sliceStart, int sliceEnd)
        {
            DateTime start = _DayColumns[0].Date;
            DateTime end = _DayColumns[_NumberOfColumns - 1].Date.AddDays(1);
            Rectangle r = Rectangle.Union(GetSliceRect(0, sliceStart), GetSliceRect(0, sliceEnd));

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
                            DrawTimeIndicator(g, r, ti);
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
        /// <param name="sRect"></param>
        /// <param name="ti"></param>
        private void DrawTimeIndicator(Graphics g, Rectangle sRect, TimeIndicator ti)
        {
            Rectangle r = GetIndicatorRect(ti);

            if (r.IntersectsWith(sRect) == true)
            {
                if (r.Height > 0)
                {
                    ColorDef cdef = GetIndicatorColor(ti);

                    if (cdef != null)
                    {
                        using (Brush br = _ViewColor.BrushPart(cdef, r))
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
                        g.DrawLine(pen, r.X + 1, r.Bottom, r.Right - 1, r.Bottom);
                }
            }
        }

        #endregion

        #region GetIndicatorRect

        /// <summary>
        /// Gets the TimeIndicator rectangle
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        internal override Rectangle GetIndicatorRect(TimeIndicator ti)
        {
            return (GetIndicatorRect(ti, ti.IndicatorDisplayTime));
        }

        /// <summary>
        /// Gets the TimeIndicator rectangle for the
        /// given DataTime
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        internal override Rectangle GetIndicatorRect(TimeIndicator ti, DateTime time)
        {
            DateTime startTime = GetTime(_DayColumns[0].Date, 0, 0);
            DateTime endTime = GetTime(_DayColumns[NumberOfColumns - 1].Date, NumberOfActiveSlices, 0);

            if (time >= startTime && time < endTime)
            {
                int offset = (int) (time.Hour * SlotsPerHour * TimeSliceHeight +
                                    (TimeSliceHeight * time.Minute) / TimeSlotDuration);

                offset -= (int) (StartSlice * TimeSliceHeight);

                Rectangle r = ViewRect;
                r.Width -= 1;

                r.Y = _DayColumns[0].Bounds.Y + offset - ti.Thickness;
                r.Height = ti.Thickness;

                return (r);
            }

            return (Rectangle.Empty);
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

        #endregion

        #region DrawBorders

        #region DrawBorders

        /// <summary>
        /// Draws time slice borders
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="sliceStart">Start slice</param>
        /// <param name="sliceEnd">End slice</param>
        /// <param name="dayStart">Day start</param>
        /// <param name="dayEnd">Day end</param>
        /// <param name="daySlots"></param>
        private void DrawBorders(Graphics g,
            int sliceStart, int sliceEnd, int dayStart, int dayEnd, DaySlot[,] daySlots)
        {
            // Draw the horizontal and vertical borders, and
            // the current "Now" border

            DrawHorizontalBorders(g, sliceStart, sliceEnd, dayStart, dayEnd, daySlots);
            DrawVerticalBorders(g, sliceStart, sliceEnd, dayStart, dayEnd);

            if (CalendarView.HighlightCurrentDay == true)
                DrawNowBorder(g, dayStart, dayEnd);
        }

        #endregion

        #region DrawHorizontalBorders

        /// <summary>
        /// Draws horizontal borders
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        /// <param name="dayStart"></param>
        /// <param name="dayEnd"></param>
        /// <param name="daySlots"></param>
        private void DrawHorizontalBorders(Graphics g,
            int sliceStart, int sliceEnd, int dayStart, int dayEnd, DaySlot[,] daySlots)
        {
            if (CalendarView.HasViewDisplayCustomizations == true)
            {
                int n = MinutesPerHour / TimeSlotDuration;

                Color hourBorderColor = _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHourBorder);
                Color halfHourBorderColor = _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHalfHourBorder);

                for (int i = dayStart; i <= dayEnd; i++)
                {
                    for (int j = sliceStart; j <= sliceEnd; j++)
                    {
                        DaySlot daySlot = daySlots[i - dayStart, j - sliceStart];

                        Rectangle r = daySlot.Bounds;
                        DaySlotAppearance dsa = daySlot.Appearance;

                        Point pt1 = r.Location;
                        Point pt2 = new Point(r.Right - 1, r.Y);

                        if (dsa != null && daySlot.Selected == false)
                        {
                            using (Pen pen = new Pen(j % n == 0 ? dsa.HourBorderColor : dsa.HalfHourBorderColor))
                                g.DrawLine(pen, pt1, pt2);

                            continue;
                        }

                        using (Pen pen = new Pen(j % n == 0 ? hourBorderColor : halfHourBorderColor))
                            g.DrawLine(pen, pt1, pt2);
                    }
                }
            }
            else
            {
                using (Pen pen1 = new Pen(
                    _ViewColor.GetColor((int) eCalendarWeekDayPart.DayHourBorder)))
                {
                    using (Pen pen2 = new Pen(
                        _ViewColor.GetColor((int) eCalendarWeekDayPart.DayHalfHourBorder)))
                    {
                        Point pt1 = new Point(_DayColumns[dayStart].Bounds.X, 0);
                        Point pt2 = new Point(_DayColumns[dayEnd].Bounds.Right - 1, 0);

                        int n = MinutesPerHour/TimeSlotDuration;

                        for (int i = sliceStart; i <= sliceEnd; i++)
                        {
                            Rectangle r = GetSliceRect(0, i);

                            pt1.Y = pt2.Y = r.Y;

                            g.DrawLine((i%n) == 0 ? pen1 : pen2, pt1, pt2);
                        }
                    }
                }
            }
        }

        #endregion

        #region DrawVerticalBorders

        /// <summary>
        /// Draws the vertical borders
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        /// <param name="dayStart"></param>
        /// <param name="dayEnd"></param>
        private void DrawVerticalBorders(Graphics g,
            int sliceStart, int sliceEnd, int dayStart, int dayEnd)
        {
            Rectangle r1 = GetSliceRect(0, sliceStart);
            Rectangle r2 = GetSliceRect(0, sliceEnd);

            using (Pen pen1 = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayViewBorder)))
            {
                Point pt1 = new Point(0, r1.Y);
                Point pt2 = new Point(0, r2.Bottom);

                for (int i = dayStart; i <= dayEnd; i++)
                {
                    pt1.X = pt2.X = _DayColumns[i].Bounds.X;

                    g.DrawLine(pen1, pt1, pt2);
                }

                pt1.X = pt2.X = _DayColumns[dayEnd].Bounds.Right;

                g.DrawLine(pen1, pt1, pt2);
            }
        }

        #endregion

        #region DrawNowBorder

        /// <summary>
        /// Draws the Current-Day Now border
        /// </summary>
        /// <param name="g"></param>
        /// <param name="dayStart"></param>
        /// <param name="dayEnd"></param>
        private void DrawNowBorder(Graphics g, int dayStart, int dayEnd)
        {
            // Draw the "Current" day border

            for (int i = dayStart; i <= dayEnd; i++)
            {
                if (_DayColumns[i].Date.Date.Equals(DateTime.Now.Date))
                {
                    Rectangle r = _DayColumns[i].Bounds;

                    using (Pen pen = new Pen(
                        _ViewColor.GetColor((int)eCalendarWeekDayPart.NowDayHeaderBorder)))
                    {
                        g.DrawRectangle(pen, r);
                    }
                    break;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region DrawWeekAppointments

        /// <summary>
        /// Initiates the drawing of weekly appointments
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="dayStart">Day start index</param>
        /// <param name="dayEnd">Day end index</param>
        private void DrawWeekAppointments(ItemPaintArgs e, int dayStart, int dayEnd)
        {
            if (e.ClipRectangle.IntersectsWith(ViewRect) == true)
            {
                // Loop through each day in each week, displaying
                // the associated day content

                CalendarItem selItem = null;

                for (int i = 0; i <= dayEnd; i++)
                {
                    List<CalendarItem> items = _DayColumns[i].CalendarItems;

                    if (items != null && items.Count > 0)
                    {
                        // Loop through each CalendarItem in the week

                        int right = _DayColumns[i].Bounds.Right - 1;

                        for (int j = 0; j < items.Count; j++)
                        {
                            CalendarItem item = items[j];

                            // If we can display the item, then initiate the paint

                            if (item.Displayed == true &&
                               (i >= dayStart || item.Bounds.Right > right))
                            {
                                if (item.IsSelected == true)
                                    selItem = item;
                                else
                                    item.Paint(e);
                            }
                        }
                    }
                }

                if (selItem != null)
                    selItem.Paint(e);
            }
        }

        #endregion

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

                    if (InPanelResize(objArg.Location) == true)
                    {
                        MyCursor = Cursors.HSplit;

                        _IsPanelResizing = true;
                    }
                    else if (ViewRect.Contains(objArg.Location) == true)
                    {
                        int col, slice;

                        if (GetPointItem(objArg.Location, out col, out slice, true))
                        {
                            MyCursor = GetContentCursor();
                            
                            if (ProcessCilButtonDown(col, objArg) == false)
                                ProcessDvlButtonDown(col, slice);

                            IsMouseDown = true;
                        }

                        IsCopyDrag = false;
                    }
                }
            }
        }

        #region CalendarItem MouseDown processing

        /// <summary>
        /// CalendarItem left mouseDown processing
        /// </summary>
        /// <param name="col">DayColumn col index</param>
        /// <param name="objArg">MouseEventArgs</param>
        private bool ProcessCilButtonDown(int col, MouseEventArgs objArg)
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
                        _LastCol = col;
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

                        if (item.HitArea == CalendarItem.eHitArea.TopResize)
                            IsStartResizing = true;

                        else if (item.HitArea == CalendarItem.eHitArea.BottomResize)
                            IsEndResizing = true;

                        else if (item.HitArea == CalendarItem.eHitArea.Move)
                            IsMoving = true;

                        // Update our initial PosWin display

                        UpdatePosWin(ViewRect);
                    }
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #region DayView MouseDown processing

        /// <summary>
        /// Handles DayView left MouseDown events
        /// </summary>
        /// <param name="col">DayColumn col index</param>
        /// <param name="slice">Time slice</param>
        private void ProcessDvlButtonDown(int col, int slice)
        {
            DateTime startDate = GetTime(_DayColumns[col].Date, slice, 0);
            DateTime endDate = startDate.AddMinutes(TimeSlotDuration);

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

            CancelScrollTimer();

            // Reset our panel resizing flag

            _IsPanelResizing = false;
        }

        #endregion

        #region MouseMove processing

        #region InternalMouseMove

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
                if (_IsPanelResizing == true)
                {
                    MyCursor = Cursors.HSplit;

                    ProcessPanelMove(objArg);
                }
                else
                {
                    if (InPanelResize(objArg.Location) == true)
                    {
                        MyCursor = Cursors.HSplit;
                    }
                    else
                    {
                        MyCursor = GetContentCursor();

                        if (objArg.Button == MouseButtons.Left)
                            ProcessContentMove(objArg);
                    }
                }
            }
        }

        #endregion

        #region InPanelResize

        /// <summary>
        /// Determines if the mouse is in the
        /// panel resize area
        /// </summary>
        /// <param name="pt">Mouse location</param>
        /// <returns>true if in the resize area</returns>
        private bool InPanelResize(Point pt)
        {
            if (CalendarView.IsMultiCalendar == true)
            {
                Rectangle r = ViewRect;

                if (pt.X > r.Left && pt.X < r.Right &&
                    pt.Y > r.Y - 3 && pt.Y < r.Y + 3)
                {
                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region ProcessPanelMove

        /// <summary>
        /// Processes the actual panel resizing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        private void ProcessPanelMove(MouseEventArgs objArg)
        {
            if (_IsPanelResizing == true)
            {
                Point pt = objArg.Location;

                if (pt.Y < ClientRect.Y + DayOfWeekHeaderHeight)
                    pt.Y = ClientRect.Y + DayOfWeekHeaderHeight;

                else if (pt.Y > ClientRect.Bottom - 50)
                    pt.Y = ClientRect.Bottom - 50;

                int n = pt.Y - (ClientRect.Y + DayOfWeekHeaderHeight);

                CalendarView.FixedAllDayPanelHeight = n;
            }
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

            int col, slice;

            if (GetPointItem(objArg.Location, out col, out slice, false))
            {
                // The slice is visible, so no need to
                // enable scrolling - just process the event

                EnableViewScrolling(false);

                ProcessMouseMove(col, slice, objArg);
            }
            else if (IsMouseDown == true)
            {
                if (DragDropAppointment(objArg) == false)
                {
                    // The selected slice is not visible,
                    // so we need to enable scrolling

                    EnableViewScrolling(true);

                    // Only process the event if the user is selecting
                    // time slice cells (auto moving apps is intrusive looking)

                    if (DateSelectionAnchor != null)
                    {
                        if (col >= 0)
                        {
                            if (slice < 0)
                                slice = 0;

                            ProcessDvMouseMove(col, slice);
                        }
                    }
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
                WeekDayView wv = bv as WeekDayView;

                if (wv != null && wv != this)
                {
                    eViewArea area = bv.GetViewAreaFromPoint(pt);

                    if (area == eViewArea.InContent)
                    {
                        DragCopy();

                        ClearMouseStates();
                        CancelScrollTimer();

                        AppointmentView av = SelectedItem as AppointmentView;

                        if (av != null)
                            return (wv.DragAppointment(this, av));

                        CustomCalendarItem ci = SelectedItem as CustomCalendarItem;

                        if (ci != null)
                            return (wv.DragCustomItem(this, ci));
                    }
                }
            }

            return (false);
        }

        #region DragCustomItem

        private bool DragCustomItem(WeekDayView pv, CustomCalendarItem ci)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            if (ci.BaseCalendarItem != null)
                ci = ci.BaseCalendarItem;

            ci.OwnerKey = OwnerKey;

            if (NumberOfColumns > 1)
            {
                DateTime sd = ci.StartTime;
                TimeSpan ts = ci.EndTime.Subtract(ci.StartTime);

                if (Bounds.X > pv.Bounds.X)
                {
                    ci.StartTime = new DateTime(_DayColumns[0].Date.Year,
                        _DayColumns[0].Date.Month, _DayColumns[0].Date.Day, sd.Hour, sd.Minute, 0);
                }
                else
                {
                    ci.StartTime = new DateTime(_DayColumns[NumberOfColumns - 1].Date.Year,
                        _DayColumns[NumberOfColumns - 1].Date.Month,
                        _DayColumns[NumberOfColumns - 1].Date.Day, sd.Hour, sd.Minute, 0);
                }

                ci.EndTime = ci.StartTime.Add(ts);
            }

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
        /// <param name="av">Item to move</param>
        private bool DragAppointment(WeekDayView pv, AppointmentView av)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            av.Appointment.OwnerKey = OwnerKey;

            if (NumberOfColumns > 1)
            {
                DateTime sd = av.Appointment.StartTime;

                if (Bounds.X > pv.Bounds.X)
                {
                    av.Appointment.MoveTo(
                        new DateTime(_DayColumns[0].Date.Year, _DayColumns[0].Date.Month,
                        _DayColumns[0].Date.Day, sd.Hour, sd.Minute, 0));
                }
                else
                {
                    av.Appointment.MoveTo(
                        new DateTime(_DayColumns[NumberOfColumns - 1].Date.Year,
                        _DayColumns[NumberOfColumns - 1].Date.Month,
                        _DayColumns[NumberOfColumns - 1].Date.Day, sd.Hour, sd.Minute, 0));
                }
            }

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

        private void SetNewDragItem(WeekDayView pv, CalendarItem view)
        {
            _LastBounds = pv._LastBounds;
            _LastMovePoint = pv._LastMovePoint;
            _LastPointOffset = pv._LastPointOffset;

            int dy = _LastPointOffset.Y - _LastBounds.Y;

            CalendarView.CalendarPanel.InternalMouseMove(new
                MouseEventArgs(MouseButtons.None, 0, view.Bounds.X, view.Bounds.Y + dy, 0));

            MouseEventArgs args = new
                MouseEventArgs(MouseButtons.Left, 1, view.Bounds.X, view.Bounds.Y + dy, 0);

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
                    case CalendarItem.eHitArea.TopResize:
                    case CalendarItem.eHitArea.BottomResize:
                        return (Cursors.SizeNS);

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
        /// <param name="slice">Slice</param>
        /// <param name="objArg"></param>
        private void ProcessMouseMove(int col, int slice, MouseEventArgs objArg)
        {
            if (DateSelectionAnchor != null)
            {
                ProcessDvMouseMove(col, slice);
            }
            else if (SelectedItem != null)
            {
                if (objArg.Location.Equals(_LastMovePoint) == false)
                {
                    if (IsMoving == true)
                        ProcessItemMove(col, slice, objArg);

                    else if (IsStartResizing == true)
                        ProcessItemTopResize(slice, objArg);

                    else if (IsEndResizing == true)
                        ProcessItemBottomResize(slice, objArg);

                    _LastMovePoint = objArg.Location;
                }
            }
        }

        #endregion

        #region DayView mouseMove processing

        /// <summary>
        /// Processes DayView mouseMove events
        /// </summary>
        /// <param name="col">DayColumn col index</param>
        /// <param name="slice">Time slice</param>
        private void ProcessDvMouseMove(int col, int slice)
        {
            DateTime date = GetTime(_DayColumns[col].Date, slice, 0);

            // Let the user select forwards or backwards

            if (date >= DateSelectionAnchor)
            {
                CalendarView.DateSelectionStart = DateSelectionAnchor.Value;
                CalendarView.DateSelectionEnd = date.AddMinutes(TimeSlotDuration);
            }
            else
            {
                CalendarView.DateSelectionStart = date;
                CalendarView.DateSelectionEnd = DateSelectionAnchor.Value.AddMinutes(TimeSlotDuration);
            }
        }

        #endregion

        #region CalendarItem MouseMove processing

        #region ProcessItemMove

        /// <summary>
        /// Processes CalendarItem mouseMove events
        /// </summary>
        /// <param name="col">DayColumn col index</param>
        /// <param name="slice">Time slice</param>
        /// <param name="objArg"></param>
        private void ProcessItemMove(int col, int slice, MouseEventArgs objArg)
        {
            // Calculate our new item date

            int dm = GetDeltaMinutes(slice, true, objArg);

            DateTime newDate = GetTime(_DayColumns[col].Date, 0, dm);
            TimeSpan ts = SelectedItem.EndTime - SelectedItem.StartTime;

            if (newDate != SelectedItem.StartTime)
            {
                // Make sure we don't cross day boundaries

                DateTime start = GetTime(_DayColumns[col].Date, 0, 0);
                DateTime end = start.AddMinutes(NumberOfActiveSlices * TimeSlotDuration);

                if (newDate < start)
                    newDate = start;

                if (newDate + ts > end)
                    newDate -= (newDate + ts) - end;

                // If we have a new date, set our item to it

                if (newDate != SelectedItem.StartTime)
                {
                    if (DragCopy() == false)
                    {
                        try
                        {
                            if (SelectedItem is CustomCalendarItem)
                                CalendarView.CustomItems.BeginUpdate();
                            else
                                CalendarModel.BeginUpdate();

                            // Make the move

                            if (CalendarView.DoAppointmentViewChanging(SelectedItem, newDate,
                                newDate + ts, eViewOperation.AppointmentMove,
                                IsCopyDrag) == false)
                            {
                                SelectedItem.StartTime = newDate;
                                SelectedItem.EndTime = newDate + ts;

                                InvalidateItems(col);
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
        }

        #endregion

        #endregion

        #region CalendarItem MouseResize processing

        #region ProcessItemTopResize

        /// <summary>
        /// Processes CalendarItem left resizing
        /// </summary>
        /// <param name="slice">Time slice</param>
        /// <param name="objArg"></param>
        private void ProcessItemTopResize(int slice, MouseEventArgs objArg)
        {
            int dm = GetDeltaMinutes(slice, true, objArg);

            DateTime newStartTime = new DateTime(
                SelectedItem.StartTime.Year, SelectedItem.StartTime.Month, SelectedItem.StartTime.Day);

            newStartTime = GetTime(newStartTime, 0, dm);

            if (newStartTime != SelectedItem.StartTime && newStartTime < SelectedItem.EndTime)
            {
                if ((SelectedItem.EndTime - newStartTime).TotalMinutes < 0)
                    newStartTime = SelectedItem.EndTime.AddMinutes(-1);

                ResizeItem(_LastCol, newStartTime, SelectedItem.EndTime);
            }
        }

        #endregion

        #region ProcessItemBottomResize

        /// <summary>
        /// Processes CalendarItem right resizing
        /// </summary>
        /// <param name="slice">Time slice</param>
        /// <param name="objArg"></param>
        private void ProcessItemBottomResize(int slice, MouseEventArgs objArg)
        {
            int dm = GetDeltaMinutes(slice, false, objArg);

            DateTime newEndTime = new DateTime(
                SelectedItem.StartTime.Year, SelectedItem.StartTime.Month, SelectedItem.StartTime.Day);

            newEndTime = GetTime(newEndTime, 0, dm);

            DateTime end = GetTime(_DayColumns[_LastCol].Date, NumberOfActiveSlices, 0);

            if (newEndTime > end)
                newEndTime = end;

            if (newEndTime != SelectedItem.EndTime && newEndTime > SelectedItem.StartTime)
            {
                if ((newEndTime - SelectedItem.StartTime).TotalMinutes < 0)
                    newEndTime = SelectedItem.StartTime.AddMinutes(1);

                ResizeItem(_LastCol, SelectedItem.StartTime, newEndTime);
            }
        }

        #endregion

        #region ResizeItem

        /// <summary>
        /// Initiates the resize of the selected item
        /// </summary>
        /// <param name="col"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        private void ResizeItem(int col, DateTime startTime, DateTime endTime)
        {
            // Let the user cancel the operation if desired

            if (CalendarView.DoAppointmentViewChanging(SelectedItem, startTime,
                endTime, eViewOperation.AppointmentResize, IsCopyDrag) == false)
            {
                SelectedItem.StartTime = startTime;
                SelectedItem.EndTime = endTime;

                InvalidateItems(col);
            }
        }

        #endregion

        #endregion

        #region GetDeltaMinutes

        /// <summary>
        /// Gets the changes in minutes
        /// from the last mouse operation (move or resize)
        /// </summary>
        /// <param name="slice">Current slice</param>
        /// <param name="ftop">Flag denoting top or bottom delta</param>
        /// <param name="objArg">MouseEventArgs</param>
        /// <returns>Delta minutes</returns>
        private int GetDeltaMinutes(int slice, bool ftop, MouseEventArgs objArg)
        {
            // Calculate our delta minutes

            Rectangle t = GetSliceRect(0, slice);

            int dm = (objArg.Location.Y - _LastPointOffset.Y);

            dm += ftop ? _LastBounds.Top - t.Top
                       : _LastBounds.Bottom - t.Bottom;

            dm = (int)(TimeSlotDuration * dm / TimeSliceHeight);

            // If the Alt key is not pressed, then round
            // our value off to the nearest slice

            if ((Control.ModifierKeys & Keys.Alt) != Keys.Alt)
            {
                slice += (dm / TimeSlotDuration);
                dm = 0;
            }

            dm += (slice * TimeSlotDuration);

            // Make sure we don't cross our day boundaries

            if (ftop == true)
            {
                if (dm < 0)
                    dm = 0;
            }
            else
            {
                dm += TimeSlotDuration;

                if (dm >= MinutesPerHour * HoursPerDay)
                    dm = MinutesPerHour * HoursPerDay - 1;
            }

            return (dm);
        }

        #endregion

        #region InvalidateItems

        /// <summary>
        /// Invalidates altered DayColumns
        /// </summary>
        /// <param name="col">Current column</param>
        internal void InvalidateItems(int col)
        {
            if (SelectedItem.IsRecurring == false)
            {
                _DayColumns[_LastCol].NeedRecalcLayout = true;
                _DayColumns[col].NeedRecalcLayout = true;

                InvalidateRect(_DayColumns[col].Bounds, true);

                if (_LastCol != col)
                    InvalidateRect(_DayColumns[_LastCol].Bounds);
            }
            else
            {
                NeedRecalcLayout = true;
            }

            _LastCol = col;
        }

        #endregion

        #region View Scrolling support

        #region EnableViewScrolling

        /// <summary>
        /// Routine to enable or disable view scrolling
        /// </summary>
        /// <param name="enable">true to enable</param>
        private void EnableViewScrolling(bool enable)
        {
            if (enable == true)
            {
                if (VsPanel.ScrollBar.Maximum > VsPanel.ScrollBar.LargeChange)
                {
                    if (_ScrollViewTimer == null)
                    {
                        _ScrollViewTimer = new Timer();

                        _ScrollViewTimer.Interval = 10;
                        _ScrollViewTimer.Tick += ScrollViewTimerTick;
                        _ScrollViewTimer.Start();
                    }
                }
            }
            else
            {
                CancelScrollTimer();
            }
        }

        #endregion

        #region CancelScrollTimer

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

        #endregion

        #region ScrollAmount

        /// <summary>
        /// Determines the amount to scroll (which is
        /// based loosely upon the delta magnitude)
        /// </summary>
        /// <param name="delta">Point delta</param>
        /// <returns>Scroll amount</returns>
        private int ScrollAmount(int delta)
        {
            int dy = Math.Abs(delta);
            int amt = (dy < 16 ? 2 : dy < 32 ? 8 : 32);

            return (delta < 0 ? -amt : amt);
        }

        #endregion

        #region ScrollViewTimer_Tick

        /// <summary>
        /// Handles view scroll timer ticks
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void ScrollViewTimerTick(object sender, EventArgs e)
        {
            Control c = (Control)this.GetContainerControl(true);

            if (c != null)
            {
                // Calculate our delta

                Point pt = c.PointToClient(Cursor.Position);
                Rectangle r = ViewRect;

                int dy = (pt.Y < r.Top) ? ScrollAmount(pt.Y - r.Top) :
                    (pt.Y >= r.Bottom) ? ScrollAmount(pt.Y - r.Bottom) : 0;

                // Scroll if necessary

                if (dy != 0)
                {
                    int value = VsPanel.ScrollBar.Value + dy;

                    value = Math.Max(value, 0);
                    value = Math.Min(value, VsPanel.ScrollBar.Maximum);

                    if (VsPanel.ScrollBar.Value != value)
                    {
                        VsPanel.ScrollBar.Value = value;

                        if (PosWin != null)
                            PosWin.Hide();

                        InternalMouseMove(new
                            MouseEventArgs(MouseButtons.Left, 0, pt.X, pt.Y, 0));
                    }
                }
            }
        }

        #endregion

        #endregion

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
                    ProcessUpDownKey(objArg, -CalendarView.TimeSlotDuration);
                    break;

                case Keys.Down:
                case Keys.Down | Keys.Shift:
                case Keys.Down | Keys.Control:
                case Keys.Down | Keys.Control | Keys.Shift:
                    ProcessUpDownKey(objArg, CalendarView.TimeSlotDuration);
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

        #region ProcessUpDownKey

        /// <summary>
        /// Processes Up and Down key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dy"></param>
        protected virtual void ProcessUpDownKey(KeyEventArgs objArg, int dy)
        {
            if (ValidDateSelection())
            {
                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if (startDate.Equals(DateSelectionAnchor.Value) == true)
                    startDate = endDate.AddMinutes(-TimeSlotDuration);

                startDate = startDate.AddMinutes(dy);

                DateTime viewStart = GetTime(DayColumns[0].Date, 0, 0);
                DateTime viewEnd = viewStart.AddMinutes(NumberOfActiveSlices * TimeSlotDuration);

                endDate = startDate.AddMinutes(TimeSlotDuration);

                if (startDate >= viewStart && endDate <= viewEnd)
                {
                    ExtendSelection(ref startDate, ref endDate);

                    CalendarView.DateSelectionStart = startDate;
                    CalendarView.DateSelectionEnd = endDate;

                    EnsureSelectionVisible();
                }
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

                startDate = startDate.AddDays(dx);
                endDate = endDate.AddDays(dx);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;
                DateSelectionAnchor = DateSelectionAnchor.Value.AddDays(dx);
            }

            CalendarView.DayViewDate = CalendarView.DayViewDate.AddDays(dx);

            objArg.Handled = true;
        }

        #endregion

        #region ProcessHomeKey

        /// <summary>
        /// Processes Home key events
        /// </summary>
        /// <param name="objArg"></param>
        protected virtual void ProcessHomeKey(KeyEventArgs objArg)
        {
            int col = GetHomeEndCol();

            if (col >= 0)
            {
                DateTime startDate = GetTime(DayColumns[col].Date, 0, 0);

                if ((objArg.Modifiers & Keys.Control) != Keys.Control)
                {
                    WorkTime wt = DayColumns[col].WorkStartTime;

                    if (wt.IsEmpty == false)
                        startDate = DayColumns[col].Date.AddMinutes(wt.Hour * 60 + wt.Minute);
                }

                DateTime endDate = startDate.AddMinutes(TimeSlotDuration);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                EnsureSelectionVisible();
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessEndKey

        /// <summary>
        /// Processes End Key events
        /// </summary>
        /// <param name="objArg"></param>
        protected virtual void ProcessEndKey(KeyEventArgs objArg)
        {
            int col = GetHomeEndCol();

            if (col >= 0)
            {
                DateTime startDate = GetTime(DayColumns[col].Date, NumberOfActiveSlices, 0);

                if ((objArg.Modifiers & Keys.Control) != Keys.Control)
                {
                    WorkTime wt = DayColumns[col].WorkEndTime;

                    if (wt.IsEmpty == false)
                        startDate = DayColumns[col].Date.AddMinutes(wt.Hour * 60 + wt.Minute);
                }

                DateTime endDate = startDate;
                startDate = startDate.AddMinutes(-TimeSlotDuration);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                EnsureSelectionVisible();
            }

            objArg.Handled = true;
        }

        #endregion

        #region GetHomeEndCol

        /// <summary>
        /// Gets the Home and End column from the current
        /// selection range
        /// </summary>
        /// <returns></returns>
        private int GetHomeEndCol()
        {
            if (ValidDateSelection() == true)
            {
                if (CalendarView.DateSelectionStart.Equals(DateSelectionAnchor.Value) == true)
                    return (GetColumnFromDate(DateSelectionEnd.Value.AddMinutes(-TimeSlotDuration)));

                return (GetColumnFromDate(DateSelectionStart.Value));
            }

            return ((NumberOfColumns == 1) ? 0 : -1);
        }

        #endregion

        #region GetColumnFromDate

        /// <summary>
        /// Gets the view column from the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected int GetColumnFromDate(DateTime date)
        {
            for (int i = 0; i < DayColumns.Length; i++)
            {
                DateTime dc = DayColumns[i].Date.Date;

                if (dc <= date && dc.AddDays(1) > date)
                    return (i);
            }

            return (-1);
        }

        #endregion

        #region EnsureSelectionVisible

        /// <summary>
        /// Ensures the given selection is visible
        /// </summary>
        protected virtual void EnsureSelectionVisible()
        {
            DateTime start = CalendarView.DateSelectionStart.Value;
            DateTime end = CalendarView.DateSelectionEnd.Value;

            bool refreshed = start.Equals(DateSelectionAnchor) ?
                EnsureDateVisible(end.AddMinutes(-CalendarView.TimeSlotDuration)) :
                EnsureDateVisible(start);

            if (refreshed == false)
                Refresh();
        }

        #endregion

        #region EnsureDateVisible

        /// <summary>
        /// Ensures the given date is visible
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected virtual bool EnsureDateVisible(DateTime date)
        {
            if (CalendarView.WeekDayVScrollPanel.ScrollBar.Maximum > 
                CalendarView.WeekDayVScrollPanel.ScrollBar.LargeChange)
            {
                Rectangle r = ViewRect;

                int offset = (int)(date.Hour * CalendarView.SlotsPerHour * CalendarView.TimeSliceHeight +
                    (CalendarView.TimeSliceHeight * date.Minute) / CalendarView.TimeSlotDuration);

                offset -= (int)(CalendarView.StartSlice * CalendarView.TimeSliceHeight);

                int top = CalendarView.WeekDayVScrollPanel.ScrollBar.Value;
                int bottom = top + r.Height;

                if (offset < top || offset + CalendarView.TimeSliceHeight > bottom)
                {
                    if (offset + CalendarView.TimeSliceHeight > bottom)
                        offset -= (int)(r.Height - CalendarView.TimeSliceHeight);

                    offset = Math.Min(offset, CalendarView.WeekDayVScrollPanel.ScrollBar.Maximum);
                    offset = Math.Max(offset, 0);

                    CalendarView.WeekDayVScrollPanel.ScrollBar.Value = offset;

                    return (true);
                }

                return (false);
            }

            return (true);
        }

        #endregion

        #endregion

        #region TimeSlice support routines

        #region GetPointItem

        /// <summary>
        /// Gets the column and slice index item for
        /// the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="col">[out] DayColumn column</param>
        /// <param name="slice">[out] Time slice</param>
        /// <param name="partial"></param>
        /// <returns>Item visible state</returns>
        private bool GetPointItem(Point pt, out int col, out int slice, bool partial)
        {
            slice = -1;

            if ((col = GetPointCol(pt)) >= 0)
            {
                slice = GetPointSlice(pt, col);

                if (slice >= 0 && slice < NumberOfActiveSlices)
                    return (IsSliceVisible(col, slice, partial));
            }

            return (false);
        }

        #endregion

        #region IsSliceVisible

        /// <summary>
        /// Determines if a given slice is visible
        /// </summary>
        /// <param name="col">DayColumn</param>
        /// <param name="slice">Slice in question</param>
        /// <param name="partial">Partially visible is ok</param>
        /// <returns>Slice visibility</returns>
        private bool IsSliceVisible(int col, int slice, bool partial)
        {
            Rectangle r = GetSliceRect(col, slice);
            Rectangle t = ViewRect;

            if (partial == true)
            {
                if (r.Bottom < t.Top)
                    return (false);

                if (r.Top > t.Bottom)
                    return (false);
            }
            else
            {
                if (r.Top < t.Top)
                    return (false);

                if (r.Bottom > t.Bottom)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region GetSliceRect

        /// <summary>
        /// Gets the given slice rectangle
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="slice">Slice</param>
        /// <returns>Bounding rectangle</returns>
        private Rectangle GetSliceRect(int col, int slice)
        {
            Rectangle r = _DayColumns[col].Bounds;

            float n = slice * TimeSliceHeight;

            r.Y += (int)(n);
            r.Height = (int)(n + TimeSliceHeight) - (int)n;

            return (r);
        }

        #endregion

        #region GetPointCol

        /// <summary>
        /// Gets the col index for the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>Column</returns>
        private int GetPointCol(Point pt)
        {
            for (int i = 0; i < _NumberOfColumns; i++)
            {
                if (pt.X >= _DayColumns[i].Bounds.X &&
                    pt.X < _DayColumns[i].Bounds.Right)
                {
                    return (i);
                }
            }

            return (-1);
        }

        #endregion

        #region GetPointSlice

        /// <summary>
        /// Gets the slice index for the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="col"></param>
        /// <returns>Slice index</returns>
        private int GetPointSlice(Point pt, int col)
        {
            int s1 = (int)(-_VScrollPos / TimeSliceHeight);

            Rectangle r = GetSliceRect(col, s1);

            if (pt.Y < r.Top)
                return (s1 - 1);

            for (int i = s1; i < NumberOfActiveSlices; i++)
            {
                r = GetSliceRect(col, i);

                if (pt.Y >= r.Top && pt.Y < r.Bottom)
                    return (i);
            }

            return (NumberOfActiveSlices - 1);
        }

        #endregion

        #region GetTime

        /// <summary>
        /// Gets the DateTime adjusted by the given
        /// slice and minutes delta
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slice"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private DateTime GetTime(DateTime date, int slice, int minutes)
        {
            return (date.AddMinutes((StartSlice + slice) * TimeSlotDuration + minutes));
        }

        #endregion

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                ResetView();

                BusyBrush = null;
                WorkBrush = null;
                OffWorkBrush = null;
                SelectedBrush = null;

                HookEvents(false);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Copy object support

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            WeekDayView objCopy = new WeekDayView(this.CalendarView, this.ECalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the WeekDayView specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New WeekDayView instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            WeekDayView objCopy = copy as WeekDayView;
            base.CopyToItem(objCopy);
        }

        #endregion

        #region DaySlot class def

        private class DaySlot
        {
            public Rectangle Bounds;
            public bool Selected;
            public DaySlotAppearance Appearance;
        }

        #endregion
    }
}
#endif
