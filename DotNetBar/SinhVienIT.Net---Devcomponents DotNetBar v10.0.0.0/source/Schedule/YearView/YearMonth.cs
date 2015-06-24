#if FRAMEWORK20
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class YearMonth
    {
        #region Private constants

        private const int MaxRows = 6;
        private const int MaxCols = 7;
        private const int MaxDays = (MaxRows * MaxCols);

        private const int CellPadding = 4;

        #endregion

        #region Private variables

        private YearView _YearView;

        private DateTime _RootDate;
        private DateTime _StartDate;
        private DateTime _EndDate;

        private Rectangle _Bounds;
        private Rectangle _ContentBounds;
        private Rectangle _MonthHeaderBounds;
        private Rectangle _DayOfWeekHeaderBounds;

        private int _DaysInMonth;
        private int _DaysOffset;

        private ItemRects _DayRects;

        private BitArray _AppBits;
        private bool _DaysSelected;

        private DaysOfTheWeek.eDayType _DayType = DaysOfTheWeek.eDayType.None;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="yearView"></param>
        /// <param name="date"></param>
        public YearMonth(YearView yearView, DateTime date)
        {
            _YearView = yearView;

            StartDate = date;

            _DayRects = new ItemRects(yearView, MaxDays);
            _AppBits = new BitArray(32);
        }

        #region Internal properties

        #region AppBits

        /// <summary>
        /// Appointment (and CustomItem) bit array
        /// </summary>
        internal BitArray AppBits
        {
            get { return (_AppBits); }
            set { _AppBits = value; }
        }

        #endregion

        #region DayOfWeekHeaderHeight

        /// <summary>
        /// DayOfWeekHeaderHeight
        /// </summary>
        internal int DayOfWeekHeaderHeight
        {
            get { return (_YearView.Font.Height + 4); }
        }

        #endregion

        #region DayRects

        /// <summary>
        /// Gets the day Rectangles
        /// </summary>
        internal ItemRects DayRects
        {
            get { return (_DayRects); }
        }

        #endregion

        #region MonthHeaderHeight

        /// <summary>
        /// MonthHeaderHeight
        /// </summary>
        internal int MonthHeaderHeight
        {
            get { return (_YearView.Font.Height + 4); }
        }

        #endregion

        #endregion

        #region Public properties

        #region Bounds

        /// <summary>
        /// Gets and sets the week bounding Rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }

            internal set
            {
                if (_Bounds.Equals(value) == false)
                {
                    _Bounds = value;
                    _DayType = DaysOfTheWeek.eDayType.None;

                    CalcBoundingRects();
                    CalcDayRects();
                }
            }
        }

        #endregion

        #region DaysInMonth

        /// <summary>
        /// Gets the number of Days in the Month
        /// </summary>
        public int DaysInMonth
        {
            get { return (_DaysInMonth); }
        }

        #endregion

        #region StartDate

        /// <summary>
        /// Gets or sets the month starting date
        /// </summary>
        public DateTime StartDate
        {
            get { return (_StartDate); }

            internal set
            {
                value = value.Date.AddDays(-(value.Day - 1));

                if (_StartDate.Equals(value) == false)
                {
                    _StartDate = value;

                    _DaysInMonth = DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                    _RootDate = DateHelper.GetDateForDayOfWeek(_StartDate, DateHelper.GetFirstDayOfWeek());
                    _EndDate = _StartDate.AddDays(_DaysInMonth).AddDays(-1);

                    _DaysOffset = (_StartDate.Date - _RootDate.Date).Days;
                }
            }
        }

        #endregion

        #region EndDate

        /// <summary>
        /// Gets the month end date
        /// </summary>
        public DateTime EndDate
        {
            get { return (_EndDate); }
        }

        #endregion

        #region YearView

        /// <summary>
        /// Gets the parent YearView
        /// </summary>
        public YearView YearView
        {
            get { return (_YearView); }
        }

        #endregion

        #endregion

        #region CalcBoundingRects

        /// <summary>
        /// Calculates the control's Bounding Rects
        /// </summary>
        private void CalcBoundingRects()
        {
            // Calculate main content bounding rects

            _ContentBounds = _Bounds;
            _ContentBounds.Y += (MonthHeaderHeight + DayOfWeekHeaderHeight);
            _ContentBounds.Height -= (MonthHeaderHeight + DayOfWeekHeaderHeight);

            _MonthHeaderBounds = _Bounds;
            _MonthHeaderBounds.Height = MonthHeaderHeight;

            _DayOfWeekHeaderBounds = _Bounds;
            _DayOfWeekHeaderBounds.Y += MonthHeaderHeight;
            _DayOfWeekHeaderBounds.Height = DayOfWeekHeaderHeight;
        }

        #endregion

        #region CalcDayRects

        /// <summary>
        /// Calculates the day rectangles for the
        /// current bounding rectangle
        /// </summary>
        private void CalcDayRects()
        {
            // Loop through each day in the week

            Size cellSize = _YearView.CellSize;
            Rectangle r = new Rectangle(_Bounds.Location, cellSize);

            r.X += CellPadding;
            r.Y += DayOfWeekHeaderHeight + MonthHeaderHeight + CellPadding;

            int bWidth = _Bounds.Width - (CellPadding * 2);
            int bHeight = _Bounds.Height - (DayOfWeekHeaderHeight + MonthHeaderHeight) - (CellPadding * 2);

            int dx = (bWidth - (cellSize.Width * MaxCols)) / MaxCols;
            int dy = (bHeight - (cellSize.Height * MaxRows)) / MaxRows;

            int width = (dx + cellSize.Width) * MaxCols;
            int colSpread = (width < bWidth) ? bWidth - width : 0;

            int height = (dy + cellSize.Height) * MaxRows;
            int rowSpread = (height < bHeight) ? bHeight - height : 0;

            for (int i = 0; i < MaxRows; i++)
            {
                r.Height = cellSize.Height + dy;

                if (i < rowSpread)
                    r.Height++;

                for (int j = 0; j < MaxCols; j++)
                {
                    r.Width = cellSize.Width + dx;

                    if (j < colSpread)
                        r.Width++;

                    _DayRects[i * MaxCols + j].Bounds = r;

                    r.X += r.Width;
                }

                r.X = _Bounds.X + CellPadding;
                r.Y += r.Height;
            }
        }

        #endregion

        #region GetDateFromIndex

        /// <summary>
        /// Gets the month date from the given day index
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        public DateTime GetDateFromIndex(int dayIndex)
        {
            if ((uint)dayIndex < MaxDays)
                return (_RootDate.AddDays(dayIndex).Date);

            return (_RootDate.Date);
        }

        #endregion

        #region GetDateFromPoint

        /// <summary>
        /// Gets the month date from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool GetDateFromPoint(Point pt, ref DateTime date)
        {
            int dayIndex = GetDayIndexFromPoint(pt);

            if (dayIndex >= _DaysOffset && dayIndex < _DaysOffset + _DaysInMonth)
            {
                date = _RootDate.AddDays(dayIndex);

                return (true);
            }

            return (false);
        }

        #endregion

        #region GetViewAreaFromPoint

        /// <summary>
        /// Gets the month view area from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public eViewArea GetViewAreaFromPoint(Point pt)
        {
            if (_ContentBounds.Contains(pt))
                return (eViewArea.InContent);

            if (_MonthHeaderBounds.Contains(pt))
                return (eViewArea.InMonthHeader);

            if (_DayOfWeekHeaderBounds.Contains(pt))
                return (eViewArea.InDayOfWeekHeader);

            return (eViewArea.NotInView);
        }

        #endregion

        #region GetNormalizedDayIndex

        /// <summary>
        /// Gets the normalized month date for the given dayIndex
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        internal int GetNormalizedDayIndex(int dayIndex)
        {
            if (dayIndex < _DaysOffset)
                dayIndex = _DaysOffset;

            else if (dayIndex >= _DaysOffset + _DaysInMonth)
                dayIndex = _DaysOffset + _DaysInMonth - 1;

            return (dayIndex);
        }

        #endregion

        #region GetDayIndexFromPoint

        /// <summary>
        /// Gets the month dayIndex from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        internal int GetDayIndexFromPoint(Point pt)
        {
            for (int i = 0; i < MaxDays; i++)
            {
                if (_DayRects[i].Bounds.Contains(pt))
                    return (i);
            }

            return (-1);
        }

        #endregion

        #region GetDayIndexFromDate

        /// <summary>
        /// Gets the month dayIndex from the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal int GetDayIndexFromDate(DateTime date)
        {
            if (ContainsDate(date) == true)
                return (date.Day - _StartDate.Day + _DaysOffset);

            return (-1);
        }

        #endregion

        #region ContainsDate

        /// <summary>
        /// Determines if the given date is contained in the month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool ContainsDate(DateTime date)
        {
            return (date >= _StartDate && date <= _EndDate);
        }

        #endregion

        #region DayHasAppointments

        /// <summary>
        /// Determines if the given day of the month has
        /// Appointments or CustomItems associated with it
        /// </summary>
        /// <param name="day">Day of the month</param>
        /// <returns>true, if there are Appointments associated with this day</returns>
        public bool DayHasAppointments(int day)
        {
            return (DayIndexHasAppointments(day + _DaysOffset - 1));
        }

        /// <summary>
        /// Determines if the given dayIndex has
        /// Appointments or CustomItems associated with it
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        internal bool DayIndexHasAppointments(int dayIndex)
        {
            dayIndex -= _DaysOffset;

            return (_AppBits.Count > (uint)dayIndex && _AppBits[dayIndex] == true);

        }

        #endregion

        #region DayIsSelected

        /// <summary>
        /// Determines if the given day of the month is selected
        /// </summary>
        /// <param name="day">Day of the month</param>
        /// <returns>true if selected</returns>
        public bool DayIsSelected(int day)
        {
            day += (_DaysOffset - 1);

            return (_DayRects.Rects.Length > (uint)day && _DayRects[day].IsSelected == true);
        }

        #endregion

        #region UpdateDateSelection

        /// <summary>
        /// Updates the date selection for the month
        /// </summary>
        internal void UpdateDateSelection()
        {
            if (_YearView.DateSelectionStart.HasValue == false ||
                _YearView.DateSelectionEnd.HasValue == false)
            {
                ClearSelection();
            }
            else
            {
                DateTime selStart = _YearView.DateSelectionStart.Value;
                DateTime selEnd = _YearView.DateSelectionEnd.Value;

                if (_StartDate > selEnd || _EndDate < selStart)
                {
                    ClearSelection();
                }
                else
                {
                    _DaysSelected = false;

                    DateTime startDate = _StartDate;

                    for (int i = 0; i < _DaysInMonth; i++)
                    {
                        DateTime endDate = startDate.AddDays(1);

                        bool selected = (endDate > selStart && startDate < selEnd);

                        DayRects[i + _DaysOffset].IsSelected = selected;

                        _DaysSelected |= selected;

                        startDate = startDate.AddDays(1);
                    }
                }
            }
        }

        #region ClearSelection

        /// <summary>
        /// ClearSelection
        /// </summary>
        private void ClearSelection()
        {
            if (_DaysSelected == true)
            {
                for (int i = 0; i < MaxDays; i++)
                    _DayRects[i].IsSelected = false;

                _DaysSelected = false;
            }
        }

        #endregion

        #endregion

        #region GetPreferredSize

        /// <summary>
        /// Gets the Preferred control size for the month
        /// </summary>
        /// <returns></returns>
        internal Size GetPreferredSize()
        {
            Size size = _YearView.CellSize;

            size.Width = (size.Width + CellPadding) * MaxCols;

            size.Height = (size.Height + CellPadding) * MaxRows +
                DayOfWeekHeaderHeight + MonthHeaderHeight;

            return (size);
        }

        #endregion

        #region Paint

        /// <summary>
        /// Paint
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isNow"></param>
        internal void Paint(ItemPaintArgs e, bool isNow)
        {
            DrawContent(e);

            DrawMonthHeader(e, isNow);
            DrawDayOfWeekHeader(e, isNow);
            DrawBorder(e, isNow);
        }

        #region DrawContent

        /// <summary>
        /// DrawContent
        /// </summary>
        /// <param name="e"></param>
        private void DrawContent(ItemPaintArgs e)
        {
            DrawBackground(e);
            DrawDayContent(e);
            DrawGridLines(e);
            DrawNowHighlight(e);
        }

        #region DrawBackground

        /// <summary>
        /// DrawBackground
        /// </summary>
        /// <param name="e"></param>
        private void DrawBackground(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            using (Brush br = _YearView.ViewColor.BrushPart(
                (int)eCalendarMonthPart.DayContentActiveBackground, _ContentBounds))
            {
                g.FillRectangle(br, _Bounds);
            }
        }

        #endregion

        #region DrawDayContent

        /// <summary>
        /// DrawDayContent
        /// </summary>
        /// <param name="e"></param>
        private void DrawDayContent(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            Color color = _YearView.ViewColor.GetColor((int)eCalendarMonthPart.DayHeaderForeground);
            Color color1 = _YearView.ViewColor.GetColor((int)eCalendarMonthPart.DayContactInactiveBackground);
            Color color2 = _YearView.ViewColor.GetColor((int)eCalendarMonthPart.DayContentActiveBackground);

            DateTime date = _StartDate;

            for (int i = 0; i < _DaysInMonth; i++)
            {
                ItemRect ir = _DayRects[i + _DaysOffset];

                DrawDayContentBackground(g, date, ir, color1, color2);
                DrawDayContentText(g, date, ir, color);

                date = date.AddDays(1);
            }
        }

        #region DrawDayContentBackground

        /// <summary>
        /// DrawDayContentBackground
        /// </summary>
        /// <param name="g"></param>
        /// <param name="date"></param>
        /// <param name="ir"></param>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        private void DrawDayContentBackground(Graphics g,
            DateTime date, ItemRect ir, Color color1, Color color2)
        {
            eYearViewLinkStyle linkStyle =
                _YearView.CalendarView.YearViewAppointmentLinkStyle;

            if (_YearView.CalendarView.DoYearViewDrawDayBackground(g, this, date, ir.Bounds, ref linkStyle) == false)
            {
                if (_YearView.AllowDateSelection == true && ir.IsSelected == true)
                {
                    using (Brush br = _YearView.ViewColor.BrushPart(
                        (int) eCalendarMonthPart.DayContentSelectionBackground, ir.Bounds))
                    {
                        g.FillRectangle(br, ir.Bounds);
                    }
                }

                if (_AppBits.Get(date.Day - 1) == true)
                {
                    if (ir.IsSelected == false)
                        DrawDayHighLight(g, linkStyle, ir.Bounds, color1, color2);
                }
            }
        }

        #endregion

        #region DrawDayContentText

        /// <summary>
        /// DrawDayContentText
        /// </summary>
        /// <param name="g"></param>
        /// <param name="date"></param>
        /// <param name="ir"></param>
        /// <param name="color"></param>
        private void DrawDayContentText(Graphics g,
            DateTime date, ItemRect ir, Color color)
        {
            if (_YearView.CalendarView.DoYearViewDrawDayText(g, this, date, ir.Bounds) == false)
            {
                Font font = _YearView.Font;

                if (_AppBits.Get(date.Day - 1) == true)
                    font = _YearView.BoldFont;

                TextDrawing.DrawString(g, date.Day.ToString(), font, color, ir.Bounds,
                                       eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter |
                                       eTextFormat.NoPadding);
            }
        }

        #endregion

        #endregion

        #region DrawDayHighLight

        /// <summary>
        /// Draws the day highlight
        /// </summary>
        /// <param name="g"></param>
        /// <param name="style"></param>
        /// <param name="r"></param>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        private void DrawDayHighLight(Graphics g,
            eYearViewLinkStyle style, Rectangle r, Color color1, Color color2)
        {
            switch (style)
            {
                case eYearViewLinkStyle.Style1:
                    DrawDayHighLightStyle1(g, r, color1, color2);
                    break;

                case eYearViewLinkStyle.Style2:
                    DrawDayHighLightStyle2(g, r, color1, color2);
                    break;

                case eYearViewLinkStyle.Style3:
                    DrawDayHighLightStyle3(g, r, color1, color2);
                    break;

                case eYearViewLinkStyle.Style4:
                    DrawDayHighLightStyle4(g, r, color1, color2);
                    break;

                case eYearViewLinkStyle.Style5:
                    DrawDayHighLightStyle5(g, r, color1);
                    break;
            }
        }

        #region DrawDayHighLightStyle1

        private void DrawDayHighLightStyle1(Graphics g, Rectangle r, Color color1, Color color2)
        {
            using (Brush br = new LinearGradientBrush(r, color2, color1, 45f))
                g.FillRectangle(br, r);
        }

        #endregion

        #region DrawDayHighLightStyle2

        private void DrawDayHighLightStyle2(Graphics g, Rectangle r, Color color1, Color color2)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle t = r;

                if (t.Height > t.Width)
                {
                    t.Y += (t.Height - t.Width) / 2;
                    t.Height = t.Width;
                }
                else if (t.Width > t.Height)
                {
                    t.X += (t.Width - t.Height) / 2;
                    t.Width = t.Height;
                }

                t.Inflate(2, 2);

                path.AddEllipse(t);

                using (PathGradientBrush pbr = new PathGradientBrush(path))
                {
                    pbr.CenterPoint = new
                        PointF(r.X + r.Width / 2, r.Y + r.Height / 2);

                    pbr.CenterColor = color1;
                    pbr.SurroundColors = new Color[] { color2 };

                    g.FillPath(pbr, path);
                }
            }
        }

        #endregion

        #region DrawDayHighLightStyle3

        private void DrawDayHighLightStyle3(Graphics g, Rectangle r, Color color1, Color color2)
        {
            r.Height /= 2;

            using (LinearGradientBrush br = new LinearGradientBrush(r, color1, color2, 90f))
            {
                r.Height *= 2;
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }
        }

        #endregion

        #region DrawDayHighLightStyle4

        private void DrawDayHighLightStyle4(Graphics g, Rectangle r, Color color1, Color color2)
        {
            r.Width /= 2;

            using (LinearGradientBrush br = new LinearGradientBrush(r, color1, color2, 0f))
            {
                r.Width *= 2;
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }
        }

        #endregion

        #region DrawDayHighLightStyle5

        private void DrawDayHighLightStyle5(Graphics g, Rectangle r, Color color1)
        {
            using (Brush br = new SolidBrush(color1))
                g.FillRectangle(br, r);
        }

        #endregion

        #endregion

        #region DrawGridLines

        /// <summary>
        /// DrawGridLines
        /// </summary>
        /// <param name="e"></param>
        private void DrawGridLines(ItemPaintArgs e)
        {
            if (_YearView.ShowGridLines == true)
            {
                Graphics g = e.Graphics;

                using (Pen pen = new Pen(
                    ControlPaint.Light(_YearView.ViewColor.GetColor((int)eCalendarMonthPart.DayContentBorder))))
                {
                    int right = DayRects[MaxCols - 1].Bounds.Right;

                    for (int i = 1; i < MaxRows; i++)
                    {
                        Point pt1 = DayRects[i * MaxCols].Bounds.Location;
                        Point pt2 = new Point(right, pt1.Y);

                        g.DrawLine(pen, pt1, pt2);
                    }

                    int bottom = DayRects[(MaxRows - 1) * MaxCols].Bounds.Bottom;

                    for (int i = 1; i < MaxCols; i++)
                    {
                        Point pt1 = DayRects[i].Bounds.Location;
                        Point pt2 = new Point(pt1.X, bottom);

                        g.DrawLine(pen, pt1, pt2);
                    }
                }
            }
        }

        #endregion

        #region DrawNowHighlight

        /// <summary>
        /// DrawNowHighlight
        /// </summary>
        /// <param name="e"></param>
        private void DrawNowHighlight(ItemPaintArgs e)
        {
            if (_YearView.CalendarView.HighlightCurrentDay)
            {
                Graphics g = e.Graphics;

                int nowDay = GetDayIndexFromDate(DateTime.Now);

                if (nowDay >= 0)
                {
                    Rectangle r = DayRects[nowDay].Bounds;

                    using (Pen pen = new Pen(
                        _YearView.ViewColor.GetColor((int)eCalendarMonthPart.NowDayHeaderBorder), 1))
                    {
                        r.Inflate(-1, -1);

                        g.DrawRectangle(pen, r);
                    }

                    using (Pen pen = new Pen(
                        _YearView.ViewColor.GetColor((int)eCalendarMonthPart.DayContentActiveBackground)))
                    {

                        r.Inflate(1, 1);

                        g.DrawRectangle(pen, r);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region DrawMonthHeader

        /// <summary>
        /// DrawMonthHeader
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isNow"></param>
        private void DrawMonthHeader(ItemPaintArgs e, bool isNow)
        {
            Graphics g = e.Graphics;

            // Draw the header background

            Rectangle r = _MonthHeaderBounds;

            if (r.Width > 0 && r.Height > 0)
            {
                if (isNow == true)
                {
                    using (Brush br = _YearView.ViewColor.BrushPart(
                        (int)eCalendarMonthPart.NowDayHeaderBackground, r))
                    {
                        g.FillRectangle(br, r);
                    }
                }
                else
                {
                    using (Brush br = _YearView.ViewColor.BrushPart(
                    (int) eCalendarMonthPart.DayHeaderBackground, r))
                    {
                        g.FillRectangle(br, r);
                    }
                }

                // Draw the header content

                string s = String.Format("{0:y}", _StartDate);

                Color color = _YearView.ViewColor.GetColor(isNow == true
                    ? (int)eCalendarMonthPart.NowDayHeaderForeground
                    : (int)eCalendarMonthPart.DayHeaderForeground);

                TextDrawing.DrawString(g, s, _YearView.Font, color, _MonthHeaderBounds,
                                       eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.NoPadding);
            }
        }

        #endregion

        #region DrawDayOfWeekHeader

        /// <summary>
        /// DrawDayOfWeekHeader
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isNow"></param>
        private void DrawDayOfWeekHeader(ItemPaintArgs e, bool isNow)
        {
            Graphics g = e.Graphics;

            // Draw the header background

            Rectangle r = _DayOfWeekHeaderBounds;

            if (r.Width > 0 && r.Height > 0)
            {
                if (isNow == true)
                {
                    using (Brush br = _YearView.ViewColor.BrushPart(
                        (int)eCalendarMonthPart.NowDayHeaderBackground, r))
                    {
                        g.FillRectangle(br, r);
                    }
                }
                else
                {
                    using (Brush br = _YearView.ViewColor.BrushPart(
                        (int)eCalendarMonthPart.DayOfTheWeekHeaderBackground, r))
                    {
                        g.FillRectangle(br, r);
                    }
                }

                // Establish our Days Of The Week text type

                DaysOfTheWeek.eDayType type = GetDaysOfTheWeekType(e);

                // Loop through each day, drawing the
                // day of the week text in the header area

                const eTextFormat tf = eTextFormat.VerticalCenter |
                                       eTextFormat.HorizontalCenter | eTextFormat.NoPadding;

                r = _DayOfWeekHeaderBounds;

                int width = r.Width / MaxCols;
                int spread = r.Width - (width * MaxCols);

                Color color = _YearView.ViewColor.GetColor(isNow == true
                    ? (int)eCalendarMonthPart.NowDayHeaderForeground
                    : (int)eCalendarMonthPart.DayOfTheWeekHeaderForeground);

                for (int i = 0; i < MaxCols; i++)
                {
                    r.Width = width;

                    if (i < spread)
                        r.Width++;

                    TextDrawing.DrawString(g,
                        _YearView.DaysOfTheWeek.DayText[(int)type][i], _YearView.Font, color, r, tf);

                    r.X += r.Width;
                }
            }
        }

        #endregion

        #region DrawBorder

        /// <summary>
        /// DrawBorder
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isNow"></param>
        private void DrawBorder(ItemPaintArgs e, bool isNow)
        {
            Graphics g = e.Graphics;

            if (isNow == true)
            {
                using (Pen pen = new Pen(_YearView.ViewColor.GetColor(
                    (int)eCalendarMonthPart.NowDayHeaderBorder)))
                {
                    g.DrawRectangle(pen, _MonthHeaderBounds);
                    g.DrawRectangle(pen, _DayOfWeekHeaderBounds);
                    g.DrawRectangle(pen, _ContentBounds);
                }
            }
            else
            {
                using (Pen pen = new Pen(
                    _YearView.ViewColor.GetColor((int) eCalendarMonthPart.DayContentBorder)))
                {
                    g.DrawRectangle(pen, _MonthHeaderBounds);
                    g.DrawRectangle(pen, _DayOfWeekHeaderBounds);
                }

                using (Pen pen = new Pen(
                    _YearView.ViewColor.GetColor((int) eCalendarMonthPart.DayHeaderBorder)))
                {
                    g.DrawRectangle(pen, _ContentBounds);
                }
            }
        }

        #endregion

        #region GetDaysOfTheWeekType

        /// <summary>
        /// GetDaysOfTheWeekType
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private DaysOfTheWeek.eDayType GetDaysOfTheWeekType(ItemPaintArgs e)
        {
            if (_DayType != DaysOfTheWeek.eDayType.None)
                return (_DayType);
            
            Graphics g = e.Graphics;

            _YearView.DaysOfTheWeek.MeasureText(g, _YearView.Font);

            int width = _Bounds.Width/MaxCols;

            for (int i = 0; i < MaxCols; i++)
            {
                if (_YearView.DaysOfTheWeek.DaySize[
                    (int) DaysOfTheWeek.eDayType.Long][i].Width > width)
                {
                    if (_YearView.DaysOfTheWeek.DaySize[
                        (int) DaysOfTheWeek.eDayType.Short][i].Width > width)
                    {
                        return (DaysOfTheWeek.eDayType.Single);
                    }

                    return (DaysOfTheWeek.eDayType.Short);
                }
            }

            return (DaysOfTheWeek.eDayType.Long);
        }

        #endregion

        #endregion

    }
}
#endif

