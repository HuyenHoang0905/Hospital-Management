#if FRAMEWORK20
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Schedule
{
    #region enum definitions

    /// <summary>
    /// Week/Day calendar parts enum
    /// </summary>
    public enum eCalendarWeekDayPart : int
    {
        DayViewBorder,

        DayHeaderForeground,
        DayHeaderBackground,
        DayHeaderBorder,

        DayWorkHoursBackground,
        DayAllDayEventBackground,
        DayOffWorkHoursBackground,

        DayHourBorder,
        DayHalfHourBorder,

        SelectionBackground,

        OwnerTabBorder,
        OwnerTabBackground,
        OwnerTabForeground,
        OwnerTabContentBackground,
        OwnerTabSelectedForeground,
        OwnerTabSelectedBackground,

        CondensedViewBackground,

        NowDayHeaderBorder,
        NowDayHeaderForeground,
        NowDayHeaderBackground,

        TimeIndicator,
        TimeIndicatorBorder
    }

    #endregion

    public class CalendarWeekDayColor : CalendarColor
    {
        #region Private variables

        private ColorDef[][] _LocalColorTable =
             { _Blue, _Green, _Maroon, _Steel, _Teal, _Purple, _Olive,
               _Red, _DarkPeach, _DarkSteel, _DarkGreen, _Yellow};

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eColor">Default color</param>
        public CalendarWeekDayColor(eCalendarColor eColor)
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
            if (ColorSch != eCalendarColor.Automatic)
            {
                // Use the requested local color table

                ColorTable = _LocalColorTable[(int)ColorSch];
            }
            else
            {
                // Use the globally set color table

                Office2007Renderer r =
                    GlobalManager.Renderer as Office2007Renderer;

                ColorTable = (r != null)
                    ? r.ColorTable.CalendarView.WeekDayViewColors
                    : _LocalColorTable[(int)eCalendarColor.Blue];
            }
        }

        #endregion

        #region Static Color definitions

        // Array of predefined color

        #region Blue

        static ColorDef[] _Blue = new ColorDef[]
        {
            new ColorDef(0x5D8CC9),     // DayViewBorder
            new ColorDef(0x000000),     // DayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xE4ECF6, 0xD6E2F1, 0xC2D4EB, 0xD0DEEF},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x8DAED9),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0x8DAED9),     // DayAllDayEventBackground
            new ColorDef(0xE6EDF7),     // DayOffWorkHoursBackground

            new ColorDef(0xA5BFE1),     // DayHourBorder
            new ColorDef(0xD5E1F1),     // DayHalfHourBorder

            new ColorDef(0x294C7A),     // SelectionBackground

            new ColorDef(0x5D8CC9),     // OwnerTabBorder

            new ColorDef(new int[] {0xBBCFE9, 0x8DAED9},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8DAED9),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region Green

        static ColorDef[] _Green = new ColorDef[]
        {
            new ColorDef(0x72A45A),     // DayViewBorder
            new ColorDef(0x50734D),     // DayHeaderForeground

            new ColorDef(new int[] {0xE7F0E4, 0xDDE8D7, 0xCADDC0, 0xD8E5D0},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x72A45A),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xB1CDA4),     // DayAllDayEventBackground
            new ColorDef(0xE9F1E6),     // DayOffWorkHoursBackground

            new ColorDef(0xB1CDA4),     // DayHourBorder
            new ColorDef(0xD6DDD2),     // DayHalfHourBorder

            new ColorDef(0x3F5B32),     // SelectionBackground

            new ColorDef(0x72A45A),     // OwnerTabBorder

            new ColorDef(new int[] {0xC3D9B9, 0x9CBF8B},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x9CBF8B),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region Maroon

        static ColorDef[] _Maroon = new ColorDef[]
        {
            new ColorDef(0xBE6886),     // DayViewBorder
            new ColorDef(0x85496B),     // DayHeaderForeground

            new ColorDef(new int[] {0xF4E6EB, 0xEFDAE2, 0xE7C8D3, 0xEDD4DD},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0xBE6886),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xDBACBC),     // DayAllDayEventBackground
            new ColorDef(0xF5E8EC),     // DayOffWorkHoursBackground

            new ColorDef(0xDBACBC),     // DayHourBorder
            new ColorDef(0xE5D3D9),     // DayHalfHourBorder

            new ColorDef(0x693A4A),     // SelectionBackground

            new ColorDef(0xBE6886),     // OwnerTabBorder

            new ColorDef(new int[] {0xE4BFCB, 0xD195AA},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xD195AA),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
       };

        #endregion

        #region Steel

        static ColorDef[] _Steel = new ColorDef[]
        {
            new ColorDef(0x616A76),     // DayViewBorder
            new ColorDef(0x616A76),     // DayHeaderForeground

            new ColorDef(new int[] {0xDCDFE2, 0xD3D6DA, 0xB4BAC1, 0xCBCED4},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x9199A4),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xC7CBD1),     // DayAllDayEventBackground
            new ColorDef(0xE8EAEC),     // DayOffWorkHoursBackground

            new ColorDef(0xC7CBD1),     // DayHourBorder
            new ColorDef(0xDDDDDD),     // DayHalfHourBorder

            new ColorDef(0x4C535C),     // SelectionBackground

            new ColorDef(0x9199A4),     // OwnerTabBorder

            new ColorDef(new int[] {0xCFD2D8, 0xB0B6BE},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xB0B6BE),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region Teal

        static ColorDef[] _Teal = new ColorDef[]
        {
            new ColorDef(0x5AA4A4),     // DayViewBorder
            new ColorDef(0x3F7380),     // DayHeaderForeground

            new ColorDef(new int[] {0xE4F0F0, 0xD7E8E8, 0xC0DDDD, 0xD0E5E5},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x8CBFC0),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xA4CDCD),     // DayAllDayEventBackground
            new ColorDef(0xE6F1F1),     // DayOffWorkHoursBackground

            new ColorDef(0xA4CDCD),     // DayHourBorder
            new ColorDef(0xD2DDDD),     // DayHalfHourBorder

            new ColorDef(0x325B5B),     // SelectionBackground

            new ColorDef(0x5AA4A4),     // OwnerTabBorder

            new ColorDef(new int[] {0xB9D9D9, 0x8CBFC0},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8CBFC0),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region Purple

        static ColorDef[] _Purple = new ColorDef[]
        {
            new ColorDef(0x7171CD),     // DayViewBorder
            new ColorDef(0x4F4F90),     // DayHeaderForeground

            new ColorDef(new int[] {0xE7E7F6, 0xDCDCF2, 0xC9C9EC, 0xD7D7F0},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x8C8CD7),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0x9B9BDC),     // DayAllDayEventBackground
            new ColorDef(0xE9E9F7),     // DayOffWorkHoursBackground

            new ColorDef(0x9B9BDC),     // DayHourBorder
            new ColorDef(0xD5D5E8),     // DayHalfHourBorder

            new ColorDef(0x3E3E71),     // SelectionBackground

            new ColorDef(0x7171CD),     // OwnerTabBorder

            new ColorDef(new int[] {0xC1C1EA, 0x8C8CD7},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8C8CD7),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
 
            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
       };

        #endregion

        #region Olive

        static ColorDef[] _Olive = new ColorDef[]
        {
            new ColorDef(0x9D9D57),     // DayViewBorder
            new ColorDef(0x6E6E3D),     // DayHeaderForeground

            new ColorDef(new int[] {0xEFEFE3, 0xE8E8D8, 0xDADABF, 0xE3E3CF},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0xBABA89),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xC9C9A2),     // DayAllDayEventBackground
            new ColorDef(0xF0F0E5),     // DayOffWorkHoursBackground

            new ColorDef(0xC9C9A2),     // DayHourBorder
            new ColorDef(0xDBDBD0),     // DayHalfHourBorder

            new ColorDef(0x575730),     // SelectionBackground

            new ColorDef(0x9D9D57),     // OwnerTabBorder

            new ColorDef(new int[] {0xD5D5B8, 0xBABA89},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xBABA89),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
       };

        #endregion

        #region Red

        static ColorDef[] _Red = new ColorDef[]
        {
            new ColorDef(0xC16969),     // DayViewBorder
            new ColorDef(0x874A4A),     // DayHeaderForeground

            new ColorDef(new int[] {0xF5E6E6, 0xEFDADA, 0xE7C6C6, 0xEDD4D4},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0xD39696),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xDDACAC),     // DayAllDayEventBackground
            new ColorDef(0xF6E8E8),     // DayOffWorkHoursBackground

            new ColorDef(0xDDACAC),     // DayHourBorder
            new ColorDef(0xE8D7D7),     // DayHalfHourBorder

            new ColorDef(0x6B3A3A),     // SelectionBackground

            new ColorDef(0xC16969),     // OwnerTabBorder

            new ColorDef(new int[] {0xE5BFBF, 0xD39696},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xD39696),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region DarkPeach

        static ColorDef[] _DarkPeach = new ColorDef[]
        {
            new ColorDef(0xA98F5D),     // DayViewBorder
            new ColorDef(0x776441),     // DayHeaderForeground

            new ColorDef(new int[] {0xF1EDE4, 0xE9E4D7, 0xDFD5C1, 0xE6E0D1},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0xC3B08D),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xCFC1A5),     // DayAllDayEventBackground
            new ColorDef(0xF2EEE6),     // DayOffWorkHoursBackground

            new ColorDef(0xCFC1A5),     // DayHourBorder
            new ColorDef(0xE0D7C5),     // DayHalfHourBorder

            new ColorDef(0x5D4F33),     // SelectionBackground

            new ColorDef(0xA98F5D),     // OwnerTabBorder

            new ColorDef(new int[] {0xDBCFBA, 0xC3B08D},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xC3B08D),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
 
            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
       };

        #endregion

        #region DarkSteel

        static ColorDef[] _DarkSteel = new ColorDef[]
        {
            new ColorDef(0x6197B1),     // DayViewBorder
            new ColorDef(0x446A96),     // DayHeaderForeground

            new ColorDef(new int[] {0xE5EEF2, 0xD8E5EB, 0xC3D7E1, 0xD2E2E9},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x90B6C8),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xA8C5D4),     // DayAllDayEventBackground
            new ColorDef(0xE7EFF3),     // DayOffWorkHoursBackground

            new ColorDef(0xA8C5D4),     // DayHourBorder
            new ColorDef(0xDCE5E3),     // DayHalfHourBorder

            new ColorDef(0x365362),     // SelectionBackground

            new ColorDef(0x6197B1),     // OwnerTabBorder

            new ColorDef(new int[] {0xBCD2DE, 0x90B6C8},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x90B6C8),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
 
            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
       };

        #endregion

        #region DarkGreen

        static ColorDef[] _DarkGreen = new ColorDef[]
        {
            new ColorDef(0x5AA48C),     // DayViewBorder
            new ColorDef(0x3F7362),     // DayHeaderForeground

            new ColorDef(new int[] {0xE4F0EC, 0xD7E8E3, 0xC0DDD4, 0xD0E5DF},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x8BBFAE),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xA4CDBF),     // DayAllDayEventBackground
            new ColorDef(0xE6F1ED),     // DayOffWorkHoursBackground

            new ColorDef(0xA4CDBF),     // DayHourBorder
            new ColorDef(0xD0DDD9),     // DayHalfHourBorder

            new ColorDef(0x325B4D),     // SelectionBackground

            new ColorDef(0x5AA48C),     // OwnerTabBorder

            new ColorDef(new int[] {0xB9D9CE, 0x8BBFAE},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8BBFAE),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #region Yellow

        static ColorDef[] _Yellow = new ColorDef[]
        {
            new ColorDef(0xFFD151),     // DayViewBorder
            new ColorDef(0x7F6628),     // DayHeaderForeground

            new ColorDef(new int[] {0xFFF8E2, 0xFFF5D7, 0xFFEDBD, 0xFFF2CE},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0xFFDF86),     // DayHeaderBorder

            new ColorDef(0xFFFFFF),     // DayWorkHoursBackground
            new ColorDef(0xFFE69F),     // DayAllDayEventBackground
            new ColorDef(0xFFF8E4),     // DayOffWorkHoursBackground

            new ColorDef(0xFFE69F),     // DayHourBorder
            new ColorDef(0xFFE69F),     // DayHalfHourBorder

            new ColorDef(0xB39540),     // SelectionBackground

            new ColorDef(0xFFD151),     // OwnerTabBorder

            new ColorDef(new int[] {0xFFEBB6, 0xFFDF86},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xFFDF86),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xF5F5F5),     // CondensedViewBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // TimeIndicator
                         new float[] {0f, .55f ,58f, 1f}, 90f),

            new ColorDef(0xEB8900),     // TimeIndicatorBorder
        };

        #endregion

        #endregion
    }
}
#endif

