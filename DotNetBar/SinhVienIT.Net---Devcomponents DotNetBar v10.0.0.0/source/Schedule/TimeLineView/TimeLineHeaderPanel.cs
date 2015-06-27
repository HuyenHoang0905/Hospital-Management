#if FRAMEWORK20
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    public class TimeLineHeaderPanel : BaseItem
    {
        #region Private Variables

        private CalendarView _CalendarView;         // Assoc CalendarView

        private int _HScrollPos;                    // Horizontal scroll position

        private CalendarWeekDayColor _ViewColor =   // View display color table
            new CalendarWeekDayColor(eCalendarColor.Automatic);

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        public TimeLineHeaderPanel(CalendarView calendarView)
        {
            _CalendarView = calendarView;

            MouseUpNotification = true;
            Name = "TimeLineHeaderPanel";

            HookEvents(true);
        }

        #region Private properties

        #region Column properties

        /// <summary>
        /// Gets the ColumnWidth
        /// </summary>
        private int ColumnWidth
        {
            get { return (_CalendarView.TimeLineColumnWidth); }
        }

        /// <summary>
        /// Gets the TimeLineColumnCount
        /// </summary>
        private int ColumnCount
        {
            get { return (_CalendarView.TimeLineColumnCount); }
        }

        #endregion

        #region Header properties

        /// <summary>
        /// Gets the ShowPeriodHeader property
        /// </summary>
        private bool ShowPeriodHeader
        {
            get { return (_CalendarView.TimeLineShowPeriodHeader); }
        }

        /// <summary>
        /// Gets the ShowIntervalHeader property
        /// </summary>
        private bool ShowIntervalHeader
        {
            get { return (_CalendarView.TimeLineShowIntervalHeader); }
        }


        /// <summary>
        /// Interval header height
        /// </summary>
        private int IntervalHeaderHeight
        {
            get { return (_CalendarView.TimeLineIntervalHeaderHeight); }
        }

        /// <summary>
        /// Period header height
        /// </summary>
        private int PeriodHeaderHeight
        {
            get
            {
                return (ShowPeriodHeader && _CalendarView.TimeLinePeriodHeaderHeight > 0 ?
                    _CalendarView.TimeLinePeriodHeaderHeight - 1 : 0);
            }
        }

        /// <summary>
        /// Header font
        /// </summary>
        private Font HeaderFont
        {
            get { return (_CalendarView.Font); }
        }

        #endregion

        #region StartDate

        /// <summary>
        /// TimeLine start date
        /// </summary>
        private DateTime StartDate
        {
            get { return (_CalendarView.TimeLineViewStartDate); }
        }

        #endregion

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the view color
        /// </summary>
        public eCalendarColor CalendarColor
        {
            get { return (_ViewColor.ColorSch); }

            set
            {
                _ViewColor.ColorSch = value;
                Refresh();
            }
        }
        #endregion

        #region HookEvents

        /// <summary>
        /// Routine hooks all necessary events for this control
        /// </summary>
        /// <param name="hook">True to hook, false to unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _CalendarView.TimeLineIntervalChanged += TimeLineIntervalChanged;
                _CalendarView.TimeLineIntervalPeriodChanged += TimeLineIntervalPeriodChanged;
                _CalendarView.TimeIndicators.TimeIndicatorCollectionChanged += TimeIndicatorCollectionChanged;
                _CalendarView.TimeIndicatorTimeChanged += TimeIndicatorTimeChanged;

                _CalendarView.TimeLineHScrollPanel.ScrollPanelChanged += ScrollPanelChanged;
            }
            else
            {
                _CalendarView.TimeLineIntervalChanged -= TimeLineIntervalChanged;
                _CalendarView.TimeLineIntervalPeriodChanged -= TimeLineIntervalPeriodChanged;
                _CalendarView.TimeIndicators.TimeIndicatorCollectionChanged -= TimeIndicatorCollectionChanged;
                _CalendarView.TimeIndicatorTimeChanged -= TimeIndicatorTimeChanged;

                _CalendarView.TimeLineHScrollPanel.ScrollPanelChanged -= ScrollPanelChanged;
            }
        }

        #endregion

        #region Event processing

        #region TimeLineIntervalPeriodChanged

        /// <summary>
        /// TimeLineIntervalPeriod Change notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineIntervalPeriodChanged(object sender, TimeLineIntervalPeriodChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region TimeLineIntervalChanged

        /// <summary>
        /// TimeLineInterval Change notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeLineIntervalChanged(object sender, TimeLineIntervalChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region TimeIndicatorCollectionChanged

        /// <summary>
        /// Handles TimeIndicatorCollectionChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeIndicatorCollectionChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        #endregion

        #region TimeIndicatorTimeChanged

        /// <summary>
        /// Handles TimeIndicatorTimeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeIndicatorTimeChanged(object sender, TimeIndicatorTimeChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region ScrollPanelChanged

        /// <summary>
        /// Horizontal Scroll Panel change notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScrollPanelChanged(object sender, EventArgs e)
        {
            _HScrollPos = -_CalendarView.TimeLineHScrollPanel.ScrollBar.Value *
                _CalendarView.TimeLineColumnWidth;

            // Redraw our view

            Refresh();
        }

        #endregion

        #endregion

        #region IsMarkupSupported

        /// <summary>
        /// IsMarkupSupported
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return (_CalendarView != null ?
                _CalendarView.TimeLinePeriodHeaderEnableMarkup : true); }
        }

        #endregion

        #region GetViewAreaFromPoint

        /// <summary>
        /// Gets the view area under the given mouse point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>eViewArea</returns>
        public eViewArea GetViewAreaFromPoint(Point pt)
        {
            if (Bounds.Contains(pt) == true)
            {
                if (ShowPeriodHeader == true)
                {
                    if (pt.Y < PeriodHeaderHeight)
                        return (eViewArea.InPeriodHeader);
                }

                return (eViewArea.InIntervalHeader);
            }

            return (eViewArea.NotInView);
        }

        #endregion

        #region Paint processing

        /// <summary>
        /// Paint processing routine
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(ItemPaintArgs e)
        {
            if (ShowPeriodHeader == true || ShowIntervalHeader == true)
            {
                // Set our current color table

                _ViewColor.SetColorTable();

                // Only draw something if we have something to draw

                if (_CalendarView.TimeLineColumnCount > 0)
                {
                    // Draw our calendar parts

                    int colStart, colEnd;

                    if (GetColRange(e, out colStart, out colEnd) > 0)
                    {
                        Region rgnSave = e.Graphics.Clip;

                        using (Region rgn = new Region(Bounds))
                        {
                            e.Graphics.Clip = rgn;

                            if (ShowPeriodHeader == true)
                                DrawDateHeader(e);

                            if (ShowIntervalHeader == true)
                                DrawIntervalHeader(e, colStart, colEnd);
                        }

                        e.Graphics.Clip = rgnSave;
                    }
                }
            }
        }

        #region DrawDateHeader

        /// <summary>
        /// Draws the encompassing Date header
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        private void DrawDateHeader(ItemPaintArgs e)
        {
            int scol = -_HScrollPos / ColumnWidth;
            int ecol = (-_HScrollPos + Bounds.Width + ColumnWidth - 2) / ColumnWidth;

            DateTime date1 = _CalendarView.TimeLineAddInterval(StartDate, scol);
            DateTime date2 = date1;

            for (int i = scol + 1; i < ecol; i++)
            {
                date2 = _CalendarView.TimeLineAddInterval(StartDate, i);

                if (PeriodChange(date1, date2, i) == true)
                {
                    DrawDatePeriod(e, scol, i - 1, date1, date2.AddMinutes(-1));

                    scol = i;
                    date1 = date2;
                }
            }

            DrawDatePeriod(e, scol, ecol, date1, date2);
        }

        #region PeriodChange

        /// <summary>
        /// Determines if a date period change has occurred
        /// </summary>
        /// <param name="date1">Initial date</param>
        /// <param name="date2">Current date</param>
        /// <param name="col">Current column</param>
        /// <returns></returns>
        private bool PeriodChange(DateTime date1, DateTime date2, int col)
        {
            switch (_CalendarView.TimeLinePeriod)
            {
                case eTimeLinePeriod.Days:
                    if (_CalendarView.TimeLineInterval == 1)
                        return (date1.Month != date2.Month);

                    return (date1.Year != date2.Year);

                case eTimeLinePeriod.Years:
                    return ((col > 0) && (col % 10 == 0));

                default:
                    return (date1.Day != date2.Day);
            }
        }

        #endregion

        #region DrawDatePeriod

        /// <summary>
        /// Draws a given date period or range
        /// </summary>
        /// <param name="e"></param>
        /// <param name="scol">Starting column</param>
        /// <param name="ecol">Ending column</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void DrawDatePeriod(ItemPaintArgs e,
            int scol, int ecol, DateTime startDate, DateTime endDate)
        {
            Graphics g = e.Graphics;

            Rectangle rs = GetColumnRect(scol);
            Rectangle re = GetColumnRect(ecol);

            Rectangle r = new Rectangle(rs.X, rs.Y, re.Right - rs.X, PeriodHeaderHeight);

            // Keep us in bounds

            if (r.X < Bounds.X)
            {
                r.Width -= (Bounds.X - r.X);
                r.X = Bounds.X;
            }

            if (r.Right > Bounds.Right)
                r.Width -= (r.Right - Bounds.Right) + 1;

            if (r.Width > 0 && r.Height > 0)
            {
                // Fill the area

                using (Brush lbr =
                    _ViewColor.BrushPart((int)eCalendarWeekDayPart.DayHeaderBackground, r))
                {
                    g.FillRectangle(lbr, r);
                }

                // Format the period text and output it both
                // vertically and horizontally centered

                string text = GetDatePeriodText(ref startDate, ref endDate);

                if (_CalendarView.DoTimeLineViewRenderPeriodHeader(g, startDate, endDate, r, ref text) == false)
                {
                    if (String.IsNullOrEmpty(text) == false)
                    {
                        Text = text;

                        if (TextMarkupBody != null)
                        {
                            TextMarkup.MarkupDrawContext d =
                                new TextMarkup.MarkupDrawContext(g, HeaderFont, _ViewColor.GetColor(
                                    (int) eCalendarWeekDayPart.DayHeaderForeground), IsRightToLeft);

                            TextMarkupBody.InvalidateElementsSize();
                            TextMarkupBody.Measure(new Size(5000, 5000), d);
                            TextMarkupBody.Arrange(new Rectangle(Point.Empty, TextMarkupBody.Bounds.Size), d);

                            TextMarkupBody.Bounds = AlignMarkUpText(r);

                            RenderMarkup(g, d);
                        }
                        else
                        {
                            Size sz = TextDrawing.MeasureString(g, Text, HeaderFont);

                            eTextFormat tf = eTextFormat.VerticalCenter;

                            switch (_CalendarView.TimeLinePeriodHeaderAlignment)
                            {
                                case eItemAlignment.Center:
                                    if (sz.Width < r.Width)
                                        tf |= eTextFormat.HorizontalCenter;
                                    break;

                                case eItemAlignment.Far:
                                    if (sz.Width < r.Width)
                                        tf |= eTextFormat.Right;
                                    break;
                            }

                            // Draw the text

                            TextDrawing.DrawString(g, Text, HeaderFont, _ViewColor.GetColor(
                                (int) eCalendarWeekDayPart.DayHeaderForeground), r, tf);
                        }
                    }
                }

                // Draw the border

                using (Pen pen = new Pen(
                    _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHeaderBorder)))
                {
                    g.DrawRectangle(pen, r);
                }
            }
        }

        #region AlignMarkUpText

        /// <summary>
        /// Aligns the MarkUp text
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private Rectangle AlignMarkUpText(Rectangle r)
        {
            if (r.Height > TextMarkupBody.Bounds.Height)
                r.Y += (r.Height - TextMarkupBody.Bounds.Height) / 2;

            if (r.Width > TextMarkupBody.Bounds.Width)
            {
                switch (_CalendarView.TimeLinePeriodHeaderAlignment)
                {
                    case eItemAlignment.Center:
                        r.X += (r.Width - TextMarkupBody.Bounds.Width) / 2;
                        break;

                    case eItemAlignment.Far:
                        r.X += (r.Width - TextMarkupBody.Bounds.Width);
                        break;

                    default:
                        r.X += 3;
                        break;
                }
            }

            return (r);
        }

        #endregion

        #region RenderMarkup

        /// <summary>
        /// Renders the current MarkUp
        /// </summary>
        /// <param name="g"></param>
        /// <param name="d"></param>
        private void RenderMarkup(Graphics g, TextMarkup.MarkupDrawContext d)
        {
            Region oldClip = g.Clip;
            Rectangle clipRect = TextMarkupBody.Bounds;

            g.SetClip(clipRect, CombineMode.Intersect);

            TextMarkupBody.Render(d);

            g.Clip = oldClip;
        }

        #endregion

        #region GetDatePeriodText

        /// <summary>
        /// Gets our default Period text
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string GetDatePeriodText(ref DateTime startDate, ref DateTime endDate)
        {
            // Days text

            if (_CalendarView.TimeLinePeriod == eTimeLinePeriod.Days)
            {
                if (_CalendarView.TimeLineInterval == 1)
                    return (startDate.ToString("y"));

                return (startDate.Year.ToString());
            }

            // Years text

            if (_CalendarView.TimeLinePeriod == eTimeLinePeriod.Years)
            {
                int n = _CalendarView.TimeLineInterval * 10;
                int m = startDate.Year - _CalendarView.TimeLineViewStartDate.Year;

                m = (m / n) * n;

                startDate = _CalendarView.TimeLineViewStartDate.AddYears(m);
                endDate = startDate.AddYears(n - 1);

                return (startDate.Year + " - " + endDate.Year);
            }

            // Everything else

            return (startDate.Date.ToShortDateString());
        }

        #endregion

        #endregion

        #endregion

        #region DrawIntervalHeader

        /// <summary>
        /// Draws the time interval header
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="colStart">Starting column</param>
        /// <param name="colEnd">Ending column</param>
        private void DrawIntervalHeader(ItemPaintArgs e, int colStart, int colEnd)
        {
            Graphics g = e.Graphics;

            Rectangle rs = GetColumnRect(colStart);
            Rectangle re = GetColumnRect(colEnd);

            Rectangle r = new Rectangle(rs.X, rs.Y + PeriodHeaderHeight,
                re.Right - rs.X, IntervalHeaderHeight);

            if (r.Width > 0 && r.Height > 0)
            {
                DrawBackground(g, r);
                DrawTimeIndicators(g, colStart, colEnd, r);
                DrawContent(g, colStart, colEnd, r);
            }
        }

        #region DrawBackground

        /// <summary>
        /// DrawBackground
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        private void DrawBackground(Graphics g, Rectangle r)
        {
            using (Brush lbr = _ViewColor.BrushPart((int)eCalendarWeekDayPart.DayHeaderBackground, r))
                g.FillRectangle(lbr, r);
        }

        #endregion

        #region DrawTimeIndicators

        #region DrawTimeIndicators

        /// <summary>
        /// Draws view TimeIndicators
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        /// <param name="r"></param>
        private void DrawTimeIndicators(Graphics g,
            int colStart, int colEnd, Rectangle r)
        {
            DateTime start = _CalendarView.TimeLineAddInterval(StartDate, colStart);
            DateTime end = _CalendarView.TimeLineAddInterval(StartDate, colEnd);

            for (int i = 0; i < _CalendarView.TimeIndicators.Count; i++)
            {
                TimeIndicator ti = _CalendarView.TimeIndicators[i];

                if (ti.IndicatorArea == eTimeIndicatorArea.All ||
                    ti.IndicatorArea == eTimeIndicatorArea.Header)
                {
                    if (ti.IsVisible())
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
        /// CalcIndicatorRect
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="startDate"></param>
        /// <param name="sRect"></param>
        /// <returns></returns>
        private Rectangle GetIndicatorRect(
            TimeIndicator ti, DateTime startDate, Rectangle sRect)
        {
            double x = ColumnWidth / _CalendarView.BaseInterval;

            int offset = (int)((ti.IndicatorDisplayTime - startDate).TotalMinutes * x);

            sRect.X += (offset - ti.Thickness - 1);
            sRect.Width = ti.Thickness;

            return (sRect);
        }

        #endregion

        #endregion

        #region DrawContent

        /// <summary>
        /// DrawContent
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        /// <param name="r"></param>
        private void DrawContent(Graphics g,
            int colStart, int colEnd, Rectangle r)
        {
            bool dcDay = _CalendarView.HighlightCurrentDay == true &&
                _CalendarView.TimeLinePeriod == eTimeLinePeriod.Days && _CalendarView.TimeLineInterval == 1;

            DateTime now = DateTime.Now.Date;
            DateTime date = _CalendarView.TimeLineAddInterval(StartDate, colStart);

            r.Width = ColumnWidth;

            int x = -1;

            using (Pen pen = new Pen(
                _ViewColor.GetColor((int)eCalendarWeekDayPart.DayHeaderBorder)))
            {
                // Loop through each column, drawing the
                // time text in the header area

                for (int i = colStart; i <= colEnd; i++)
                {
                    if (dcDay == false || date.Date.Equals(now) == false)
                    {
                        // Draw the text and header border

                        TextDrawing.DrawString(g, GetIntervalText(date), HeaderFont,
                                               _ViewColor.GetColor((int) eCalendarWeekDayPart.DayHeaderForeground),
                                               r, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter);

                        g.DrawRectangle(pen, r);
                    }
                    else
                    {
                        x = r.X;
                    }

                    date = _CalendarView.TimeLineAddInterval(date, 1);

                    r.X += ColumnWidth;
                }
            }

            if (x >= 0)
            {
                r.X = x;

                using (Brush br = _ViewColor.BrushPart((int)eCalendarWeekDayPart.NowDayHeaderBackground, r))
                    g.FillRectangle(br, r);

                TextDrawing.DrawString(g, GetIntervalText(now), HeaderFont,
                                       _ViewColor.GetColor((int)eCalendarWeekDayPart.NowDayHeaderForeground),
                                       r, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter);

                using (Pen pen = new Pen(_ViewColor.GetColor((int)eCalendarWeekDayPart.NowDayHeaderBorder)))
                    g.DrawRectangle(pen, r);
            }
        }

        #region GetIntervalText

        /// <summary>
        /// Gets the interval text
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>Interval text</returns>
        private string GetIntervalText(DateTime date)
        {
            switch (_CalendarView.TimeLinePeriod)
            {
                case eTimeLinePeriod.Days:
                    return (GetDayIntervalText(date));

                case eTimeLinePeriod.Years:
                    return (GetYearIntervalText(date));

                default:
                    return (GetMinuteIntervalText(date));
            }
        }

        /// <summary>
        /// Gets minute interval text
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>Interval text</returns>
        private string GetMinuteIntervalText(DateTime date)
        {
            if (_CalendarView.Is24HourFormat == true)
                return (date.ToString("t", DateTimeFormatInfo.InvariantInfo));
            
            return (date.ToString("t", null));
        }

        /// <summary>
        /// Gets Day interval text
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Interval text</returns>
        private string GetDayIntervalText(DateTime date)
        {
            DateTimeFormatInfo dtfi =
                ScheduleSettings.GetActiveCulture().DateTimeFormat;

            string s = dtfi.MonthDayPattern;

            s = s.Replace("MMMM", "M");
            s = s.Replace(" ", dtfi.DateSeparator);

            return (date.ToString(s));
        }

        /// <summary>
        /// Gets year interval text
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Interval text</returns>
        private string GetYearIntervalText(DateTime date)
        {
            return (date.Year.ToString());
        }

        #endregion

        #endregion

        #endregion

        #region GetColumnRect

        private Rectangle GetColumnRect(int col)
        {
            int x = Bounds.X + (col * ColumnWidth) + _HScrollPos;

            return (new Rectangle(x, Bounds.Y,
                ColumnWidth, Bounds.Height));
        }

        #endregion

        #region GetColRange

        /// <summary>
        /// Calculates the range of days needed to be drawn
        /// to satisfy the specified paint request
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="colStart">[out] Column start index</param>
        /// <param name="colEnd">[out] Column end index</param>
        /// <returns>Day range count (end - start)</returns>
        private int GetColRange(ItemPaintArgs e, out int colStart, out int colEnd)
        {
            // Calc our starting index

            int start = -_HScrollPos / ColumnWidth;
            int x = Bounds.X + start * ColumnWidth + _HScrollPos;

            while (start < ColumnCount)
            {
                if (x + ColumnWidth > e.ClipRectangle.X)
                    break;

                x += ColumnWidth;

                start++;
            }

            // Calc our ending index

            int end = start;

            while (end < ColumnCount)
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

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
                HookEvents(false);

            base.Dispose(disposing);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            TimeLineHeaderPanel objCopy = new TimeLineHeaderPanel(_CalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the TimeLineHeaderPanel specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New TimeLineHeaderPanel instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            TimeLineHeaderPanel objCopy = copy as TimeLineHeaderPanel;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy.CalendarColor = this.CalendarColor;
            }
        }

        #endregion
    }
}
#endif

