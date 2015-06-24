#if FRAMEWORK20
namespace DevComponents.DotNetBar.Schedule
{
    #region enum definitions

    #region eCalendarViewPart

    /// <summary>
    /// View calendar parts enum
    /// </summary>
    public enum eCalendarViewPart
    {
        OwnerTabBorder,
        OwnerTabBackground,
        OwnerTabForeground,
        OwnerTabContentBackground
    }

    #endregion

    #region eCalendarColor

    /// <summary>
    /// Defines available custom calendar color
    /// </summary>
    public enum eCalendarColor
    {
        Blue,
        Green,
        Maroon,
        Steel,
        Teal,
        Purple,
        Olive,
        Red,
        DarkPeach,
        DarkSteel,
        DarkGreen,
        Yellow,

        Automatic
    }

    #endregion

    #endregion

    public class CalendarViewColor : CalendarColor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eColor">Default color</param>
        public CalendarViewColor(eCalendarColor eColor)
            : base(eColor)
        {
        }

        #region SetColorTable routine

        /// <summary>
        /// Sets our current color table to either
        /// a local or global definition
        /// </summary>
        public override void SetColorTable()
        {
        }

        #endregion
    }
}
#endif

