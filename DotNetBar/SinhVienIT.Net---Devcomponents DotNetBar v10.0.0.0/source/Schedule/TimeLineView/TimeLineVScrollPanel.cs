#if FRAMEWORK20
namespace DevComponents.DotNetBar.Schedule
{
    public class TimeLineVScrollPanel : VScrollPanel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        public TimeLineVScrollPanel(CalendarView calendarView)
            : base(calendarView)
        {
        }

        #region Private properties

        /// <summary>
        /// Gets the ScrollBar SmallChange value
        /// </summary>
        protected override int ScrollPanelSmallChange
        {
            get { return (CalendarView.TimeLineHeight); }
        }

        /// <summary>
        /// Gets the ScrollBar Maximum value
        /// </summary>
        protected override int ScrollPanelMaximum
        {
            get
            {
                return (CalendarView.TimeLineHeight *
                    CalendarView.DisplayedOwners.Count);
            }
        }

        #endregion

    }
}
#endif
