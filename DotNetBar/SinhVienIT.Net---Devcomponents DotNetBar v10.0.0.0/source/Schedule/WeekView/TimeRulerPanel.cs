#if FRAMEWORK20
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Schedule
{
    public class TimeRulerPanel : BaseItem
    {
        #region Private Variables

        private CalendarView _CalendarView;         // Assoc CalendarView

        private int _VScrollPos;                    // Vertical scroll position

        private Font _TimeRulerFont =               // _TimeRuler Font
            new Font(SystemFonts.CaptionFont.FontFamily, SystemFonts.CaptionFont.Size * 1.3f);

        private Font _TimeRulerFontSm =             // _TimeRulerFont (small)
            new Font(SystemFonts.CaptionFont.FontFamily, SystemFonts.CaptionFont.Size * .8f);

        private TimeRulerColor _ViewColor =         // View display color table
            new TimeRulerColor();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        public TimeRulerPanel(CalendarView calendarView)
        {
            _CalendarView = calendarView;

            Name = "TimeRulerPanel";

            HookEvents(true);
        }

        #region Private properties

        #region General display properties

        /// <summary>
        /// Gets the TimeRuler font
        /// </summary>
        private Font TimeRulerFont
        {
            get { return (_TimeRulerFont); }
        }

        /// <summary>
        /// Gets the TimeRuler font (small)
        /// </summary>
        private Font TimeRulerFontSm
        {
            get { return (_TimeRulerFontSm); }
        }

        #endregion

        #region Time Slice Properties

        /// <summary>
        /// Gets the default Time Slice height
        /// </summary>
        private float TimeSliceHeight
        {
            get { return (_CalendarView.TimeSliceHeight); }
        }

        /// <summary>
        /// Gets the TimeSlotDuration
        /// </summary>
        private int TimeSlotDuration
        {
            get { return (_CalendarView.TimeSlotDuration); }
        }

        /// <summary>
        /// Gets the SlotsPerHour
        /// </summary>
        private int SlotsPerHour
        {
            get { return (_CalendarView.SlotsPerHour); }
        }

        /// <summary>
        /// Gets the NumberOfSlices
        /// </summary>
        private int NumberOfSlices
        {
            get { return (_CalendarView.NumberOfSlices); }
        }

        /// <summary>
        /// Gets the starting Time Slice
        /// </summary>
        private int StartSlice
        {
            get { return (_CalendarView.StartSlice); }
        }

        #endregion

        #region AM / PM designators

        /// <summary>
        /// Gets the culturally correct AM time designator
        /// </summary>
        private string AmDesignator
        {
            get
            {
                return (ScheduleSettings.GetActiveCulture().
                    DateTimeFormat.AMDesignator.ToLower());
            }
        }

        /// <summary>
        /// Gets the culturally correct PM time designator
        /// </summary>
        private string PmDesignator
        {
            get
            {
                return (ScheduleSettings.GetActiveCulture().
                    DateTimeFormat.PMDesignator.ToLower());
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
                _CalendarView.LabelTimeSlotsChanged += CalendarView_LabelTimeSlotsChanged;
                _CalendarView.Is24HourFormatChanged += CalendarView_Is24HourFormatChanged;
                _CalendarView.TimeSlotDurationChanged += CalendarView_TimeSlotDurationChanged;
                _CalendarView.TimeIndicatorsChanged += CalendarView_TimeIndicatorsChanged;
                _CalendarView.TimeIndicatorTimeChanged += CalendarView_TimeIndicatorTimeChanged;

                _CalendarView.WeekDayVScrollPanel.ScrollBarChanged +=  VScrollPanel_ScrollBarChanged;
            }
            else
            {
                _CalendarView.LabelTimeSlotsChanged -= CalendarView_LabelTimeSlotsChanged;
                _CalendarView.Is24HourFormatChanged -= CalendarView_Is24HourFormatChanged;
                _CalendarView.TimeSlotDurationChanged -= CalendarView_TimeSlotDurationChanged;
                _CalendarView.TimeIndicatorsChanged -= CalendarView_TimeIndicatorsChanged;
                _CalendarView.TimeIndicatorTimeChanged -= CalendarView_TimeIndicatorTimeChanged;

                _CalendarView.WeekDayVScrollPanel.ScrollBarChanged -= VScrollPanel_ScrollBarChanged;
            }
        }

        #endregion

        #region Event Processing

        #region CalendarView_LabelTimeSlotsChanged

        /// <summary>
        /// Processes LabelTimeSlotsChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarView_LabelTimeSlotsChanged(
            object sender, LabelTimeSlotsChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region CalendarView_Is24HourFormatChanged

        /// <summary>
        /// Processes Is24HourFormatChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarView_Is24HourFormatChanged(
            object sender, Is24HourFormatChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region CalendarView_TimeSlotDurationChanged

        /// <summary>
        /// Processes TimeSlotDurationChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarView_TimeSlotDurationChanged(
            object sender, TimeSlotDurationChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region CalendarView_TimeIndicatorsChanged

        /// <summary>
        /// Processes CalendarView_TimeIndicatorsChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarView_TimeIndicatorsChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        #endregion

        #region CalendarView_TimeIndicatorTimeChanged

        /// <summary>
        /// Processes CalendarView_TimeIndicatorTimeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarView_TimeIndicatorTimeChanged(
            object sender, TimeIndicatorTimeChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region VScrollPanel_ScrollBarChanged

        void VScrollPanel_ScrollBarChanged(object sender, EventArgs e)
        {
            Refresh();

            Rectangle r = Bounds;

            r.Y = _CalendarView.WeekDayVScrollPanel.Bounds.Y;
            r.Height = _CalendarView.WeekDayVScrollPanel.Bounds.Height;

            Bounds = r;

            _VScrollPos = -_CalendarView.WeekDayVScrollPanel.ScrollBar.Value;

            Refresh();
        }

        #endregion

        #endregion

        #region Paint processing

        /// <summary>
        /// Paint processing routine
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(ItemPaintArgs e)
        {
            int sliceStart, sliceEnd;
            int sliceCount = GetSliceRange(e, out sliceStart, out sliceEnd);

            if (sliceCount > 0)
            {
                _ViewColor.SetColorTable();

                DrawTimeRuler(e, sliceStart, sliceEnd);
            }
        }

        #region Slice Support Routines

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

            while (start < NumberOfSlices)
            {
                Rectangle r = GetSliceRect(start);

                if (r.Bottom > Bounds.Top)
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
                Rectangle r = GetSliceRect(end);
                
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

        /// <summary>
        /// Gets the given slice rectangle
        /// </summary>
        /// <param name="slice">Slice</param>
        /// <returns>Bounding rectangle</returns>
        private Rectangle GetSliceRect(int slice)
        {
            Rectangle r = Bounds;

            float n = slice * TimeSliceHeight;

            r.Y += (int)(n) + _VScrollPos;
            r.Height = (int)(n + TimeSliceHeight) - (int)n;

            return (r);
        }

        #endregion

        #region DrawTimeRuler

        /// <summary>
        /// Draws the TimeRuler
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        private void DrawTimeRuler(ItemPaintArgs e, int sliceStart, int sliceEnd)
        {
            Graphics g = e.Graphics;

            Region regSav = g.Clip;
            g.SetClip(Bounds);

            DrawBackGround(g);
            DrawTimeIndicators(g, sliceStart, sliceEnd);
            DrawRulerContent(g, sliceStart, sliceEnd);

            // Restore our original clip region

            g.Clip = regSav;
        }

        #endregion

        #region DrawBackGround

        /// <summary>
        /// DrawBackGround
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackGround(Graphics g)
        {
            using (Brush br = _ViewColor.BrushPart((int)eTimeRulerPart.TimeRulerBackground, Bounds))
                g.FillRectangle(br, Bounds);
        }

        #endregion

        #region DrawTimeIndicators

        #region DrawTimeIndicators

        /// <summary>
        /// Draws TimeIndicators
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sliceStart"></param>
        /// <param name="sliceEnd"></param>
        private void DrawTimeIndicators(Graphics g, int sliceStart, int sliceEnd)
        {
            DateTime start, end;
            GetViewDates(out start, out end);

            Rectangle r = Rectangle.Union(GetSliceRect(sliceStart), GetSliceRect(sliceEnd));

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
                            DrawTimeIndicator(g, r, ti);
                    }
                }
            }
        }

        #endregion

        #region DrawTimeIndicator

        #region DrawTimeIndicator

        /// <summary>
        /// Draws individual TimeIndicators
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
                                ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipX;

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
                cdef = _ViewColor.GetColorDef((int)eTimeRulerPart.TimeRulerIndicator);

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
                _ViewColor.GetColor((int)eTimeRulerPart.TimeRulerIndicatorBorder));
        }

        #endregion

        #endregion

        #region GetViewDates

        /// <summary>
        /// GetViewDates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void GetViewDates(out DateTime start, out DateTime end)
        {
            switch (_CalendarView.SelectedView)
            {
                case eCalendarView.Day:
                    start = _CalendarView.DayViewDate;
                    end = _CalendarView.DayViewDate.AddDays(1);
                    break;

                default:
                    start = _CalendarView.WeekViewStartDate;
                    end = _CalendarView.WeekViewEndDate.AddDays(1);
                    break;
            }
        }

        #endregion

        #region GetIndicatorRect

        /// <summary>
        /// GetIndicatorRect
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        private Rectangle GetIndicatorRect(TimeIndicator ti)
        {
            DateTime time = ti.IndicatorDisplayTime;

            int offset = (int)(time.Hour * SlotsPerHour * TimeSliceHeight +
                    (TimeSliceHeight * time.Minute) / TimeSlotDuration);

            offset -= (int)(StartSlice * TimeSliceHeight);

            Rectangle r = Bounds;

            r.Y += (offset - ti.Thickness + _VScrollPos);
            r.Height = ti.Thickness;

            return (r);
        }

        #endregion

        #endregion

        #region DrawRulerContent

        private void DrawRulerContent(Graphics g, int sliceStart, int sliceEnd)
        {
            Point pt1 = new Point();
            Point pt2 = new Point();

            using (Pen pen = new Pen(
                _ViewColor.GetColor((int)eTimeRulerPart.TimeRulerBorder)))
            {
                pt1.X = 3;
                pt2.X = Bounds.Width - 4;

                for (int i = sliceStart; i <= sliceEnd; i++)
                {
                    Rectangle r = GetSliceRect(i);

                    // Draw an hourly separation border line

                    if ((i % SlotsPerHour) == 0)
                    {
                        pt1.Y = pt2.Y = r.Y;

                        g.DrawLine(pen, pt1, pt2);
                    }

                    // Draw the time text

                    DrawTimeRulerText(g, r, i);
                }
            }
        }

        #endregion

        #region DrawTimeRulerText

        /// <summary>
        /// Draws the time text
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="slice"></param>
        private void DrawTimeRulerText(Graphics g, Rectangle r, int slice)
        {
            // Get our hour and minute display text

            slice += StartSlice;

            int hour = slice / SlotsPerHour;
            int minute = (slice % SlotsPerHour) * TimeSlotDuration;

            if (_CalendarView.LabelTimeSlots == true || minute == 0)
            {
                string sHour = GetRulerHour(hour);
                string sMinute = GetRulerMinute(hour, minute);

                // Setup for our output

                Color color = _ViewColor.GetColor(
                    (int)eTimeRulerPart.TimeRulerForeground);

                eTextFormat tf = eTextFormat.Top | eTextFormat.Left | eTextFormat.NoPadding;

                Size sz = TextDrawing.MeasureString(g, sHour, TimeRulerFont, 0, tf);

                Rectangle r2 = new Rectangle(Bounds.X, r.Y + 1,
                    Bounds.Width / 2, (int)TimeSliceHeight - 1);

                // If we are displaying an hourly marker, then display
                // it as an offset hour and minute (or AM/PM designator) display

                if (minute == 0)
                {
                    r2.X = r2.Right - sz.Width;

                    if (r2.X < 0)
                        r2.X = 0;

                    r2.Width = sz.Width;

                    TextDrawing.DrawString(g, sHour, TimeRulerFont, color, r2, tf);

                    r2.X = r2.Right + 2;
                    r2.Y += 1;

                    tf = eTextFormat.Top | eTextFormat.Left | eTextFormat.NoPadding;

                    TextDrawing.DrawString(g, sMinute, TimeRulerFontSm, color, r2, tf);
                }
                else
                {
                    // Non-hourly display

                    r2.Width = Bounds.Width - 4;
                    r2.Y += 2;

                    tf = eTextFormat.Top | eTextFormat.Right | eTextFormat.NoPadding;

                    TextDrawing.DrawString(g,
                        sHour + ScheduleSettings.GetActiveCulture().DateTimeFormat.TimeSeparator +
                        sMinute, TimeRulerFontSm, color, r2, tf);
                }
            }
        }

        /// <summary>
        /// Gets the hourly display text
        /// </summary>
        /// <param name="hour">Hour</param>
        /// <returns>Hourly text</returns>
        private string GetRulerHour(int hour)
        {
            if (_CalendarView.Is24HourFormat == false)
            {
                int h = hour % 12;
                hour = (h == 0) ? 12 : h;
            }
            else
            {
                int h = hour % 24;
                hour = (h == 0) ? 0 : h;
            }

            return (hour.ToString(ScheduleSettings.TimeRulerHourFormatString));
        }

        /// <summary>
        /// Gets the minute display text
        /// </summary>
        /// <param name="hour">Hour</param>
        /// <param name="minute">Minute</param>
        /// <returns>Minute text</returns>
        private string GetRulerMinute(int hour, int minute)
        {
            if (minute == 0 && _CalendarView.Is24HourFormat == false)
            {
                hour %= 24;

                if (hour == 0)
                    return (AmDesignator);

                if (hour == 12)
                    return (PmDesignator);
            }

            return (minute.ToString(ScheduleSettings.TimeRulerMinuteFormatString));
        }

        #endregion

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                HookEvents(false);

                // Dispose of our fonts

                if (_TimeRulerFont != null)
                {
                    _TimeRulerFont.Dispose();
                    _TimeRulerFont = null;
                }

                if (_TimeRulerFontSm != null)
                {
                    _TimeRulerFontSm.Dispose();
                    _TimeRulerFontSm = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            TimeRulerPanel objCopy = new TimeRulerPanel(_CalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the TimeRulerPanel specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New TimeRulerPanel instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            TimeRulerPanel objCopy = copy as TimeRulerPanel;
            base.CopyToItem(objCopy);
        }

        #endregion
    }
}
#endif

