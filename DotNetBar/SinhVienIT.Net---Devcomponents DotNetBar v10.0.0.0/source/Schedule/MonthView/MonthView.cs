#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class MonthView : BaseView
    {
        #region Constants
        private const int MaxNumberOfWeeks = 52;
        #endregion

        #region Events

        public event EventHandler<IsSideBarVisibleChangedEventArgs> IsSideBarVisibleChanged;

        #endregion

        #region Private variables

        private int _NumberOfWeeks;             // Number of week in display
        private MonthWeek[] _MonthWeeks;        // Array of weeks

        private CalendarMonthColor _ViewColor =         // View display color table
            new CalendarMonthColor(eCalendarColor.Automatic);

        private int _DaysOffset;                // Moving day offset
        private bool _IsSideBarVisible = true;  // Flag denoting SideBar visibility

        private Size _MoreImageSize = new Size(16, 16);
        private Bitmap _MoreImage;
        private Bitmap _MoreImageHot;

        private ItemRect _MoreRect;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView">Parent CalendarView</param>
        public MonthView(CalendarView calendarView)
            : base(calendarView, eCalendarView.Month)
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

        #region Public properties

        #region MonthWeek

        public MonthWeek[] MonthWeeks
        {
            get { return (_MonthWeeks); }
        }

        #endregion

        #region IsSideBarVisible

        /// <summary>
        /// Gets and sets the SideBar visibility
        /// </summary>
        public bool IsSideBarVisible
        {
            get { return (_IsSideBarVisible); }

            set
            {
                if (_IsSideBarVisible != value)
                {
                    bool oldValue = _IsSideBarVisible;
                    _IsSideBarVisible = value;

                    OnIsSideBarVisibleChanged(oldValue, value);

                    InvalidateRect(true);
                }
            }
        }

        /// <summary>
        /// OnIsSideBarVisibleChanged event propagation
        /// </summary>
        protected virtual void OnIsSideBarVisibleChanged(bool oldVal, bool newVal)
        {
            if (IsSideBarVisibleChanged != null)
                IsSideBarVisibleChanged(this, new IsSideBarVisibleChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the sidebar width
        /// </summary>
        private int SideBarWidth
        {
            get
            {
                return (_IsSideBarVisible ? Font.Height + 6 : 0);
            }
        }

        /// <summary>
        /// Gets the DayHeader height
        /// </summary>
        private int DayHeaderHeight
        {
            get { return (Font.Height + 4); }
        }

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
                CalendarView.MonthViewStartDateChanged += MonthViewStartDateChanged;
                CalendarView.MonthViewEndDateChanged += MonthViewEndDateChanged;
                CalendarView.IsMonthSideBarVisibleChanged += IsMonthSideBarVisibleChanged;
            }
            else
            {
                CalendarView.SelectedViewChanged -= SelectedViewChanged;
                CalendarView.MonthViewStartDateChanged -= MonthViewStartDateChanged;
                CalendarView.MonthViewEndDateChanged -= MonthViewEndDateChanged;
                CalendarView.IsMonthSideBarVisibleChanged -= IsMonthSideBarVisibleChanged;
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
            IsViewSelected = (e.NewValue == eCalendarView.Month);

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

        #region MonthViewStartDateChanged

        /// <summary>
        /// Processes StartDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MonthViewStartDateChanged(object sender, DateChangeEventArgs e)
        {
            StartDate = e.NewValue;
        }

        #endregion

        #region MonthViewEndDateChanged

        /// <summary>
        /// Processes EndDate changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MonthViewEndDateChanged(object sender, DateChangeEventArgs e)
        {
            EndDate = e.NewValue;
        }

        #endregion

        #region IsMonthSideBarVisibleChanged

        /// <summary>
        /// Processes CalendarView IsMonthSideBarVisibleChanged events
        /// </summary>
        /// <param name="sender">CalendarView</param>
        /// <param name="e">SelectedViewEventArgs</param>
        void IsMonthSideBarVisibleChanged(
            object sender, IsMonthSideBarVisibleChangedEventArgs e)
        {
            IsSideBarVisible = e.NewValue;
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
                    if (pt.X < Bounds.X + SideBarWidth)
                        return (eViewArea.InSideBarPanel);

                    if (pt.Y < ClientRect.Y + DayOfWeekHeaderHeight)
                        return (eViewArea.InDayOfWeekHeader);

                    if (pt.X >= _MonthWeeks[0].Bounds.X)
                    {
                        for (int i = 0; i < _NumberOfWeeks; i++)
                        {
                            if (pt.Y < _MonthWeeks[i].Bounds.Y + DayHeaderHeight)
                                return (eViewArea.InWeekHeader);

                            if (pt.Y < _MonthWeeks[i].Bounds.Bottom)
                                return (eViewArea.InContent);
                        }
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

            int week, day;

            if (GetPointItem(pt, out week, out day))
            {
                startDate = _MonthWeeks[week].FirstDayOfWeek.AddDays(day);
                endDate = startDate.AddDays(1);

                return (true);
            }

            return (false);
        }

        #endregion

        #region GetItemWeek

        public MonthWeek GetItemWeek(CalendarItem item)
        {
            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                if (_MonthWeeks[i].CalendarItems.Contains(item))
                    return (_MonthWeeks[i]);
            }

            return (null);
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

            if (ci != null && IsSelecting == false)
            {
                try
                {
                    IsSelecting = true;

                    if (ci.IsSelected == true)
                    {
                        SelectedItem = SetSelectedItem(SelectedItem, ci);
                    }
                    else
                    {
                        if (ci == SelectedItem)
                            SelectedItem = null;
                    }
                }
                finally
                {
                    IsSelecting = false;
                }
            }
        }

        /// <summary>
        /// Sets the current selected item
        /// </summary>
        /// <param name="pci">Previous CalendarItem</param>
        /// <param name="nci">New CalendarItem to select</param>
        /// <returns>Base selected CalendarItem</returns>
        protected override CalendarItem SetSelectedItem(CalendarItem pci, CalendarItem nci)
        {
            CalendarItem selItem = null;

            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                List<CalendarItem> items = _MonthWeeks[i].CalendarItems;

                for (int j = 0; j < items.Count; j++)
                {
                    CalendarItem item = items[j];

                    item.IsSelected = (nci != null ? (item.ModelItem == nci.ModelItem) : false);

                    if (item.IsSelected == true && selItem == null)
                    {
                        selItem = item;
                        SelectedItem = item;
                    }
                }
            }

            return (selItem);
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
                // Loop through each week

                for (int i = 0; i < _NumberOfWeeks; i++)
                {
                    DateTime date = _MonthWeeks[i].FirstDayOfWeek;

                    // loop through each day of the week, setting
                    // the selection status according to the date
                    // selection range

                    for (int j = 0; j < DaysInWeek; j++)
                    {
                        bool selected = (DateSelectionStart.HasValue && DateSelectionEnd.HasValue &&
                            date >= DateSelectionStart && date < DateSelectionEnd);

                        _MonthWeeks[i].DayRects[j].IsSelected = selected;

                        date = date.AddDays(1);
                    }
                }
            }
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

                // Calculate each weeks display info

                CalcMonthWeeks(startDate, endDate);

                // Update our Model connection and views

                UpdateView();
                UpdateDateSelection();

                if (DaysOfTheWeek != null)
                    DaysOfTheWeek.NeedsMeasured = true;

                NeedRecalcLayout = false;
            }
        }

        #region NormalizeDates

        /// <summary>
        /// Normalizes the user specified start and end dates
        /// </summary>
        /// <param name="startDate">[out] Normalized start date</param>
        /// <param name="endDate">[out] Normalized end date</param>
        private void NormalizeDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = this.StartDate;
            endDate = this.EndDate;

            // If both values are unset, then set them to
            // today's date / + 1 month

            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                startDate = DateTime.Today.Date;
                endDate = startDate.AddMonths(1);
            }

            endDate = DateHelper.GetEndOfWeek(endDate, DateHelper.GetLastDayOfWeek());
            startDate = DateHelper.GetDateForDayOfWeek(startDate, DateHelper.GetFirstDayOfWeek());

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
                    Connector = new ModelMonthViewConnector(CalendarModel, this);
            }

            // We have a connection, so update our CalendarItems

            if (Connector != null)
            {
                UpdateCalendarItems();

                CalendarView.DoViewLoadComplete(this);
            }
        }

        #region UpdateCalendarItems

        /// <summary>
        /// Updates our CalendarItems list
        /// </summary>
        private void UpdateCalendarItems()
        {
            // Get a working list of our CalendarItems

            List<CalendarItem>[] items = SortCalendarItems();

            // Go through our CalendarItems on a per week basis

            bool moreItems = false;

            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                // Alloc space to track time slot usage

                int[] acc = new int[DaysInWeek];

                // Calculate the display bounds for each item

                for (int j = 0; j < items[i].Count; j++)
                    CalcAppointmentBounds(items[i][j], i, ref acc);

                moreItems |= UpdateMoreItems(_MonthWeeks[i], acc);
            }

            // If we have no need of our previously allocated "More" 
            // images then dispose of them and reset our handles

            if (moreItems == false)
            {
                if (_MoreImage != null)
                {
                    _MoreImage.Dispose();
                    _MoreImage = null;
                }

                if (_MoreImageHot != null)
                {
                    _MoreImageHot.Dispose();
                    _MoreImageHot = null;
                }
            }

            // Update our current day selection

            UpdateDateSelection();
        }

        #region SortCalendarItems

        /// <summary>
        /// Sorts the provided CalendarItems
        /// </summary>
        /// <returns>Sorted CalendarItems</returns>
        private List<CalendarItem>[] SortCalendarItems()
        {
            List<CalendarItem>[] items = new List<CalendarItem>[_NumberOfWeeks];

            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                items[i] = new List<CalendarItem>(_MonthWeeks[i].CalendarItems.Count);

                if (_MonthWeeks[i].CalendarItems.Count > 0)
                {
                    items[i].AddRange(_MonthWeeks[i].CalendarItems);

                    items[i].Sort(

                       delegate(CalendarItem c1, CalendarItem c2)
                       {
                           if (c1.StartTime > c2.StartTime)
                               return (1);

                           if (c1.StartTime < c2.StartTime)
                               return (-1);

                           return (0);
                       }
                   );
                }
            }

            return (items);
        }

        #endregion

        #region CalcAppointmentBounds

        /// <summary>
        /// Calculates the display bounds for the AppointmentView
        /// </summary>
        /// <param name="item">CalendarItem</param>
        /// <param name="week">Week associated with the view</param>
        /// <param name="acc">Row accumulator</param>
        private void CalcAppointmentBounds(
            CalendarItem item, int week, ref int[] acc)
        {
            // Determine the starting day index for
            // the given appointment

            int ns = GetDayIndex(week, item);

            // Calculate the top and height for the item

            Rectangle r = _MonthWeeks[week].DayRects[ns].Bounds;

            r.Y += DayHeaderHeight + 1;
            r.Height = AppointmentHeight;

            // Check to see if the appointment spans
            // multiple days

            if (item.StartTime.Day != item.EndTime.Day ||
                (item.EndTime - item.StartTime).TotalDays > 1)
            {
                // Determine the ending day index

                DateTime st = _MonthWeeks[week].FirstDayOfWeek.AddDays(ns);

                int ne = ns + (item.EndTime - st).Days;

                if (item.EndTime.Hour > 0 || item.EndTime.Minute > 0 || item.EndTime.Second > 0)
                    ne++;

                if (ne > DaysInWeek)
                    ne = DaysInWeek;

                // Loop through each covered day, accumulating
                // the day width and max key slot height

                int maxAcc = acc[ns];

                for (int i = ns + 1; i < ne; i++)
                {
                    r.Width += _MonthWeeks[week].DayRects[i].Bounds.Width;

                    if (acc[i] > maxAcc)
                        maxAcc = acc[i];
                }

                // Set the top of this item to the calculated
                // slot height

                r.Y += maxAcc;

                // Loop through each effected slot and adjust it's
                // accumulated values accordingly

                maxAcc += AppointmentHeight;

                for (int i = ns; i < ne; i++)
                    acc[i] = maxAcc;
            }
            else
            {
                // This is a single day appointment, so set and
                // adjust it's values accordingly

                r.Y += acc[ns];
                acc[ns] += AppointmentHeight;
            }

            // Now that we have calculated the items height and
            // width, invoke a Recalc on the item

            item.WidthInternal = r.Width - 1;
            item.HeightInternal = r.Height - 1;

            item.RecalcSize();

            // Set our bounds for the item

            r.Width = item.WidthInternal;
            r.Height = item.HeightInternal;

            item.Bounds = r;

            // Set it's display state

            if (CalendarView.IsMonthMoreItemsIndicatorVisible == true)
                item.Displayed = (r.Bottom < _MonthWeeks[week].Bounds.Bottom - _MoreImageSize.Height);
            else
                item.Displayed = (r.Top < _MonthWeeks[week].Bounds.Bottom - 2);
        }

        #region GetDayIndex

        /// <summary>
        /// Gets the starting day index for the given appointment
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="item">CalendarItem</param>
        /// <returns>Day of week index (0-6)</returns>
        private int GetDayIndex(int week, CalendarItem item)
        {
            DateTime date = _MonthWeeks[week].FirstDayOfWeek;

            for (int i = 0; i < DaysInWeek; i++)
            {
                date = date.AddDays(1);

                if (date > item.StartTime)
                    return (i);
            }

            return (0);
        }

        #endregion

        #endregion

        #region UpdateMoreItems

        /// <summary>
        /// UpdateMoreItems
        /// </summary>
        /// <param name="monthWeek"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        private bool UpdateMoreItems(MonthWeek monthWeek, int[] acc)
        {
            bool moreItems = false;

            int rowHeight = monthWeek.DayRects[0].Bounds.Height -
                DayHeaderHeight - _MoreImageSize.Height - 1;

            for (int j = 0; j < DaysInWeek; j++)
            {
                if (CalendarView.IsMonthMoreItemsIndicatorVisible == false || acc[j] <= rowHeight)
                {
                    monthWeek.MoreItems[j].Bounds = Rectangle.Empty;
                }
                else
                {
                    moreItems = true;

                    Rectangle r = monthWeek.DayRects[j].Bounds;

                    r.X = r.Right - _MoreImageSize.Width;
                    r.Y = r.Bottom - _MoreImageSize.Height;
                    r.Size = _MoreImageSize;

                    monthWeek.MoreItems[j].Bounds = r;
                }
            }

            return (moreItems);
        }

        #endregion

        #endregion

        #region CalcMonthWeeks

        /// <summary>
        /// Calculates display info for the MonthWeek data
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        private void CalcMonthWeeks(DateTime startDate, DateTime endDate)
        {
            // Get the number of weeks for the date range

            _NumberOfWeeks = DateHelper.GetNumberOfWeeks(startDate, endDate);

            _NumberOfWeeks = Math.Max(1, _NumberOfWeeks);
            _NumberOfWeeks = Math.Min(_NumberOfWeeks, MaxNumberOfWeeks);

            // Allocate our MonthWeeks array to
            // hold our weeks info

            if (_MonthWeeks == null || _MonthWeeks.Length != _NumberOfWeeks)
                _MonthWeeks = new MonthWeek[_NumberOfWeeks];

            // Update display info on each week

            float dy = 0;

            float weekHeight =
                ((float)ClientRect.Height - DayOfWeekHeaderHeight) / _NumberOfWeeks;

            int sx = ClientRect.X + SideBarWidth;
            int sy = ClientRect.Y + DayOfWeekHeaderHeight;
            int y2 = sy;

            // Loop through each week

            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                if (_MonthWeeks[i] == null)
                    _MonthWeeks[i] = new MonthWeek(this);

                // Set the weeks first day of the week

                _MonthWeeks[i].FirstDayOfWeek = startDate.AddDays(i * DaysInWeek);

                // Calculate the bounding rect limits

                dy += weekHeight;

                int y1 = y2;
                y2 = sy + (int)dy;

                if (i + 1 == _NumberOfWeeks)
                    y2 = ClientRect.Y + ClientRect.Height - 1;

                // Set the bounding rect

                _MonthWeeks[i].Bounds =
                    new Rectangle(sx, y1, ClientRect.Width - SideBarWidth - 1, y2 - y1);
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
            // Set our current color table

            _ViewColor.SetColorTable();

            // Only draw something if we have something to draw

            if (_NumberOfWeeks > 0)
            {
                // Calculate our drawing ranges

                int dayStart, dayEnd, dayCount;
                int weekStart, weekEnd, weekCount;

                if (e.ClipRectangle.Contains(ClientRect))
                {
                    // The clipping rect encompasses our entire calendar
                    // so set our start and end values accordingly

                    dayStart = 0; dayEnd = DaysInWeek - 1;
                    dayCount = DaysInWeek;

                    weekStart = 0; weekEnd = _NumberOfWeeks - 1;
                    weekCount = _NumberOfWeeks;
                }
                else
                {
                    // We only need to draw part of our calendar, so
                    // calculate our day and week count and range

                    dayCount = GetDayRange(e, out dayStart, out dayEnd);
                    weekCount = GetWeekRange(e, out weekStart, out weekEnd);
                }

                // Draw our calendar parts

                if (dayCount > 0)
                {
                    DrawDayOfTheWeekHeader(e, dayStart, dayEnd);

                    if (weekCount > 0)
                    {
                        DrawDays(e, weekStart, weekEnd, dayStart, dayEnd);

                        if (Connector != null && Connector.IsConnected)
                            DrawWeekAppointments(e, weekStart, weekEnd);
                    }
                }

                if (_IsSideBarVisible == true && weekCount > 0)
                    DrawSideBarHeader(e, weekStart, weekEnd);
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

            while (start < DaysInWeek)
            {
                Rectangle dr1 = _MonthWeeks[0].DayRects[start].Bounds;

                if (dr1.Right > e.ClipRectangle.X)
                    break;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < DaysInWeek)
            {
                Rectangle dr2 = _MonthWeeks[0].DayRects[end].Bounds;

                if (dr2.X >= e.ClipRectangle.Right)
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
        /// Calculates the range of weeks needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="weekStart">[out] Week start index</param>
        /// <param name="weekEnd">[out] Week end index</param>
        /// <returns>Week range count (end - start)</returns>
        private int GetWeekRange(ItemPaintArgs e, out int weekStart, out int weekEnd)
        {
            // Calc our starting index

            int start = 0;

            while (start < _NumberOfWeeks)
            {
                if (_MonthWeeks[start].Bounds.Bottom > e.ClipRectangle.Y)
                    break;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < _NumberOfWeeks)
            {
                if (_MonthWeeks[end].Bounds.Y >= e.ClipRectangle.Bottom)
                    break;

                end++;
            }

            // Set the user supplied 'out' values, and
            // return the range count to the caller

            if (end - start == 0)
            {
                weekStart = 0;
                weekEnd = 0;

                return (0);
            }

            weekStart = start;
            weekEnd = end - 1;

            return (end - start);
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

            Rectangle r = new Rectangle(ClientRect.X - 1, ClientRect.Y - 1,
                SideBarWidth + 1, DayOfWeekHeaderHeight + 1);

            using (Brush br =
                _ViewColor.BrushPart((int)eCalendarMonthPart.OwnerTabContentBackground, r))
            {
                g.FillRectangle(br, r);
            }

            // Calculate our contiguous header rect
            // area so we can fill it all at once

            r = new Rectangle(
                _MonthWeeks[0].DayRects[dayStart].Bounds.X, ClientRect.Y,
                _MonthWeeks[0].DayRects[dayEnd].Bounds.Right -
                _MonthWeeks[0].DayRects[dayStart].Bounds.X, DayOfWeekHeaderHeight);

            if (r.Width > 0 && r.Height > 0)
            {
                using (Brush lbr =
                    _ViewColor.BrushPart((int)eCalendarMonthPart.DayOfTheWeekHeaderBackground, r))
                {
                    g.FillRectangle(lbr, r);
                }

                // Establish our Days Of The Week text type

                DaysOfTheWeek.eDayType type = GetDaysOfTheWeekType(g);

                // Loop through each day, drawing the
                // day of the week text in the header area

                eTextFormat tf = eTextFormat.VerticalCenter | eTextFormat.NoPadding;

                using (Pen pen = new Pen(
                    _ViewColor.GetColor((int)eCalendarMonthPart.DayOfTheWeekHeaderBorder)))
                {
                    for (int i = dayStart; i <= dayEnd; i++)
                    {
                        r.X = _MonthWeeks[0].DayRects[i].Bounds.X;
                        r.Width = _MonthWeeks[0].DayRects[i].Bounds.Width;

                        // Center the header text until the text
                        // will no longer fit, then left justify it

                        if (DaysOfTheWeek.DaySize[(int)type][i].Width < r.Width)
                            tf |= eTextFormat.HorizontalCenter;

                        // Draw the text and header border

                        TextDrawing.DrawString(g, DaysOfTheWeek.DayText[(int)type][i], Font,
                            _ViewColor.GetColor((int)eCalendarMonthPart.DayOfTheWeekHeaderForeground), r, tf);

                        g.DrawRectangle(pen, r);
                    }
                }
            }
        }

        #endregion

        #region GetDaysOfTheWeekType

        private DaysOfTheWeek.eDayType GetDaysOfTheWeekType(Graphics g)
        {
            // Determine if the current DayRect bounds
            // are constrained by the text threshold

            DaysOfTheWeek.MeasureText(g, Font);

            for (int i = 0; i < DaysInWeek; i++)
            {
                if (_MonthWeeks[0].DayRects[i].Bounds.Width <
                    DaysOfTheWeek.DaySize[(int)DaysOfTheWeek.eDayType.Long][i].Width)
                {
                    return (DaysOfTheWeek.eDayType.Short);
                }
            }

            return (DaysOfTheWeek.eDayType.Long);
        }

        #endregion

        #endregion

        #region Day drawing

        #region DrawDays

        /// <summary>
        /// Draws day header and content
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="weekStart">Week start index</param>
        /// <param name="weekEnd">Week end index</param>
        /// <param name="dayStart">Day start index</param>
        /// <param name="dayEnd">Day end index</param>
        private void DrawDays(ItemPaintArgs e,
            int weekStart, int weekEnd, int dayStart, int dayEnd)
        {
            Graphics g = e.Graphics;

            Rectangle r = new Rectangle(
                _MonthWeeks[0].DayRects[dayStart].Bounds.X, 0,
                _MonthWeeks[0].DayRects[dayEnd].Bounds.Right -
                _MonthWeeks[0].DayRects[dayStart].Bounds.X, DayHeaderHeight);

            if (r.Width > 0 && r.Height > 0)
            {
                // Loop through each day in each week, displaying
                // the associated day content and header

                int nowHeaderWeek = -1;
                int nowHeaderDay = -1;

                DateTime now = DateTime.Now;

                using (Pen pen1 = new Pen(
                    _ViewColor.GetColor((int) eCalendarMonthPart.DayContentBorder)))
                {
                    using (Pen pen2 = new Pen(
                        _ViewColor.GetColor((int) eCalendarMonthPart.DayHeaderBorder)))
                    {
                        for (int i = weekStart; i <= weekEnd; i++)
                        {
                            for (int j = dayStart; j <= dayEnd; j++)
                            {
                                if (CalendarView.HighlightCurrentDay == true)
                                {
                                    DateTime date = _MonthWeeks[i].FirstDayOfWeek.AddDays(j);

                                    if (now.Date.Equals(date.Date) == true)
                                    {
                                        nowHeaderDay = i;
                                        nowHeaderWeek = j;

                                        continue;
                                    }
                                }

                                DrawDayBackground(g, i, j, eCalendarMonthPart.DayHeaderBackground);

                                DrawDayContent(g, i, j, pen1);
                                DrawDayHeader(g, i, j, pen2);
                            }
                        }
                    }
                }

                // Draw "Now" day header and border

                if (nowHeaderDay >= 0)
                {
                    using (Pen pen1 = new Pen(
                        _ViewColor.GetColor((int) eCalendarMonthPart.NowDayHeaderBorder)))
                    {
                        using (Pen pen2 = new Pen(
                            _ViewColor.GetColor((int) eCalendarMonthPart.NowDayHeaderBorder)))
                        {
                            DrawDayBackground(g, nowHeaderDay, nowHeaderWeek, eCalendarMonthPart.NowDayHeaderBackground);

                            DrawDayContent(g, nowHeaderDay, nowHeaderWeek, pen1);
                            DrawDayHeader(g, nowHeaderDay, nowHeaderWeek, pen2);
                        }
                    }
                }
            }
        }

        #region DrawDayBackground

        /// <summary>
        /// DrawDayBackground
        /// </summary>
        /// <param name="g"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="part"></param>
        private void DrawDayBackground(Graphics g,
            int week, int day, eCalendarMonthPart part)
        {
            Rectangle r = _MonthWeeks[week].DayRects[day].Bounds;
            r.Height = DayHeaderHeight;

            using (Brush br = _ViewColor.BrushPart((int)part, r))
                g.FillRectangle(br, r);
        }

        #endregion

        #endregion

        #region DrawDayContent

        /// <summary>
        /// Draws the day content
        /// </summary>
        /// <param name="g"></param>
        /// <param name="iweek">Week index</param>
        /// <param name="iday">Day index</param>
        /// <param name="pen">Pen</param>
        private void DrawDayContent(Graphics g, int iweek, int iday, Pen pen)
        {
            Rectangle r = _MonthWeeks[iweek].DayRects[iday].Bounds;

            r.Y += DayHeaderHeight;
            r.Height -= DayHeaderHeight;

            DrawBackground(g, iweek, iday, r);
            DrawMoreImage(g, iweek, iday, r);
            DrawBorder(g, pen, iweek, r);
        }

        #region DrawBackground

        private void DrawBackground(Graphics g,
            int iweek, int iday, Rectangle r)
        {
            DateTime date = _MonthWeeks[iweek].FirstDayOfWeek.AddDays(iday);
            int dayMonth = date.Month;

            DateTime startTime = date;
            DateTime endTime = date.AddDays(1).AddMinutes(-1);

            eSlotDisplayState state = GetSlotState(iweek, iday, dayMonth);

            if (CalendarView.DoMonthViewPreRenderSlotBackground(
                g, this, startTime, endTime, r, ref state) == false)
            {
                eCalendarMonthPart part = GetContentPart(state);

                using (Brush br = _ViewColor.BrushPart((int)part, r))
                    g.FillRectangle(br, r);

                CalendarView.DoMonthViewPostRenderSlotBackground(
                    g, this, startTime, endTime, r, state);
            }
        }

        #region GetSlotState

        /// <summary>
        /// GetSlotState
        /// </summary>
        /// <returns></returns>
        private eSlotDisplayState
            GetSlotState(int iweek, int iday, int dayMonth)
        {
            eSlotDisplayState state = eSlotDisplayState.None;

            if (DisplayedOwnerKeyIndex == CalendarView.SelectedOwnerIndex)
            {
                if (_MonthWeeks[iweek].DayRects[iday].IsSelected)
                    state |= eSlotDisplayState.Selected;
            }

            if (dayMonth == StartDate.Month)
                state |= eSlotDisplayState.Work;

            return (state);
        }

        #endregion

        #region GetContentPart

        /// <summary>
        /// Gets the content calendar part for the given
        /// week and dayMonth
        /// </summary>
        /// <returns></returns>
        private eCalendarMonthPart GetContentPart(eSlotDisplayState state)
        {
            if ((state & eSlotDisplayState.Selected) == eSlotDisplayState.Selected)
                return (eCalendarMonthPart.DayContentSelectionBackground);

            if ((state & eSlotDisplayState.Work) == eSlotDisplayState.Work)
                return (eCalendarMonthPart.DayContentActiveBackground);

            return (eCalendarMonthPart.DayContactInactiveBackground);
        }

        #endregion

        #endregion

        #region DrawMoreImage

        private void DrawMoreImage(Graphics g, int iweek, int iday, Rectangle r)
        {
            ItemRect moreRect = _MonthWeeks[iweek].MoreItems[iday];

            if (moreRect.Bounds.IsEmpty == false)
            {
                Bitmap bm = moreRect.IsSelected ? GetMoreImageHot(g) : GetMoreImage(g);

                if (bm != null)
                {
                    Region regSave = g.Clip;

                    g.SetClip(r, CombineMode.Intersect);
                    g.DrawImageUnscaledAndClipped(bm, moreRect.Bounds);
                    g.Clip = regSave;
                }
            }
        }

        #endregion

        #region DrawBorder

        private void DrawBorder(
            Graphics g, Pen pen, int iweek, Rectangle r)
        {
            if (iweek < _NumberOfWeeks - 1)
                r.Height--;

            g.DrawRectangle(pen, r);
        }

        #endregion

        #endregion

        #region DrawDayHeader

        /// <summary>
        /// Draws day header
        /// </summary>
        /// <param name="g"></param>
        /// <param name="iweek">Week index</param>
        /// <param name="iday">Day index</param>
        /// <param name="pen">Text pen</param>
        private void DrawDayHeader(Graphics g, int iweek, int iday, Pen pen)
        {
            // Calculate the display rect and draw the text

            Rectangle r = _MonthWeeks[iweek].DayRects[iday].Bounds;
            r.Height = Math.Min(DayHeaderHeight, r.Height);

            if (r.Height > 0)
            {
                // Draw the border area

                g.DrawRectangle(pen, r);

                // Get the associated day and text

                DateTime day = _MonthWeeks[iweek].FirstDayOfWeek.AddDays(iday);

                string s = GetDayHeaderText(day);

                r.Inflate(-2, 0);

                TextDrawing.DrawString(g, s, BoldFont,
                    _ViewColor.GetColor((int)eCalendarMonthPart.DayHeaderForeground), r,
                    eTextFormat.VerticalCenter | eTextFormat.Left | eTextFormat.NoPadding);
            }
        }

        #region GetDayHeaderText

        /// <summary>
        /// GetDayHeaderText
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDayHeaderText(DateTime date)
        {
            if (date.Day == 1)
            {
                DateTimeFormatInfo dtfi =
                    ScheduleSettings.GetActiveCulture().DateTimeFormat;

                string s = dtfi.MonthDayPattern.Replace("dd", "d");

                return (date.ToString(s));
            }

            return (String.Format("{0}", date.Day));
        }

        #endregion

        #endregion

        #endregion

        #region DrawWeekAppointments

        /// <summary>
        /// Initiates the drawing of weekly appointments
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="weekStart">Week start index</param>
        /// <param name="weekEnd">Week end index</param>
        private void DrawWeekAppointments(ItemPaintArgs e, int weekStart, int weekEnd)
        {
            Graphics g = e.Graphics;

            // Loop through each subItem - on a weekly basis - so that
            // we can set an appropriate clipping region for the week

            for (int i = weekStart; i <= weekEnd; i++)
            {
                List<CalendarItem> items = _MonthWeeks[i].CalendarItems;

                if (items.Count > 0)
                {
                    // Set our week-based clipping region

                    Rectangle r = _MonthWeeks[i].Bounds;
                    r.Height -= 1;

                    Region regSave = g.Clip;

                    g.SetClip(r, CombineMode.Intersect);

                    // Loop through each CalendarItem in the week

                    for (int j = 0; j < items.Count; j++)
                    {
                        // If we can display the item, then initiate the paint

                        if (items[j].Displayed == true)
                        {
                            if (e.ClipRectangle.IsEmpty ||
                                e.ClipRectangle.IntersectsWith(items[j].DisplayRectangle))
                            {
                                items[j].Paint(e);
                            }
                        }
                    }

                    // Restore the original clip region

                    g.Clip = regSave;

                    regSave.Dispose();
                }
            }
        }

        #endregion

        #region GetMoreImage

        private Bitmap GetMoreImage(Graphics g)
        {
            if (_MoreImage == null)
                _MoreImage = GetMoreBitmap(g, Color.LightGray);

            return (_MoreImage);
        }

        #endregion

        #region GetMoreImageHot

        private Bitmap GetMoreImageHot(Graphics g)
        {
            if (_MoreImageHot == null)
                _MoreImageHot = GetMoreBitmap(g, _ViewColor.GetColor((int)eCalendarMonthPart.NowDayHeaderBorder));

            return (_MoreImageHot);
        }

        #endregion

        #region GetMoreBitmap

        private Bitmap GetMoreBitmap(Graphics g, Color color)
        {
            Bitmap bmp = new Bitmap(_MoreImageSize.Width, _MoreImageSize.Height, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = GetMoreImagePath())
                {
                    using (Brush br = new SolidBrush(color))
                        gBmp.FillPath(br, path);

                    gBmp.DrawPath(Pens.White, path);               
                }
            }

            return (bmp);
        }

        #endregion

        #region GetMoreImagePath

        /// <summary>
        /// Gets the More image path
        /// </summary>
        /// <returns></returns>
        private GraphicsPath GetMoreImagePath()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[] 
            {
                new Point(2, 3), 
                new Point(7, 11), 
                new Point(12, 3) 
            });

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region SideBar header drawing

        /// <summary>
        /// Draws the SideBar header
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="weekStart">Week start index</param>
        /// <param name="weekEnd">Week end index</param>
        private void DrawSideBarHeader(ItemPaintArgs e, int weekStart, int weekEnd)
        {
            // Calc the contiguous SideBar background rect

            Rectangle r = new Rectangle(ClientRect.X, _MonthWeeks[weekStart].Bounds.Y,
                SideBarWidth, _MonthWeeks[weekEnd].Bounds.Bottom - _MonthWeeks[weekStart].Bounds.Y);

            // Make sure we have something to draw

            if (r.Height > 0 && r.Width > 0 && e.ClipRectangle.IntersectsWith(r))
            {
                Graphics g = e.Graphics;

                // Fill the bar with our color gradient

                using (Brush lbr = _ViewColor.BrushPart((int)eCalendarMonthPart.SideBarBackground, r))
                {
                    g.FillRectangle(lbr, r);
                }

                // Set uo our output to have both
                // vertically and horizontally centered text

                using (StringFormat sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    // Calc our rect origin and prepare to
                    // output the SideBar text

                    PointF ptf = new PointF(ClientRect.X, 0);
                    Rectangle rf = new Rectangle(0, 0, 0, SideBarWidth);

                    using (Brush br =
                        _ViewColor.BrushPart((int)eCalendarMonthPart.SideBarForeground, rf))
                    {
                        using (Pen pen = new Pen(
                            _ViewColor.GetColor((int)eCalendarMonthPart.SideBarBorder)))
                        {
                            // Loop through each individual week, displaying
                            // the rotated header text appropriately

                            for (int i = weekStart; i <= weekEnd; i++)
                            {
                                rf.Width = _MonthWeeks[i].Bounds.Height;

                                if (rf.Width > 0)
                                {
                                    ptf.Y = _MonthWeeks[i].Bounds.Y + rf.Width;

                                    g.TranslateTransform(ptf.X, ptf.Y);
                                    g.RotateTransform(-90);

                                    g.DrawString(_MonthWeeks[i].WeekRange, Font, br, rf, sf);
                                    g.DrawRectangle(pen, rf.X, rf.Y, rf.Width, rf.Height);

                                    g.ResetTransform();
                                }
                            }
                        }
                    }
                }
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

                    int week, day;

                    if (GetPointItem(objArg.Location, out week, out day))
                    {
                        MyCursor = GetCursor();

                        if (ProcessCilButtonDown(week, day) == false)
                        {
                            if (ProcessMoreButtonDown(week, day) == false)
                                ProcessMvlButtonDown(week, day);
                        }
                    }

                    IsCopyDrag = false;
                }
            }
        }

        #region CalendarItem MouseDown processing

        /// <summary>
        /// CalendarItem left mouseDown processing
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="day">Day index</param>
        private bool ProcessCilButtonDown(int week, int day)
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
                        {
                            TimeSpan ts = _MonthWeeks[week].FirstDayOfWeek.AddDays(day) - item.StartTime;

                            _DaysOffset = (int)(Math.Ceiling(ts.TotalDays));

                            IsMoving = true;
                        }
                    }
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #region MonthView MouseDown processing

        /// <summary>
        /// Handles MonthView left MouseDown events
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="day">Day index</param>
        private void ProcessMvlButtonDown(int week, int day)
        {
            DateTime startDate = _MonthWeeks[week].FirstDayOfWeek.AddDays(day);
            DateTime endDate = startDate.AddDays(1);

            ExtendSelection(ref startDate, ref endDate);

            CalendarView.DateSelectionStart = startDate;
            CalendarView.DateSelectionEnd = endDate;

            SelectedItem = null;
        }

        #endregion

        #region MoreItems MouseDown processing

        /// <summary>
        /// ProcessMoreButtonDown
        /// </summary>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private bool ProcessMoreButtonDown(int week, int day)
        {
            ItemRect moreRect = _MonthWeeks[week].MoreItems[day];

            if (moreRect.IsSelected == true)
            {
                moreRect.IsSelected = false;

                DateTime startDate = _MonthWeeks[week].FirstDayOfWeek.AddDays(day);

                DateSelectionAnchor = null;
                CalendarView.DateSelectionStart = null;
                CalendarView.DateSelectionEnd = null;

                CalendarView.SelectedView = eCalendarView.Day;
                CalendarView.EnsureVisible(startDate, startDate);

                return (true);
            }

            return (false);
        }

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

            if (!IsMoving && !IsStartResizing && !IsEndResizing)
                base.InternalMouseMove(objArg);

            if (IsTabMoving == false)
            {
                // Set the cursor

                MyCursor = GetCursor();

                int week, day;

                bool inItem = GetPointItem(objArg.Location, out week, out day);

                switch (objArg.Button)
                {
                    case MouseButtons.Left:
                        if (DragDropAppointment(objArg) == false)
                        {
                            // Locate where the event took place
                            // and process it accordingly

                            if (inItem == true)
                            {
                                if (DateSelectionAnchor != null)
                                {
                                    ProcessMvMouseMove(week, day);
                                }
                                else if (SelectedItem != null)
                                {
                                    if (IsMoving == true)
                                        ProcessItemMove(week, day);

                                    else if (IsStartResizing == true)
                                        ProcessItemLeftResize(week, day);

                                    else if (IsEndResizing == true)
                                        ProcessItemRightResize(week, day);
                                }
                            }
                        }
                        break;

                    case MouseButtons.None:
                        if (inItem == true)
                            ProcessMoreMouseMove(week, day, objArg);
                        break;
                }
            }
        }

        #region GetCursor

        /// <summary>
        /// Gets the cursor
        /// </summary>
        /// <returns>Cursor</returns>
        private Cursor GetCursor()
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
                MonthView mv = bv as MonthView;

                if (mv != null && mv != this)
                {
                    eViewArea area = mv.GetViewAreaFromPoint(pt);

                    if (area == eViewArea.InContent)
                    {
                        DragCopy();
                        ClearMouseStates();

                        AppointmentView av = SelectedItem as AppointmentView;

                        if (av != null)
                            return (mv.DragAppointment(this, av, pt));

                        CustomCalendarItem ci = SelectedItem as CustomCalendarItem;

                        if (ci != null)
                            return (mv.DragCustomItem(this, ci, pt));
                    }
                }
            }

            return (false);
        }

        #region DragCustomItem

        private bool DragCustomItem(MonthView pv, CustomCalendarItem ci, Point pt)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            ci.OwnerKey = OwnerKey;

            DateTime sd = ci.StartTime;
            TimeSpan ts = ci.EndTime.Subtract(ci.StartTime);

            int week = GetPointWeek(pt);
            int day = GetPointDay(pt, week);

            MonthWeek mw = _MonthWeeks[week];
            DateTime date = mw.FirstDayOfWeek.AddDays(day - pv._DaysOffset);

            ci.StartTime = new
                DateTime(date.Year, date.Month, date.Day, sd.Hour, sd.Minute, 0);

            ci.EndTime = ci.StartTime.Add(ts);

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
        /// <param name="av">AppointmentView</param>
        /// <param name="pt">Point</param>
        private bool DragAppointment(MonthView pv, AppointmentView av, Point pt)
        {
            // Set the new owner and selected view, and
            // recalc the new layout

            av.Appointment.OwnerKey = OwnerKey;

            DateTime sd = av.Appointment.StartTime;

            int week = GetPointWeek(pt);
            int day = GetPointDay(pt, week);

            MonthWeek mw = _MonthWeeks[week];

            DateTime date = mw.FirstDayOfWeek.AddDays(day - pv._DaysOffset);

            av.Appointment.MoveTo(
                new DateTime(date.Year, date.Month, date.Day, sd.Hour, sd.Minute, 0));

            NeedRecalcLayout = true;
            RecalcSize();

            CalendarView.SelectedOwnerIndex = this.DisplayedOwnerKeyIndex;

            // Get the new appointment view

            AppointmentView view = GetAppointmentView(av.Appointment);

            if (view != null)
                SetNewDragItem(pv, view);

            return (true);
        }

        #endregion

        #region SetNewDragItem

        private void SetNewDragItem(MonthView pv, CalendarItem view)
        {
            CalendarView.CalendarPanel.InternalMouseMove(new
                MouseEventArgs(MouseButtons.None, 0, view.Bounds.X + 5, view.Bounds.Y, 0));

            MouseEventArgs args = new
                MouseEventArgs(MouseButtons.Left, 1, view.Bounds.X + 5, view.Bounds.Y, 0);

            IsMoving = true;

            InternalMouseMove(args);
            CalendarView.CalendarPanel.InternalMouseDown(args);

            _DaysOffset = pv._DaysOffset;

            SelectedItem = view;

            IsCopyDrag = true;
        }

        #endregion

        #endregion

        #region MonthView mouseMove processing

        /// <summary>
        /// Processes MonthView mouseMove events
        /// </summary>
        /// <param name="week"></param>
        /// <param name="day"></param>
        private void ProcessMvMouseMove(int week, int day)
        {
            DateTime date = _MonthWeeks[week].FirstDayOfWeek.AddDays(day);

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

        #endregion

        #region MoreItems MouseMove processing

        /// <summary>
        /// ProcessMoreMouseMove
        /// </summary>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="objArg"></param>
        private void ProcessMoreMouseMove(int week, int day, MouseEventArgs objArg)
        {
            ItemRect moreRect = _MonthWeeks[week].MoreItems[day];

            if (moreRect.Bounds.Contains(objArg.Location) == true)
            {
                if (_MoreRect != moreRect)
                {
                    if (_MoreRect != null)
                        _MoreRect.IsSelected = false;

                    _MoreRect = moreRect;
                    _MoreRect.IsSelected = true;
                }
            }
            else
            {
                if (_MoreRect != null)
                {
                    _MoreRect.IsSelected = false;
                    _MoreRect = null;
                }
            }
        }

        #endregion

        #region CalendarItem mouseMove processing

        /// <summary>
        /// Processes CalendarItem mouseMove events
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="day">Day index</param>
        private void ProcessItemMove(int week, int day)
        {
            day -= _DaysOffset;

            DateTime date =
                _MonthWeeks[week].FirstDayOfWeek.AddDays(day);

            DateTime newDate = new DateTime(date.Year, date.Month, date.Day,
                SelectedItem.StartTime.Hour, SelectedItem.StartTime.Minute,
                SelectedItem.StartTime.Second);

            TimeSpan ts = SelectedItem.EndTime - SelectedItem.StartTime;

            int startWeek1, endWeek1;
            int day1 = GetCalendarItemWeekRange(SelectedItem, out startWeek1, out endWeek1);

            if (startWeek1 != week || day1 != day)
            {
                if (DragCopy() == false)
                {
                    try
                    {
                        if (SelectedItem is CustomCalendarItem)
                            CalendarView.CustomItems.BeginUpdate();
                        else
                            CalendarModel.BeginUpdate();

                        // Let the user cancel the operation if desired

                        if (CalendarView.DoAppointmentViewChanging(SelectedItem, newDate,
                            newDate + ts, eViewOperation.AppointmentMove, IsCopyDrag) == false)
                        {
                            SelectedItem.StartTime = newDate;
                            SelectedItem.EndTime = newDate + ts;

                            int startWeek2, endWeek2;
                            GetCalendarItemWeekRange(SelectedItem, out startWeek2, out endWeek2);

                            startWeek1 = Math.Min(startWeek1, startWeek2);
                            endWeek1 = Math.Max(endWeek1, endWeek2);

                            for (int i = startWeek1; i <= endWeek1; i++)
                                InvalidateRect(_MonthWeeks[i].Bounds, true);
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

        /// <summary>
        /// Gets the CalendarItems current week/day range
        /// </summary>
        /// <param name="item">CalendarItem</param>
        /// <param name="startWeek">[out] Start week index</param>
        /// <param name="endWeek">[out] End week index</param>
        /// <returns>Day index</returns>
        private int GetCalendarItemWeekRange(
            CalendarItem item, out int startWeek, out int endWeek)
        {
            startWeek = -1;
            endWeek = -1;

            int day = 0;

            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                DateTime startTime = _MonthWeeks[i].FirstDayOfWeek;
                DateTime endTime = startTime.AddDays(DaysInWeek);

                if (item.StartTime < endTime && item.EndTime > startTime)
                {
                    if (startWeek < 0)
                    {
                        startWeek = i;
                        day = item.StartTime.Day - startTime.Day;
                    }

                    endWeek = i;
                }
            }

            if (startWeek == -1)
            {
                startWeek = 0;
                endWeek = _NumberOfWeeks - 1;

                day = 0;
            }

            return (day);
        }

        #endregion

        #region CalendarItem mouseResize processing

        /// <summary>
        /// Processes CalendarItem left resizing
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="day">Day index</param>
        private void ProcessItemLeftResize(int week, int day)
        {
            DateTime date =
                _MonthWeeks[week].FirstDayOfWeek.AddDays(day);

            DateTime newStartTime = new DateTime(date.Year, date.Month, date.Day,
                SelectedItem.StartTime.Hour, SelectedItem.StartTime.Minute,
                SelectedItem.StartTime.Second);

            if (newStartTime < SelectedItem.EndTime && SelectedItem.StartTime != newStartTime)
            {
                // Let the user cancel the operation if desired

                if (CalendarView.DoAppointmentViewChanging(SelectedItem, newStartTime,
                    SelectedItem.EndTime, eViewOperation.AppointmentResize, IsCopyDrag) == false)
                {
                    SelectedItem.StartTime = newStartTime;

                    InvalidateRect(true);
                }
            }
        }

        /// <summary>
        /// Processes CalendarItem right resizing
        /// </summary>
        /// <param name="week">Week index</param>
        /// <param name="day">Day index</param>
        private void ProcessItemRightResize(int week, int day)
        {
            DateTime date =
                _MonthWeeks[week].FirstDayOfWeek.AddDays(day);

            DateTime newEndTime = new DateTime(date.Year, date.Month, date.Day,
                SelectedItem.EndTime.Hour, SelectedItem.EndTime.Minute,
                SelectedItem.EndTime.Second);

            if (newEndTime.Hour == 0 && newEndTime.Minute == 0 && newEndTime.Second == 0)
                newEndTime = newEndTime.AddDays(1);

            if (newEndTime > SelectedItem.StartTime && SelectedItem.EndTime != newEndTime)
            {
                // Let the user cancel the operation if desired

                if (CalendarView.DoAppointmentViewChanging(SelectedItem, SelectedItem.StartTime,
                    newEndTime, eViewOperation.AppointmentResize, IsCopyDrag) == false)
                {
                    SelectedItem.EndTime = newEndTime;

                    InvalidateRect(true);
                }
            }
        }

        #endregion

        #endregion

        #region Mouse support routines

        /// <summary>
        /// Gets the week and day index item for
        /// the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="week">[out] Week index</param>
        /// <param name="day">[out] Day index</param>
        /// <returns>Success or failure</returns>
        private bool GetPointItem(Point pt, out int week, out int day)
        {
            day = -1;

            if ((week = GetPointWeek(pt)) >= 0)
            {
                if ((day = GetPointDay(pt, week)) >= 0)
                    return (true);
            }

            return (false);
        }

        /// <summary>
        /// Gets the week index for the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>Week index</returns>
        private int GetPointWeek(Point pt)
        {
            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                if (_MonthWeeks[i].Bounds.Contains(pt) == true)
                    return (i);
            }

            return (-1);
        }

        /// <summary>
        /// Gets the day index for the given point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="week">Week index</param>
        /// <returns>Day index</returns>
        private int GetPointDay(Point pt, int week)
        {
            ItemRects rs = _MonthWeeks[week].DayRects;

            for (int i = 0; i < DaysInWeek; i++)
            {
                if (rs[i].Bounds.Contains(pt) == true)
                    return (i);
            }

            return (-1);
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

                if (startDate.Equals(DateSelectionAnchor.Value) == true)
                    startDate = endDate.AddDays(-1);

                startDate = startDate.AddDays(dy);
                endDate = startDate.AddDays(1);

                int week = GetWeekFromDate(startDate);

                if (week >= 0)
                {
                    ExtendSelection(ref startDate, ref endDate);

                    CalendarView.DateSelectionStart = startDate;
                    CalendarView.DateSelectionEnd = endDate;
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

                if (startDate.Equals(DateSelectionAnchor.Value) == true)
                    startDate = endDate.AddDays(-1);

                startDate = startDate.AddDays(dx);
                endDate = startDate.AddDays(1);

                DateTime viewStart = _MonthWeeks[0].FirstDayOfWeek;
                DateTime viewEnd = _MonthWeeks[_MonthWeeks.Length - 1].FirstDayOfWeek.AddDays(7);

                if (startDate < viewStart || endDate > viewEnd)
                    CalendarView.EnsureVisible(startDate, startDate.AddDays(1));

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
            int week = ((objArg.Modifiers & Keys.Control) != Keys.Control)
                           ? GetHomeEndWeek()
                           : 0;

            if (week >= 0)
            {
                DateTime startDate = _MonthWeeks[week].FirstDayOfWeek;
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
            int week = ((objArg.Modifiers & Keys.Control) != Keys.Control)
                           ? GetHomeEndWeek()
                           : _NumberOfWeeks - 1;

            if (week >= 0)
            {
                DateTime startDate = _MonthWeeks[week].FirstDayOfWeek.AddDays(6);
                DateTime endDate = startDate.AddDays(1);

                ExtendSelection(ref startDate, ref endDate);

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                SelectedItem = null;
            }

            objArg.Handled = true;
        }

        #endregion

        #region GetWeekFromDate

        /// <summary>
        /// Gets the week containing the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Week or -1</returns>
        private int GetWeekFromDate(DateTime date)
        {
            for (int i = 0; i < _NumberOfWeeks; i++)
            {
                DateTime dc = _MonthWeeks[i].FirstDayOfWeek;

                if (dc <= date && dc.AddDays(7) > date)
                    return (i);
            }

            return (-1);
        }

        #endregion

        #region GetHomeEndWeek

        /// <summary>
        /// Gets the Home and End week from the
        /// current selection range
        /// </summary>
        /// <returns></returns>
        private int GetHomeEndWeek()
        {
            if (ValidDateSelection() == true)
            {
                if (CalendarView.DateSelectionStart.Equals(DateSelectionAnchor.Value) == true)
                    return (GetWeekFromDate(DateSelectionEnd.Value.AddDays(-1)));

                return (GetWeekFromDate(DateSelectionStart.Value));
            }

            return (-1);
        }

        #endregion

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
            MonthView objCopy = new MonthView(this.CalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the MonthView specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New MonthView instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            MonthView objCopy = copy as MonthView;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy._IsSideBarVisible = this._IsSideBarVisible;
            }
        }

        #endregion
    }

    #region EventArgs

    #region IsSideBarVisibleChangedEventArgs
    /// <summary>
    /// IsSideBarVisibleChangedEventArgs
    /// </summary>
    public class IsSideBarVisibleChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public IsSideBarVisibleChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #endregion
}
#endif

