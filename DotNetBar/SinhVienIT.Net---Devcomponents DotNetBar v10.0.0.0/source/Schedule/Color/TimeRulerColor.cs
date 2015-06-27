#if FRAMEWORK20
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Schedule
{
    #region enum definitions

    /// <summary>
    /// Week/Day calendar parts enum
    /// </summary>
    public enum eTimeRulerPart : int
    {
        TimeRulerBackground,
        TimeRulerForeground,
        TimeRulerBorder,
        TimeRulerTickBorder,

        TimeRulerIndicator,
        TimeRulerIndicatorBorder
    }

    #endregion

    public class TimeRulerColor : CalendarColor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TimeRulerColor()
            : base(eCalendarColor.Automatic)
        {
        }

        #region SetColorTable routine

        /// <summary>
        /// Sets our current color table to either
        /// a local or global definition
        /// </summary>
        public override void SetColorTable()
        {
            // Use the globally set color table

            Office2007Renderer r =
                GlobalManager.Renderer as Office2007Renderer;

            if (r != null)
                ColorTable = r.ColorTable.CalendarView.TimeRulerColors;
        }

        #endregion
    }
}
#endif
