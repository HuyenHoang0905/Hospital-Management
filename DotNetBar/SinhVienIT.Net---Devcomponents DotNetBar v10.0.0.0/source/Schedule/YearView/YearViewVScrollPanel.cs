#if FRAMEWORK20
namespace DevComponents.DotNetBar.Schedule
{
    public class YearVScrollPanel : VScrollPanel
    {
        public YearVScrollPanel(CalendarView calendarView)
            : base(calendarView)
        {
        }

        #region Private properties

        protected override int ScrollPanelSmallChange
        {
            get { return (10); }
        }

        protected override int ScrollPanelMaximum
        {
            get { return (CalendarView.YearViewMax); }
        }

        #endregion

    }
}
#endif