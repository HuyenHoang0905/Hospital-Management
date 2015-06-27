#if FRAMEWORK20
using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class WeekView : WeekDayView
    {
        public WeekView(CalendarView calendarView)
            : base(calendarView, eCalendarView.Week)
        {
        }

        #region RecalcSize support

        /// <summary>
        /// Normalizes the user specified start and end dates
        /// </summary>
        /// <param name="startDate">[out] Normalized start date</param>
        /// <param name="endDate">[out] Normalized end date</param>
        protected override void NormalizeDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = this.StartDate;
            endDate = this.EndDate;

            DaysOfTheWeek = new DaysOfTheWeek(startDate.DayOfWeek, DaysInWeek);
        }

        #endregion

        #region ProcessUpDownKey

        /// <summary>
        /// Processes Up and Down key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dy"></param>
        protected override void ProcessUpDownKey(KeyEventArgs objArg, int dy)
        {
            if (ValidDateSelection())
            {
                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if (startDate.Equals(DateSelectionAnchor.Value) == true)
                    startDate = endDate.AddMinutes(-CalendarView.TimeSlotDuration);

                int col = GetColumnFromDate(startDate);

                startDate = startDate.AddMinutes(dy);

                if (GetColumnFromDate(startDate) == col)
                {
                    if (col < 0)
                    {
                        startDate = CalendarView.DateSelectionStart.Value;
                        col = GetColumnFromDate(startDate);
                    }

                    if (col < 0)
                    {
                        startDate = CalendarView.DateSelectionEnd.Value;
                        col = GetColumnFromDate(startDate);
                    }

                    if (col >= 0)
                    {
                        endDate = startDate.AddMinutes(CalendarView.TimeSlotDuration);

                        DateTime sd = DayColumns[col].Date.AddMinutes(CalendarView.StartSlice * CalendarView.TimeSlotDuration);
                        DateTime ed = sd.AddMinutes(CalendarView.NumberOfActiveSlices * CalendarView.TimeSlotDuration);

                        if (startDate >= sd && endDate <= ed)
                        {
                            ExtendSelection(ref startDate, ref endDate);

                            CalendarView.DateSelectionStart = startDate;
                            CalendarView.DateSelectionEnd = endDate;

                            EnsureSelectionVisible();
                        }
                    }
                }
            }

            objArg.Handled = true;
        }

        #endregion

        #region ProcessLeftRightKey

        /// <summary>
        /// Processes Left and Right Key events
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="dx"></param>
        protected override void ProcessLeftRightKey(KeyEventArgs objArg, int dx)
        {
            if (ValidDateSelection())
            {
                DateTime startDate = CalendarView.DateSelectionStart.Value;
                DateTime endDate = CalendarView.DateSelectionEnd.Value;

                if ((objArg.Modifiers & Keys.Shift) == Keys.Shift)
                {
                    if (startDate.Equals(DateSelectionAnchor.Value) == true)
                        startDate = endDate.AddMinutes(-CalendarView.TimeSlotDuration);

                    startDate = startDate.AddDays(dx);
                    endDate = startDate.AddMinutes(CalendarView.TimeSlotDuration);

                    if (startDate < DayColumns[0].Date || endDate > DayColumns[NumberOfColumns - 1].Date.AddDays(1))
                        CalendarView.EnsureVisible(startDate, startDate.AddDays(1));

                    ExtendSelection(ref startDate, ref endDate);
                }
                else
                {
                    startDate = startDate.AddDays(dx);
                    endDate = endDate.AddDays(dx);

                    if (startDate < DayColumns[0].Date || endDate > DayColumns[NumberOfColumns - 1].Date.AddDays(1))
                        CalendarView.EnsureVisible(startDate, startDate.AddDays(1));

                    DateSelectionAnchor = startDate;
                }

                CalendarView.DateSelectionStart = startDate;
                CalendarView.DateSelectionEnd = endDate;

                EnsureSelectionVisible();
            }

            objArg.Handled = true;
        }

        #endregion
    }
}
#endif

