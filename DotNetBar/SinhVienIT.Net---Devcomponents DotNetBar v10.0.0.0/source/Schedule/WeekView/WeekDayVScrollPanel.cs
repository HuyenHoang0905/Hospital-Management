#if FRAMEWORK20
namespace DevComponents.DotNetBar.Schedule
{
    public class WeekDayVScrollPanel : VScrollPanel
    {
        public WeekDayVScrollPanel(CalendarView calendarView)
            : base(calendarView)
        {
        }

        #region Private properties

        protected override int ScrollPanelSmallChange
        {
            get { return ((int)CalendarView.TimeSliceHeight); }
        }

        protected override int ScrollPanelMaximum
        {
            get { return ((int)(CalendarView.TimeSliceHeight * CalendarView.NumberOfSlices)); }
        }

        #endregion
        
    }
}
#endif